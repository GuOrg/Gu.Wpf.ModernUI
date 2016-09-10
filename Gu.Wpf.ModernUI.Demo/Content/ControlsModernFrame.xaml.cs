namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System.Globalization;
    using System.Windows.Controls;

    using Navigation;

    /// <summary>
    /// Interaction logic for ControlsModernFrame.xaml
    /// </summary>
    public partial class ControlsModernFrame : UserControl
    {
        private string eventLogMessage;

        public ControlsModernFrame()
        {
            this.InitializeComponent();

            this.TextEvents.Text = this.eventLogMessage;
        }

        private void LogMessage(string message, params object[] o)
        {
            message = string.Format(CultureInfo.CurrentUICulture, message, o);

            if (this.TextEvents == null) {
                this.eventLogMessage += message;
            }
            else {
                this.TextEvents.AppendText(message);
            }
        }

        private void Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
        {
            this.LogMessage("FragmentNavigation: {0}\r\n", e.Fragment);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.LogMessage("Navigated: [{0}] {1}\r\n", e.NavigationType, e.Source);
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.LogMessage("Navigating: [{0}] {1}\r\n", e.NavigationType, e.Source);
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            this.LogMessage("NavigationFailed: {0}\r\n", e.Error.Message);
        }
    }
}
