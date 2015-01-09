namespace Gu.ModernUI.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using Gu.ModernUI.Windows;
    using Gu.ModernUI.Windows.Controls;
    using Gu.ModernUI.Windows.Navigation;

    using Moq;

    using NUnit.Framework;

    [RequiresSTA]
    public class ModernFrameTests
    {
        private Mock<IContentLoader> contentLoaderMock;

        private ModernFrame parent;

        private ModernFrame child;

        private Mock<IContent> childContent;

        [SetUp]
        public void SetUp()
        {
            this.contentLoaderMock = new Mock<IContentLoader>();
            this.contentLoaderMock.Setup(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                             .Returns((Uri u, CancellationToken t) => Task.FromResult((object)u));
            this.childContent = new Mock<IContent>();
            this.parent = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
            this.child = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
            this.parent.Content = this.child;
            this.child.Content = this.childContent.Object;
            this.parent.RaiseEvent(new RoutedEventArgs { RoutedEvent = FrameworkElement.LoadedEvent });
            this.child.RaiseEvent(new RoutedEventArgs { RoutedEvent = FrameworkElement.LoadedEvent });
        }

        [Test]
        public void ParentNavigationNotifiesChild()
        {
            this.parent.Source = new Uri("/Content/1.xaml", UriKind.Relative);
            this.childContent.Verify(x => x.OnNavigatedFrom(It.IsAny<NavigationEventArgs>()), Times.Once);
        }
    }
}
