﻿namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Represents a control that contains multiple pages that share the same space on screen.
    /// </summary>
    [TemplatePart(Name = PART_ContentFrame, Type = typeof(ModernFrame))]
    [ContentProperty("Links")]
    public class ModernTab : Control, IList, INavigator
    {
        public const string PART_ContentFrame = "PART_ContentFrame";
        public static readonly DependencyProperty OrientationProperty = ModernLinks.OrientationProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                Orientation.Horizontal,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty SelectedLinkProperty = ModernLinks.SelectedLinkProperty.AddOwner(typeof(ModernTab));

        public static readonly DependencyProperty SelectedSourceProperty = ModernLinks.SelectedSourceProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty LinksProperty = LinkGroup.LinksPropertyKey.DependencyProperty.AddOwner(typeof(ModernTab));

        public static readonly DependencyProperty NavigationTargetProperty =
            Modern.NavigationTargetProperty.AddOwner(
                typeof(ModernTab),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.None));

        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(
            typeof(ModernTab),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        private static readonly DependencyProperty SelectedLinkProxyProperty = DependencyProperty.Register(
            "SelectedLinkProxy", 
            typeof(Link), 
            typeof(ModernTab),
            new PropertyMetadata(null, OnSelectedLinkProxyChanged));

        static ModernTab()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernTab), new FrameworkPropertyMetadata(typeof(ModernTab)));
        }

        public ModernTab()
        {
            var tabLinks = new TabLinks();
            this.SetValue(LinkGroup.LinksPropertyKey, tabLinks);

            BindingHelper.Bind(
                this, LinksProperty, ModernLinks.OrientationProperty,
                this, OrientationProperty,
                BindingMode.OneWayToSource,
                UpdateSourceTrigger.PropertyChanged); // dunno why dp inheritance does not work here

            BindingHelper.Bind(
                this, LinksProperty, ModernLinks.NavigationTargetProperty,
                this, NavigationTargetProperty,
                BindingMode.OneWayToSource,
                UpdateSourceTrigger.PropertyChanged);

            BindingHelper.Bind(
                this, LinksProperty, ModernLinks.SelectedSourceProperty,
                this, SelectedSourceProperty,
                BindingMode.TwoWay,
                UpdateSourceTrigger.PropertyChanged);

            BindingHelper.Bind(
                this, LinksProperty, ModernLinks.SelectedLinkProperty,
                this, SelectedLinkProxyProperty,
                BindingMode.OneWay,
                UpdateSourceTrigger.PropertyChanged);
        }

        /// <summary>
        /// Gets or sets a value indicating how the tab should be rendered.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets the collection of links
        /// </summary>
        public TabLinks Links => (TabLinks)this.GetValue(LinksProperty);

        IEnumerable<ILink> INavigator.Links => this.Links != null ? this.Links.Links : Enumerable.Empty<ILink>();

        /// <summary>
        /// Gets or sets the source URI of the selected link.
        /// </summary>
        /// <value>The source URI of the selected link.</value>
        public Link SelectedLink
        {
            get { return (Link)this.GetValue(SelectedLinkProperty); }
            protected set { this.SetValue(ModernLinks.SelectedLinkPropertyKey, value); }
        }

        ILink INavigator.SelectedLink
        {
            get { return this.Links?.SelectedLink; }
            set { /* Nop */ }
        }

        public ModernFrame NavigationTarget
        {
            get { return (ModernFrame)this.GetValue(NavigationTargetProperty); }
            set { this.SetValue(NavigationTargetProperty, value); }
        }

        /// <summary>
        /// Get or sets the Uri of the selected Link
        /// </summary>
        public Uri SelectedSource
        {
            get { return (Uri)this.GetValue(SelectedSourceProperty); }
            set { this.SetValue(ModernLinks.SelectedSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ILinkNavigator that manages navigation
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)this.GetValue(LinkNavigatorProperty); }
            set { this.SetValue(LinkNavigatorProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            this.NavigationTarget = this.GetTemplateChild(PART_ContentFrame) as ModernFrame;
            base.OnApplyTemplate();
        }

        private static void OnSelectedLinkProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ModernTab)d).SelectedLink = (Link)e.NewValue;
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

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

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

        int ICollection.Count => this.Links.Items.Count;

        bool ICollection.IsSynchronized => ((ICollection)this.Links.Items).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)this.Links.Items).SyncRoot;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Links.Items).GetEnumerator();
        }

        #endregion IList
    }
}
