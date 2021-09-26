using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace Xlfdll.Windows.Presentation.Dialogs
{
    /// <summary>
    /// WizardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class PageWizardWindow
    {
        private PageWizardWindow()
        {
            InitializeComponent();
        }

        public PageWizardWindow(IEnumerable<Uri> pageURIs)
            : this()
        {
            if (pageURIs == null || pageURIs.Count() == 0)
            {
                throw new ArgumentException("Page URI list cannot be null or empty.");
            }

            this.SetBinding(PageWizardWindow.BeforeGoBackProperty, "BeforeGoBack");
            this.SetBinding(PageWizardWindow.BeforeGoNextProperty, "BeforeGoNext");
            this.SetBinding(PageWizardWindow.BeforeCancelProperty, "BeforeCancel");
            this.SetBinding(PageWizardWindow.CanGoBackProperty, "CanGoBack");
            this.SetBinding(PageWizardWindow.CanGoNextProperty, "CanGoNext");
            this.SetBinding(PageWizardWindow.CanCancelProperty, "CanCancel");

            this.PageURIs = new ObservableCollection<Uri>(pageURIs);
            this.SelectedPageIndex = 0;
        }

        public PageWizardWindow(IEnumerable<String> pageURIs)
            : this(pageURIs.Select(p => new Uri(p))) { }

        private void WizardWindow_ContentRendered(object sender, EventArgs e)
        {
            this.CenterWindowToScreen();
        }

        // The LoadCompleted and DataContextChanged event handlers here are required to update Page's DataContext
        // Due to WPF content isolation, DataContext will not be passed through Frame to Pages automatically
        // (Frame-Page was originally designed to run in restricted environment like Internet Explorer)

        private void PageFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            PageFrame.UpdateContentContext();
        }

        private void PageFrame_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PageFrame.UpdateContentContext();
        }

        public ObservableCollection<Uri> PageURIs { get; }

        #region Dependency Properties

        public Int32 SelectedPageIndex
        {
            get
            {
                return (Int32)this.GetValue(SelectedPageIndexProperty);
            }
            private set
            {
                this.SetValue(SelectedPageIndexProperty, value);
                this.SetValue(SelectedPageURIProperty, this.PageURIs[value]);
                this.SetValue(IsLastPageProperty, value == this.PageURIs.Count - 1);
            }
        }

        public Uri SelectedPageURI
        {
            get => (Uri)this.GetValue(SelectedPageURIProperty);
        }

        public Boolean IsLastPage
        {
            get => (Boolean)this.GetValue(IsLastPageProperty);
        }

        public static DependencyProperty SelectedPageIndexProperty = DependencyProperty.Register
            ("SelectedPageIndex", typeof(Int32), typeof(PageWizardWindow), new PropertyMetadata(-1));
        public static DependencyProperty SelectedPageURIProperty = DependencyProperty.Register
            ("SelectedPageURI", typeof(Uri), typeof(PageWizardWindow), new PropertyMetadata(null));
        public static DependencyProperty IsLastPageProperty = DependencyProperty.Register
            ("IsLastPage", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(false));

        #region Wizard States

        public Boolean BeforeGoBack
        {
            get => (Boolean)this.GetValue(BeforeGoBackProperty);
        }
        public Boolean BeforeGoNext
        {
            get => (Boolean)this.GetValue(BeforeGoNextProperty);
        }
        public Boolean BeforeCancel
        {
            get => (Boolean)this.GetValue(BeforeCancelProperty);
        }
        public Boolean CanGoBack
        {
            get => (Boolean)this.GetValue(CanGoBackProperty);
        }
        public Boolean CanGoNext
        {
            get => (Boolean)this.GetValue(CanGoNextProperty);
        }
        public Boolean CanCancel
        {
            get => (Boolean)this.GetValue(CanCancelProperty);
        }

        public static DependencyProperty BeforeGoBackProperty = DependencyProperty.Register
            ("BeforeGoBack", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));
        public static DependencyProperty BeforeGoNextProperty = DependencyProperty.Register
            ("BeforeGoNext", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));
        public static DependencyProperty BeforeCancelProperty = DependencyProperty.Register
            ("BeforeCancel", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));
        public static DependencyProperty CanGoBackProperty = DependencyProperty.Register
            ("CanGoBack", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));
        public static DependencyProperty CanGoNextProperty = DependencyProperty.Register
            ("CanGoNext", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));
        public static DependencyProperty CanCancelProperty = DependencyProperty.Register
            ("CanCancel", typeof(Boolean), typeof(PageWizardWindow), new PropertyMetadata(true));

        #endregion

        #endregion

        #region Commands

        // All commands need to use Window type
        // as XAML data binding cannot convert it to the derived one

        public RelayCommand<Window> BackCommand => new RelayCommand<Window>
        (
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    if (this.BeforeGoBack)
                    {
                        wizard.SelectedPageIndex--;
                    }
                }
            },
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    return wizard.SelectedPageIndex > 0 && wizard.CanGoBack;
                }

                return false;
            }
        );

        public RelayCommand<Window> NextCommand => new RelayCommand<Window>
        (
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    if (this.BeforeGoNext)
                    {
                        if (!wizard.IsLastPage)
                        {
                            wizard.SelectedPageIndex++;
                        }
                        else
                        {
                            wizard.DialogResult = true;

                            wizard.Close();
                        }
                    }
                }
            },
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    return wizard.CanGoNext;
                }

                return false;
            }
        );

        public RelayCommand<Window> CancelCommand => new RelayCommand<Window>
        (
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    if (wizard.BeforeCancel)
                    {
                        wizard.DialogResult = false;

                        wizard.Close();
                    }
                }
            },
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    return wizard.CanCancel;
                }

                return false;
            }
        );

        #endregion
    }
}