namespace Gu.Wpf.ModernUI.Demo
{
    using System.Globalization;
    using System.Windows;

    using Gu.Wpf.ModernUI;

    /// <summary>
    /// An ICommand implementation that displays the provided command parameter in a message box.
    /// </summary>
    internal class SampleParameterCommand
        : CommandBase
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        protected override void OnExecute(object parameter)
        {
            ModernDialog.ShowMessage(string.Format(CultureInfo.CurrentUICulture, "Executing command, command parameter = '{0}'", parameter), "SampleCommand", MessageBoxButton.OK);
        }
    }
}
