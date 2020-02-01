using System;
using System.IO;
using System.Text;

namespace Xlfdll.IO
{
    public static class PathExtensions
    {
        public static String GetSafeFileName(String fileName)
        {
            Char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            StringBuilder sb = new StringBuilder(fileName);

            foreach (Char c in invalidFileNameChars)
            {
                sb.Replace(c, '_');
            }

            return sb.ToString();
        }

        public static String GetSafePath(String path)
        {
            Char[] invalidPathChars = Path.GetInvalidPathChars();

            StringBuilder sb = new StringBuilder(path);

            foreach (Char c in invalidPathChars)
            {
                sb.Replace(c, '_');
            }

            return sb.ToString();
        }
    }
}