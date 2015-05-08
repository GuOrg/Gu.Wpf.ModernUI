namespace Gu.Wpf.ModernUI.Tests.Converters
{
    using System.Windows;
    using System.Windows.Data;

    using Gu.Wpf.ModernUI.Internals;

    using NUnit.Framework;

    public class NullOrEmptyStringToVisibilityConverterTests
    {
        [Test]
        public void Convert()
        {
            ModernUIHelper.isInDesignMode = true;
            var converter = new NullOrEmptyStringToVisibilityConverter
                                {
                                    WhenNullOrEmpty = Visibility.Visible,
                                    Else = Visibility.Hidden
                                };
            var convert = ((IValueConverter)converter).Convert(null, null, null, null);
            Assert.AreEqual(Visibility.Visible, convert);
        }
    }
}
