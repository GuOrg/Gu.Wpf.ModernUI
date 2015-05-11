namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Gu.ModernUi.Interfaces;

    using ModernUI;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private bool activated;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnClose));
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            var result = this.DialogHandler.Show("Do you want to close?", "Closing", MessageBoxButtons.YesNo);
            if (result == ModernUi.Interfaces.DialogResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private async void MainWindow_OnLoaded(object sender, EventArgs eventArgs)
        {
            await Dispatcher.Yield(DispatcherPriority.Loaded);
            this.DialogHandler.Show("", "loaded", MessageBoxButtons.OK);
        }
    }
}
