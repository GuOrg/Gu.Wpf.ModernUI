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
    using Navigation;

    /// <summary>
    /// Base class for links
    /// </summary>
    [DefaultProperty("Items")]
    [ContentProperty("Items")]
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
            get { return (Link)GetValue(SelectedLinkProperty); }
            set { SetValue(SelectedLinkPropertyKey, value); }
        }

        ILink INavigator.SelectedLink
        {
            get { return this.SelectedLink; }
            set { this.SelectedLink = (Link)value; }
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

        /// <summary>
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
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
        /// 
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        public IEnumerable<Link> Links
        {
            get { return this.Items.OfType<Link>(); }
        }

        IEnumerable<ILink> INavigator.Links
        {
            get { return this.Links; }
        }

        #region IList Implementing for convenience from xaml

        public int Add(object value)
        {
            return this.Items.Add(value);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Items.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return this.Items.IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            this.Items.Insert(index, value);
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
            this.Items.Remove(value);
        }

        void IList.RemoveAt(int index)
        {
            this.Items.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this.Items[index]; }
            set { this.Items[index] = value; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.Items.CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return this.Items.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.Items).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.Items).SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Items).GetEnumerator();
        }

        #endregion IList
    }
}