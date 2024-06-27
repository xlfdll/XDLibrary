using System;

namespace Xlfdll.Xamarin.Forms.Localization
{
    /// <summary>
	/// Helper class for splitting locales like
	///   iOS: ms_MY, gsw_CH
	///   Android: in-ID
	/// into parts so we can create a .NET culture (or fallback culture)
	/// </summary>
    public class PlatformCulture
    {
        public PlatformCulture(String platformCultureString)
        {
            if (String.IsNullOrEmpty(platformCultureString))
            {
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString));
            }

            this.PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore

            Int32 dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);

            if (dashIndex > 0)
            {
                String[] parts = PlatformString.Split('-');

                this.LanguageName = parts[0];
                this.LocaleName = parts[1];
            }
            else
            {
                this.LanguageName = this.PlatformString;
                this.LocaleName = String.Empty;
            }
        }

        public String PlatformString { get; }
        public String LanguageName { get; }
        public String LocaleName { get; }

        public override String ToString()
        {
            return this.PlatformString;
        }
    }
}