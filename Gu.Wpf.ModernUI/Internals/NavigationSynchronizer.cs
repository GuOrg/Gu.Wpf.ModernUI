namespace Gu.Wpf.ModernUI.Internals
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// This class manages synchronisation between SelectedSource, SelectedLink & NavigationTarget.Source
    /// </summary>
    public sealed class NavigationSynchronizer : IDisposable
    {
        private readonly INavigationSource links;
        private readonly DependencyPropertyListener linkListener;
        private readonly DependencyPropertyListener selectedSourceListener;
        private readonly DependencyPropertyListener navigationTargetListener;
        private readonly NavigationTargetSourceChangedListener targetSourceListener;

        private NavigationSynchronizer(INavigationSource links)
        {
            this.links = links;
            this.linkListener = new DependencyPropertyListener((DependencyObject)links, ModernLinks.LinksProperty);
            this.linkListener.Changed += OnLinksChanged;

            this.selectedSourceListener = new DependencyPropertyListener((DependencyObject)links, ModernLinks.SelectedSourceProperty);
            this.selectedSourceListener.Changed += OnSelectedSourceChanged;

            this.navigationTargetListener = new DependencyPropertyListener((DependencyObject)links, Modern.NavigationTargetProperty);
            this.navigationTargetListener.Changed += OnNavigationTargetChanged;

            this.targetSourceListener = new NavigationTargetSourceChangedListener((DependencyObject)links);
            this.targetSourceListener.Changed += OnNavigationTargetSourceChanged;
        }

        public static NavigationSynchronizer Create<T>(T links) where T : DependencyObject, INavigationSource
        {
            return new NavigationSynchronizer(links);
        }

        public void Dispose()
        {
            this.linkListener.Dispose();
            this.selectedSourceListener.Dispose();
            this.navigationTargetListener.Dispose();
            this.targetSourceListener.Dispose();
        }

        private void OnLinksChanged(object d, DependencyPropertyChangedEventArgs e)
        {
            var oldLinks = e.OldValue as LinkCollection;
            if (oldLinks != null)
            {
                WeakEventManager<LinkCollection, EventArgs>.RemoveHandler(oldLinks, LinkCollection.SourcesChangedEventName, OnLinkCollectionSourcesChanged);
            }
            var newLinks = e.NewValue as LinkCollection;
            if (newLinks != null)
            {
                WeakEventManager<LinkCollection, EventArgs>.AddHandler(newLinks, LinkCollection.SourcesChangedEventName, OnLinkCollectionSourcesChanged);
            }
            OnLinkCollectionSourcesChanged(null, null);
        }

        private void OnLinkCollectionSourcesChanged(object sender, EventArgs e)
        {
            if (this.links.Links == null || !this.links.Links.Any())
            {
                return;
            }
            if (this.links.SelectedSource != null)
            {
                this.links.SelectedLink = this.links.Links.FirstOrDefault(l => Equals(l.Source, this.links.SelectedSource));
                return;
            }
            var link = this.links.Links.FirstOrDefault();
            this.links.SelectedSource = link != null
                                            ? link.Source
                                            : null;
        }

        private void OnSelectedSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.links.Links == null || !this.links.Links.Any())
            {
                this.links.SelectedLink = null;
                return;
            }
            this.links.SelectedLink = this.links.Links.FirstOrDefault(l => Equals(l.Source, e.NewValue));
        }

        private void OnNavigationTargetChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var modernFrame = e.NewValue as ModernFrame;
            if (modernFrame != null)
            {
                var selectedLink = this.links.SelectedLink;
                if (selectedLink != null && selectedLink.CanNavigatorNavigate())
                {
                    selectedLink.Navigate();
                }
            }
        }

        private void OnNavigationTargetSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var source = e.NewValue as Uri;
            if (this.links.Links == null || !this.links.Links.Any())
            {
                if (this.links.SelectedSource != null)
                {
                    Debug.WriteLine("{0} {1} this.links.Links == null || !this.links.Links.Any() this.links.SelectedSource = null", GetType().Name, this.links.ToString());
                    this.links.SelectedSource = null;
                }
                return;
            }
            var link = this.links.Links.FirstOrDefault(l => Equals(l.Source, source)); // Same frame can be used by many linkcollections in a menu.
            if (link != null)
            {
                if (!Equals(this.links.SelectedSource, source))
                {
                    Debug.WriteLine("{0} {1} link != null this.links.SelectedSource = {2}", GetType().Name, this.links.ToString(), source != null ? source.ToString() : null);
                    this.links.SelectedSource = source;
                }
            }
        }
    }
}