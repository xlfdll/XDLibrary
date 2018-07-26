using System;
using System.Windows.Forms;

namespace Xlfdll.Windows.Forms.Dialogs
{
    public static class ExceptionMessageBox
    {
        public static void Show(String title, String text, Exception exception)
        {
            MessageBox.Show(String.Format(ExceptionMessageBoxFormat,
                text, Environment.NewLine, exception.GetType().ToString(), exception.Message),
                title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Show(IWin32Window owner, String title, String text, Exception exception)
        {
            MessageBox.Show(owner, String.Format(ExceptionMessageBoxFormat,
                text, Environment.NewLine, exception.GetType().ToString(), exception.Message),
                title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static readonly String ExceptionMessageBoxFormat = "{0}{1}{1}Exception:{1}{2}{1}{1}Description:{1}{3}";
    }
}