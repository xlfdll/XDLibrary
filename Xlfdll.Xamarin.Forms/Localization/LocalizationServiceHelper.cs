using System;
using System.Globalization;

namespace Xlfdll.Xamarin.Forms.Localization
{
    public static class LocalizationServiceHelper
    {
        public static CultureInfo GetValidCultureInfo(String name)
        {
            CultureInfo culture = null;

            try
            {
                culture = new CultureInfo(name);
            }
            catch (CultureNotFoundException)
            {
                // Input locale is not a valid .NET culture
                // (eg. "en-ES" : English in Spain)
                // fallback to first characters, in this case "en"
                try
                {
                    String fallbackCultureName = ToDotNetFallbackLanguage(new PlatformCulture(name));

                    Console.WriteLine($"Culture {name} could not be set. Using fallback culture {fallbackCultureName}.");

                    culture = new CultureInfo(fallbackCultureName);
                }
                catch (CultureNotFoundException)
                {
                    // Selected fallback culture is still not a valid .NET culture, falling back to English
                    Console.WriteLine($"Fallback culture could not be set. Using English.");

                    culture = new CultureInfo("en");
                }
            }

            return culture;
        }

        public static String ToDotNetFallbackLanguage(PlatformCulture platformCulture)
        {
            String netLanguage = platformCulture.LanguageName; // use the first part of the identifier (two chars, usually);

            switch (platformCulture.LanguageName)
            {
                // Add more application-specific cases here (if required)
                // ONLY use cultures that have been tested and known to work
                default:
                    break;
            }

            Console.WriteLine($".NET Fallback Language: {platformCulture.LanguageName}");

            return netLanguage;
        }
    }
}