namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;

    using Gu.ModernUI.Interfaces;

    /// <summary>
    /// The viewmodel for a modernpopup
    /// </summary>
    public class DialogViewModel
    {
        public DialogViewModel(string title, object content, MessageBoxIcon icon, MessageBoxButtons buttons)
            : this(title, content, icon, CreateButtons(buttons))
        {
        }

        public DialogViewModel(string title, object content, MessageBoxIcon icon, IEnumerable<DialogResult> buttons)
        {
            this.Title = title;
            this.Content = content;
            this.Icon = icon;
            this.Buttons = buttons;
        }

        public string Title { get; }

        public object Content { get; }

        public MessageBoxIcon Icon { get; }

        public IEnumerable<DialogResult> Buttons { get; }

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
                    throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
            }
        }
    }
}
