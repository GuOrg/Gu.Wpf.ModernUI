namespace Gu.Wpf.ModernUI
{
    using Navigation;

    public interface ICancelNavigation
    {
        /// <summary>
        /// Called just before this instance is no longer the active content in a frame.
        /// </summary>
        /// <param name="e">An object that contains the navigation data.</param>
        /// <remarks>The method is also invoked when parent frames are about to navigate.</remarks>
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
    }
}