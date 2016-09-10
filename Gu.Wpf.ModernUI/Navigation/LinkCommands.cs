namespace Gu.Wpf.ModernUI.Navigation
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Properties;

    /// <summary>
    /// The routed link commands.
    /// This should only be used by implementors of ILink
    /// </summary>
    public static class LinkCommands
    {
        /// <summary>
        /// Gets the navigate link routed command.
        /// </summary>
        public static RoutedUICommand NavigateLink { get; } = new RoutedUICommand(Resources.NavigateLink, "NavigateLink", typeof(LinkCommands));

        /// <summary>
        /// Must be a nicer way to write this .
        /// Maybe the worst example of partial application ever.
        /// </summary>
        /// <returns></returns>
        internal static CommandBinding CreateNavigateLinkCommandBinding(INavigator navigator)
        {
            return new CommandBinding(
                NavigateLink,
                (o, e) => OnNavigateLink(navigator, o as UIElement, e),
                (o, e) => OnCanNavigateLink(navigator, o as UIElement, e));
        }

        internal static void OnCanNavigateLink(INavigator navigator, UIElement sender, CanExecuteRoutedEventArgs e)
        {
            if (sender == null)
            {
                return; // throw better here?
            }

            var link = e.OriginalSource as ILink;
            if (link == null)
            {
                return; // throw better here?
            }

            var linkNavigator = GetLinkNavigator(e, navigator);
            linkNavigator?.CanNavigate(navigator, link, e); // putting the ugly stuff behind interface so it is extensible
        }

        internal static void OnNavigateLink(INavigator navigator, UIElement sender, ExecutedRoutedEventArgs e)
        {
            if (sender == null)
            {
                return; // throw better here?
            }

            if (e.Handled)
            {
                return; // prevent SO when we raise Execute
            }

            var link = e.OriginalSource as ILink;
            if (link == null)
            {
                return; // throw better here?
            }

            var linkNavigator = GetLinkNavigator(e, navigator);
            linkNavigator?.Navigate(navigator, link, e);

        }

        private static Uri AsUri(this object parameter)
        {
            var uri = parameter as Uri;
            if (uri != null)
            {
                return uri;
            }

            var link = parameter as Link;
            return link?.Source;
        }

        private static ILinkNavigator GetLinkNavigator(RoutedEventArgs e, INavigator navigator)
        {
            var link = e.OriginalSource as ILink;
            if (link != null)
            {
                if (link.LinkNavigator != null)
                {
                    return link.LinkNavigator;
                }

                var commandTarget = link.CommandTarget as DependencyObject;
                var linkNavigator = commandTarget?.GetLinkNavigator();
                if (linkNavigator != null)
                {
                    return linkNavigator;
                }
            }

            return navigator.LinkNavigator;
        }
    }
}
