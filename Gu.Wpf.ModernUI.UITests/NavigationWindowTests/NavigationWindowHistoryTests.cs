namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class NavigationWindowHistoryTests : NavigationWindow
    {
        [Test]
        public void NavigatesBack()
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

            this.Window.Get<Button>("NavigateBack")
                .Click();
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
        }
    }
}