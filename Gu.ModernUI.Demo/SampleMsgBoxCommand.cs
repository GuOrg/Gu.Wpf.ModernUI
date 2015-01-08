namespace Gu.ModernUI.Demo
{
    using System.Windows;

    using Gu.ModernUI.Presentation;
    using Gu.ModernUI.Windows.Controls;

    /// <summary>
    /// An ICommand implementation displaying a message box.
    /// </summary>
    public class SampleMsgBoxCommand
        : CommandBase
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute(object parameter)
        {
            ModernDialog.ShowMessage("A messagebox triggered by selecting a hyperlink", "Messagebox", MessageBoxButton.OK);
        }
    }
}
