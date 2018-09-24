using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

using Xlfdll.Collections;

namespace Xlfdll.Windows.Presentation
{
    public static class ControlExtensions
    {
        public static void PerformClick(this Button button)
        {
            button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        public static void ShowDropMenu(this Button button)
        {
            if (button.ContextMenu != null)
            {
                button.ContextMenu.IsEnabled = true;
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.Placement = PlacementMode.Bottom;
                button.ContextMenu.IsOpen = true;
            }
        }

        public static void UniformColumnWidth(this GridView gridView, Double totalWidth)
        {
            ControlExtensions.UniformColumnWidth(gridView, totalWidth, new List<GridViewColumn>());
        }

        public static void UniformColumnWidth(this GridView gridView, Double totalWidth, ICollection<GridViewColumn> skippedGridViewColumns)
        {
            gridView.Dispatcher.BeginInvoke
            (
                new Action(() =>
                {
                    skippedGridViewColumns.ForEach
                    (
                        (column) =>
                        {
                            column.Width = Double.NaN;
                        }
                    );

                    Double columnWidth = (totalWidth - skippedGridViewColumns.Sum(c => c.ActualWidth) - 32) / (gridView.Columns.Count - skippedGridViewColumns.Count);

                    gridView.Columns.AsParallel().ForEach
                    (
                        (column) =>
                        {
                            if (skippedGridViewColumns == null || !skippedGridViewColumns.Contains(column))
                            {
                                column.Width = columnWidth;
                            }
                        }
                    );
                }),
                DispatcherPriority.Loaded // Execute after the DependencyObject is measured and rendered
            );
        }

        public static void AutoColumnWidth(this GridView gridView)
        {
            gridView.Columns.AsParallel().ForEach
            (
                (column) =>
                {
                    column.Width = Double.NaN;
                }
            );
        }

        public static T GetDescendant<T>(this Visual visual, String name) where T : Visual
        {
            if (visual.GetType() == typeof(T))
            {
                FrameworkElement frameworkElement = visual as FrameworkElement;

                return ((frameworkElement != null && frameworkElement.Name == name) ? frameworkElement : null) as T;
            }
            else
            {
                Visual result = null;

                if (visual is FrameworkElement)
                {
                    (visual as FrameworkElement).ApplyTemplate();
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
                {
                    Visual v = VisualTreeHelper.GetChild(visual, i) as Visual;

                    result = ControlExtensions.GetDescendant<T>(v, name);

                    if (result != null)
                    {
                        break;
                    }
                }

                return result as T;
            }
        }

        public static T GetParent<T>(this UIElement element) where T : UIElement
        {
            UIElement parent = element;

            while (parent != null)
            {
                T correctlyTyped = parent as T;

                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }

            return null;
        }
    }
}