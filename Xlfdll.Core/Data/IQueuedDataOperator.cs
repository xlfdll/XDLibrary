using System;
using System.Data.Common;

namespace Xlfdll.Data
{
    public interface IQueuedDataOperator : IDisposable
    {
        DbConnection Connection { get; }

        void StartQueueProcessing();
        void StopQueueProcessing();

        Int64 EnqueueCommand(String commandText);
        Int64 EnqueueCommand(String commandText, params DbParameter[] commandParameters);
        Int64 EnqueueCommand(String commandText, Int32 commandTimeout, params DbParameter[] commandParameters);
        Int64 EnqueueCommand(DbCommand command);
        Int64 EnqueueCommand(DbCommand command, Int32 commandTimeout);

        Boolean IsCommandProcessed(Int64 id);
        Object GetCommandResult(Int64 id);

        Boolean IsDatabaseConnected { get; }
        Boolean IsTaskRunning { get; }
    }
}