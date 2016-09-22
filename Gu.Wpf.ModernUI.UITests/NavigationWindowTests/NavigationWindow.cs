namespace Gu.Wpf.ModernUI.UITests
{
    using TestStack.White.UIItems;

    public abstract class NavigationWindow : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp.exe";

        protected override string WindowName { get; } = "MainWindow";

        protected Label Content => this.Window.Get<Label>("ContentTextBlock");

        protected Button Link1 => this.GetLink();

        protected Button Link2 => this.GetLink();

        protected Button Link4 => this.GetLink();

        protected Button Link5 => this.GetLink();

        protected Button Link6 => this.GetLink();

        protected Button Link7 => this.GetLink();

        protected Button TitleLink1 => this.GetLink();

        protected Button TitleLink2 => this.GetLink();

        protected Button TitleLink3 => this.GetLink();

        protected Button TitleLink5 => this.GetLink();

        protected Button Group1 => this.GetLink();

        protected Button Group2 => this.GetLink();

        protected Button Group3 => this.GetLink();

        protected Button NavigationButtonsLink => this.GetLink();
    }
}