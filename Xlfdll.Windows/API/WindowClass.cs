using System;
using System.Runtime.InteropServices;

namespace Xlfdll.Windows.API
{
    public static class WindowClass
    {
        #region Functions

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        #endregion

        #region Constants

        public const Int32 GWL_STYLE = -16;

        #endregion
    }
}