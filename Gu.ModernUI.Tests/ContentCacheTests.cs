namespace Gu.ModernUI.Tests
{
    using System;

    using NUnit.Framework;

    [RequiresSTA]
    public class ContentCacheTests
    {
        [TestCase(@"/1.xaml#1", @"/1.xaml", true)]
        [TestCase(@"/2.xaml", @"/1.xaml", false)]
        [TestCase(@"/1.xaml#1", @"/1.xaml#1", true)]
        [TestCase(@"/1.xaml", @"/Gu.ModernUI.Tests;component/1.xaml", true, Description = "Leaving this red, not sure what is right here", Explicit = true)]
        [TestCase(@"/1.xaml", @"pack://application:,,,/1.xaml", true, Description = "Leaving this red, not sure what is right here")]
        public void AddThenGet(string addUriString, string getUriString, bool expected)
        {
            var frame = new ModernFrame { KeepContentAlive = true };
            var contentCache = new ModernFrame.ContentCache(frame);
            var addUri = new Uri(addUriString, UriKind.RelativeOrAbsolute);
            contentCache.AddOrUpdate(addUri, 1);
            object value;
            var getUri = new Uri(getUriString, UriKind.RelativeOrAbsolute);
            Assert.AreEqual(expected, contentCache.TryGetValue(getUri, out value));
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