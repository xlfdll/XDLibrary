using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xlfdll.Xamarin.Forms.Localization
{
    [ContentProperty("Key")]
    public class ResourceLocalizerExtension : IMarkupExtension
    {
        public ResourceLocalizerExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                culture = DependencyService.Get<ILocalizationService>().GetCurrentCultureInfo();
            }
        }

        private CultureInfo culture = null;

        public String Key { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(this.Key))
            {
                return String.Empty;
            }

            String translatedText = ResourceLocalizerExtension.ResourceManager.Value.GetString(this.Key, culture);

            if (String.IsNullOrEmpty(translatedText))
            {
#if DEBUG
                throw new ArgumentException(
                    String.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.",
                        this.Key, ResourceLocalizerExtension.ResourceID, culture.Name),
                    "Text");
#else
                // HACK: returns the key, which GETS DISPLAYED TO THE USER
                translatedText = $"[{this.Key}]";
#endif
            }
            return translatedText;
        }

        public static String ResourceID { get; set; }

        private static readonly Lazy<ResourceManager> ResourceManager = new Lazy<ResourceManager>(
            () => new ResourceManager(ResourceLocalizerExtension.ResourceID,
                IntrospectionExtensions.GetTypeInfo(typeof(ResourceLocalizerExtension)).Assembly));
    }
}