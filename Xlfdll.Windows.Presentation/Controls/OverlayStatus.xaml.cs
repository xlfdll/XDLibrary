using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xlfdll.Windows.Presentation.Controls
{
    /// <summary>
    /// OverlayStatus.xaml の相互作用ロジック
    /// </summary>
    public partial class OverlayStatus : UserControl
    {
        public OverlayStatus()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public Color OverlayBackgroundColor
        {
            get { return (Color)this.GetValue(OverlayBackgroundColorProperty); }
            set { this.SetValue(OverlayBackgroundColorProperty, value); }
        }

        public Double OverlayOpacity
        {
            get { return (Double)this.GetValue(OverlayOpacityProperty); }
            set { this.SetValue(OverlayOpacityProperty, value); }
        }

        public String OverlayText
        {
            get { return (String)this.GetValue(OverlayTextProperty); }
            set { this.SetValue(OverlayTextProperty, value); }
        }

        public Double OverlayTextSize
        {
            get { return (Double)this.GetValue(OverlayTextSizeProperty); }
            set { this.SetValue(OverlayTextSizeProperty, value); }
        }

        public Brush OverlayTextForeground
        {
            get { return (Brush)this.GetValue(OverlayTextForegroundProperty); }
            set { this.SetValue(OverlayTextForegroundProperty, value); }
        }

        public static readonly DependencyProperty OverlayBackgroundColorProperty = DependencyProperty.Register(
            "OverlayBackgroundColor",
            typeof(Color),
            typeof(OverlayStatus),
            new FrameworkPropertyMetadata(Colors.White));

        public static readonly DependencyProperty OverlayOpacityProperty = DependencyProperty.Register(
            "OverlayOpacity",
            typeof(Double),
            typeof(OverlayStatus),
            new FrameworkPropertyMetadata(0.5));

        public static readonly DependencyProperty OverlayTextProperty = DependencyProperty.Register(
            "OverlayText",
            typeof(String),
            typeof(OverlayStatus),
            new FrameworkPropertyMetadata(String.Empty));

        public static readonly DependencyProperty OverlayTextSizeProperty = DependencyProperty.Register(
            "OverlayTextSize",
            typeof(Double),
            typeof(OverlayStatus),
            new FrameworkPropertyMetadata(24.0));

        public static readonly DependencyProperty OverlayTextForegroundProperty = DependencyProperty.Register(
            "OverlayTextForeground",
            typeof(Brush),
            typeof(OverlayStatus),
            new FrameworkPropertyMetadata(Brushes.Black));

        #endregion
    }
}