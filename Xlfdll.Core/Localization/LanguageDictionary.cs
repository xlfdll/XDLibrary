using System;
using System.Globalization;

using XDConfig = Xlfdll.Configuration;

namespace Xlfdll.Localization
{
    public class LanguageDictionary : XDConfig.Configuration
    {
        public LanguageDictionary(String cultureName)
            : base()
        {
            this.Culture = CultureInfo.GetCultureInfo(cultureName);
        }

        public LanguageDictionary(Int32 lcid)
            : base()
        {
            this.Culture = CultureInfo.GetCultureInfo(lcid);
        }

        public CultureInfo Culture { get; }
    }
}