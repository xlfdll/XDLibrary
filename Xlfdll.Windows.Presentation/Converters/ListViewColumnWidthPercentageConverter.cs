using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Xlfdll.Windows.Presentation
{
    public class ListViewColumnWidthPercentageConverter : MarkupExtension, IValueConverter
    {
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            ListView listView = value as ListView;
            GridView gridView = listView.View as GridView;
            Int32 minWidth = 0;
            Boolean widthIsPercentage = parameter != null && !int.TryParse(parameter.ToString(), out minWidth);

            if (widthIsPercentage)
            {
                String widthParam = parameter.ToString();
                Double percentage = Double.Parse(widthParam.Substring(0, widthParam.Length - 1));

                return listView.ActualWidth * percentage;
            }
            else
            {
                Double totalActualWidth = gridView.Columns.Sum(c => c.ActualWidth)
                    - gridView.Columns[gridView.Columns.Count - 1].ActualWidth;

                for (int i = 0; i < gridView.Columns.Count - 1; i++)
                {
                    totalActualWidth += gridView.Columns[i].ActualWidth;
                }

                Double remainingWidth = listView.ActualWidth - totalActualWidth;

                return (remainingWidth > minWidth) ? remainingWidth : minWidth;
            }
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}