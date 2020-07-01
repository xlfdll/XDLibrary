using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;

namespace Xlfdll.Windows.Security
{
    public static class UAC
    {
        public static Boolean IsRunAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void ReRunAsAdmin(Boolean waitElevatedProcess)
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.UseShellExecute = true;
            info.WorkingDirectory = Environment.CurrentDirectory;
            info.FileName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            info.Verb = "runas";

            try
            {
                if (!waitElevatedProcess)
                {
                    using (Process.Start(info)) { }
                }
                else
                {
                    using (Process process = Process.Start(info))
                    {
                        process.WaitForExit();
                    }
                }
            }
            catch (Win32Exception)
            {
                // UAC request was denied by user
                throw;
            }
        }
    }
}