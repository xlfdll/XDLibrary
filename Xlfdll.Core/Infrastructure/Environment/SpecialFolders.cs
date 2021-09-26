using System;

namespace Xlfdll
{
    public static class SpecialFolders
    {
        public static String ProgramFilesBasedOnOS
            => Environment.Is64BitOperatingSystem
            ? SpecialFolders.ProgramFilesInX64
            : SpecialFolders.ProgramFilesInX86;

        public static String ProgramFilesBasedOnProcess
            => Environment.Is64BitProcess
            ? SpecialFolders.ProgramFilesInX64
            : SpecialFolders.ProgramFilesInX86;

        public static String ProgramFilesInX86
            => !Environment.Is64BitOperatingSystem
            ? Environment.GetEnvironmentVariable("ProgramFiles")
            : Environment.GetEnvironmentVariable("ProgramFiles(x86)");

        public static String ProgramFilesInX64
            => Environment.GetEnvironmentVariable("ProgramW6432");
    }
}