using System;
using System.Windows;
using System.Windows.Interop;

using Xlfdll.Windows.API;

namespace Xlfdll.Windows.Presentation
{
    public static class WindowExtensions
	{
        public static void EnableMinimizeBox(this Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);

            Int64 windowLong = WindowClass.GetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE);

            WindowClass.SetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE, (Int32)(windowLong | WindowStyles.WS_MINIMIZEBOX));
        }

        public static void EnableControlBox(this Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);

            Int64 windowLong = WindowClass.GetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE);

            WindowClass.SetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE, (Int32)(windowLong | WindowStyles.WS_SYSMENU));
        }

        public static void DisableMinimizeBox(this Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);

            Int64 windowLong = WindowClass.GetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE);

            WindowClass.SetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE, (Int32)(windowLong & ~WindowStyles.WS_MINIMIZEBOX));
        }

        public static void DisableControlBox(this Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);

            Int64 windowLong = WindowClass.GetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE);

            WindowClass.SetWindowLong(windowInteropHelper.EnsureHandle(), WindowClass.GWL_STYLE, (Int32)(windowLong & ~WindowStyles.WS_SYSMENU));
        }
    }
}