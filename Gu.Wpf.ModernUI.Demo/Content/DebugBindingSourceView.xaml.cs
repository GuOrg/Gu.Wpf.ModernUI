namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DebugBindingSourceView.xaml
    /// </summary>
    public partial class DebugBindingSourceView : UserControl
    {
        public static readonly DependencyProperty Uri1Property = DependencyProperty.Register("Uri1", typeof(Uri), typeof(DebugBindingSourceView), new PropertyMetadata(default(Uri)));

        public static readonly DependencyProperty Uri2Property = DependencyProperty.Register("Uri2", typeof(Uri), typeof(DebugBindingSourceView), new PropertyMetadata(default(Uri)));

        public DebugBindingSourceView()
        {
            InitializeComponent();
        }

        public Uri Uri1
        {
            get
            {
                return (Uri)GetValue(Uri1Property);
            }
            set
            {
                SetValue(Uri1Property, value);
            }
        }

        public Uri Uri2
        {
            get
            {
                return (Uri)GetValue(Uri2Property);
            }
            set
            {
                SetValue(Uri2Property, value);
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.Uri1 = new Uri("/Content/DebugBindingSourceView.xaml", UriKind.Relative);
            this.Uri2 = new Uri("/Pages/Settings.xaml", UriKind.Relative);
        }
    }
}
