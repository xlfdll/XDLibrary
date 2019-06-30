using System;
using System.Globalization;
using System.Threading;

using Xamarin.Forms;

using Java.Util;

using Xlfdll.Xamarin.Forms.Localization;

[assembly: Dependency(typeof(Xlfdll.Xamarin.Android.Localization.LocalizationService))]

namespace Xlfdll.Xamarin.Android.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            Locale androidLocale = Locale.Default;
            String netLanguage = LocalizationService.AndroidToDotNetLanguage(androidLocale.ToString().Replace("_", "-"));

            return LocalizationServiceHelper.GetValidCultureInfo(netLanguage);
        }

        public void SetLocale(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            Console.WriteLine($"CurrentCulture = {culture.Name}");
        }

        #region Helper Methods

        public static String AndroidToDotNetLanguage(String androidLanguage)
        {
            String netLanguage = androidLanguage;

            // Certain languages need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                // Add more application-specific cases here (if required)
                // ONLY use cultures that have been tested and known to work
                default:
                    break;
            }

            Console.WriteLine($"Android Language: {androidLanguage}");
            Console.WriteLine($".NET Language / Locale: {netLanguage}");

            return netLanguage;
        }

        #endregion
    }
}