using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Xlfdll.Text;

namespace Xlfdll.Logging
{
    public class QueuedLogger
    {
        public QueuedLogger(String fileName)
        {
            this.FileName = fileName;

            this.LogQueue = new ConcurrentQueue<String>();
            this.CancellationTokenSource = new CancellationTokenSource();
            this.LogTask = new Task(ProcessLogs, this.CancellationTokenSource.Token);
        }

        public String FileName { get; }

        public void Start()
        {
            this.LogTask.Start();
        }

        public void Stop()
        {
            this.CancellationTokenSource.Cancel();

            while (!this.LogTask.IsCompleted) { }

            // No need to dispose Task objects
            // See https://stackoverflow.com/questions/3734280/is-it-considered-acceptable-to-not-call-dispose-on-a-tpl-task-object
        }

        public void WriteLog(String log)
        {
            this.LogQueue.Enqueue(log);
        }

        private void ProcessLogs()
        {
            using (StreamWriter writer = new StreamWriter(this.FileName, true, AdditionalEncodings.UTF8WithoutBOM)
            { AutoFlush = true })
            {
                String line;

                while (!this.CancellationTokenSource.IsCancellationRequested)
                {
                    while (this.LogQueue.TryDequeue(out line))
                    {
                        writer.WriteLine(line);
                    }

                    Thread.Sleep(500);
                }

                // After cancellation, process the remaining log messages
                while (this.LogQueue.TryDequeue(out line))
                {
                    writer.WriteLine(line);
                }
            }
        }

        private ConcurrentQueue<String> LogQueue { get; }
        private Task LogTask { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
    }
}