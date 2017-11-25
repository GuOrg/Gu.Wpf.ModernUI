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
        public static readonly DependencyProperty Uri1Property = DependencyProperty.Register(nameof(Uri1), typeof(Uri), typeof(DebugBindingSourceView), new PropertyMetadata(default(Uri)));

        public static readonly DependencyProperty Uri2Property = DependencyProperty.Register(nameof(Uri2), typeof(Uri), typeof(DebugBindingSourceView), new PropertyMetadata(default(Uri)));

        public DebugBindingSourceView()
        {
            this.InitializeComponent();
        }

        public Uri Uri1
        {
            get => (Uri)this.GetValue(Uri1Property);
            set => this.SetValue(Uri1Property, value);
        }

        public Uri Uri2
        {
            get => (Uri)this.GetValue(Uri2Property);
            set => this.SetValue(Uri2Property, value);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.SetCurrentValue(Uri1Property, new Uri("/Content/DebugBindingSourceView.xaml", UriKind.Relative));
            this.SetCurrentValue(Uri2Property, new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }
    }
}
