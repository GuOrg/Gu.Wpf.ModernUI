namespace Gu.ModernUI.Interfaces
{
    public interface IDialogHandler
    {
        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="title">The title text.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <returns>The result picked by the user.</returns>
        DialogResult Show(string message, string title, MessageBoxButtons buttons);

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread.
        /// </summary>
        /// <param name="message">The message text.</param>
        /// <param name="title">The title text.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <param name="icon">The icon to render with the message.</param>
        /// <returns>The result picked by the user.</returns>
        DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread.
        /// </summary>
        /// <param name="content">The content to render.</param>
        /// <param name="title">The title text.</param>
        /// <param name="buttons">The buttons to show.</param>
        /// <returns>The result picked by the user.</returns>
        DialogResult Show(object content, string title, MessageBoxButtons buttons);
    }
}
