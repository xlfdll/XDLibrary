﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Xlfdll.Data.SQLite
{
    public class SQLiteDirectDataOperator : IDirectDataOperator
    {
        public SQLiteDirectDataOperator(String databaseFileName) : this(databaseFileName, String.Empty) { }

        public SQLiteDirectDataOperator(String databaseFileName, String password)
        {
            SQLiteConnectionStringBuilder connectionStringBuilder = new SQLiteConnectionStringBuilder()
            {
                DataSource = databaseFileName,
                Password = password,
                ForeignKeys = true
            };

            this.Connection = new SQLiteConnection(connectionStringBuilder.ToString());
            this.GeneratedCommands = new List<SQLiteCommand>();
        }

        public SQLiteConnection Connection { get; }

        public void BeginTransaction()
        {
            this.ExecuteNonQuery("BEGIN TRANSACTION;");
        }

        public void CommitTransaction()
        {
            this.ExecuteNonQuery("COMMIT TRANSACTION;");
        }

        public void RollbackTransaction()
        {
            this.ExecuteNonQuery("ROLLBACK TRANSACTION;");
        }

        public SQLiteCommand GetCommand(String commandText)
        {
            return this.GetCommand(commandText, null);
        }

        public SQLiteCommand GetCommand(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.GetCommand(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public SQLiteCommand GetCommand(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            SQLiteCommand command = new SQLiteCommand(commandText, this.Connection) { CommandTimeout = commandTimeout };

            if (commandParameters != null)
            {
                command.Parameters.AddRange(commandParameters);
            }

            this.GeneratedCommands.Add(command);

            return command;
        }

        public Int32 ExecuteNonQuery(String commandText)
        {
            return this.ExecuteNonQuery(commandText, SQLiteSystemConstants.DefaultCommandTimeout, null);
        }

        public Int32 ExecuteNonQuery(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Int32 ExecuteNonQuery(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            int result = -1;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (SQLiteCommand command = new SQLiteCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
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

        public Object ExecuteScalar(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public Object ExecuteScalar(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            Object result = null;

            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            using (SQLiteCommand command = new SQLiteCommand(commandText, this.Connection) { CommandTimeout = commandTimeout })
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

        public DataTable ExecuteDataTable(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataTable ExecuteDataTable(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            DataTable result = new DataTable();

            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(commandText, this.Connection))
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

        public DataSet ExecuteDataSet(String commandText, params SQLiteParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, SQLiteSystemConstants.DefaultCommandTimeout, commandParameters);
        }

        public DataSet ExecuteDataSet(String commandText, Int32 commandTimeout, params SQLiteParameter[] commandParameters)
        {
            DataSet result = new DataSet();

            using (SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(commandText, this.Connection))
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

        private List<SQLiteCommand> GeneratedCommands { get; }

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
            return this.GetCommand(commandText, commandParameters as SQLiteParameter[]);
        }

        DbCommand IDirectDataOperator.GetCommand(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.GetCommand(commandText, commandTimeout, commandParameters as SQLiteParameter[]);
        }

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandParameters as SQLiteParameter[]);
        }

        Int32 IDirectDataOperator.ExecuteNonQuery(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteNonQuery(commandText, commandTimeout, commandParameters as SQLiteParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandParameters as SQLiteParameter[]);
        }

        Object IDirectDataOperator.ExecuteScalar(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, commandTimeout, commandParameters as SQLiteParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandParameters as SQLiteParameter[]);
        }

        DataTable IDirectDataOperator.ExecuteDataTable(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataTable(commandText, commandTimeout, commandParameters as SQLiteParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandParameters as SQLiteParameter[]);
        }

        DataSet IDirectDataOperator.ExecuteDataSet(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, commandTimeout, commandParameters as SQLiteParameter[]);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            foreach (SQLiteCommand command in this.GeneratedCommands)
            {
                command.Dispose();
            }

            this.Connection.Dispose();
        }

        #endregion

        public void ChangePassword(String newPassword)
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }

            this.Connection.ChangePassword(newPassword);
            this.Connection.Close();
        }

        public static void CreateDatabase(String databaseFileName)
        {
            SQLiteDirectDataOperator.CreateDatabase(databaseFileName, String.Empty);
        }

        public static void CreateDatabase(String databaseFileName, String password)
        {
            SQLiteConnection.CreateFile(databaseFileName);

            using (SQLiteDirectDataOperator dataOperator = new SQLiteDirectDataOperator(databaseFileName))
            {
                dataOperator.ChangePassword(password);
            }
        }
    }
}