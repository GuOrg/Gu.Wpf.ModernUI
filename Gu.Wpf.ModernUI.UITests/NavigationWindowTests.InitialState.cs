namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    public partial class NavigationWindowTests
    {
        [Test]
        public void InitialState()
        {
            this.RestartApplication();
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

        [Test]
        public void Group2StartsAtSelectedSource()
        {
            this.RestartApplication();
            this.Group2.Click();
            this.AssertEnabled(
                this.Link1,
                this.Link2,
                this.Link6,
                this.TitleLink1,
                this.TitleLink3,
                this.TitleLink5,
                this.Group1,
                this.Group3);
            this.AssertDisabled(this.Group2, this.Link7);
            Assert.AreEqual("7", this.Content.Text);
        }
    }
}