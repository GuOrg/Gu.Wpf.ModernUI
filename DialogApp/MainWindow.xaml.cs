namespace DialogApp
{
    using System;

    using Gu.ModernUI.Interfaces;
    using Gu.Wpf.ModernUI;

    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void OnLoaded(object sender, EventArgs eventArgs)
        {
            this.DialogHandler.Show("Here we are testing that showing a dialog on loaded works.", "Loaded", MessageBoxButtons.OK);
        }
    }
}
