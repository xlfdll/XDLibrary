using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Xlfdll.Core.Diagnostics;

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

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, ImageSource aboutImageSource)
            : this(ownerWindow, assemblyMetadata)
        {
            AboutImage.Source = aboutImageSource;
        }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, Uri aboutImageUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(aboutImageUri)) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, String aboutImagePath)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(new Uri(aboutImagePath))) { }

        public AboutWindow(Window ownerWindow, AssemblyMetadata assemblyMetadata, ApplicationPackUri aboutImagePackUri)
            : this(ownerWindow, assemblyMetadata, new BitmapImage(aboutImagePackUri)) { }
    }
}