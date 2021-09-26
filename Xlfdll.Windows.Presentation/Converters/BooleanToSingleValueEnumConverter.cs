using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Xlfdll.Windows.Presentation.Converters
{
    public class BooleanToSingleValueEnumConverter : MarkupExtension, IValueConverter
    {
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter.ToString();

            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            if (!Enum.IsDefined(value.GetType(), value))
            {
                return DependencyProperty.UnsetValue;
            }

            Object parameterValue = Enum.Parse(value.GetType(), parameterString);

            return parameterValue.Equals(value);
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            String parameterString = parameter.ToString();

            if (parameterString == null)
            {
                return DependencyProperty.UnsetValue;
            }

            return Enum.Parse(targetType, parameterString);
        }
    }
}