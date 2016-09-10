namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for InstanceCountView.xaml
    /// </summary>
    public partial class InstanceCountView : UserControl
    {
        private static int count;
        public InstanceCountView()
        {
            this.InitializeComponent();
            count++;
        }

        public int Count => count;
    }
}
