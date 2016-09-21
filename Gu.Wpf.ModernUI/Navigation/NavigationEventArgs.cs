namespace Gu.Wpf.ModernUI.Navigation
{
    using System;

    /// <summary>
    /// Provides data for frame navigation events.
    /// </summary>
    public class NavigationEventArgs
        : NavigationBaseEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationEventArgs"/> class.
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
