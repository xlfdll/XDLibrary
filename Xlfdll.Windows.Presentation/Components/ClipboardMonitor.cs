using System;
using System.Windows;
using System.Windows.Interop;

using Xlfdll.Windows.API;

namespace Xlfdll.Windows.Presentation.Components
{
    public class ClipboardMonitor : IDisposable
    {
        public ClipboardMonitor(Window window)
        {
            this.WindowInteropHelper = new WindowInteropHelper(window);
            this.WindowInteropHelper.EnsureHandle();

            this.HwndSource = HwndSource.FromHwnd(this.WindowInteropHelper.Handle);
            this.HwndSource.AddHook(new HwndSourceHook(WndProc));

            this.NextClipboardViewerHandle = DataExchange.SetClipboardViewer(this.WindowInteropHelper.Handle);
        }

        public event EventHandler<EventArgs> ClipboardContentChanged;

        private WindowInteropHelper WindowInteropHelper { get; }
        private IntPtr NextClipboardViewerHandle { get; set; }
        private HwndSource HwndSource { get; }

        private IntPtr WndProc(IntPtr hwnd, Int32 msg, IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            switch (msg)
            {
                case WindowMessages.WM_DRAWCLIPBOARD:
                    this.ClipboardContentChanged?.Invoke(this, new EventArgs());

                    handled = true;

                    break;
                case WindowMessages.WM_CHANGECBCHAIN:
                    WindowMessages.SendMessage(this.NextClipboardViewerHandle, (UInt32)msg, wParam, lParam);

                    handled = true;

                    break;
            }

            return IntPtr.Zero;
        }

        #region IDisposable Members

        public void Dispose()
        {
            DataExchange.ChangeClipboardChain(this.WindowInteropHelper.Handle, this.NextClipboardViewerHandle);
        }

        #endregion
    }
}