using System;
using System.Runtime.InteropServices;

namespace Xlfdll.Windows.API
{
    public class UserShell
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        private static extern String SHGetKnownFolderPath
            ([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, UInt32 dwFlags, IntPtr hToken);
    }
}