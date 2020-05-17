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

        public static Boolean GetIsInputViewIgnored(DependencyObject element)
        {
            return (Boolean)element.GetValue(IsInputViewIgnoredProperty);
        }

        public static void SetIsInputViewIgnored(DependencyObject element, Boolean value)
        {
            element.SetValue(IsInputViewIgnoredProperty, value);
        }

        public static readonly DependencyProperty CommandProperty
            = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(DoubleClickBehavior),
                new PropertyMetadata(default(ICommand), OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty
            = DependencyProperty.RegisterAttached("CommandParameter", typeof(Object), typeof(DoubleClickBehavior),
                new PropertyMetadata(default(Object)));

        public static readonly DependencyProperty IsInputViewIgnoredProperty
            = DependencyProperty.RegisterAttached("IsInputViewIgnored", typeof(Boolean), typeof(DoubleClickBehavior),
                new PropertyMetadata(true));

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Control control = sender as Control;

            if (control != null)
            {
                control.MouseDoubleClick -= DoubleClickBehavior.OnDoubleClick;

                if (GetCommand(control) != null)
                {
                    control.MouseDoubleClick += DoubleClickBehavior.OnDoubleClick;
                }
            }
            else
            {
                throw new InvalidOperationException($"This property can only be attached to {nameof(Control)}");
            }
        }

        private static void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = sender as DependencyObject;

            if (DoubleClickBehavior.GetIsInputViewIgnored(dependencyObject))
            {
                if (e.OriginalSource.GetType().Name.Contains("TextBox"))
                {
                    return;
                }
            }

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