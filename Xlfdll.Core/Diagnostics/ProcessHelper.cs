using System;
using System.Diagnostics;

namespace Xlfdll.Diagnostics
{
    public static class ProcessHelper
    {
        public static void Start(String fileName)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName);

            ProcessHelper.Start(processStartInfo);
        }

        public static void Start(String fileName, String arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);

            ProcessHelper.Start(processStartInfo);
        }

        public static void Start(String fileName, Boolean useShellExecute)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = useShellExecute,
                Verb = "open"
            };

            ProcessHelper.Start(processStartInfo);
        }

        public static void Start(ProcessStartInfo processStartInfo)
        {
            using (Process.Start(processStartInfo)) { }
        }
    }
}