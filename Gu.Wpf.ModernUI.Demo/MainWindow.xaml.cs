namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Gu.ModernUI.Interfaces;

    using ModernUI;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        private bool activated;
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, this.OnClose));
            this.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, this.OnClose));
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            var result = this.DialogHandler.Show("Do you want to close?", "Closing", MessageBoxButtons.YesNo);
            if (result == Gu.ModernUI.Interfaces.DialogResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
