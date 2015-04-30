namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using Internals;

    /// <summary>
    /// Represents an observable collection of links.
    /// </summary>
    public class LinkCollection : ObservableCollection<Link>
    {
        public static readonly string SourcesChangedEventName = "SourcesChanged";
        private readonly List<DependencyPropertyListener> sourceListeners = new List<DependencyPropertyListener>();
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection"/> class.
        /// </summary>
        public LinkCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCollection"/> class that contains specified links.
        /// </summary>
        /// <param name="links">The links that are copied to this collection.</param>
        public LinkCollection(IEnumerable<Link> links)
        {
            if (links == null)
            {
                throw new ArgumentNullException("links");
            }
            foreach (var link in links)
            {
                Add(link);
            }
        }

        public event EventHandler SourcesChanged;

        protected override void ClearItems()
        {
            base.ClearItems();
            foreach (var listener in this.sourceListeners)
            {
                listener.Changed -= OnSourceChanged;
                listener.Dispose();
            }
            this.sourceListeners.Clear();
            OnSourcesChanged();
        }

        protected override void InsertItem(int index, Link item)
        {
            base.InsertItem(index, item);
            AddSourceListener(item);
            OnSourcesChanged();
        }

        protected override void RemoveItem(int index)
        {
            var link = base[index];
            base.RemoveItem(index);
            RemoveSourceListener(link);
            OnSourcesChanged();
        }

        protected override void SetItem(int index, Link item)
        {
            var link = base[index];
            RemoveSourceListener(link);
            base.SetItem(index, item);
            AddSourceListener(item);
            OnSourcesChanged();
        }

        protected virtual void OnSourcesChanged()
        {
            var handler = this.SourcesChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnSourcesChanged();
        }

        private void AddSourceListener(Link item)
        {
            var listener = new DependencyPropertyListener(item, LinkBase.SourceProperty);
            listener.Changed += OnSourceChanged;
            this.sourceListeners.Add(listener);
        }

        private void RemoveSourceListener(Link link)
        {
            var listener = this.sourceListeners.FirstOrDefault(x => x.Binding.Source == link);
            if (listener != null)
            {
                listener.Changed -= OnSourceChanged;
                listener.Dispose();
                this.sourceListeners.Remove(listener);
            }
        }
    }
}
