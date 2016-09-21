namespace Gu.Wpf.ModernUI.UITests
{
    using System.Runtime.CompilerServices;

    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class NavigationWindowTests : WindowTests
    {
        protected override string AppName { get; } = "NavigationApp.exe";

        protected override string WindowName { get; } = "MainWindow";

        public Button Link1 => this.GetLink();

        public Button Link2 => this.GetLink();

        [Test]
        public void Loads()
        {
            Assert.AreEqual(true, this.Link1.Enabled);
            Assert.AreEqual(false, this.Link2.Enabled);
        }
    }
}