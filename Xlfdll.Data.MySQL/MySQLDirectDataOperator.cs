using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MySql.Data.MySqlClient;

namespace Xlfdll.Data.MySQL
{
    public class MySQLDirectDataOperator : IDirectDataOperator
    {
        public MySQLDirectDataOperator(String server, String userName, String password, String database)
        {
            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = server,
                Database = database,
                UserID = userName,
                Password = password
            };

            this.Connection = new MySqlConnection(connectionStringBuilder.ToString());
            this.GeneratedDataReaders = new List<MySqlDataReader>();
        }

        public MySqlConnection Connection { get; }

        public void BeginTransaction()
        {
            this.ExecuteNonQuery("START TRANSACTION;");
        }

        public void CommitTransaction()
        {
            this.ExecuteNonQuery("COMMIT;");
        }

        public void RollbackTransaction()
        {
            this.ExecuteNonQuery("ROLLBACK;");
        }

        public Int32 ExecuteNonQuery(String commandText)
        {
            return this.ExecuteNonQuery(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public Int32 ExecuteNonQuery(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Int32 ExecuteNonQuery(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            Int32 result = -1;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
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
            return this.ExecuteScalar(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public Object ExecuteScalar(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Object ExecuteScalar(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            Object result = null;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
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

        public MySqlDataReader ExecuteReader(String commandText)
        {
            return this.ExecuteReader(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public MySqlDataReader ExecuteReader(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.ExecuteReader(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public MySqlDataReader ExecuteReader(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            MySqlDataReader dataReader = null;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (MySqlCommand command = new MySqlCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
            {
                if (commandParameters != null)
                {
                    command.Parameters.AddRange(commandParameters);
                }

                dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);

                this.GeneratedDataReaders.Add(dataReader);
            }

            return dataReader;
        }

        public DataTable ExecuteDataTable(String commandText)
        {
            return this.ExecuteDataTable(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public DataTable ExecuteDataTable(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataTable ExecuteDataTable(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            DataTable dataTable = new DataTable();

            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(commandText, this.Connection))
            {
                if (commandParameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(commandParameters);
                }

                dataAdapter.SelectCommand.CommandTimeout = commandTimeout;

                dataAdapter.Fill(dataTable);
            }

            return dataTable;
        }

        public DataSet ExecuteDataSet(String commandText)
        {
            return this.ExecuteDataSet(commandText, MySQLSystemConstants.DefaultCommandTimeout, null);
        }

        public DataSet ExecuteDataSet(String commandText, params MySqlParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, MySQLSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataSet ExecuteDataSet(String commandText, Int32 commandTimeout, params MySqlParameter[] commandParameters)
        {
            DataSet dataSet = new DataSet();

            using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(commandText, this.Connection))
            {
                if (commandParameters != null)
                {
                    dataAdapter.SelectCommand.Parameters.AddRange(commandParameters);
                }

                dataAdapter.SelectCommand.CommandTimeout = commandTimeout;

                dataAdapter.Fill(dataSet);
            }

            return dataSet;
        }

        private List<MySqlDataReader> GeneratedDataReaders { get; }

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

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandParameters as MySqlParameter[]);
        }

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandTimeout, commandParameters as MySqlParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandParameters as MySqlParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandTimeout, commandParameters as MySqlParameter[]);
        }

        DbDataReader IDirectDataOperator.ExecuteReader(String commandText)
        {
            return this.ExecuteReader(commandText);
        }

        DbDataReader IDirectDataOperator.ExecuteReader(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteReader(commandText, commandParameters as MySqlParameter[]);
        }

        DbDataReader IDirectDataOperator.ExecuteReader(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteReader(commandText, commandTimeout, commandParameters as MySqlParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandParameters as MySqlParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandTimeout, commandParameters as MySqlParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandParameters as MySqlParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandTimeout, commandParameters as MySqlParameter[]);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (MySqlDataReader dataReader in this.GeneratedDataReaders)
            {
                dataReader.Dispose();
            }

            this.Connection.Dispose();
        }

        #endregion
    }
}