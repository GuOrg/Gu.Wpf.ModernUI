namespace Gu.Wpf.ModernUI
{
    using System.Collections.Generic;

    using Gu.ModernUi.Interfaces;

    /// <summary>
    /// The viewmodel for a modernpopup
    /// </summary>
    public class DialogViewModel
    {
        public DialogViewModel(string title, object content, MessageBoxIcon icon, IEnumerable<DialogResult> buttons)
        {
            this.Title = title;
            this.Content = content;
            this.Icon = icon;
            this.Buttons = buttons;
        }

        public string Title { get; private set; }

        public object Content { get; private set; }

        public MessageBoxIcon Icon { get; private set; }

        public IEnumerable<DialogResult> Buttons { get; private set; }
    }
}
