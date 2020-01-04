using System;

namespace Xlfdll
{
    public static class SpecialFolders
    {
        public static String ProgramFilesInX86
            => !Environment.Is64BitOperatingSystem
            ? Environment.GetEnvironmentVariable("ProgramFiles")
            : Environment.GetEnvironmentVariable("ProgramFiles(x86)");

        public static String ProgramFilesInX64
            => Environment.GetEnvironmentVariable("ProgramW6432");
    }
}