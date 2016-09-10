namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SettingsAppearance.xaml
    /// </summary>
    public partial class SettingsAppearance : UserControl
    {
        public SettingsAppearance()
        {
            this.InitializeComponent();

            // a simple view model for appearance configuration
            this.DataContext = new SettingsAppearanceViewModel();
        }
    }
}
