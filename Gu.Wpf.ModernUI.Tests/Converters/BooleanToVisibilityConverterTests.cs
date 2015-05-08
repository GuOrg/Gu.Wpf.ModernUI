namespace Gu.Wpf.ModernUI.Tests.Converters
{
    using System.Windows;
    using System.Windows.Data;

    using Gu.Wpf.ModernUI.Internals;

    using NUnit.Framework;

    public class BooleanToVisibilityConverterTests
    {
        [Test]
        public void Convert()
        {
            ModernUIHelper.isInDesignMode = true;
            var converter = new BooleanToVisibilityConverter
                                {
                                    WhenNull = Visibility.Visible,
                                    WhenFalse = Visibility.Hidden
                                };
            var convert = ((IValueConverter)converter).Convert(null, null, null, null);
            Assert.AreEqual(Visibility.Visible, convert);
        }
    }
}