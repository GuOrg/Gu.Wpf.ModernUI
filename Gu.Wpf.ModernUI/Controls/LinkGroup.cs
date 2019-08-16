namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls.Primitives;
    using System.Windows.Markup;
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Represents a named group of links.
    /// </summary>
    [DefaultProperty("Links")]
    [ContentProperty("Links")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Needed for xaml")]
    public class LinkGroup : ButtonBase, INavigator, ILink, IList
    {
        /// <summary>Identifies the <see cref="DisplayName"/> dependency property.</summary>
        public static readonly DependencyProperty DisplayNameProperty = Link.DisplayNameProperty.AddOwner(typeof(LinkGroup));

        /// <summary>Identifies the <see cref="SelectedLink"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(LinkGroup));

        /// <summary>Identifies the <see cref="SelectedSource"/> dependency property.</summary>
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(LinkGroup),
                new FrameworkPropertyMetadata(
                    default(Uri),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (d, e) => ((LinkGroup)d).OnSelectedSourceChanged((Uri)e.OldValue, (Uri)e.NewValue)));

        /// <summary>Identifies the <see cref="CanNavigate"/> dependency property.</summary>
        public static readonly DependencyProperty CanNavigateProperty = Link.CanNavigateProperty.AddOwner(typeof(LinkGroup));

        /// <summary>Identifies the <see cref="IsNavigatedTo"/> dependency property.</summary>
        public static readonly DependencyProperty IsNavigatedToProperty = Link.IsNavigatedToProperty.AddOwner(typeof(LinkGroup));

        /// <summary>Identifies the <see cref="LinkNavigator"/> dependency property.</summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(LinkGroup), new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="NavigationTarget"/> dependency property.</summary>
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(
            typeof(LinkGroup),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="Links"/> dependency property.</summary>
        internal static readonly DependencyPropertyKey LinksPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Links),
            typeof(ModernLinks),
            typeof(LinkGroup),
            new PropertyMetadata(default(ModernLinks)));

        /// <summary>Identifies the <see cref="Links"/> dependency property.</summary>
        public static readonly DependencyProperty LinksProperty = LinksPropertyKey.DependencyProperty;

        static LinkGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinkGroup), new FrameworkPropertyMetadata(typeof(LinkGroup)));
        }

        public LinkGroup()
        {
            this.SetValue(LinksPropertyKey, new LinkGroupLinks());
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
            get => (string)this.GetValue(DisplayNameProperty);
            set => this.SetValue(DisplayNameProperty, value);
        }

        /// <summary>
        /// Gets the collection of links
        /// </summary>
        public LinkGroupLinks Links => (LinkGroupLinks)this.GetValue(LinksProperty);

        IEnumerable<ILink> INavigator.Links => this.Links.OfType<ILink>();

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get => (Link)this.GetValue(SelectedLinkProperty);
            protected set => this.SetValue(ModernLinks.SelectedLinkPropertyKey, value);
        }

        /// <summary>
        /// Explicit implementation here to only expose set to consumers of INavigator
        /// </summary>
        ILink INavigator.SelectedLink
        {
            get { return this.SelectedLink; }
#pragma warning disable WPF0014 // SetValue must use registered type. This is nasty
            set { this.SetValue(ModernLinks.SelectedLinkPropertyKey, value); }
#pragma warning restore WPF0014 // SetValue must use registered type.
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get => (Uri)this.GetValue(SelectedSourceProperty);
            set => this.SetValue(SelectedSourceProperty, value);
        }

        /// <summary>
        /// Explicit implementation here to set current value
        /// </summary>
        Uri INavigator.SelectedSource
        {
            get => this.SelectedSource;
            set => this.SetCurrentValue(SelectedSourceProperty, value);
        }

        Uri ILink.Source => this.SelectedSource;

        /// <summary>
        ///
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get => (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            set => this.SetValue(LinkNavigatorProperty, value);
        }

        /// <summary>
        /// Get or sets the target frame
        /// </summary>
        public ModernFrame NavigationTarget
        {
            get => (ModernFrame)this.GetValue(NavigationTargetProperty);
            set => this.SetValue(NavigationTargetProperty, value);
        }

        /// <summary>
        /// Gets if the current linknavigator can navigate to Source
        /// </summary>
        public bool CanNavigate
        {
            get => (bool)this.GetValue(CanNavigateProperty);
            protected set => this.SetValue(Link.CanNavigatePropertyKey, value);
        }

        /// <summary>
        /// LinkCommands updates this
        /// </summary>
        bool ILink.CanNavigate
        {
            get => this.CanNavigate;
            set => this.CanNavigate = value;
        }

        /// <summary>
        /// Gets if the current navigationtarget Source == this.Source
        /// </summary>
        public bool IsNavigatedTo
        {
            get => (bool)this.GetValue(IsNavigatedToProperty);
            protected set => this.SetValue(Link.IsNavigatedToPropertyKey, value);
        }

        /// <summary>
        /// LinkCommands updates this
        /// </summary>
        bool ILink.IsNavigatedTo
        {
            get => this.IsNavigatedTo;
            set => this.IsNavigatedTo = value;
        }

        public override string ToString()
        {
            var links = string.Join(", ", this.Links.OfType<ILink>().Select(x => x.DisplayName));
            return $"{this.GetType()}, DisplayName: {this.DisplayName}, Links: {{{links}}}";
        }

        /// <summary>
        /// This method is called when button is clicked via IInvokeProvider.
        /// </summary>
        internal void AutomationButtonBaseClick()
        {
            this.OnClick();
        }

        /// <inheritdoc/>
        protected override AutomationPeer OnCreateAutomationPeer() => new LinkGroupAutomationPeer(this);

        /// <inheritdoc/>
        protected override void AddChild(object value)
        {
            this.Links.Items.Add(value);
        }

        /// <summary>This method is invoked when the <see cref="SelectedSourceProperty"/> changes.</summary>
        /// <param name="oldSource">The old value of <see cref="SelectedSourceProperty"/>.</param>
        /// <param name="newSource">The new value of <see cref="SelectedSourceProperty"/>.</param>
        protected virtual void OnSelectedSourceChanged(Uri oldSource, Uri newSource)
        {
            if (newSource == null)
            {
                return;
            }

            if (this.Command != LinkCommands.NavigateLink)
            {
                this.SetCurrentValue(CommandProperty, LinkCommands.NavigateLink);
            }
        }

