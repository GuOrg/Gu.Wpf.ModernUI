using System.Collections.Generic;

namespace Gu.ModernUi.Interfaces
{
    public interface IDialogHandler
    {
        string Caption { get; }

        object Content { get; }
       
        MessageBoxIcon Icon { get; }
        
        IEnumerable<DialogResult> Buttons { get; }

        DialogResult Show(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.Information);

        DialogResult Show(string title, object content, MessageBoxButtons buttons);
    }
}
