namespace Gu.Wpf.ModernUI.Demo.Content.ModernFrame
{
    using System.Globalization;
    using System.Windows.Controls;

    using Gu.ModernUi.Interfaces;

    using ModernUI;
    using Navigation;

    /// <summary>
    /// Interaction logic for CancelNavigateSample.xaml
    /// </summary>
    public partial class CancelNavigateSample : UserControl, INavigationView
    {
        public CancelNavigateSample()
        {
            this.InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            // display the current navigated fragment
            this.fragmentNav.BBCode = string.Format(CultureInfo.CurrentUICulture, "Current navigation fragment: '[b]{0}[/b]'", e.Fragment);
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            // clear fragment text
            this.fragmentNav.BBCode = null;
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            // ask user if navigating away is ok
            var frameDescription = e.IsParentFrameNavigating
                ? "A parent frame"
                : "This frame";

            // modern message dialog supports BBCode tags
            var question = string.Format(CultureInfo.CurrentUICulture, "[b]{0}[/b] is about to navigate to new content. Do you want to allow this?", frameDescription);

            if (DialogResult.No == ModernDialog.ShowMessage(question, "navigate", MessageBoxButtons.YesNo)) {
                e.Cancel = true;
            }
        }
    }
}
