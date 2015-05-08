namespace Gu.Wpf.ModernUI.Demo
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Gu.ModernUi.Interfaces;

    using ModernUI;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            var result = this.DialogHandler.Show("Do you want to close", "Closing", MessageBoxButtons.OKCancel);
            if (result == ModernUi.Interfaces.DialogResult.OK)
            {
                Application.Current.Shutdown();
            }
        }

        private void MainWindow_OnLoaded(object sender, EventArgs eventArgs)
        {
            //this.DialogHandler.Show("test", "meg", MessageBoxButtons.OK);
        }

        private void ModernWindow_ContentRendered(object sender, EventArgs e)
        {

        }

        private void ModernWindow_Activated(object sender, EventArgs e)
        {
            this.DialogHandler.Show("test", "meg", MessageBoxButtons.OK);
        }

        private void ModernWindow_Initialized(object sender, EventArgs e)
        {

        }

        private void ModernWindow_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void ModernWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void ModernWindow_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e)
        {

        }

        private void ModernWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
