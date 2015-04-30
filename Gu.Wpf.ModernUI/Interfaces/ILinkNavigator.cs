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
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        /// <returns>If the uri can be navigated to</returns>
        bool CanNavigate(ModernFrame target, Uri uri);

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        void Navigate(ModernFrame target, Uri uri);
    }
}
