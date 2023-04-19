using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Xlfdll.Diagnostics
{
    public class RedirectedProcess : IDisposable
    {
        public RedirectedProcess(String fileName, String arguments = null, Boolean useProcessWorkingDirectory = true)
        {
            this.StartInfo = new ProcessStartInfo(fileName, arguments)
            {
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            if (useProcessWorkingDirectory)
            {
                this.StartInfo.WorkingDirectory = Path.GetDirectoryName(fileName);
            }

            this.BaseProcess = new Process()
            {
                StartInfo = this.StartInfo,
                EnableRaisingEvents = true
            };

            this.BaseProcess.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                this.OutputDataReceived?.Invoke(sender, e);
            };
            this.BaseProcess.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                this.ErrorDataReceived?.Invoke(sender, e);
            };
            this.BaseProcess.Exited += delegate (object sender, EventArgs e)
            {
                this.Exited?.Invoke(sender, e);
            };
        }

        public ProcessStartInfo StartInfo { get; }
        public Process BaseProcess { get; }

        public Boolean IsOutputRedirectionAsync { get; private set; }
        public Boolean IsErrorRedirectionAsync { get; private set; }

        public StreamWriter StandardInput
            => this.BaseProcess.StandardInput;
        public StreamReader StandardOutput
            => this.BaseProcess.StandardOutput;
        public StreamReader StandardError
            => this.BaseProcess.StandardError;

        public String Name
            => Path.GetFileName(this.StartInfo.FileName);

        public void Start(Boolean asyncRedirection = false)
        {
            this.BaseProcess.Start();

            this.Started?.Invoke(this, new EventArgs());

            if (asyncRedirection)
            {
                this.BeginOutputReadLine();
                this.BeginErrorReadLine();
            }
        }

        public void WaitForExit(Int32 milliseconds = Int32.MaxValue)
        {
            this.BaseProcess.WaitForExit(milliseconds);
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                this.Start(true);
                this.WaitForExit();
            });
        }

        public void BeginOutputReadLine()
        {
            this.BaseProcess.BeginOutputReadLine();
            this.IsOutputRedirectionAsync = true;
        }

        public void BeginErrorReadLine()
        {
            this.BaseProcess.BeginErrorReadLine();
            this.IsErrorRedirectionAsync = true;
        }

        public void CancelOutputReadLine()
        {
            this.BaseProcess.CancelOutputRead();
            this.IsOutputRedirectionAsync = false;
        }

        public void CancelErrorReadLine()
        {
            this.BaseProcess.CancelErrorRead();
            this.IsErrorRedirectionAsync = false;
        }

        public event EventHandler Started;
        public event DataReceivedEventHandler OutputDataReceived;
        public event DataReceivedEventHandler ErrorDataReceived;
        public event EventHandler Exited;

        #region IDisposable Members

        public void Dispose()
        {
            if (this.IsOutputRedirectionAsync)
            {
                this.CancelOutputReadLine();
            }
            if (this.IsErrorRedirectionAsync)
            {
                this.CancelErrorReadLine();
            }

            this.BaseProcess.Dispose();
        }

        #endregion
    }
}