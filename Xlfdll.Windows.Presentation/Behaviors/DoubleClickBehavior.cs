using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Xlfdll.Windows.Presentation
{
    public static class DoubleClickBehavior
    {
        public static ICommand GetCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static Object GetCommandParameter(DependencyObject element)
        {
            return (Object)element.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(DependencyObject element, Object value)
        {
            element.SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickBehavior),
                new PropertyMetadata(default(ICommand), OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty
            = DependencyProperty.RegisterAttached("CommandParameter", typeof(Object), typeof(DoubleClickBehavior),
                new PropertyMetadata(default(Object)));

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control control = sender as Control;

            if (control == null)
            {
                throw new InvalidOperationException($"This property can only be attached to {nameof(Control)}");
            }

            control.MouseDoubleClick -= DoubleClickBehavior.OnDoubleClick;

            if (GetCommand(control) != null)
            {
                control.MouseDoubleClick += DoubleClickBehavior.OnDoubleClick;
            }
        }

        private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = sender as DependencyObject;

            if (dependencyObject != null)
            {
                ICommand command = DoubleClickBehavior.GetCommand(dependencyObject);

                if (command != null)
                {
                    Object parameter = DoubleClickBehavior.GetCommandParameter(dependencyObject);

                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                }
            }
        }
    }
}