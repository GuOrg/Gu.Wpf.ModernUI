namespace Gu.Wpf.ModernUI.Tests.Misc
{
    using System;
    using System.IO.Packaging;
    using Gu.Wpf.ModernUI.Internals;
    using NUnit.Framework;

    public class UriHelperTests
    {
        [SetUp]
        public void SetUp()
        {
            var uriSchemePack = PackUriHelper.UriSchemePack;
        }

        [TestCase(@"https://msdn.microsoft.com/en-us/library/system.io.packaging.packurihelper.isrelationshipparturi(v=vs.110).aspx", UriKind.Absolute, false)]
        [TestCase(@"/Pages/Settings.xaml", UriKind.RelativeOrAbsolute, true)]
        [TestCase(@"/Gu.Wpf.ModernUI;component/Assets/ModernUI.xaml", UriKind.RelativeOrAbsolute, true)]
        [TestCase(@"pack://application:,,,/;component/Assets/ModernUI.xaml", UriKind.Absolute, true)]
        public void IsResourceUri(string s, UriKind kind, bool expected)
        {
            var uri = new Uri(s, kind);
            var actual = UriHelper.IsResourceUri(uri);
            Assert.AreEqual(expected, actual);
        }
    }
}