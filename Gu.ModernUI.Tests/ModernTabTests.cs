namespace Gu.ModernUI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Moq;

    using NUnit.Framework;

    [RequiresSTA]
    public class ModernTabTests
    {
        private Mock<IContentLoader> contentLoaderMock;
        private ModernTab tab;

        [SetUp]
        public void SetUp()
        {
            this.contentLoaderMock = new Mock<IContentLoader>();
            this.contentLoaderMock.Setup(x => x.LoadContentAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
                                  .Returns((Uri u, CancellationToken t) => Task.FromResult((object)u));

            this.tab = new ModernTab
            {
                ContentLoader = this.contentLoaderMock.Object,
            };
        }

        [Test]
        public void Navigate()
        {
            var sourceEventArgses = new List<SourceEventArgs>();
            this.tab.SelectedSourceChanged += (_, e) => sourceEventArgses.Add(e);
            var uri = new Uri("1.xaml", UriKind.RelativeOrAbsolute);
            this.tab.SelectedSource = uri;
            Assert.AreEqual(uri, this.tab.SelectedSource);
            Assert.AreEqual(1, sourceEventArgses.Count);
        }
    }
}
