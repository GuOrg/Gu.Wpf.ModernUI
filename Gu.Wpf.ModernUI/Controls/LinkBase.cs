namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Internals;

    /// <summary>
    /// Baseclass for links
    /// </summary>
    public abstract class LinkBase : Control, ILink, IDisposable
    {
        /// <summary>
        /// Identifies the DisplayName property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName",
            typeof(string),
            typeof(LinkBase),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the SourceProperty property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(LinkBase),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                OnSourceChanged));

        private static readonly DependencyPropertyKey isNavigatedToPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatedTo",
            typeof(bool),
            typeof(LinkBase),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatedToProperty = isNavigatedToPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey canNavigatePropertyKey = DependencyProperty.RegisterReadOnly(
            "CanNavigate",
            typeof(bool),
            typeof(LinkBase),
            new PropertyMetadata(true));

        public static readonly DependencyProperty CanNavigateProperty = canNavigatePropertyKey.DependencyProperty;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(typeof(LinkBase));

        public static readonly DependencyProperty LinkStyleProperty = Modern.LinkStyleProperty.AddOwner(
            typeof(LinkBase),
            new FrameworkPropertyMetadata(
                default(Style),
                FrameworkPropertyMetadataOptions.Inherits,
                OnLinkStyleChanged,
                CoerceLinkStyle));

        public static readonly DependencyProperty NavigationTargetProperty = Modern.NavigationTargetProperty.AddOwner(typeof(LinkBase));

        private readonly NavigationTargetSourceChangedListener sourceChangedListener;

        public LinkBase()
        {
            this.sourceChangedListener = new NavigationTargetSourceChangedListener(this);
            this.sourceChangedListener.Changed += OnNavigationTargetSourceChanged;
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
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Gets if the current linknavigator can navigate to Source
        /// </summary>
        public bool CanNavigate
        {
            get { return (bool)GetValue(CanNavigateProperty); }
            protected set { SetValue(canNavigatePropertyKey, value); }
        }

        /// <summary>
        /// Gets if the current navigationtarget Source == this.Source
        /// </summary>
        public bool IsNavigatedTo
        {
            get { return (bool)GetValue(IsNavigatedToProperty); }
            protected set { SetValue(isNavigatedToPropertyKey, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get { return (ILinkNavigator)GetValue(LinkNavigatorProperty); }
            set { SetValue(LinkNavigatorProperty, value); }
        }

        public bool CanNavigatorNavigate()
        {
            if (this.LinkNavigator == null)
            {
                return false;
            }
            var navigationTarget = this.GetNavigationTarget();
            return this.LinkNavigator.CanNavigate(navigationTarget, this.Source);
        }

        public void Dispose()
        {
            this.sourceChangedListener.Dispose();
        }

        public override string ToString()
        {
            return string.Format("{0}, DisplayName: {1}, Source: {2}, CanNavigate: {3}, IsNavigatedTo: {4}", GetType().Name, this.DisplayName, this.Source, this.CanNavigate, this.IsNavigatedTo);
        }

        protected virtual void OnNavigationTargetSourceChanged(Uri oldSource, Uri newSource)
        {
        }

        protected virtual void OnSourceChanged(Uri oldSource, Uri newSource)
        {
        }

        private static object CoerceLinkStyle(DependencyObject d, object basevalue)
        {
            var link = d as LinkBase;
            if (link == null)
            {
                return basevalue;
            }
            if (link.Style != null)
            {
                return link.Style;
            }
            return basevalue;
        }

        private static void OnLinkStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var link = d as LinkBase;
            if (link == null)
            {
                return;
            }
            var newValue = e.NewValue as Style;
            if (!Equals(link.Style, newValue))
            {
                link.Style = newValue;
            }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var linkBase = (LinkBase)d;
            linkBase.OnSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }

        private void OnNavigationTargetSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            OnNavigationTargetSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }
    }
}