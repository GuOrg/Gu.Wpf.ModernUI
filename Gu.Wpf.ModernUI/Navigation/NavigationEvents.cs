namespace Gu.Wpf.ModernUI.Navigation
{
    using System;
    using System.Diagnostics;

    using Gu.Wpf.ModernUI;

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
            Debug.WriteLine("Navigating: type: {0} source:{1} isParentFrameNavigating: {2}", e.NavigationType, e.Source, e.IsParentFrameNavigating);
        }

        internal static void OnNavigated(ModernFrame sender, NavigationEventArgs e)
        {
            var handler = Navigated;
            if (handler != null)
            {
                handler(sender, e);
            }
            Debug.WriteLine("Navigated: type: {0} source: {1}", e.NavigationType,e.Source);
        }

        internal static void OnFragmentNavigation(ModernFrame sender, FragmentNavigationEventArgs e)
        {
            var handler = FragmentNavigation;
            if (handler != null)
            {
                handler(sender, e);
            }
            Debug.WriteLine("FragmentNavigation: fragment:{0}", e.Fragment);
        }

        internal static void OnNavigationFailed(ModernFrame sender, NavigationFailedEventArgs e)
        {
            var handler = NavigationFailed;
            if (handler != null)
            {
                handler(sender, e);
            }
            Debug.WriteLine("NavigationFailed: source:{0} error: {1}", e.Source, e.Error.Message);
        }
    }
}
