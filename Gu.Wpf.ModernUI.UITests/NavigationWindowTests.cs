namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class NavigationWindowTests : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp.exe";

        protected override string WindowName { get; } = "MainWindow";

        private Label Content => this.Window.Get<Label>("ContentTextBlock");

        private Button Link1 => this.GetLink();

        private Button Link2 => this.GetLink();

        private Button TitleLink1 => this.GetLink();

        private Button TitleLink2 => this.GetLink();

        [Test]
        public void TogglesEnabledOnNavigate()
        {
            this.RestartApplication();
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(false, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(false, this.TitleLink2.Enabled);
            Assert.AreEqual("2", this.Content.Text);

            this.Link1.Click();
            Assert.AreEqual(false, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(false, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("1", this.Content.Text);
        }

        [Test]
        public void TogglesEnabledOnNavigateTitle()
        {
            this.RestartApplication();
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(false, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(false, this.TitleLink2.Enabled);
            Assert.AreEqual("2", this.Content.Text);

            this.TitleLink1.Click();
            Assert.AreEqual(false, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(false, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("1", this.Content.Text);
        }
    }
}