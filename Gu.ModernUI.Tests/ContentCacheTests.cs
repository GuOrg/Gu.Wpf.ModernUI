namespace Gu.ModernUI.Tests
{
    using System;
    using System.IO.Packaging;

    using Gu.ModernUI.Windows.Controls;

    using NUnit.Framework;

    [RequiresSTA]
    public class ContentCacheTests
    {
        [TestCase(@"/1.xaml#1", @"/1.xaml", true)]
        [TestCase(@"/2.xaml", @"/1.xaml", false)]
        [TestCase(@"/1.xaml#1", @"/1.xaml#1", true)]
        [TestCase(@"/1.xaml", @"/Gu.ModernUI.Tests;component/1.xaml", true, Description = "Leaving this red, not sure what is right here")]
        public void AddThenGet(string addUri, string getUri, bool expected)
        {
            var frame = new ModernFrame { KeepContentAlive = true };
            var contentCache = new ModernFrame.ContentCache(frame);
            contentCache.AddOrUpdate(new Uri(addUri, UriKind.Relative), 1);
            object value;
            Assert.AreEqual(expected, contentCache.TryGetValue(new Uri(getUri, UriKind.Relative), out value));
            if (expected)
            {
                Assert.AreEqual(1, value);
            }
            else
            {
                Assert.IsNull(value);
            }
        }

        [Test]
        public void DoesNotCacheIfNotKeepContentAlive()
        {
            var frame = new ModernFrame { KeepContentAlive = false };
            var contentCache = new ModernFrame.ContentCache(frame);
            var uri = new Uri("@/1.xaml", UriKind.Relative);
            contentCache.AddOrUpdate(uri, 1);
            object value = 2;
            Assert.IsFalse(contentCache.TryGetValue(uri, out value));
            Assert.IsNull(value);
        }
    }
}