using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Xlfdll.Windows.API
{
    public static class WindowMessages
    {
        #region Functions

        //[Out] StringBuilder: require initialization of the string builder with proper length first.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Constants

        public const Int32 WM_DRAWCLIPBOARD = 0x0308;
        public const Int32 WM_CHANGECBCHAIN = 0x030D;

        #endregion
    }
}