using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Xlfdll.Windows.Presentation.Dialogs
{
    /// <summary>
    /// WizardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class WizardWindow
    {
        private WizardWindow()
        {
            InitializeComponent();
        }

        public WizardWindow(IEnumerable<Uri> pageURIs)
            : this()
        {
            if (pageURIs == null || pageURIs.Count() == 0)
            {
                throw new ArgumentException("Page URI list cannot be null or empty.");
            }

            this.PageURIs = new ObservableCollection<Uri>(pageURIs);
            this.SelectedPageIndex = 0;
        }

        public WizardWindow(IEnumerable<String> pageURIs)
            : this(pageURIs.Select(p => new Uri(p))) { }

        private void WizardWindow_ContentRendered(object sender, EventArgs e)
        {
            this.CenterWindowToScreen();
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
                this.CanGoBack = WizardWindow.TrueFunction;
                this.CanGoNext = WizardWindow.TrueFunction;
                this.CanCancel = WizardWindow.TrueFunction;
                this.BeforeGoBack = WizardWindow.TrueFunction;
                this.BeforeGoNext = WizardWindow.TrueFunction;
                this.BeforeCancel = WizardWindow.TrueFunction;

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
            ("SelectedPageIndex", typeof(Int32), typeof(WizardWindow), new PropertyMetadata(-1));
        public static DependencyProperty SelectedPageURIProperty = DependencyProperty.Register
            ("SelectedPageURI", typeof(Uri), typeof(WizardWindow), new PropertyMetadata(null));
        public static DependencyProperty IsLastPageProperty = DependencyProperty.Register
            ("IsLastPage", typeof(Boolean), typeof(WizardWindow), new PropertyMetadata(false));

        #endregion

        #region Commands

        // All commands need to use Window type
        // as XAML data binding cannot convert it to the derived one

        public RelayCommand<Window> BackCommand => new RelayCommand<Window>
        (
            (window) =>
            {
                if (window is WizardWindow wizard)
                {
                    if (this.BeforeGoBack())
                    {
                        wizard.SelectedPageIndex--;
                    }
                }
            },
            (window) =>
            {
                if (window is WizardWindow wizard)
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
                if (window is WizardWindow wizard)
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
                if (window is WizardWindow wizard)
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
                if (window is WizardWindow wizard)
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
                if (window is WizardWindow wizard)
                {
                    return wizard.CanCancel();
                }

                return false;
            }
        );

        #endregion

        #region Condition Methods

        public Func<Boolean> CanGoBack { get; set; }
            = WizardWindow.TrueFunction;
        public Func<Boolean> CanGoNext { get; set; }
            = WizardWindow.TrueFunction;
        public Func<Boolean> CanCancel { get; set; }
            = WizardWindow.TrueFunction;
        public Func<Boolean> BeforeGoBack { get; set; }
            = WizardWindow.TrueFunction;
        public Func<Boolean> BeforeGoNext { get; set; }
            = WizardWindow.TrueFunction;
        public Func<Boolean> BeforeCancel { get; set; }
            = WizardWindow.TrueFunction;

        public static readonly Func<Boolean> TrueFunction
            = () => { return true; };

        #endregion
    }
}