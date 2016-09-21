namespace Gu.Wpf.ModernUI.UITests
{
    using NUnit.Framework;

    public class NavigationWindowTests : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp";

        protected override string WindowName { get; } = "MainWindow";

        [Test]
        public void Loads()
        {
            Assert.NotNull(this.Window);
        }
    }
}