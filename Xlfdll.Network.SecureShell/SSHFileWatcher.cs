﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace Xlfdll.Network.SecureShell
{
    public class SSHFileWatcher : IDisposable
    {
        public SSHFileWatcher(String host, String userName, String password, String path)
        {
            this.Host = host;
            this.UserName = userName;
            this.Path = path;

            // The default SftpClient.OperationTimeout is Infinite
            this.Client = new SftpClient(host, userName, password)
            {
                OperationTimeout = new TimeSpan(0, 0, 5)
            };
            this.Client.HostKeyReceived += (sender, e) => { e.CanTrust = true; };

            this.LineQueue = new ConcurrentQueue<String>();

            this.CancellationTokenSource = new CancellationTokenSource();
            this.FileWatcherTask = new Task(FileWatcherTaskMain, this.CancellationTokenSource.Token);
        }

        public String Host { get; }
        public String UserName { get; }
        public String Path { get; }

        public Int32 LineCapacity { get; set; } = 100;

        public Boolean IsRunning
            => (this.FileWatcherTask.Status == TaskStatus.Running);
        public Exception Exception
            => this.FileWatcherTask.Exception;

        public void Start()
        {
            this.FileWatcherTask.Start();
        }

        public void Stop()
        {
            this.CancellationTokenSource.Cancel();

            try
            {
                this.FileWatcherTask.Wait();
            }
            catch (AggregateException) { }
        }

        public String GetLine()
        {
            String result = null;

            this.LineQueue.TryDequeue(out result);

            return result;
        }

        private void FileWatcherTaskMain()
        {
            while (!this.CancellationTokenSource.IsCancellationRequested)
            {
                if (!this.Client.IsConnected)
                {
                    try
                    {
                        this.Client.Connect();
                    }
                    catch { }
                }
                else
                {
                    ProcessLines();
                }
            }
        }

        private void ProcessLines()
        {
            try
            {
                using (SftpFileStream stream = this.Client.OpenRead(this.Path))
                {
                    stream.Seek(0, SeekOrigin.End);

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!this.CancellationTokenSource.IsCancellationRequested)
                        {
                            String line = reader.ReadLine();

                            if (!String.IsNullOrEmpty(line))
                            {
                                this.LineQueue.Enqueue(line);

                                if (this.LineQueue.Count > this.LineCapacity)
                                {
                                    this.LineQueue.TryDequeue(out line);
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (this.FileWatcherTask.Status == TaskStatus.Running)
            {
                this.Stop();
            }

            this.Client.Dispose();
            this.CancellationTokenSource.Dispose();
        }

        #endregion

        private SftpClient Client { get; }
        private ConcurrentQueue<String> LineQueue { get; }
        private Task FileWatcherTask { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
    }
}