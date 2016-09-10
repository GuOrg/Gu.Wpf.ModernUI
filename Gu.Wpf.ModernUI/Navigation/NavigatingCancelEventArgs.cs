namespace Gu.Wpf.ModernUI.Navigation
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="INavigationView.OnNavigatingFrom" /> method and the <see cref="ModernFrame.Navigating"/> event.
    /// </summary>
    public class NavigatingCancelEventArgs
        : NavigationBaseEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="source"></param>
        /// <param name="isParentFrameNavigating"></param>
        /// <param name="navigationType"></param>
        public NavigatingCancelEventArgs(ModernFrame frame, Uri source, bool isParentFrameNavigating, NavigationType navigationType)
            : base(frame, source)
        {
            this.IsParentFrameNavigating = isParentFrameNavigating;
            this.NavigationType = navigationType;
        }

        /// <summary>
        /// Gets a value indicating whether the frame performing the navigation is a parent frame or the frame itself.
        /// </summary>
        public bool IsParentFrameNavigating { get; }
        /// <summary>
        /// Gets a value that indicates the type of navigation that is occurring.
        /// </summary>
        public NavigationType NavigationType { get; }
        /// <summary>
        /// Gets or sets a value indicating whether the event should be canceled.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
