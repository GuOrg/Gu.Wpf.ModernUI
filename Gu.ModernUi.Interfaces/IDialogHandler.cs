namespace Gu.ModernUi.Interfaces
{
    public interface IDialogHandler
    {
        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        DialogResult Show(string message, string title, MessageBoxButtons buttons);

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread. 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        DialogResult Show(object content, string title, MessageBoxButtons buttons);
    }
}
