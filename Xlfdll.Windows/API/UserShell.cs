﻿using System;
using System.Runtime.InteropServices;

namespace Xlfdll.Windows.API
{
    public static class UserShell
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        public static extern String SHGetKnownFolderPath
            ([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, UInt32 dwFlags, IntPtr hToken);
    }

    #region Constants

    [Flags]
    public enum KnownFolderFlags : UInt32
    {
        KF_FLAG_DEFAULT = 0x00000000,
        KF_FLAG_FORCE_APP_DATA_REDIRECTION = 0x00080000,
        KF_FLAG_RETURN_FILTER_REDIRECTION_TARGET = 0x00040000,
        KF_FLAG_FORCE_PACKAGE_REDIRECTION = 0x00020000,
        KF_FLAG_NO_PACKAGE_REDIRECTION = 0x00010000,
        KF_FLAG_FORCE_APPCONTAINER_REDIRECTION = 0x00020000,
        KF_FLAG_NO_APPCONTAINER_REDIRECTION = 0x00010000,
        KF_FLAG_CREATE = 0x00008000,
        KF_FLAG_DONT_VERIFY = 0x00004000,
        KF_FLAG_DONT_UNEXPAND = 0x00002000,
        KF_FLAG_NO_ALIAS = 0x00001000,
        KF_FLAG_INIT = 0x00000800,
        KF_FLAG_DEFAULT_PATH = 0x00000400,
        KF_FLAG_NOT_PARENT_RELATIVE = 0x00000200,
        KF_FLAG_SIMPLE_IDLIST = 0x00000100,
        KF_FLAG_ALIAS_ONLY = 0x80000000
    }

    #endregion
}