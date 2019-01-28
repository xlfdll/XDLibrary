using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

using Xlfdll.Localization;

namespace Xlfdll.Windows.Presentation.Localization
{
    public class LocalizationValueConverter : MarkupExtension, IValueConverter
    {
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            String[] parameters = parameter.ToString().Split('.');
            String viewName = parameters[0];
            String elementName = parameters[1];

            LanguageDictionary languagePack = value as LanguageDictionary;

            // Use DependencyProperty.UnsetValue to indicate conversion failure
            // In this case, the FallbackValue property will be used
            return (languagePack != null
                && languagePack.ContainsSection(viewName)
                && languagePack[viewName].ContainsKey(elementName))
                ? languagePack[viewName][elementName] : DependencyProperty.UnsetValue;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}