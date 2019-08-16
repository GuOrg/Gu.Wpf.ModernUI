namespace Gu.Wpf.ModernUI
{
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Defines the optional contract for content loaded in a ModernFrame.
    /// </summary>
    public interface INavigationView : ICancelNavigation
    {
        /// <summary>
        /// Called when navigation to a content fragment begins.
        /// </summary>
        /// <param name="e">An object that contains the navigation data.</param>
        void OnFragmentNavigation(FragmentNavigationEventArgs e);

        /// <summary>
        /// Called when this instance is no longer the active content in a frame.
        /// </summary>
        /// <param name="e">An object that contains the navigation data.</param>
        void OnNavigatedFrom(NavigationEventArgs e);

        /// <summary>
        /// Called when a this instance becomes the active content in a frame.
        /// </summary>
        /// <param name="e">An object that contains the navigation data.</param>
        void OnNavigatedTo(NavigationEventArgs e);
    }
}
