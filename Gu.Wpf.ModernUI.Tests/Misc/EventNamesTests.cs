namespace Gu.Wpf.ModernUI.Tests.Misc
{
    using NUnit.Framework;

    public class EventNamesTests
    {
        [Test]
        public void NavigatedEventName()
        {
            var eventName = ModernFrame.NavigatedEventName;
            Assert.IsNotNull(typeof(ModernFrame).GetEvent(eventName));
        }
    }
}
