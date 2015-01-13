namespace Gu.ModernUI.Windows.Navigation
{
    using System;

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
        /// Checks if navigation can be performed to the link
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="current"></param>
        /// <param name="commandParameter">Used when the link is a command</param>
        /// <returns></returns>
        bool CanNavigate(Uri uri, Uri current = null, object commandParameter = null);

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="uri">The uri to navigate to.</param>
        /// <param name="frameNavigation"></param>
        /// <param name="commandParameter">Used when the link is a command</param>
        void Navigate(Uri uri, Action<Uri> frameNavigation, object commandParameter = null);
    }
}
