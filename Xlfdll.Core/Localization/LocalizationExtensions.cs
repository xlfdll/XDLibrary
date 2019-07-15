using System;

using Xlfdll.Localization;

namespace Xlfdll.Core.Localization
{
    public static class LocalizationExtensions
    {
        public static String GetTranslatedString(this LanguageDictionary languageDictionary, String path, String fallbackText)
        {
            String[] segments = path.Trim().Split('.');

            return languageDictionary.GetTranslatedString(segments[0], segments[1], fallbackText);
        }

        public static String GetTranslatedString(this LanguageDictionary languageDictionary, String sectionName, String keyName, String fallbackText)
        {
            return (languageDictionary != null
                && languageDictionary.ContainsSection(sectionName)
                && languageDictionary[sectionName].ContainsKey(keyName))
                ? languageDictionary[sectionName][keyName]
                : fallbackText;
        }
    }
}