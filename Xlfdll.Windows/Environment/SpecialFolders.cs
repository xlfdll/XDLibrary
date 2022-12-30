using System;

using Xlfdll.Windows.API;

namespace Xlfdll.Windows
{
    public static class ProgramFolders
    {
        public static String ProgramFilesBasedOnOS
            => Environment.Is64BitOperatingSystem
            ? ProgramFolders.ProgramFilesInX64
            : ProgramFolders.ProgramFilesInX86;

        public static String ProgramFilesBasedOnProcess
            => Environment.Is64BitProcess
            ? ProgramFolders.ProgramFilesInX64
            : ProgramFolders.ProgramFilesInX86;

        public static String ProgramFilesInX86
            => !Environment.Is64BitOperatingSystem
            ? Environment.GetEnvironmentVariable("ProgramFiles")
            : Environment.GetEnvironmentVariable("ProgramFiles(x86)");

        public static String ProgramFilesInX64
            => Environment.GetEnvironmentVariable("ProgramW6432");
    }

    public static class UserFolders
    {
        public static String Downloads
            => UserShell.SHGetKnownFolderPath
                (new Guid("374DE290-123F-4565-9164-39C4925E467B"),
                (UInt32)KnownFolderFlags.KF_FLAG_DEFAULT, (IntPtr)0);
    }
}