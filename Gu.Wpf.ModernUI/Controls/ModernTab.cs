namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    using Gu.Wpf.ModernUI.Internals;
    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// Represents a control that contains multiple pages that share the same space on screen.
    /// </summary>
    public class ModernTab : Control, IList, INavigator
    {
        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = ModernLinks.OrientationProperty.AddOwner(typeof(ModernTab));
        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernTab));
        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                default(Uri), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static readonly DependencyProperty LinksProperty = LinkGroup.LinksPropertyKey.DependencyProperty.AddOwner(typeof(ModernTab));
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(typeof(ModernTab));
        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(typeof(ModernTab));
        static ModernTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernTab), new FrameworkPropertyMetadata(typeof(ModernTab)));
        }

        public ModernTab()
        {
            SetValue(LinkGroup.LinksPropertyKey, new TabLinks());
            var commandBinding = LinkCommands.CreateNavigateLinkCommandBinding(this);
            this.CommandBindings.Add(commandBinding);
            BindingHelper.Bind(
                this,
                LinksProperty,
                ModernLinks.OrientationProperty,
                this,
                OrientationProperty,
                BindingMode.OneWayToSource,
                UpdateSourceTrigger.PropertyChanged); // dunno why dp inheritance does not work here
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
        /// Gets the collection of links
        /// </summary>
        public TabLinks Links
        {
            get { return (TabLinks)GetValue(LinksProperty); }
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
            set { this.SelectedLink = (Link)value; }
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get { return (Uri)GetValue(SelectedSourceProperty); }
            set { SetValue(ModernLinks.SelectedSourceProperty, value); }
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
        /// Gets or sets the ILinkNavigator that manages navigation
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

        #region IList

        public int Add(object value)
        {
            return this.Links.Items.Add(value);
        }

        public void Clear()
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
