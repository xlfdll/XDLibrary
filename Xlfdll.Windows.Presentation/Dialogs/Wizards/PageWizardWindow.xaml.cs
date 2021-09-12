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
                this.CanGoBack = PageWizardWindow.TrueFunction;
                this.CanGoNext = PageWizardWindow.TrueFunction;
                this.CanCancel = PageWizardWindow.TrueFunction;
                this.BeforeGoBack = PageWizardWindow.TrueFunction;
                this.BeforeGoNext = PageWizardWindow.TrueFunction;
                this.BeforeCancel = PageWizardWindow.TrueFunction;

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
                    if (this.BeforeGoBack())
                    {
                        wizard.SelectedPageIndex--;
                    }
                }
            },
            (window) =>
            {
                if (window is PageWizardWindow wizard)
                {
                    return wizard.SelectedPageIndex > 0 && wizard.CanGoBack();
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
                    if (this.BeforeGoNext())
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
                    return wizard.CanGoNext();
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
                    if (wizard.BeforeCancel())
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
                    return wizard.CanCancel();
                }

                return false;
            }
        );

        #endregion

        #region Condition Methods

        public Func<Boolean> CanGoBack { get; set; }
            = PageWizardWindow.TrueFunction;
        public Func<Boolean> CanGoNext { get; set; }
            = PageWizardWindow.TrueFunction;
        public Func<Boolean> CanCancel { get; set; }
            = PageWizardWindow.TrueFunction;
        public Func<Boolean> BeforeGoBack { get; set; }
            = PageWizardWindow.TrueFunction;
        public Func<Boolean> BeforeGoNext { get; set; }
            = PageWizardWindow.TrueFunction;
        public Func<Boolean> BeforeCancel { get; set; }
            = PageWizardWindow.TrueFunction;

        public static readonly Func<Boolean> TrueFunction
            = () => { return true; };

        #endregion
    }
}