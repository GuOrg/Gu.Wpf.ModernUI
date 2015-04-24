namespace Gu.ModernUi.Interfaces
{
    public interface IDialogHandler
    {
        DialogResult ShowMessage(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon = MessageBoxIcon.Information);

        DialogResult ShowContent(string title, object content, MessageBoxButtons buttons);
    }
}
