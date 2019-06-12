using System;
using System.Windows;

namespace Xlfdll.Windows.Presentation
{
    public static class DependencyPropertyHelper
    {
        public static DependencyProperty Create<PropertyType, OwnerType>(String propertyName, PropertyType defaultValue = default(PropertyType))
        {
            return DependencyProperty.Register(propertyName,
                typeof(PropertyType), typeof(OwnerType),
                new FrameworkPropertyMetadata(defaultValue));
        }
    }
}