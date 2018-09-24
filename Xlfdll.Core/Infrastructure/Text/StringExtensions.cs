using System;
using System.Text.RegularExpressions;

namespace Xlfdll
{
    public static class StringExtensions
    {
        public static String Replace(this String input, String findWhat, String replaceWith, Boolean ignoreCase)
        {
            RegexOptions options = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;

            return Regex.Replace(input, Regex.Escape(findWhat), replaceWith.Replace("$", "$$"), options);
        }
    }
}