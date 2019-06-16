// Partially derived from MinimalisticTelnet library:
// https://www.codeproject.com/Articles/19071/Quick-tool-A-minimalistic-Telnet-library
// Author: Tom Janssens
// Originally licensed under The Code Project Open License (CPOL):
// https://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xlfdll.Network
{
    public class TelnetClient : IDisposable
    {
        public TelnetClient()
        {
            this.InputQueueCapacity = Int32.MaxValue;
            this.OutputQueueCapacity = Int32.MaxValue;

            this.Client = new TcpClient()
            {
                ReceiveTimeout = 5000,
                SendTimeout = 5000
            };
        }

        private TcpClient Client { get; set; }
        private Task BackgroundTask { get; set; }
        private CancellationTokenSource CancellationTokenSource { get; set; }

        private ConcurrentQueue<String> InputQueue { get; set; }
        private ConcurrentQueue<String> OutputQueue { get; set; }

        public Int32 InputQueueCapacity { get; set; }
        public Int32 OutputQueueCapacity { get; set; }

        public String Address { get; private set; }
        public Int32 Port { get; private set; }

        public Boolean IsConnected
        {
            get { return this.Client.Connected; }
        }

        public void Connect(String address)
        {
            this.Connect(address, 23);
        }

        public void Connect(String address, Int32 port)
        {
            this.Address = address;
            this.Port = port;

            this.InputQueue = new ConcurrentQueue<String>();
            this.OutputQueue = new ConcurrentQueue<String>();

            this.CancellationTokenSource = new CancellationTokenSource();
            this.BackgroundTask = new Task(ProcessTelnet, this.CancellationTokenSource.Token);

            this.BackgroundTask.Start();
        }

        public void Disconnect()
        {
            this.Address = null;
            this.Port = -1;

            this.CancellationTokenSource.Cancel();

            this.Client.Dispose();

            this.Client = new TcpClient()
            {
                ReceiveTimeout = 5000,
                SendTimeout = 5000
            };
        }

        public String ReadAll()
        {
            String line = this.ReadLine();
            StringBuilder sb = new StringBuilder();

            while (!String.IsNullOrEmpty(line))
            {
                sb.Append(line);

                line = this.ReadLine();
            }

            return sb.Length > 0 ? sb.ToString() : null;
        }

        public String ReadLine()
        {
            String line = null;

            if (!this.OutputQueue.TryDequeue(out line))
            {
                //  If no object was available to be removed from the queue, the value is unspecified
                line = null;
            }

            return line;
        }

        public void Write(String command)
        {
            this.InputQueue.Enqueue(command);
        }

        public void WriteLine(String command)
        {
            this.Write(command + "\n");
        }

        private void ProcessTelnet()
        {
            try
            {
                this.Client.Connect(this.Address, this.Port);

                NetworkStream stream = this.Client.GetStream();
                StringBuilder sb = new StringBuilder();
                String inputLine = null;

                while (!this.CancellationTokenSource.Token.IsCancellationRequested)
                {
                    while (this.Client.Available > 0)
                    {
                        Int32 inputByte = stream.ReadByte();

                        switch (inputByte)
                        {
                            case -1:
                                {
                                    break;
                                }
                            case (Int32)TelnetCommand.IAC:
                                {
                                    Int32 telnetCommandByte = stream.ReadByte();

                                    switch (telnetCommandByte)
                                    {
                                        case (Int32)TelnetCommand.IAC:
                                            sb.Append(telnetCommandByte);
                                            break;
                                        case (Int32)TelnetCommand.DO:
                                        case (Int32)TelnetCommand.DONT:
                                        case (Int32)TelnetCommand.WILL:
                                        case (Int32)TelnetCommand.WONT:
                                            Int32 telnetOptionsByte = stream.ReadByte();

                                            if (telnetOptionsByte != -1)
                                            {
                                                stream.WriteByte((Byte)TelnetCommand.IAC);

                                                if (telnetOptionsByte == (Byte)TelnetOptions.SGA)
                                                {
                                                    stream.WriteByte(telnetCommandByte == (Byte)TelnetCommand.DO ? (Byte)TelnetCommand.WILL : (Byte)TelnetCommand.DO);
                                                }
                                                else
                                                {
                                                    stream.WriteByte(telnetCommandByte == (Byte)TelnetCommand.DO ? (Byte)TelnetCommand.WONT : (Byte)TelnetCommand.DONT);
                                                }

                                                stream.WriteByte((Byte)telnetOptionsByte);
                                            }

                                            break;
                                        default:
                                            break;
                                    }

                                    break;
                                }
                            default:
                                {
                                    sb.Append((Char)inputByte);
                                    break;
                                }
                        }
                    }

                    if (sb.Length > 0)
                    {
                        this.OutputQueue.Enqueue(sb.ToString());

                        sb.Clear();
                    }

                    while (this.InputQueue.TryDequeue(out inputLine))
                    {
                        Byte[] buffer = Encoding.ASCII.GetBytes(inputLine.Replace("\0xFF", "\0xFF\0xFF"));

                        stream.Write(buffer, 0, buffer.Length);
                    }
                }

                this.Client.Close();
            }
            catch (Exception ex)
            {
                this.OutputQueue.Enqueue("Telnet connection has error occurred.");
                this.OutputQueue.Enqueue($"ERROR: {ex.Message}");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Client.Dispose();
        }

        #endregion
    }

    public enum TelnetCommand
    {
        // Refer to RFC 854
        // https://tools.ietf.org/html/rfc854

        /// <summary>
        /// Indicates the desire to begin performing,
        /// or confirmation that you are now performing,
        /// the indicated option.
        /// </summary>
        WILL = 251,
        /// <summary>
        /// Indicates the refusal to perform,
        /// or continue performing,
        /// the indicated option.
        /// </summary>
        WONT = 252,
        /// <summary>
        /// Indicates the request that the other party perform,
        /// or confirmation that you are expecting the other party to perform,
        /// the indicated option.
        /// </summary>
        DO = 253,
        /// <summary>
        /// Indicates the demand that the other party stop performing,
        /// or confirmation that you are no longer expecting the other party to perform,
        /// the indicated option.
        /// </summary>
        DONT = 254,
        /// <summary>
        /// Interpret as Command
        /// </summary>
        IAC = 255
    }

    public enum TelnetOptions
    {
        SGA = 3
    }
}