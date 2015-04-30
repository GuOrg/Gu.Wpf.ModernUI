namespace Gu.Wpf.ModernUI.Tests
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

        [Test]
        public void SourcesChangedEventName()
        {
            var eventName = LinkCollection.SourcesChangedEventName;
            Assert.IsNotNull(typeof(LinkCollection).GetEvent(eventName));
        }
    }
}
