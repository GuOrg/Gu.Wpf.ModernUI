namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// The hyperlink navigator contract.
    /// </summary>
    public interface ILinkNavigator
    {
        /// <summary>
        /// Gets or sets the navigable commands.
        /// </summary>
        CommandDictionary Commands { get; }

        /// <summary>
        /// Checks if navigation can be performed to the e
        /// </summary>
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        /// <returns>If the e can be navigated to</returns>
        bool CanNavigate(ModernFrame target, Uri uri);

        /// <summary>
        /// This is the heart of the navigation and links.
        /// - Synchronizes SelectedSource, SelectedLink & ContentSource on INavigator
        /// - Updates IsNavigatedTo & CanNavigate on ILink
        /// Side effect fest
        /// </summary>
        /// <param name="navigator"></param>
        /// <param name="link"></param>
        /// <param name="e"></param>
        void CanNavigate(INavigator navigator, ILink link, CanExecuteRoutedEventArgs e);

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        void Navigate(ModernFrame target, Uri uri);

        void Navigate(INavigator navigator, ILink link, ExecutedRoutedEventArgs executedRoutedEventArgs);
    }
}
