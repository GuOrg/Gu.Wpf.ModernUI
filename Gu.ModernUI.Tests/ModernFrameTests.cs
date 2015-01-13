namespace Gu.ModernUI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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


        [SetUp]
        public void SetUp()
        {
            this.contentLoaderMock = new Mock<IContentLoader>();
            this.contentLoaderMock.Setup(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                             .Returns((Uri u, CancellationToken t) => Task.FromResult((object)u));
            this.parent = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
            this.child = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
            this.parent.Content = this.child;

            this.parent.RaiseEvent(new RoutedEventArgs { RoutedEvent = FrameworkElement.LoadedEvent });
            this.child.RaiseEvent(new RoutedEventArgs { RoutedEvent = FrameworkElement.LoadedEvent });
        }

        [Test]
        public void Navigates()
        {
            var source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            this.parent.Source = source;
            this.contentLoaderMock.Verify(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.AreEqual(source, this.parent.Content); // The mock is wired up to return the Uri
        }

        [Test]
        public void CancelNavigation()
        {
            var source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            this.parent.Source = source;
            this.parent.Navigating += (_, e) => e.Cancel = true;
            var toUri = new Uri(@"/ParentContent/2.xaml", UriKind.Relative);
            this.parent.Source = toUri;
            this.contentLoaderMock.Verify(x => x.LoadContentAsync(toUri, It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(source, this.parent.Content); // The mock is wired up to return the Uri
        }

        [Test]
        public void NavigationNotifies()
        {
            this.parent.Source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            var localNavigatings = new List<NavigatingCancelEventArgs>();
            var globalNavigatings = new List<NavigatingCancelEventArgs>();
            this.parent.Navigating += (_, e) => localNavigatings.Add(e);
            NavigationEvents.Navigating += (_, e) => globalNavigatings.Add(e);

            var localNavigations = new List<NavigationEventArgs>();
            var globalNavigations = new List<NavigationEventArgs>();
            this.parent.Navigated += (_, e) => localNavigations.Add(e);
            NavigationEvents.Navigated += (_, e) => globalNavigations.Add(e);

            this.parent.Source = new Uri(@"/ParentContent/2.xaml", UriKind.Relative);

            var navigatings = new[] { localNavigatings.Single(), globalNavigatings.Single() };
            foreach (var args in navigatings)
            {
                Assert.AreEqual(this.parent.Source, args.Source);
                Assert.AreSame(this.parent, args.Frame);
                Assert.IsFalse(args.IsParentFrameNavigating);
            }

            var navigatinons = new[] { localNavigations.Single(), globalNavigations.Single() };
            foreach (var args in navigatinons)
            {
                Assert.AreEqual(this.parent.Source, args.Source);
                Assert.AreSame(this.parent, args.Frame);
            }
        }

        [Test]
        public void NotifiesContentWhenNavigating()
        {
            var oldContentMock = new Mock<IContent>();
            var newContentMock = new Mock<IContent>();
            this.parent.Content = oldContentMock.Object;
            var source = new Uri("/ParentContent/1.xaml", UriKind.Relative);

            this.contentLoaderMock.Setup(x => x.LoadContentAsync(source, It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(newContentMock.Object);

            this.parent.Source = source;

            oldContentMock.Verify(x => x.OnNavigatingFrom(It.IsAny<NavigatingCancelEventArgs>()), Times.Once);
            oldContentMock.Verify(x => x.OnNavigatedFrom(It.IsAny<NavigationEventArgs>()), Times.Once);
            newContentMock.Verify(x => x.OnNavigatedTo(It.IsAny<NavigationEventArgs>()), Times.Once);
        }

        [Test]
        public void ParentNavigationNotifiesChildContent()
        {
            var childContent = new Mock<IContent>();
            this.child.Content = childContent.Object;
            this.parent.Source = new Uri("/ParentContent/1.xaml", UriKind.Relative);
            childContent.Verify(x => x.OnNavigatingFrom(It.IsAny<NavigatingCancelEventArgs>()), Times.Once);
            childContent.Verify(x => x.OnNavigatedFrom(It.IsAny<NavigationEventArgs>()), Times.Once);
        }
    }
}
