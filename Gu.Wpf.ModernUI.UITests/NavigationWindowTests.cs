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

        private Button Link4 => this.GetLink();

        private Button Link5 => this.GetLink();

        private Button Link6 => this.GetLink();

        private Button Link7 => this.GetLink();

        private Button TitleLink1 => this.GetLink();

        private Button TitleLink2 => this.GetLink();

        private Button TitleLink3 => this.GetLink();

        private Button Group1 => this.GetLink();

        private Button Group2 => this.GetLink();

        private Button Group3 => this.GetLink();

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

        [Test]
        public void NavigateToUniqueTitleLink()
        {
            this.TitleLink3.Click();
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual(false, this.TitleLink3.Enabled);
            Assert.AreEqual(true, this.Group1.Enabled);
            Assert.AreEqual(true, this.Group2.Enabled);
            Assert.AreEqual("3", this.Content.Text);
        }

        [Test]
        public void GroupNavigatesAndRemembers()
        {
            this.Group1.Click();
            if (this.Link4.Enabled)
            {
                this.Link4.Click();
            }
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("4", this.Content.Text);
            Assert.AreEqual(false, this.Group1.Enabled);
            Assert.AreEqual(true, this.Group2.Enabled);
            Assert.AreEqual(false, this.Link4.Enabled);
            Assert.AreEqual(true, this.Link5.Enabled);

            this.Link5.Click();
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("5", this.Content.Text);
            Assert.AreEqual(false, this.Group1.Enabled);
            Assert.AreEqual(true, this.Group2.Enabled);
            Assert.AreEqual(true, this.Link4.Enabled);
            Assert.AreEqual(false, this.Link5.Enabled);

            this.Group2.Click();
            if (this.Link7.Enabled)
            {
                this.Link7.Click();
            }
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("7", this.Content.Text);
            Assert.AreEqual(true, this.Group1.Enabled);
            Assert.AreEqual(false, this.Group2.Enabled);
            Assert.AreEqual(true, this.Link6.Enabled);
            Assert.AreEqual(false, this.Link7.Enabled);

            this.Group1.Click();
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(true, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual("5", this.Content.Text);
            Assert.AreEqual(false, this.Group1.Enabled);
            Assert.AreEqual(true, this.Group2.Enabled);
            Assert.AreEqual(true, this.Link4.Enabled);
            Assert.AreEqual(false, this.Link5.Enabled);
        }

        [Test]
        public void SelectedSourceOnLoad()
        {
            this.RestartApplication();
            this.Group2.Click();
            // Group 2 has SelectedSource="/View7.xaml" so it should start at that.
            Assert.AreEqual(false, this.Link7.Enabled);
        }

        [TestCase("GoToPageButton")]
        [TestCase("GoToPageLink")]
        [TestCase("SourceLink")]
        public void NavigationButtons(string name)
        {
            this.RestartApplication();
            this.Group3.Click();
            this.Window.Get<Button>("NavigationButtonsLink").ClickIfEnabled();
            this.Window.Get<Button>(name).Click();
            Assert.AreEqual(false, this.Link1.Enabled);
            Assert.AreEqual(true, this.Link2.Enabled);
            Assert.AreEqual(false, this.TitleLink1.Enabled);
            Assert.AreEqual(true, this.TitleLink2.Enabled);
            Assert.AreEqual(true, this.TitleLink3.Enabled);
            Assert.AreEqual(true, this.Group1.Enabled);
            Assert.AreEqual(true, this.Group2.Enabled);
            Assert.AreEqual("1", this.Content.Text);
        }

        [Test]
        public void NavigatesToViewWithSameName()
        {
            this.Link1.Click();
            Assert.AreEqual("1", this.Content.Text);
            this.Group3.Click();
            this.Window.Get<Button>("DupeLink").Click();
            Assert.AreEqual("1.1", this.Content.Text);
        }
    }
}