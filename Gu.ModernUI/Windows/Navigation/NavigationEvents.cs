namespace Gu.ModernUI.Windows.Navigation
{
    using System;
    using Controls;

    /// <summary>
    /// Exposing navigation events. Watch out for memory leaks when subscribing.
    /// </summary>
    public static class NavigationEvents
    {
        /// <summary>
        /// Occurs when navigation to a content fragment begins.
        /// </summary>
        public static event EventHandler<FragmentNavigationEventArgs> FragmentNavigation;

        /// <summary>
        /// Occurs when a new navigation is requested.
        /// </summary>
        /// <remarks>
        /// The navigating event is also raised when a parent frame is navigating. This allows for cancelling parent navigation.
        /// </remarks>
        public static event EventHandler<NavigatingCancelEventArgs> Navigating;

        /// <summary>
        /// Occurs when navigation to new content has completed.
        /// </summary>
        public static event EventHandler<NavigationEventArgs> Navigated;

        /// <summary>
        /// Occurs when navigation has failed.
        /// </summary>
        public static event EventHandler<NavigationFailedEventArgs> NavigationFailed;

        internal static void OnNavigating(ModernFrame sender, NavigatingCancelEventArgs e)
        {
            var handler = Navigating;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        internal static void OnNavigated(ModernFrame sender, NavigationEventArgs e)
        {
            var handler = Navigated;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        internal static void OnFragmentNavigation(ModernFrame sender, FragmentNavigationEventArgs e)
        {
            var handler = FragmentNavigation;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        internal static void OnNavigationFailed(ModernFrame sender, NavigationFailedEventArgs e)
        {
            var handler = NavigationFailed;
            if (handler != null)
            {
                handler(sender, e);
            }
        }
    }
}
