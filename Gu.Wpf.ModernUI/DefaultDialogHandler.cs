namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Threading;
    using Gu.ModernUI.Interfaces;

    /// <summary>
    /// Exposing methods for showing banner dialogs over a Window
    /// </summary>
    public class DefaultDialogHandler : IDialogHandler
    {
        private static DefaultDialogHandler instance;

        private DefaultDialogHandler()
        {
        }

        /// <summary>
        /// The default instance.
        /// </summary>
        public static DefaultDialogHandler Instance => instance ?? (instance = new DefaultDialogHandler());

        /// <inheritdoc/>
        public virtual DialogResult Show(
            string message,
            string title,
            MessageBoxButtons buttons)
        {
            return this.Show(new DialogViewModel(title, message, MessageBoxIcon.None, buttons));
        }

        /// <inheritdoc/>
        public virtual DialogResult Show(
            string message,
            string title,
            MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return this.Show(new DialogViewModel(title, message, icon, buttons));
        }

        /// <inheritdoc/>
        public virtual DialogResult Show(object content, string title, MessageBoxButtons buttons)
        {
            return this.Show(new DialogViewModel(title, content, MessageBoxIcon.None, buttons));
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
                }, DispatcherPriority.Background);
            return result;
        }
    }
}
