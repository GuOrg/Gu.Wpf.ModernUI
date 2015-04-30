namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SlowPage.xaml
    /// </summary>
    public partial class SlowPage : UserControl
    {
        public SlowPage()
        {
            var sw = Stopwatch.StartNew();
            InitializeComponent();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Textbox.Text = string.Format("This page took {0} ms to load", sw.ElapsedMilliseconds);
        }
    }
}
