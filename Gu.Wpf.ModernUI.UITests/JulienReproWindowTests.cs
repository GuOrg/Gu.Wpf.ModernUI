namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class JulienReproWindowTests : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp.exe";

        protected override string WindowName { get; } = "JulienReproWindow";

        private Button TitleSettings => this.GetLink();

        private Button HomeGroup => this.GetLink();

        private Button SettingsGroup => this.GetLink();

        private Button SettingsLink => this.GetLink();

        private Button HomeLink => this.GetLink();

        private Button NestedSettingsLink => this.GetLink();

        private Button GotoPageSettingsButton => this.GetLink();

        private Button RelayCommandGotoSettingsButton => this.GetLink();

        private Button NestedHomeLink => this.GetLink();

        private Button GotoPageHomeButton => this.GetLink();

        private Button RelayCommandGotoHomeButton => this.GetLink();

        [SetUp]
        public void SetUp()
        {
            // This is needed to refresh
            this.SettingsGroup.ClickIfEnabled();
            this.HomeGroup.Click();
        }

        [Test]
        public void InitialState()
        {
            this.RestartApplication();
            this.AssertEnabled(
                this.HomeGroup,
                this.NestedHomeLink,
                this.RelayCommandGotoHomeButton);
            this.AssertDisabled(this.TitleSettings, this.SettingsGroup, this.SettingsLink);
        }

        [Test]
        public void InitialStateNavigationCommandGotoHome()
        {
            this.RestartApplication();
            Assert.Inconclusive("Not sure if this is our bug or a framework bug.");
            this.AssertEnabled(this.GotoPageHomeButton);
        }

        [Test]
        public void NavigateWithLinks()
        {
            this.SettingsGroup.Click();
            this.NestedHomeLink.Click();
            this.AssertEnabled(
                this.TitleSettings,
                this.SettingsGroup,
                this.NestedSettingsLink,
                this.GotoPageSettingsButton,
                this.RelayCommandGotoSettingsButton);
            this.AssertDisabled(this.HomeGroup, this.HomeLink);

            this.NestedSettingsLink.Click();
            this.AssertEnabled(
                this.HomeGroup,
                this.NestedHomeLink,
                this.GotoPageHomeButton,
                this.RelayCommandGotoHomeButton);
            this.AssertDisabled(this.TitleSettings, this.SettingsGroup, this.SettingsLink);
        }

        [Test]
        public void NavigateGotoPageButtons()
        {
            this.SettingsGroup.Click();
            this.GotoPageHomeButton.Click();
            this.AssertEnabled(
                this.TitleSettings,
                this.SettingsGroup,
                this.NestedSettingsLink,
                this.GotoPageSettingsButton,
                this.RelayCommandGotoSettingsButton);
            this.AssertDisabled(this.HomeGroup, this.HomeLink);

            this.GotoPageSettingsButton.Click();
            this.AssertEnabled(
                this.HomeGroup,
                this.NestedHomeLink,
                this.GotoPageHomeButton,
                this.RelayCommandGotoHomeButton);
            this.AssertDisabled(this.TitleSettings, this.SettingsGroup, this.SettingsLink);
        }

        [Test]
        public void NavigateRelayCommandButtons()
        {
            this.SettingsGroup.Click();
            this.RelayCommandGotoHomeButton.Click();
            this.AssertEnabled(
                this.TitleSettings,
                this.SettingsGroup,
                this.NestedSettingsLink,
                this.GotoPageSettingsButton,
                this.RelayCommandGotoSettingsButton);
            this.AssertDisabled(this.HomeGroup, this.HomeLink);

            this.RelayCommandGotoSettingsButton.Click();
            this.AssertEnabled(
                this.HomeGroup,
                this.NestedHomeLink,
                this.GotoPageHomeButton,
                this.RelayCommandGotoHomeButton);
            this.AssertDisabled(this.TitleSettings, this.SettingsGroup, this.SettingsLink);
        }
    }
}