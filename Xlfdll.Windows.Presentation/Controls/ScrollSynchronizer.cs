// Derived from https://www.codeproject.com/Articles/39244/Scroll-Synchronization

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Xlfdll.Windows.Presentation.Controls
{
    public class ScrollSynchronizer : DependencyObject
    {
        /// <summary>
        /// Identifies the attached property ScrollGroup
        /// </summary>
        public static readonly DependencyProperty ScrollGroupProperty = DependencyProperty.RegisterAttached
            ("ScrollGroup", typeof(String), typeof(ScrollSynchronizer),
            new PropertyMetadata(new PropertyChangedCallback(OnScrollGroupChanged)));

        /// <summary>
        /// List of all registered scroll viewers.
        /// </summary>
        private static Dictionary<ScrollViewer, String> scrollViewers = new Dictionary<ScrollViewer, String>();
        /// <summary>
        /// Contains the latest horizontal scroll offset for each scroll group.
        /// </summary>
        private static Dictionary<String, Double> horizontalScrollOffsets = new Dictionary<String, Double>();
        /// <summary>
        /// Contains the latest vertical scroll offset for each scroll group.
        /// </summary>
        private static Dictionary<String, Double> verticalScrollOffsets = new Dictionary<String, Double>();

        /// <summary>
        /// Sets the value of the attached property ScrollGroup.
        /// </summary>
        /// <param name="obj">Object on which the property should be applied.</param>
        /// <param name="scrollGroup">Value of the property.</param>
        public static void SetScrollGroup(DependencyObject obj, String scrollGroup)
        {
            obj.SetValue(ScrollGroupProperty, scrollGroup);
        }

        /// <summary>
        /// Gets the value of the attached property ScrollGroup.
        /// </summary>
        /// <param name="obj">Object for which the property should be read.</param>
        /// <returns>Value of the property StartTime</returns>
        public static String GetScrollGroup(DependencyObject obj)
        {
            return (String)obj.GetValue(ScrollGroupProperty);
        }

        /// <summary>
        /// Occurs, when the ScrollGroupProperty has changed.
        /// </summary>
        /// <param name="d">The DependencyObject on which the property has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes to the effective value of this property.</param>
        private static void OnScrollGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer scrollViewer = d as ScrollViewer;

            if (scrollViewer != null)
            {
                if (!String.IsNullOrEmpty((String)e.OldValue))
                {
                    // Remove scrollviewer
                    if (scrollViewers.ContainsKey(scrollViewer))
                    {
                        scrollViewer.ScrollChanged -= new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);

                        scrollViewers.Remove(scrollViewer);
                    }
                }

                if (!String.IsNullOrEmpty((String)e.NewValue))
                {
                    // If group already exists, set scrollposition of new scrollviewer to the scrollposition of the group
                    if (horizontalScrollOffsets.Keys.Contains((String)e.NewValue))
                    {
                        scrollViewer.ScrollToHorizontalOffset(horizontalScrollOffsets[(String)e.NewValue]);
                    }
                    else
                    {
                        horizontalScrollOffsets.Add((String)e.NewValue, scrollViewer.HorizontalOffset);
                    }

                    if (verticalScrollOffsets.Keys.Contains((String)e.NewValue))
                    {
                        scrollViewer.ScrollToVerticalOffset(verticalScrollOffsets[(String)e.NewValue]);
                    }
                    else
                    {
                        verticalScrollOffsets.Add((String)e.NewValue, scrollViewer.VerticalOffset);
                    }

                    // Add scrollviewer
                    scrollViewers.Add(scrollViewer, (String)e.NewValue);

                    scrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollViewer_ScrollChanged);
                }
            }
        }

        /// <summary>
        /// Occurs, when the scroll offset of one scrollviewer has changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">EventArgs of the event.</param>
        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalChange != 0 || e.HorizontalChange != 0)
            {
                ScrollViewer changedScrollViewer = sender as ScrollViewer;

                Scroll(changedScrollViewer);
            }
        }

        /// <summary>
        /// Scrolls all scroll viewers of a group to the position of the selected scroll viewer.
        /// </summary>
        /// <param name="changedScrollViewer">Sroll viewer, that specifies the current position of the group.</param>
        private static void Scroll(ScrollViewer changedScrollViewer)
        {
            String group = scrollViewers[changedScrollViewer];

            verticalScrollOffsets[group] = changedScrollViewer.VerticalOffset;
            horizontalScrollOffsets[group] = changedScrollViewer.HorizontalOffset;

            foreach (var scrollViewer in scrollViewers.Where(
                (s) => s.Value == group && s.Key != changedScrollViewer))
            {
                if (scrollViewer.Key.VerticalOffset != changedScrollViewer.VerticalOffset)
                {
                    scrollViewer.Key.ScrollToVerticalOffset(changedScrollViewer.VerticalOffset);
                }

                if (scrollViewer.Key.HorizontalOffset != changedScrollViewer.HorizontalOffset)
                {
                    scrollViewer.Key.ScrollToHorizontalOffset(changedScrollViewer.HorizontalOffset);
                }
            }
        }
    }
}