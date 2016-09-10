namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    public class DemoWindow : WindowTests
    {
        protected override string WindowName { get; } = "Modern UI for WPF Demo";

        [Test]
        public void Loads()
        {
            Assert.NotNull(this.Window);
        }
    }
}
