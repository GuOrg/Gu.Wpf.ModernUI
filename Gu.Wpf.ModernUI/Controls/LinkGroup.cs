namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using Navigation;

    /// <summary>
    /// Represents a named group of links.
    /// </summary>
    [DefaultProperty("Links")]
    [ContentProperty("Links")]
    public class LinkGroup : ButtonBase, INavigator, ILink, IList
    {
        public static readonly DependencyProperty DisplayNameProperty = Link.DisplayNameProperty.AddOwner(typeof(LinkGroup));
        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(LinkGroup));

        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(LinkGroup),
                new FrameworkPropertyMetadata(
                    default(Uri), 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedSourceChanged));
        public static readonly DependencyProperty CanNavigateProperty = Link.CanNavigateProperty.AddOwner(typeof(LinkGroup));
        public static readonly DependencyProperty IsNavigatedToProperty = Link.IsNavigatedToProperty.AddOwner(typeof(LinkGroup));
      
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(LinkGroup), new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));
        
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(
            typeof(LinkGroup),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        internal static readonly DependencyPropertyKey LinksPropertyKey = DependencyProperty.RegisterReadOnly(
            "Links",
            typeof(ModernLinks),
            typeof(LinkGroup),
            new PropertyMetadata(default(ModernLinks)));

        public static readonly DependencyProperty LinksProperty = LinksPropertyKey.DependencyProperty;

        static LinkGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkGroup), new FrameworkPropertyMetadata(typeof(LinkGroup)));
        }

        public LinkGroup()
        {
            SetValue(LinksPropertyKey, new LinkGroupLinks());
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);
            this.Command = LinkCommands.NavigateLink;
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        /// <summary>
        /// Gets the collection of links
        /// </summary>
        public LinkGroupLinks Links
        {
            get { return (LinkGroupLinks)GetValue(LinksProperty); }
        }

        IEnumerable<ILink> INavigator.Links
        {
            get { return this.Links.OfType<ILink>(); }
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
        /// Explicit implementation here to only expose set to consumers of INavigator
        /// </summary>
        ILink INavigator.SelectedLink
        {
            get { return this.SelectedLink; }
            set { SetValue(ModernLinks.SelectedLinkPropertyKey, value); }
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
        /// Explicit implementation here to set current value
        /// </summary>
        Uri INavigator.SelectedSource
        {
            get { return this.SelectedSource; }
            set { SetCurrentValue(SelectedSourceProperty, value); }
        }

        Uri ILink.Source
        {
            get { return this.SelectedSource; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)GetValue(NavigationTargetProperty); }
            set { SetValue(NavigationTargetProperty, value); }
        }

        /// <summary>
        /// Gets if the current linknavigator can navigate to Source
        /// </summary>
        public bool CanNavigate
        {
            get { return (bool)GetValue(CanNavigateProperty); }
            protected set { SetValue(Link.CanNavigatePropertyKey, value); }
        }

        /// <summary>
        /// LinkCommands updates this
        /// </summary>
        bool ILink.CanNavigate
        {
            get { return this.CanNavigate; }
            set { this.CanNavigate = value; }
        }

        /// <summary>
        /// Gets if the current navigationtarget Source == this.Source
        /// </summary>
        public bool IsNavigatedTo
        {
            get { return (bool)GetValue(IsNavigatedToProperty); }
            protected set { SetValue(Link.IsNavigatedToPropertyKey, value); }
        }

        /// <summary>
        /// LinkCommands updates this
        /// </summary>
        bool ILink.IsNavigatedTo
        {
            get { return this.IsNavigatedTo; }
            set { this.IsNavigatedTo = value; }
        }

        public override string ToString()
        {
            var links = string.Join(", ", this.Links.OfType<ILink>().Select(x => x.DisplayName));
            return string.Format("{0}, DisplayName: {1}, Links: {{{2}}}", GetType(), this.DisplayName, links);
        }

        protected override void AddChild(object value)
        {
            this.Links.Items.Add(value);
        }

        protected virtual void OnSelectedSourceChanged(Uri oldSource, Uri newSource)
        {
            if (newSource == null)
            {
                return;
            }
            if (this.Command != LinkCommands.NavigateLink)
            {
                this.Command = LinkCommands.NavigateLink;
            }
        }

        private static void OnSelectedSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LinkGroup)d).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }

        #region IList, this is pretty hacky and nonstandard

        int IList.Add(object value)
        {
            return this.Links.Items.Add(value);
        }

        void IList.Clear()
        {
            this.Links.Items.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Links.Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.Links.Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.Links.Items.Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.Links.Items.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            this.Links.Items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this.Links.Items[index]; }
            set { this.Links.Items[index] = value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.Links.Items.CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return this.Links.Items.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.Links.Items).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.Links.Items).SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Links.Items).GetEnumerator();
        }

        #endregion IList
    }
}
