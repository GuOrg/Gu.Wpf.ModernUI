namespace Gu.Wpf.ModernUI.UITests
{
    using System.Runtime.CompilerServices;

    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.InputDevices;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public abstract class WindowTests
    {
        private Application application;

        public static Window StaticWindow { get; private set; }

        protected Window Window => StaticWindow;

        protected abstract string AppName { get; }

        protected abstract string WindowName { get; }

        public void RestartApplication()
        {
            this.OneTimeTearDown();
            this.OneTimeSetUp();
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(this.AppName, this.WindowName));
            StaticWindow = this.application.GetWindow(this.WindowName);
            //this.SaveScreenshotToArtifacsDir("start");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Keyboard.Instance.LeaveAllKeys();
            //this.SaveScreenshotToArtifacsDir("finish");
            this.application?.Dispose();
            StaticWindow = null;
        }

        protected Button GetLink([CallerMemberName]string name = null)
        {
            return this.Window.Get<Button>(name);
        }

        protected void PressTab()
        {
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
        }

        protected void AssertEnabled(params Button[] links)
        {
            CollectionAssert.AllItemsAreUnique(links);
            foreach (var button in links)
            {
                Assert.IsTrue(button.Enabled, $"Expected button {button.Text} to be enabled.");
                if (button.AutomationElement.Current.ClassName != "Button")
                {
                    Assert.AreEqual("IsNavigatedTo: False", button.ItemStatus, $"Expected button {button.Text} to not be navigated to.");
                }
            }
        }

        protected void AssertDisabled(params Button[] links)
        {
            CollectionAssert.AllItemsAreUnique(links);
            foreach (var button in links)
            {
                Assert.IsFalse(button.Enabled, $"Expected button {button.Text} to be disabled.");
                if (button.AutomationElement.Current.ClassName != "Button")
                {
                    Assert.AreEqual("IsNavigatedTo: True", button.ItemStatus, $"Expected button {button.Text} to be navigated to.");
                }
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void SaveScreenshotToArtifacsDir(string suffix)
        {
            var fileName = System.IO.Path.Combine(Info.ArtifactsDirectory(), $"{this.WindowName}_{suffix}.png");
            using (var image = new TestStack.White.ScreenCapture().CaptureDesktop())
            {
                image.Save(fileName);
            }
        }
    }
}