namespace Gu.Wpf.ModernUI.Tests.Internals
{
    using System;

    using NUnit.Framework;

    [RequiresSTA]
    public class ContentCacheTests
    {
        [TestCase(@"/1.xaml#1", @"/1.xaml", true)]
        [TestCase(@"/2.xaml", @"/1.xaml", false)]
        [TestCase(@"/1.xaml#1", @"/1.xaml#1", true)]
        [TestCase(@"/1.xaml", @"/Gu.Wpf.ModernUI.Tests;component/1.xaml", true, Description = "Leaving this red, not sure what is right here", Explicit = true)]
        [TestCase(@"/1.xaml", @"pack://application:,,,/1.xaml", true, Description = "Leaving this red, not sure what is right here")]
        public void AddThenGet(string addUriString, string getUriString, bool expected)
        {
            var contentCache = new ContentCache();
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
    }
}