#pragma warning disable SA1124, SA1201 // We use a region for the IList bloat
        #region IList, this is pretty hacky and nonstandard

        /// <inheritdoc/>
        int ICollection.Count => this.Links.Items.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => ((ICollection)this.Links.Items).IsSynchronized;

        /// <inheritdoc/>
        object ICollection.SyncRoot => ((ICollection)this.Links.Items).SyncRoot;

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => false;

        /// <inheritdoc/>
        object IList.this[int index]
        {
            get => this.Links.Items[index];
            set => this.Links.Items[index] = value;
        }

        /// <inheritdoc/>
        int IList.Add(object value) => this.Links.Items.Add(value);

        /// <inheritdoc/>
        void IList.Clear() => this.Links.Items.Clear();

        /// <inheritdoc/>
        bool IList.Contains(object value) => this.Links.Items.Contains(value);

        /// <inheritdoc/>
        int IList.IndexOf(object value) => this.Links.Items.IndexOf(value);

        /// <inheritdoc/>
        void IList.Insert(int index, object value) => this.Links.Items.Insert(index, value);

        /// <inheritdoc/>
        void IList.Remove(object value) => this.Links.Items.Remove(value);

        /// <inheritdoc/>
        void IList.RemoveAt(int index) => this.Links.Items.RemoveAt(index);

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index) => this.Links.Items.CopyTo(array, index);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.Links.Items).GetEnumerator();

        #endregion IList
#pragma warning restore SA1124, SA1201 // We use a region for the IList bloat
    }
}
