namespace Gu.ModernUI.Windows.Navigation
{
    using System.Windows.Input;

    /// <summary>
    /// The routed link commands.
    /// </summary>
    public static class LinkCommands
    {
        private static readonly RoutedUICommand navigateLink = new RoutedUICommand(Properties.Resources.NavigateLink, "NavigateLink", typeof(LinkCommands));

        /// <summary>
        /// Gets the navigate link routed command.
        /// </summary>
        public static RoutedUICommand NavigateLink
        {
            get { return navigateLink; }
        }
    }
}
