namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;
    using ModernUi.Interfaces;

    /// <summary>
    /// Shows banner dialogs over a Window
    /// </summary>
    public class DefaultDialogHandler : IDialogHandler
    {
        private static DefaultDialogHandler instance;
        private DefaultDialogHandler()
        {
        }

        public static DefaultDialogHandler Instance
        {
            get
            {
                return instance ?? (instance = new DefaultDialogHandler());
            }
        }

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public virtual DialogResult Show(
            string message,
            string title,
            MessageBoxButtons buttons,
            MessageBoxIcon icon = MessageBoxIcon.Asterisk)
        {
            return Show(new DialogViewModel(title, message, icon, CreateButtons(buttons)));
        }

        /// <summary>
        /// Shows a popup and returns the result.
        /// Can be called from any thread. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content">The content to render</param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public virtual DialogResult Show(object content, string title, MessageBoxButtons buttons)
        {
            return Show(new DialogViewModel(title, content, MessageBoxIcon.None, CreateButtons(buttons)));
        }

        protected virtual DialogResult Show(DialogViewModel viewModel)
        {
            var result = Application.Current.Dispatcher.Invoke(
                () =>
                {
                    var window = Application.Current.MainWindow as ModernWindow;
                    if (window == null)
                    {
                        return DialogResult.None;
                    }
                    var dialog = new ModernPopup { DataContext = viewModel };
                    return dialog.RunDialog(window, this, viewModel);
                }, DispatcherPriority.Input);
            return result;
        }

        private static IEnumerable<DialogResult> CreateButtons(MessageBoxButtons buttons)
        {
            switch (buttons)
            {
                case MessageBoxButtons.OK:
                    return new[] { DialogResult.OK };
                case MessageBoxButtons.OKCancel:
                    return new[] { DialogResult.OK, DialogResult.Cancel };
                case MessageBoxButtons.AbortRetryIgnore:
                    return new[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore };
                case MessageBoxButtons.YesNoCancel:
                    return new[] { DialogResult.Yes, DialogResult.No, DialogResult.Cancel };
                case MessageBoxButtons.YesNo:
                    return new[] { DialogResult.Yes, DialogResult.No };
                case MessageBoxButtons.RetryCancel:
                    return new[] { DialogResult.Retry, DialogResult.Cancel };
                default:
                    throw new ArgumentOutOfRangeException("buttons", buttons, null);
            }
        }
    }
}
