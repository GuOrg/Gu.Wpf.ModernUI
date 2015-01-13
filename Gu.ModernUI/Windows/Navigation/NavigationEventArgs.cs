namespace Gu.ModernUI.Windows.Navigation
{
    using System;

    using Gu.ModernUI.Windows.Controls;

    /// <summary>
    /// Provides data for frame navigation events.
    /// </summary>
    public class NavigationEventArgs
        : NavigationBaseEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame">The frame navigating</param>
        /// <param name="source">The uri of being navigated to</param>
        /// <param name="navigationType"></param>
        /// <param name="content">The content of the target being navigated to.</param>
        public NavigationEventArgs(ModernFrame frame, Uri source, NavigationType navigationType, object content)
            : base(frame, source)
        {
            this.NavigationType = navigationType;
            this.Content = content;
        }
        /// <summary>
        /// Gets a value that indicates the type of navigation that is occurring.
        /// </summary>
        public NavigationType NavigationType { get; private set; }
        /// <summary>
        /// Gets the content of the target being navigated to.
        /// </summary>
        public object Content { get; private set; }
    }
}
