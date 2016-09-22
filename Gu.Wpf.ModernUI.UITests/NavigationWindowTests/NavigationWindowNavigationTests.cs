namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class NavigationWindowNavigationTests : NavigationWindow
    {
        [SetUp]
        public void SetUp()
        {
            // Not restarting the application here
            // this makes the tests messier but exercises more code
            // not sure what is best.
            this.Link2.ClickIfEnabled();
            this.Group1.Click();
            this.Link4.ClickIfEnabled();
            this.Group2.Click();
            this.Link6.ClickIfEnabled();
            this.Group3.Click();
            this.NavigationButtonsLink.ClickIfEnabled();
            this.Link2.Click();
        }

        [Test]
        public void TogglesEnabledOnNavigate()
        {
            this.AssertEnabled(
                this.Link1,
                this.TitleLink1,
                this.TitleLink3,
                this.TitleLink5,
                this.Group1,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Link2, this.TitleLink2);
            Assert.AreEqual("2", this.Content.Text);

            this.Link1.Click();
            this.AssertEnabled(
                this.Link2,
                this.TitleLink2,
                this.TitleLink3,
                this.TitleLink5,
                this.Group1,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Link1, this.TitleLink1);
            Assert.AreEqual("1", this.Content.Text);
        }

        [Test]
        public void TogglesEnabledOnNavigateTitle()
        {
            this.AssertEnabled(
                this.Link1,
                this.TitleLink1,
                this.TitleLink3,
                this.TitleLink5,
                this.Group1,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Link2, this.TitleLink2);
            Assert.AreEqual("2", this.Content.Text);

            this.TitleLink1.Click();
            this.AssertEnabled(
                this.Link2,
                this.TitleLink2,
                this.TitleLink3,
                this.TitleLink5,
                this.Group1,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Link1, this.TitleLink1);
            Assert.AreEqual("1", this.Content.Text);
        }

        [Test]
        public void NavigateToUniqueTitleLink()
        {
            this.TitleLink3.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.TitleLink1,
                this.TitleLink2,
                this.TitleLink5,
                this.Group1,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.TitleLink3);
            Assert.AreEqual("3", this.Content.Text);
        }

        [Test]
        public void TitleLinkAndLinkGroupLink()
        {
            this.TitleLink5.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link4,
                this.TitleLink1,
                this.TitleLink2,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Group1, this.Link5, this.TitleLink5);
            Assert.AreEqual("5", this.Content.Text);

            this.Group2.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link7,
                this.TitleLink1,
                this.TitleLink2,
                this.TitleLink3,
                this.Group1,
                this.Group3);
            this.AssertDisabled(this.Group2, this.Link6);
            Assert.AreEqual("6", this.Content.Text);

            this.Group1.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link4,
                this.TitleLink1,
                this.TitleLink2,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Group1, this.Link5, this.TitleLink5);
            Assert.AreEqual("5", this.Content.Text);
        }

        [Test]
        public void GroupNavigatesAndRemembers()
        {
            this.Group1.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link5,
                this.TitleLink1,
                this.TitleLink2,
                this.TitleLink5,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Group1, this.Link4);
            Assert.AreEqual("4", this.Content.Text);

            this.Link5.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link4,
                this.TitleLink1,
                this.TitleLink2,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Group1, this.Link5, this.TitleLink5);
            Assert.AreEqual("5", this.Content.Text);

            this.Group2.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link7,
                this.TitleLink1,
                this.TitleLink2,
                this.TitleLink3,
                this.Group1,
                this.Group3);
            this.AssertDisabled(this.Group2, this.Link6);
            Assert.AreEqual("6", this.Content.Text);

            this.Group1.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link4,
                this.TitleLink1,
                this.TitleLink2,
                this.Group2,
                this.Group3);
            this.AssertDisabled(this.Group1, this.Link5, this.TitleLink5);
            Assert.AreEqual("5", this.Content.Text);
        }

        [TestCase("GoToPageButton")]
        [TestCase("GoToPageLink")]
        [TestCase("SourceLink")]
        public void NavigationButtons(string name)
        {
            this.Group3.Click();
            this.Window.Get<Button>(name).Click();
            this.AssertEnabled(this.Link2, this.TitleLink2, this.TitleLink3, this.TitleLink5, this.Group1, this.Group2, this.Group3);
            this.AssertDisabled(this.Link1, this.TitleLink1);
        }

        [Test]
        public void NavigatesToViewWithSameName()
        {
            this.Link1.Click();
            Assert.AreEqual("1", this.Content.Text);
            this.Group3.Click();
            this.Window.Get<Button>("DupeLink1").Click();
            Assert.AreEqual("1.1", this.Content.Text);
        }
    }
}