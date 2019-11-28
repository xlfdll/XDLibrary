using System;
using System.Windows;

namespace Xlfdll.Windows.Presentation.Dialogs
{
    public static class ExceptionMessageBox
    {
        public static void Show(String title, String text, Exception exception)
        {
            MessageBox.Show(String.Format(ExceptionMessageBoxFormat,
                text, Environment.NewLine, exception.GetType().ToString(), exception.Message),
                title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void Show(Window owner, String title, String text, Exception exception)
        {
            MessageBox.Show(owner, String.Format(ExceptionMessageBoxFormat,
                text, Environment.NewLine, exception.GetType().ToString(), exception.Message),
                title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private const String ExceptionMessageBoxFormat = "{0}{1}{1}Exception:{1}{2}{1}{1}Description:{1}{3}";
    }
}