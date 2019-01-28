using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Xlfdll.Windows.Presentation
{
    public static class ListBoxBehaviors
    {
        #region ScrollOnNewItem Attached Property

        public static readonly DependencyProperty ScrollOnNewItemProperty
            = DependencyProperty.RegisterAttached
                ("ScrollOnNewItem",
                typeof(Boolean),
                typeof(ListBoxBehaviors),
                new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

        public static Boolean GetScrollOnNewItem(DependencyObject dependencyObject)
        {
            return (Boolean)dependencyObject.GetValue(ScrollOnNewItemProperty);
        }

        public static void SetScrollOnNewItem(DependencyObject dependencyObject, Boolean value)
        {
            dependencyObject.SetValue(ScrollOnNewItemProperty, value);
        }

        private static void OnScrollOnNewItemChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ListBox listBox = dependencyObject as ListBox;

            if (listBox != null)
            {
                Boolean oldValue = (Boolean)e.OldValue;
                Boolean newValue = (Boolean)e.NewValue;

                if (newValue != oldValue)
                {
                    PropertyDescriptor itemsSourcePropertyDescriptor = TypeDescriptor.GetProperties(listBox)["ItemsSource"];

                    if (newValue)
                    {
                        listBox.Loaded += ListBox_Loaded;
                        listBox.Unloaded += ListBox_Unloaded;

                        itemsSourcePropertyDescriptor.AddValueChanged(listBox, ListBox_ItemsSourceChanged);
                    }
                    else
                    {
                        listBox.Loaded -= ListBox_Loaded;
                        listBox.Unloaded -= ListBox_Unloaded;

                        if (ListBoxBehaviors.Associations.ContainsKey(listBox))
                        {
                            ListBoxBehaviors.Associations[listBox].Dispose();
                        }

                        itemsSourcePropertyDescriptor.RemoveValueChanged(listBox, ListBox_ItemsSourceChanged);
                    }
                }
            }
        }

        private static void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null)
            {
                INotifyCollectionChanged items = listBox.Items as INotifyCollectionChanged;

                if (items != null)
                {
                    listBox.Loaded -= ListBox_Loaded;
                    ListBoxBehaviors.Associations[listBox] = new Capture(listBox);
                }
            }
        }

        private static void ListBox_Unloaded(object sender, RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null)
            {
                if (ListBoxBehaviors.Associations.ContainsKey(listBox))
                {
                    ListBoxBehaviors.Associations[listBox].Dispose();
                }

                listBox.Unloaded -= ListBox_Unloaded;
            }
        }

        private static void ListBox_ItemsSourceChanged(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;

            if (listBox != null)
            {
                if (ListBoxBehaviors.Associations.ContainsKey(listBox))
                {
                    ListBoxBehaviors.Associations[listBox].Dispose();
                }

                ListBoxBehaviors.Associations[listBox] = new Capture(listBox);
            }
        }

        private static Dictionary<ListBox, Capture> Associations { get; } =
               new Dictionary<ListBox, Capture>();

        private class Capture : IDisposable
        {
            public Capture(ListBox listBox)
            {
                this.ListBox = listBox;
                this.Items = listBox.ItemsSource as INotifyCollectionChanged;

                if (this.Items != null)
                {
                    this.Items.CollectionChanged += Items_CollectionChanged;
                }
            }

            private ListBox ListBox { get; }
            private INotifyCollectionChanged Items { get; }

            private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    ListBoxAutomationPeer svAutomation = ScrollViewerAutomationPeer.CreatePeerForElement(this.ListBox) as ListBoxAutomationPeer;
                    IScrollProvider scrollInterface = svAutomation.GetPattern(PatternInterface.Scroll) as IScrollProvider;
                    ScrollAmount scrollVertical = ScrollAmount.LargeIncrement;
                    ScrollAmount scrollHorizontal = ScrollAmount.NoAmount;

                    // If the vertical scroller is not available, the operation cannot be performed, which will raise an exception. 
                    if (scrollInterface != null && scrollInterface.VerticallyScrollable)
                    {
                        scrollInterface.Scroll(scrollHorizontal, scrollVertical);
                    }
                }
            }

            #region IDisposable Members

            public void Dispose()
            {
                if (this.Items != null)
                {
                    this.Items.CollectionChanged -= Items_CollectionChanged;
                }
            }

            #endregion
        }

        #endregion
    }
}