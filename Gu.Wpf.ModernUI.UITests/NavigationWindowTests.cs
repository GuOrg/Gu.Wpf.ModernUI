namespace Gu.Wpf.ModernUI.UITests
{
    using TestStack.White.UIItems;

    public partial class NavigationWindowTests : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp.exe";

        protected override string WindowName { get; } = "MainWindow";

        private Label Content => this.Window.Get<Label>("ContentTextBlock");

        private Button Link1 => this.GetLink();

        private Button Link2 => this.GetLink();

        private Button Link4 => this.GetLink();

        private Button Link5 => this.GetLink();

        private Button Link6 => this.GetLink();

        private Button Link7 => this.GetLink();

        private Button TitleLink1 => this.GetLink();

        private Button TitleLink2 => this.GetLink();

        private Button TitleLink3 => this.GetLink();

        private Button TitleLink5 => this.GetLink();

        private Button Group1 => this.GetLink();

        private Button Group2 => this.GetLink();

        private Button Group3 => this.GetLink();

        private Button NavigationButtonsLink => this.GetLink();
    }
}