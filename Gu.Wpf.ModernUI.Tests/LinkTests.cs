namespace Gu.Wpf.ModernUI.Tests
{
    using System;

    using Moq;

    using NUnit.Framework;

    [RequiresSTA]
    public class LinkTests
    {
        private Mock<ILinkNavigator> linkNavigatorMock;

        [SetUp]
        public void SetUp()
        {
            this.linkNavigatorMock = new Mock<ILinkNavigator>(MockBehavior.Strict);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanNavigateNoFrame(bool canNavigate)
        {
            var link = new Link { DisplayName = "1", Source = new Uri("/1", UriKind.RelativeOrAbsolute) };
            link.LinkNavigator = this.linkNavigatorMock.Object;
            this.linkNavigatorMock.Setup(x => x.CanNavigate(null, link.Source))
                .Returns(canNavigate);

            Assert.AreEqual(canNavigate, link.CanNavigatorNavigate());
        }


        [TestCase(true)]
        [TestCase(false)]
        public void CanNavigateWithFrame(bool canNavigate)
        {
            var link = new Link { DisplayName = "1", Source = new Uri("/1", UriKind.RelativeOrAbsolute) };
            link.LinkNavigator = this.linkNavigatorMock.Object;
            var frame = new ModernFrame();
            this.linkNavigatorMock.Setup(x => x.CanNavigate(frame, link.Source))
                .Returns(canNavigate);

            link.SetNavigationTarget(frame);
            Assert.AreEqual(canNavigate, link.CanNavigatorNavigate());
        }
    }
}
