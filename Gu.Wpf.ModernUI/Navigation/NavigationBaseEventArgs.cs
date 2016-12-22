namespace Gu.Wpf.ModernUI.Navigation
{
    using System;

    /// <summary>
    /// Defines the base navigation event arguments.
    /// </summary>
    public abstract class NavigationBaseEventArgs
        : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationBaseEventArgs"/> class.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="source"></param>
        protected NavigationBaseEventArgs(ModernFrame frame, Uri source)
        {
            this.Frame = frame;
            this.Source = source;
        }

        /// <summary>
        /// Gets the frame that raised this event.
        /// </summary>
        public ModernFrame Frame { get;  }

        /// <summary>
        /// Gets the source uri for the target being navigated to.
        /// </summary>
        public Uri Source { get; }
    }
}
