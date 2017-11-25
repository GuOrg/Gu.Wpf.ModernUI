namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;

    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Base class for links
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Needed for xaml")]
    public class ModernLinks : ItemsControl, INavigator, IList
    {
        /// <summary>
        /// Identifies the SelectedSource dependency property.
        /// </summary>
        internal static readonly DependencyPropertyKey SelectedLinkPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectedLink",
            typeof(Link),
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty SelectedLinkProperty = SelectedLinkPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SelectedSourceProperty = DependencyProperty.Register(
            "SelectedSource",
            typeof(Uri),
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(
                Orientation.Horizontal,
                FrameworkPropertyMetadataOptions.Inherits |
                FrameworkPropertyMetadataOptions.AffectsMeasure |
                FrameworkPropertyMetadataOptions.AffectsParentArrange));

        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(ModernLinks),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        static ModernLinks()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernLinks), new FrameworkPropertyMetadata(typeof(ModernLinks)));
        }

        public ModernLinks()
        {
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);
        }

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get => (Link)this.GetValue(SelectedLinkProperty);
            set => this.SetValue(SelectedLinkPropertyKey, value);
        }

        ILink INavigator.SelectedLink
        {
            get => this.SelectedLink;
            set => this.SelectedLink = (Link)value;
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

        /// <summary>
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)this.GetValue(OrientationProperty);
            set => this.SetValue(OrientationProperty, value);
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
        ///
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get => (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            set => this.SetValue(LinkNavigatorProperty, value);
        }

        public IEnumerable<Link> Links => this.Items.OfType<Link>();

        IEnumerable<ILink> INavigator.Links => this.Links;

#pragma warning disable SA1124, SA1201, CA1033 // We use a region for the IList bloat
        #region IList Implementing for convenience from xaml

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => false;

        /// <inheritdoc/>
        int ICollection.Count => this.Items.Count;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => ((ICollection)this.Items).IsSynchronized;

        /// <inheritdoc/>
        object ICollection.SyncRoot => ((ICollection)this.Items).SyncRoot;

        /// <inheritdoc/>
        object IList.this[int index]
        {
            get => this.Items[index];
            set => this.Items[index] = value;
        }

        /// <inheritdoc/>
        public int Add(object value) => this.Items.Add(value);

        /// <inheritdoc/>
        public void Clear() => this.Items.Clear();

        /// <inheritdoc/>
        bool IList.Contains(object value) => this.Items.Contains(value);

        /// <inheritdoc/>
        int IList.IndexOf(object value) => this.Items.IndexOf(value);

        /// <inheritdoc/>
        void IList.Insert(int index, object value) => this.Items.Insert(index, value);

        /// <inheritdoc/>
        void IList.Remove(object value) => this.Items.Remove(value);

        /// <inheritdoc/>
        void IList.RemoveAt(int index) => this.Items.RemoveAt(index);

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index)
        {
            this.Items.CopyTo(array, index);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Items).GetEnumerator();
        }

        #endregion IList
#pragma warning restore SA1124, SA1201, CA1033 // We use a region for the IList bloat
    }
}