namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for LoremIpsum.xaml
    /// </summary>
    public partial class LoremIpsum : UserControl
    {
        public LoremIpsum(string s)
        {
            this.InitializeComponent();
            this.Header.Text += s;
        }
    }
}
