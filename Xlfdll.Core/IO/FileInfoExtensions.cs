using System;
using System.IO;

namespace Xlfdll.IO
{
    public static class FileInfoExtensions
    {
        public static String GetHumanReadableSizeString(Int64 length, Int32 precision)
        {
            Double floatLength = length;

            if (length != 0L)
            {
                Int32 order = Convert.ToInt32(Math.Floor(Math.Log(floatLength, 1024)));

                floatLength /= Math.Pow(1024, order);

                return floatLength.ToString("F" + precision.ToString()) + " " + FileInfoExtensions.ByteAbbreviations[order];
            }
            else
            {
                return floatLength.ToString("F" + precision.ToString()) + " " + FileInfoExtensions.ByteAbbreviations[0];
            }
        }

        public static String GetHumanReadableSizeString(this FileInfo fileInfo, Int32 precision)
        {
            return FileInfoExtensions.GetHumanReadableSizeString(fileInfo.Length, precision);
        }

        private static String[] ByteAbbreviations = { "B", "KB", "MB", "GB" };
    }
}