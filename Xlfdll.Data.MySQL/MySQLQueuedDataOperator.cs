using System;
using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace Xlfdll.Data.MySQL
{
    public class MySQLQueuedDataOperator : IDisposable
    {
        public MySQLQueuedDataOperator(String server, String userName, String password, String database)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = server,
                Database = database,
                UserID = userName,
                Password = password
            };

            this.Connection = new MySqlConnection(connectionStringBuilder.ToString());

            this.TaskCancellationTokenSource = new CancellationTokenSource();
            this.SQLCommandTask = new Task(SQLCommandTaskMain, this.TaskCancellationTokenSource.Token);

            this.SQLCommandQueue = new ConcurrentQueue<MySqlCommand>();
            this.SQLCommandProcessingIDQueue = new ConcurrentQueue<Int64>();
            this.SQLCommandResults = new ConcurrentDictionary<Int64, Object>();
        }

        public MySqlConnection Connection { get; }

        public void StartQueueProcessing()
        {
            this.Connection.Open();
            this.SQLCommandTask.Start();
        }

        public void StopQueueProcessing()
        {
            this.TaskCancellationTokenSource.Cancel();

            try
            {
                this.SQLCommandTask.Wait();
            }
            catch (AggregateException) { }

            if (this.IsDatabaseConnected)
            {
                ProcessCommandQueue();
            }

            this.Connection.Close();
        }

        public Int64 EnqueueCommand(String commandText)
        {
            return this.EnqueueCommand(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public Int64 EnqueueCommand(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.EnqueueCommand(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Int64 EnqueueCommand(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            if (this.IsDatabaseConnected && this.IsTaskRunning && !this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                Int64 id = Interlocked.Increment(ref this.LatestSQLCommandID);

                MySqlCommand command = new MySqlCommand(commandText, this.Connection)
                {
                    CommandTimeout = commandTimeout
                };

                if (commandParameters != null)
                {
                    command.Parameters.AddRange(commandParameters);
                }

                this.SQLCommandQueue.Enqueue(command);
                this.SQLCommandProcessingIDQueue.Enqueue(id);

                return id;
            }
            else if (!this.IsDatabaseConnected)
            {
                throw new InvalidOperationException("MySQL database is not connected.");
            }
            else if (!this.IsTaskRunning)
            {
                throw new InvalidOperationException("The queue processing task is not running.");
            }
            else if (this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException("The queue processing task is requested to be canceled.");
            }

            return -1;
        }

        public Int64 EnqueueCommand(MySqlCommand command)
        {
            return this.EnqueueCommand(command, MySQLSystemConstants.DefaultCommandTimeout);
        }

        public Int64 EnqueueCommand(MySqlCommand command, Int32 commandTimeout)
        {
            if (this.IsDatabaseConnected && this.IsTaskRunning && !this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                Int64 id = Interlocked.Increment(ref this.LatestSQLCommandID);

                command.Connection = this.Connection;
                command.CommandTimeout = commandTimeout;

                this.SQLCommandQueue.Enqueue(command);
                this.SQLCommandProcessingIDQueue.Enqueue(id);

                return id;
            }
            else if (!this.IsDatabaseConnected)
            {
                throw new InvalidOperationException("MySQL database is not connected.");
            }
            else if (!this.IsTaskRunning)
            {
                throw new InvalidOperationException("The queue processing task is not running.");
            }
            else if (this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException("The queue processing task is requested to be canceled.");
            }

            return -1;
        }

        public Boolean IsCommandProcessed(Int64 id)
        {
            return this.SQLCommandResults.ContainsKey(id);
        }

        public Object GetCommandResult(Int64 id)
        {
            Object result;

            this.SQLCommandResults.TryRemove(id, out result);

            return result;
        }

        private void SQLCommandTaskMain()
        {
            while (!this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                if (this.SQLCommandQueue.IsEmpty || !this.IsDatabaseConnected)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(MySQLQueuedDataOperator.QueueProcessingPollingTime));

                    if (!this.IsDatabaseConnected)
                    {
                        try
                        {
                            this.Connection.Open();
                        }
                        catch { }
                    }
                }
                else
                {
                    ProcessCommandQueue();
                }
            }
        }

        private void ProcessCommandQueue()
        {
            MySqlCommand command = null;

            while (this.SQLCommandQueue.TryDequeue(out command))
            {
                try
                {
                    if (command.CommandText.StartsWith("SELECT", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ProcessSelectCommand(command);
                    }
                    else
                    {
                        ProcessNonQueryCommand(command);
                    }
                }
                catch (Exception)
                {
                    if (!this.IsDatabaseConnected)
                    {
                        break; // If connection is lost, stop processing commands immediately
                    }
                }
            }
        }

        private void ProcessSelectCommand(MySqlCommand command)
        {
            Int64 id;

            if (this.SQLCommandProcessingIDQueue.TryDequeue(out id))
            {
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();

                try
                {
                    dataAdapter.Fill(dataSet);

                    this.SQLCommandResults.TryAdd(id, dataSet);
                }
                catch (Exception ex)
                {
                    this.SQLCommandResults.TryAdd(id, ex);

                    throw;
                }
                finally
                {
                    command.Connection = null;
                    command.Dispose();
                }
            }

            if (this.SQLCommandResults.Count % 100 == 0)
            {
                Trace.TraceWarning("MySQLQueuedDataOperator - SQLCommandResults.Count = {0}", this.SQLCommandResults.Count.ToString());
            }
        }

        private void ProcessNonQueryCommand(MySqlCommand command)
        {
            Int64 id;

            if (this.SQLCommandProcessingIDQueue.TryDequeue(out id))
            {
                try
                {
                    Int32 result = command.ExecuteNonQuery();

                    this.SQLCommandResults.TryAdd(id, result);
                }
                catch (Exception ex)
                {
                    this.SQLCommandResults.TryAdd(id, ex);

                    throw;
                }
                finally
                {
                    command.Connection = null;
                    command.Dispose();
                }
            }

            if (this.SQLCommandResults.Count % 100 == 0)
            {
                Trace.TraceWarning("MySQLQueuedDataOperator - SQLCommandResults.Count = {0}", this.SQLCommandResults.Count.ToString());
            }
        }

        public Boolean IsDatabaseConnected => (this.Connection.State == ConnectionState.Open);
        public Boolean IsTaskRunning => (this.SQLCommandTask.Status == TaskStatus.Running);

        private Task SQLCommandTask { get; set; }
        private CancellationTokenSource TaskCancellationTokenSource { get; }

        private ConcurrentQueue<MySqlCommand> SQLCommandQueue { get; }
        private ConcurrentQueue<Int64> SQLCommandProcessingIDQueue { get; }
        private ConcurrentDictionary<Int64, Object> SQLCommandResults { get; }

        private Int64 LatestSQLCommandID = -1;

        private const Int32 QueueProcessingPollingTime = 3;

        #region IDisposable Members

        public void Dispose()
        {
            if (this.IsTaskRunning)
            {
                this.StopQueueProcessing();
            }

            this.Connection.Dispose();
            this.TaskCancellationTokenSource.Dispose();
        }

        #endregion
    }
}