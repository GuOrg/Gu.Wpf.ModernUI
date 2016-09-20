namespace Gu.Wpf.ModernUI.Tests.Internals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;

    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    public class DependencyPropertyListenerTests
    {
        [Test]
        public void TracksChanges()
        {
            var changes = new List<DependencyPropertyChangedEventArgs>();
            var textBlock = new TextBlock();
            var listener = new DependencyPropertyListener(textBlock, TextBlock.TextProperty);
            listener.Changed += (_, e) => changes.Add(e);
           
            textBlock.Text = "1";
            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual("", changes.Last().OldValue);
            Assert.AreEqual("1", changes.Last().NewValue);

            textBlock.Text = "2";
            Assert.AreEqual(2, changes.Count);
            Assert.AreEqual("1", changes.Last().OldValue);
            Assert.AreEqual("2", changes.Last().NewValue);
            listener.Dispose();
        }
    }
}
