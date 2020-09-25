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

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata)
            : this()
        {
            this.Owner = ownerWindow;
            this.Icon = ownerWindow.Icon;
            this.DataContext = assemblyMetadata;
        }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, ImageSource logoImageSource)
            : this(ownerWindow, assemblyMetadata)
        {
            this.Logo = logoImageSource;
        }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, Uri logoImageUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(logoImageUri)) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, String logoImagePath)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(new Uri(logoImagePath))) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, ApplicationPackUri logoImagePackUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(logoImagePackUri)) { }

        #region Dependency Properties

        public ImageSource Logo
        {
            get => (ImageSource)this.GetValue(LogoImageProperty);
            set => this.SetValue(LogoImageProperty, value);
        }

        public static readonly DependencyProperty LogoImageProperty
            = DependencyPropertyHelper.Create<ImageSource, AboutWindow>("LogoImage", null);

        #endregion
    }
}