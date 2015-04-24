namespace Gu.Wpf.ModernUI
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
        /// Checks if navigation can be performed to the uri
        /// </summary>
        /// <param name="uri">The uri to check if can be navigated to</param>
        /// <param name="current">The current uri of the source</param>
        /// <param name="commandParameter">Used when the link is a command</param>
        /// <returns>If the uri can be navigated to</returns>
        bool CanNavigate(Uri uri, Uri current = null, object commandParameter = null);

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="uri">The uri to navigate to.</param>
        /// <param name="frameNavigation">The action to invoke if it is a frame navigation i.e the uri is something like /content/settings.caml</param>
        /// <param name="commandParameter">Used when the link is a command</param>
        void Navigate(Uri uri, Action<Uri> frameNavigation = null, object commandParameter = null);
    }
}
