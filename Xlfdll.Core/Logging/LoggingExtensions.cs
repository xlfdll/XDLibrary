using System;
using System.Text;

namespace Xlfdll.Logging
{
    public static class LoggingExtensions
    {
        public static String GetLogContents(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception currentEx = ex;
            Int32 i = 0;

            sb.AppendLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]");
            sb.AppendLine();

            while (currentEx != null)
            {
                i++;

                sb.AppendLine("////////// Level " + i.ToString() + " //////////");
                sb.AppendLine();

                sb.AppendLine("Type: ");
                sb.AppendLine("-----");
                sb.AppendLine(currentEx.GetType().FullName);
                sb.AppendLine();

                sb.AppendLine("Module: ");
                sb.AppendLine("-------");
                sb.AppendLine(currentEx.Source);
                sb.AppendLine();

                sb.AppendLine("Method: ");
                sb.AppendLine("-------");
                sb.AppendLine(currentEx.TargetSite.ToString());
                sb.AppendLine();

                sb.AppendLine("Message:");
                sb.AppendLine("--------");
                sb.AppendLine(currentEx.Message);
                sb.AppendLine();

                sb.AppendLine("Stack Trace:");
                sb.AppendLine("------------");
                sb.AppendLine(currentEx.StackTrace);
                sb.AppendLine();

                currentEx = currentEx.InnerException;
            }

            return sb.ToString();
        }
    }
}