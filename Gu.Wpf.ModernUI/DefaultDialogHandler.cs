namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Threading;
    using ModernUi.Interfaces;

    /// <summary>
    /// Shows banner dialogs on a Window
    /// </summary>
    public class DefaultDialogHandler : DependencyObject, IDialogHandler
    {
        private static DefaultDialogHandler instance;
        private DefaultDialogHandler()
        {
        }

        public static DefaultDialogHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    Dispatcher.CurrentDispatcher.VerifyAccess();
                    instance = new DefaultDialogHandler();
                }
                return instance;
            }
        }

        public string Title { get; private set; }

        public object Content { get; private set; }

        public MessageBoxIcon Icon { get; private set; }

        public IEnumerable<DialogResult> Buttons { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="buttons"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public DialogResult Show(
            string message,
            string title,
            MessageBoxButtons buttons,
            MessageBoxIcon icon = MessageBoxIcon.Asterisk)
        {
            this.Title = title;
            this.Content = message;
            this.Buttons = CreateButtons(buttons);
            this.Icon = this.Icon;
            var args = Show();
            return args;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public DialogResult Show(object content, string title, MessageBoxButtons buttons)
        {
            this.Title = title;
            this.Content = content;
            this.Icon = MessageBoxIcon.None;
            this.Buttons = CreateButtons(buttons);
            var args = Show();
            return args;
        }

        protected virtual DialogResult Show()
        {
            var window = Application.Current.MainWindow as ModernWindow;
            if (window == null)
            {
                return DialogResult.None;
            }

            var dialog = new ModernPopup { DataContext = this };
            return dialog.RunDialog(window, this);
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
