using System;
using System.Data;
using System.Data.Common;

namespace Xlfdll.Data
{
    public interface IDirectDataOperator : IDisposable
    {
        DbConnection Connection { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

        Int32 ExecuteNonQuery(String commandText);
        Int32 ExecuteNonQuery(String commandText, params DbParameter[] commandParameters);
        Int32 ExecuteNonQuery(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
        Object ExecuteScalar(String commandText);
        Object ExecuteScalar(String commandText, params DbParameter[] commandParameters);
        Object ExecuteScalar(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
        DbDataReader ExecuteReader(String commandText);
        DbDataReader ExecuteReader(String commandText, params DbParameter[] commandParameters);
        DbDataReader ExecuteReader(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
        DataTable ExecuteDataTable(String commandText);
        DataTable ExecuteDataTable(String commandText, params DbParameter[] commandParameters);
        DataTable ExecuteDataTable(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
        DataSet ExecuteDataSet(String commandText);
        DataSet ExecuteDataSet(String commandText, params DbParameter[] commandParameters);
        DataSet ExecuteDataSet(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
    }
}