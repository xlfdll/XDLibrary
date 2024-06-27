using System;
using System.Globalization;
using System.Threading;

using Foundation;

using Xamarin.Forms;

using Xlfdll.Xamarin.Forms.Localization;

[assembly: Dependency(typeof(Xlfdll.Xamarin.iOS.Localization.LocalizationService))]

namespace Xlfdll.Xamarin.iOS.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            String netLanguage = "en";

            if (NSLocale.PreferredLanguages.Length > 0)
            {
                String preferredLanguage = NSLocale.PreferredLanguages[0];

                netLanguage = LocalizationService.iOSToDotNetLanguage(preferredLanguage);
            }

            return LocalizationServiceHelper.GetValidCultureInfo(netLanguage);
        }

        public void SetLocale(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.WriteLine($"CurrentCulture = {culture.Name}");
        }

        #region Helper Methods

        public static String iOSToDotNetLanguage(String iOSLanguage)
        {
            String netLanguage = iOSLanguage;

            // Certain languages need to be converted to CultureInfo equivalent
            switch (iOSLanguage)
            {
                // Add more application-specific cases here (if required)
                // ONLY use cultures that have been tested and known to work
                default:
                    break;
            }

            Console.WriteLine($"iOS Language: {iOSLanguage}");
            Console.WriteLine($".NET Language / Locale: {netLanguage}");

            return netLanguage;
        }

        #endregion
    }
}