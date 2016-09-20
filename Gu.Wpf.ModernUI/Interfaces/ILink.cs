namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows.Input;

    public interface ILink
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        string DisplayName { get; }

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        Uri Source { get; }

        /// <summary>
        /// Gets if the current linknavigator can navigate to Source
        /// This is updated by LinkCommands.OnCanNavigateLink
        /// </summary>
        bool CanNavigate { get; set; }

        /// <summary>
        /// Gets if the current navigationtarget Source == this.Source
        /// This is updated by LinkCommands.OnCanNavigateLink
        /// </summary>
        bool IsNavigatedTo { get; set; }

        /// <summary>
        ///
        /// </summary>
        ILinkNavigator LinkNavigator { get; }

        ICommand Command { get; }

        System.Windows.IInputElement CommandTarget { get; set; }
    }
}