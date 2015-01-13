namespace Gu.ModernUI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

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

        [SetUp]
        public void SetUp()
        {
            this.contentLoaderMock = new Mock<IContentLoader>();
            this.contentLoaderMock.Setup(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                             .Returns((Uri u, CancellationToken t) => Task.FromResult((object)u));
            this.parent = new ModernFrame { ContentLoader = this.contentLoaderMock.Object };
        }

        [Test]
        public void Navigates()
        {
            var source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            this.parent.Source = source;
            this.contentLoaderMock.Verify(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()), Times.Once());
            Assert.AreEqual(source, this.parent.Content); // The mock is wired up to return the Uri
            Assert.IsFalse(this.parent.IsLoadingContent);
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
        public void ChildCancelNavigation()
        {
            var source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            this.parent.Source = source;
            this.parent.Navigating += (_, e) => e.Cancel = true;
            var toUri = new Uri(@"/ParentContent/2.xaml", UriKind.Relative);
            var child = new ModernFrame
            {
                Source = new Uri(@"/ChildContent/1.xaml", UriKind.Relative),
                ContentLoader = this.contentLoaderMock.Object
            };
            this.parent.Content = child;
            this.parent.AddVisualChild(child);
            child.Navigating += (_, e) => e.Cancel = true;

            this.parent.Source = toUri;

            this.contentLoaderMock.Verify(x => x.LoadContentAsync(toUri, It.IsAny<CancellationToken>()), Times.Never);
            Assert.AreEqual(child, this.parent.Content); // The mock is wired up to return the Uri
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
                Assert.AreEqual(NavigationType.New, args.NavigationType);
            }

            var navigatinons = new[] { localNavigations.Single(), globalNavigations.Single() };
            foreach (var args in navigatinons)
            {
                Assert.AreEqual(this.parent.Source, args.Source);
                Assert.AreSame(this.parent, args.Frame);
                Assert.AreEqual(NavigationType.New, args.NavigationType);
            }
        }

        [Test]
        public void ParentNavigationChildNotifies()
        {
            this.parent.Source = new Uri(@"/ParentContent/1.xaml", UriKind.Relative);
            var child = new ModernFrame
                            {
                                Source = new Uri(@"/ChildContent/1.xaml", UriKind.Relative),
                                ContentLoader = this.contentLoaderMock.Object
                            };
            this.parent.Content = child;
            this.parent.AddVisualChild(child);

            var localNavigatings = new List<NavigatingCancelEventArgs>();
            var globalNavigatings = new List<NavigatingCancelEventArgs>();
            child.Navigating += (_, e) => localNavigatings.Add(e);
            NavigationEvents.Navigating += (_, e) => globalNavigatings.Add(e);

            var localNavigations = new List<NavigationEventArgs>();
            var globalNavigations = new List<NavigationEventArgs>();
            child.Navigated += (_, e) => localNavigations.Add(e);
            NavigationEvents.Navigated += (_, e) => globalNavigations.Add(e);

            this.parent.Source = new Uri(@"/ParentContent/2.xaml", UriKind.Relative);

            var navigatings = new[] { localNavigatings.Single(), globalNavigatings.Single(x => x.IsParentFrameNavigating) };
            foreach (var args in navigatings)
            {
                Assert.IsNull(args.Source);
                Assert.AreEqual(NavigationType.Parent, args.NavigationType);
                Assert.AreSame(child, args.Frame);
                Assert.IsTrue(args.IsParentFrameNavigating);
            }

            var navigatinons = new[] { localNavigations.Single(), globalNavigations.Single(x => x.NavigationType == NavigationType.Parent) };
            foreach (var args in navigatinons)
            {
                Assert.IsNull(args.Source);
                Assert.AreEqual(NavigationType.Parent, args.NavigationType);
                Assert.AreSame(child, args.Frame);
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
            this.parent.Source = new Uri("/ParentContent/1.xaml", UriKind.Relative);
            var childContent = new Mock<IContent>();
            var child = new ModernFrame
            {
                Content = childContent.Object,
                ContentLoader = this.contentLoaderMock.Object
            };
            this.parent.Content = child;
            this.parent.AddVisualChild(child);
            this.parent.Source = new Uri("/ParentContent/2.xaml", UriKind.Relative);
            childContent.Verify(x => x.OnNavigatingFrom(It.IsAny<NavigatingCancelEventArgs>()), Times.Once);
            childContent.Verify(x => x.OnNavigatedFrom(It.IsAny<NavigationEventArgs>()), Times.Once);
        }
    }
}
