namespace Gu.ModernUI.Navigation
{
    using System;

    /// <summary>
    /// Provides data for the <see cref="ModernFrame.NavigationFailed"/> event.
    /// </summary>
    public class NavigationFailedEventArgs
        : NavigationBaseEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="source"></param>
        /// <param name="error"></param>
        public NavigationFailedEventArgs(ModernFrame frame, Uri source, Exception error)
            : base(frame, source)
        {
            this.Error = error;
        }
        /// <summary>
        /// Gets the error from the failed navigation.
        /// </summary>
        public Exception Error { get; private set; }
        /// <summary>
        /// Gets or sets a value that indicates whether the failure event has been handled.
        /// </summary>
        /// <remarks>
        /// When not handled, the error is displayed in the ModernFrame raising the NavigationFailed event.
        /// </remarks>
        public bool Handled { get; set; }
    }
}
