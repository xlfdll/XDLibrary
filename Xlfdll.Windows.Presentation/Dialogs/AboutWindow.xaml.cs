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
            // Could not use data binding + dependency property here
            // Due to the horrible changes in .NET Framework 4.5+
            // Reference: https://stackoverflow.com/questions/21788855/binding-an-image-in-wpf-mvvm
            LogoImage.Source = logoImageSource;
        }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, Uri logoImageUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(logoImageUri)) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, String logoImagePath)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(new Uri(logoImagePath))) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, ApplicationPackUri logoImagePackUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(logoImagePackUri)) { }
    }
}