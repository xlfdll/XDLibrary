using System;
using System.Windows;
using System.Windows.Interop;

using Xlfdll.Windows.API;

namespace Xlfdll.Windows.Presentation
{
    public static class WindowExtensions
    {
        public static void CenterWindowToScreen(this Window window)
        {
            Double screenWidth = SystemParameters.PrimaryScreenWidth;
            Double screenHeight = SystemParameters.PrimaryScreenHeight;
            Double windowWidth = window.Width;
            Double windowHeight = window.Height;

            window.Left = (screenWidth / 2) - (windowWidth / 2);
            window.Top = (screenHeight / 2) - (windowHeight / 2);
        }

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