namespace Gu.Wpf.ModernUI.Tests.Navigation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Windows;

    using Gu.Wpf.ModernUI.Navigation;

    using Moq;

    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    [TestFixture]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DefaultLinkNavigator_CanNavigate_Tests
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly UIElement Sender = new UIElement();
        private Link link1;
        // ReSharper disable once NotAccessedField.Local
        private Link link2;
        private ModernFrame frame;
        private Mock<ILink> linkMock;
        private Mock<INavigator> navigatorMock;
        private Mock<IContentLoader> contentLoaderMock;

        private DefaultLinkNavigator linkNavigator;

        [SetUp]
        public void SetUp()
        {
            this.link1 = new Link { DisplayName = "1", Source = new Uri("/1", UriKind.RelativeOrAbsolute), LinkNavigator = null };
            this.link2 = new Link { DisplayName = "2", Source = new Uri("/2", UriKind.RelativeOrAbsolute) };
            this.contentLoaderMock = new Mock<IContentLoader>();
            this.frame = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
            this.linkMock = new Mock<ILink>(MockBehavior.Strict);

            this.navigatorMock = new Mock<INavigator>(MockBehavior.Strict);
            this.navigatorMock.Setup(x => x.NavigationTarget)
                .Returns(this.frame);
            this.linkNavigator = new DefaultLinkNavigator();
        }

        [TearDown]
        public void TearDown()
        {
            this.linkMock.VerifyAll();
            this.navigatorMock.VerifyAll();
            this.contentLoaderMock.VerifyAll();
        }

        [TestCase(true)]
        public void NavigatesIfContentSourceIsNull(bool navigatesOnload)
        {
            this.linkNavigator.NavigatesToContentOnLoad = navigatesOnload;
            this.navigatorMock.SetupProperty(x => x.SelectedLink);
            this.navigatorMock.SetupProperty(x => x.SelectedSource);
            this.navigatorMock.SetupGet(x => x.Links)
                .Returns(new[] { this.link1 });
            var args = LinkCommandsHelper.CreateCanExecuteRoutedEventArgs(this.link1);

            this.linkNavigator.CanNavigate(this.navigatorMock.Object, this.link1, args);

            var times = navigatesOnload
                ? Times.Once()
                : Times.Never();
            this.contentLoaderMock.Verify(x => x.LoadContentAsync(this.link1.Source, It.IsAny<CancellationToken>()), times);
            this.navigatorMock.VerifySet(x => x.SelectedSource = this.link1.Source, times);
            this.navigatorMock.VerifySet(x => x.SelectedLink = this.link1, times);
            Assert.AreEqual(navigatesOnload, this.link1.IsNavigatedTo);
            Assert.IsFalse(this.link1.CanNavigate);
            Assert.IsTrue(args.Handled);
        }
    }
}
