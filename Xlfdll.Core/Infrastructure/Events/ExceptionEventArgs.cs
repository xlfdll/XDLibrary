using System;

namespace Xlfdll
{
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception ex)
            : base()
        {
            this.Exception = ex;
        }

        public Exception Exception { get; }
    }
}