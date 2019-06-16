using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Xlfdll.Data.SQLite
{
    public class SQLiteQueuedDataOperator : IQueuedDataOperator
    {
        public SQLiteQueuedDataOperator(String databaseFileName) : this(databaseFileName, String.Empty) { }

        public SQLiteQueuedDataOperator(String databaseFileName, String password)
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder()
            {
                DataSource = databaseFileName,
                Password = password,
                ForeignKeys = true
            };

            this.Connection = new SQLiteConnection(connectionStringBuilder.ToString());

            this.TaskCancellationTokenSource = new CancellationTokenSource();
            this.SQLCommandTask = new Task(SQLCommandTaskMain, this.TaskCancellationTokenSource.Token);

            this.SQLCommandQueue = new ConcurrentQueue<SQLiteCommand>();
            this.SQLCommandProcessingIDQueue = new ConcurrentQueue<Int64>();
            this.SQLCommandResults = new ConcurrentDictionary<Int64, Object>();
        }

        public SQLiteConnection Connection { get; }

        public void StartQueueProcessing()
        {
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
            return this.EnqueueCommand(commandText, SQLiteSystemConstants.DefaultCommandTimeout, null);
        }

        public Int64 EnqueueCommand(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.EnqueueCommand(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Int64 EnqueueCommand(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            if (this.IsDatabaseConnected && this.IsTaskRunning && !this.TaskCancellationTokenSource.Token.IsCancellationRequested)
            {
                Int64 id = Interlocked.Increment(ref this.LatestSQLCommandID);

                SQLiteCommand command = new SQLiteCommand(commandText, this.Connection)
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
                throw new InvalidOperationException("SQLite database is not connected.");
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

        public Int64 EnqueueCommand(SQLiteCommand command)
        {
            return this.EnqueueCommand(command, SQLiteSystemConstants.DefaultCommandTimeout);
        }

        public Int64 EnqueueCommand(SQLiteCommand command, Int32 commandTimeout)
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
                throw new InvalidOperationException("SQLite database is not connected.");
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
                    Thread.Sleep(TimeSpan.FromSeconds(SQLiteQueuedDataOperator.QueueProcessingPollingTime));

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
            SQLiteCommand command = null;

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

        private void ProcessSelectCommand(SQLiteCommand command)
        {
            Int64 id;

            if (this.SQLCommandProcessingIDQueue.TryDequeue(out id))
            {
                SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
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
                Trace.TraceWarning("SQLiteQueuedDataOperator - SQLCommandResults.Count = {0}", this.SQLCommandResults.Count.ToString());
            }
        }

        private void ProcessNonQueryCommand(SQLiteCommand command)
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
                Trace.TraceWarning("SQLiteQueuedDataOperator - SQLCommandResults.Count = {0}", this.SQLCommandResults.Count.ToString());
            }
        }

        public Boolean IsDatabaseConnected => (this.Connection.State == ConnectionState.Open);
        public Boolean IsTaskRunning => (this.SQLCommandTask.Status == TaskStatus.Running);

        private Task SQLCommandTask { get; set; }
        private CancellationTokenSource TaskCancellationTokenSource { get; }

        private ConcurrentQueue<SQLiteCommand> SQLCommandQueue { get; }
        private ConcurrentQueue<Int64> SQLCommandProcessingIDQueue { get; }
        private ConcurrentDictionary<Int64, Object> SQLCommandResults { get; }

        private Int64 LatestSQLCommandID = -1;

        private const Int32 QueueProcessingPollingTime = 3;

        #region IQueuedDataOperator Members

        DbConnection IQueuedDataOperator.Connection
        {
            get { return this.Connection; }
        }

        Boolean IQueuedDataOperator.IsDatabaseConnected
        {
            get { return this.IsDatabaseConnected; }
        }

        Boolean IQueuedDataOperator.IsTaskRunning
        {
            get { return this.IsTaskRunning; }
        }

        void IQueuedDataOperator.StartQueueProcessing()
        {
            this.StartQueueProcessing();
        }

        void IQueuedDataOperator.StopQueueProcessing()
        {
            this.StopQueueProcessing();
        }

        Int64 IQueuedDataOperator.EnqueueCommand(String commandText)
        {
            return this.EnqueueCommand(commandText);
        }

        Int64 IQueuedDataOperator.EnqueueCommand(String commandText, params DbParameter[] commandParameters)
        {
            return this.EnqueueCommand(commandText, commandParameters.OfType<SQLiteParameter>().ToArray());
        }

        Int64 IQueuedDataOperator.EnqueueCommand(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.EnqueueCommand(commandText, commandTimeout, commandParameters.OfType<SQLiteParameter>().ToArray());
        }

        Int64 IQueuedDataOperator.EnqueueCommand(DbCommand command)
        {
            return this.EnqueueCommand(command as SQLiteCommand);
        }

        Int64 IQueuedDataOperator.EnqueueCommand(DbCommand command, Int32 commandTimeout)
        {
            return this.EnqueueCommand(command as SQLiteCommand, commandTimeout);
        }

        Boolean IQueuedDataOperator.IsCommandProcessed(Int64 id)
        {
            return this.IsCommandProcessed(id);
        }

        Object IQueuedDataOperator.GetCommandResult(Int64 id)
        {
            return this.GetCommandResult(id);
        }

        #endregion

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