using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Xlfdll.Diagnostics;

namespace Xlfdll.Windows.Presentation.Dialogs
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutWindow : Window
    {
        private AboutWindow()
        {
            InitializeComponent();
        }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            String licenseText = null)
            : this()
        {
            this.Owner = ownerWindow;
            this.Icon = ownerWindow.Icon;

            MetadataGrid.DataContext = assemblyMetadata;

            this.License = licenseText;
        }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            ImageSource logoImageSource,
            String licenseText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  licenseText)
        {
            // Could not use data binding + dependency property here
            // Due to the horrible changes in .NET Framework 4.5+
            // Reference: https://stackoverflow.com/questions/21788855/binding-an-image-in-wpf-mvvm
            LogoImage.Source = logoImageSource;
        }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            Uri logoImageUri,
            String licenseText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  new BitmapImage(logoImageUri),
                  licenseText)
        { }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            String logoImagePath,
            String licenseText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  new BitmapImage(new Uri(logoImagePath)),
                  licenseText)
        { }

        #region Dependency Properties

        public String License
        {
            get => (String)this.GetValue(LicenseProperty);
            private set => this.SetValue(LicenseProperty, value);
        }

        private static readonly DependencyProperty LicenseProperty = DependencyProperty.Register
            ("License", typeof(String), typeof(AboutWindow), new PropertyMetadata(null));

        #endregion
    }
}