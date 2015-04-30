namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using Internals;

    /// <summary>
    /// Represents a named group of links.
    /// </summary>
    [ContentProperty("Links")]
    public class LinkGroup : LinkBase, INavigationSource
    {
        /// <summary>
        /// Identifies the Links dependency property.
        /// </summary>
        public static readonly DependencyProperty LinksProperty = ModernLinks.LinksProperty.AddOwner(typeof(LinkGroup));
        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(LinkGroup));
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(typeof(LinkGroup));
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(typeof(LinkGroup));
        private readonly NavigationSynchronizer synchronizer;

        static LinkGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkGroup), new FrameworkPropertyMetadata(typeof(LinkGroup)));
        }

        public LinkGroup()
        {
            this.synchronizer = NavigationSynchronizer.Create(this);
            SetValue(LinksProperty, new LinkCollection());
        }

        /// <summary>
        /// Gets or sets the collection of links that define the available content in this tab.
        /// </summary>
        public LinkCollection Links
        {
            get { return (LinkCollection)GetValue(LinksProperty); }
            set { SetValue(LinksProperty, value); }
        }

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get { return (Link)GetValue(SelectedLinkProperty); }
            protected set { SetValue(ModernLinks.SelectedLinkPropertyKey, value); }
        }

        /// <summary>
        /// Hacking access for synchronizer like this
        /// </summary>
        Link INavigationSource.SelectedLink
        {
            get { return this.SelectedLink; }
            set { this.SelectedLink = value; }
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(SelectedSourceProperty, value); }
        }

        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)GetValue(NavigationTargetProperty); }
            set { SetValue(NavigationTargetProperty, value); }
        }

        public void Dispose()
        {
            this.synchronizer.Dispose();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (this.LinkNavigator == null)
            {
                base.OnMouseLeftButtonDown(e);
                return;
            }
            var target = this.GetNavigationTarget();
            if (this.Source != null)
            {
                if (this.LinkNavigator.CanNavigate(target, this.Source))
                {
                    this.LinkNavigator.Navigate(target, this.Source);
                }
            }
            else
            {
                if (this.SelectedLink == null)
                {
                    var link = this.Links.FirstOrDefault(x => x.Source != null);
                    this.SelectedLink = link;
                }
                if (this.SelectedLink != null)
                {
                    if (this.LinkNavigator.CanNavigate(target, this.SelectedLink.Source))
                    {
                        this.LinkNavigator.Navigate(target, this.SelectedLink.Source);
                    }
                }
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnNavigationTargetSourceChanged(Uri oldSource, Uri newSource)
        {
            var navigationTarget = this.GetNavigationTarget();
            if (this.LinkNavigator == null)
            {
                this.CanNavigate = false;
            }
            else
            {
                if (this.Source != null)
                {
                    this.CanNavigate = this.LinkNavigator.CanNavigate(navigationTarget, this.Source);
                    this.IsNavigatedTo = navigationTarget != null && Equals(navigationTarget.Source, this.Source);
                }
                else if (this.Links != null)
                {
                    this.CanNavigate = this.Links.Any(l => this.LinkNavigator.CanNavigate(navigationTarget, l.Source));
                    this.IsNavigatedTo = navigationTarget != null && this.Links.Any(l => Equals(navigationTarget.Source, l.Source));
                }
                else
                {
                    this.CanNavigate = false;
                    this.IsNavigatedTo = false;
                }
            }
        }
    }
}
