namespace Gu.ModernUI.Demo.Content
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SettingsAppearance.xaml
    /// </summary>
    public partial class SettingsAppearance : UserControl
    {
        public SettingsAppearance()
        {
            InitializeComponent();

            // a simple view model for appearance configuration
            this.DataContext = new SettingsAppearanceViewModel();
        }
    }
}
