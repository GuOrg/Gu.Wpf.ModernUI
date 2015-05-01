namespace Gu.ModernUi.Interfaces
{
    using System.Collections.Generic;

    public interface IDialogHandler
    {
        string Title { get; }

        object Content { get; }

        MessageBoxIcon Icon { get; }

        IEnumerable<DialogResult> Buttons { get; }

        DialogResult Show(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.Information);

        DialogResult Show(object content, string title, MessageBoxButtons buttons);
    }
}
