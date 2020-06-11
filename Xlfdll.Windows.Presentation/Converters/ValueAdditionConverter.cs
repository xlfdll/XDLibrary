using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Xlfdll.Windows.Presentation
{
    public class ValueAdditionConverter : MarkupExtension, IValueConverter
    {
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return (Double)value + Double.Parse(parameter.ToString());
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}