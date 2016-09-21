namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    public class DemoWindowTests : WindowTests
    {
        protected override string AppName { get; } = "Gu.Wpf.ModernUI.Demo.exe";

        protected override string WindowName { get; } = "Modern UI for WPF Demo";

        [Test]
        public void Loads()
        {
            Assert.NotNull(this.Window);
        }
    }
}
