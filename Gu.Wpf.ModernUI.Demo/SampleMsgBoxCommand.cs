namespace Gu.Wpf.ModernUI.Demo
{
    using System.Windows;

    using Gu.Wpf.ModernUI.Internals;

    using ModernUI;

    /// <summary>
    /// An ICommand implementation displaying a message box.
    /// </summary>
    internal class SampleMsgBoxCommand
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
