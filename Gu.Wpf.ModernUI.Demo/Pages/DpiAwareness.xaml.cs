namespace Gu.Wpf.ModernUI.Demo.Pages
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for DpiAwareness.xaml
    /// </summary>
    public partial class DpiAwareness : UserControl
    {
        public DpiAwareness()
        {
            this.InitializeComponent();

            this.DataContext = new DpiAwarenessViewModel();
        }
    }
}
