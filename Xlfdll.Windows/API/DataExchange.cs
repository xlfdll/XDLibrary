using System;
using System.Runtime.InteropServices;

namespace Xlfdll.Windows.API
{
    public static class DataExchange
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern Boolean ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
    }
}