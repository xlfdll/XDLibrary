using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace Xlfdll.Data.Access
{
    public class AccessDirectDataOperator : IDirectDataOperator
    {
        public AccessDirectDataOperator(String databaseFileName)
        {
            OleDbConnectionStringBuilder connectionStringBuilder = new OleDbConnectionStringBuilder()
            {
                DataSource = databaseFileName
            };

            this.Connection = new OleDbConnection(connectionStringBuilder.ToString());
            this.GeneratedCommands = new List<OleDbCommand>();
        }

        public OleDbConnection Connection { get; }

        public void BeginTransaction()
        {
            this.ExecuteNonQuery("BEGIN TRANSACTION;");
        }

        public void CommitTransaction()
        {
            this.ExecuteNonQuery("COMMIT;");
        }

        public void RollbackTransaction()
        {
            this.ExecuteNonQuery("ROLLBACK;");
        }

        public OleDbCommand GetCommand(String commandText)
        {
            return this.GetCommand(commandText, AccessSystemConstants.DefaultCommandTimeout, null);
        }

        public OleDbCommand GetCommand(String commandText, params OleDbParameter[] commandParameters)
        {
            return this.GetCommand(commandText, AccessSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public OleDbCommand GetCommand(String commandText, Int32 commandTimeout, params OleDbParameter[] commandParameters)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            OleDbCommand command = new OleDbCommand(commandText, this.Connection) { CommandTimeout = commandTimeout };

            if (commandParameters != null)
            {
                command.Parameters.AddRange(commandParameters);
            }

            this.GeneratedCommands.Add(command);

            return command;
        }

        public Int32 ExecuteNonQuery(String commandText)
        {
            return this.ExecuteNonQuery(commandText, null);
        }

        public Int32 ExecuteNonQuery(String commandText, params OleDbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, AccessSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Int32 ExecuteNonQuery(String commandText, Int32 commandTimeout, params OleDbParameter[] commandParameters)
        {
            int result = -1;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (OleDbCommand command = new OleDbCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
            {
                if (commandParameters != null)
                {
                    command.Parameters.AddRange(commandParameters);
                }

                result = command.ExecuteNonQuery();
            }

            this.Connection.Close();

            return result;
        }

        public Object ExecuteScalar(String commandText)
        {
            return this.ExecuteScalar(commandText, null);
        }

        public Object ExecuteScalar(String commandText, params OleDbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, AccessSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Object ExecuteScalar(String commandText, Int32 commandTimeout, params OleDbParameter[] commandParameters)
        {
            Object result = null;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (OleDbCommand command = new OleDbCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
            {
                if (commandParameters != null)
                {
                    command.Parameters.AddRange(commandParameters);
                }

                result = command.ExecuteScalar();
            }

            this.Connection.Close();

            return result;
        }

        public DataTable ExecuteDataTable(String commandText)
        {
            return this.ExecuteDataTable(commandText, null);
        }

        public DataTable ExecuteDataTable(String commandText, params OleDbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, AccessSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataTable ExecuteDataTable(String commandText, Int32 commandTimeout, params OleDbParameter[] commandParameters)
        {
            DataTable result = new DataTable();

            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandText, this.Connection))
            {
                if (commandParameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(commandParameters);
                }

                dataAdapter.SelectCommand.CommandTimeout = commandTimeout;

                dataAdapter.Fill(result);
            }

            return result;
        }

        public DataSet ExecuteDataSet(String commandText)
        {
            return this.ExecuteDataSet(commandText, null);
        }

        public DataSet ExecuteDataSet(String commandText, params OleDbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, AccessSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataSet ExecuteDataSet(String commandText, Int32 commandTimeout, params OleDbParameter[] commandParameters)
        {
            DataSet result = new DataSet();

            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(commandText, this.Connection))
            {
                if (commandParameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(commandParameters);
                }

                dataAdapter.SelectCommand.CommandTimeout = commandTimeout;

                dataAdapter.Fill(result);
            }

            return result;
        }

        private List<OleDbCommand> GeneratedCommands { get; }

        #region IDirectDataOperator Members

        DbConnection IDirectDataOperator.Connection => this.Connection;

        void IDirectDataOperator.BeginTransaction()
        {
            this.BeginTransaction();
        }

        void IDirectDataOperator.CommitTransaction()
        {
            this.CommitTransaction();
        }

        void IDirectDataOperator.RollbackTransaction()
        {
            this.RollbackTransaction();
        }

        DbCommand IDirectDataOperator.GetCommand(String commandText)
        {
            return this.GetCommand(commandText);
        }

        DbCommand IDirectDataOperator.GetCommand(String commandText, params DbParameter[] commandParameters)
        {
            return this.GetCommand(commandText, commandParameters as OleDbParameter[]);
        }

        DbCommand IDirectDataOperator.GetCommand(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.GetCommand(commandText, commandTimeout, commandParameters as OleDbParameter[]);
        }

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandParameters as OleDbParameter[]);
        }

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandTimeout, commandParameters as OleDbParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandParameters as OleDbParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandTimeout, commandParameters as OleDbParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandParameters as OleDbParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandTimeout, commandParameters as OleDbParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandParameters as OleDbParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandTimeout, commandParameters as OleDbParameter[]);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (OleDbCommand command in this.GeneratedCommands)
            {
                command.Dispose();
            }

            this.Connection.Dispose();
        }

        #endregion
    }
}