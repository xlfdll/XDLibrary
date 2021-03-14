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
    public partial class AboutWindow
    {
        private AboutWindow()
        {
            InitializeComponent();
        }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            String externalSourceText = null)
            : this()
        {
            this.Owner = ownerWindow;
            this.Icon = ownerWindow.Icon;

            MetadataGrid.DataContext = assemblyMetadata;

            this.ExternalSources = externalSourceText;
        }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            ImageSource logoImageSource,
            String externalSourceText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  externalSourceText)
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
            String externalSourceText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  new BitmapImage(logoImageUri),
                  externalSourceText)
        { }

        public AboutWindow
            (Window ownerWindow,
            AssemblyMetadata assemblyMetadata,
            String logoImagePath,
            String externalSourceText = null)
            : this(ownerWindow,
                  assemblyMetadata,
                  new BitmapImage(new Uri(logoImagePath)),
                  externalSourceText)
        { }

        #region Dependency Properties

        public String ExternalSources
        {
            get => (String)this.GetValue(ExternalSourcesProperty);
            private set => this.SetValue(ExternalSourcesProperty, value);
        }

        private static readonly DependencyProperty ExternalSourcesProperty = DependencyProperty.Register
            ("ExternalSources", typeof(String), typeof(AboutWindow), new PropertyMetadata(null));

        #endregion
    }
}