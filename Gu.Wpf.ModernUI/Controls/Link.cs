namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls.Primitives;

    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// A link that navigates to things
    /// </summary>
    public class Link : ButtonBase, ILink
    {
#pragma warning disable SA1202 // Elements must be documented
        /// <summary>
        /// Identifies the DisplayName property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(
            "DisplayName",
            typeof(string),
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        /// <summary>
        /// Identifies the SourceProperty property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSourceChanged));

        internal static readonly DependencyPropertyKey IsNavigatedToPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatedTo",
            typeof(bool),
            typeof(Link),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatedToProperty = IsNavigatedToPropertyKey.DependencyProperty;

        internal static readonly DependencyPropertyKey CanNavigatePropertyKey = DependencyProperty.RegisterReadOnly(
            "CanNavigate",
            typeof(bool),
            typeof(Link),
            new PropertyMetadata(true));

        public static readonly DependencyProperty CanNavigateProperty = CanNavigatePropertyKey.DependencyProperty;

        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(typeof(Link));

#pragma warning restore SA1202 // Elements must be documented

        static Link()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Link), new FrameworkPropertyMetadata(typeof(Link)));
        }

        public Link()
        {
            this.Command = LinkCommands.NavigateLink; // For some reason the command must be set in ctor for it to work with title links.
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
        /// Gets or sets the source uri.
        /// </summary>
        /// <value>The source.</value>
        public Uri Source
        {
            get => (Uri)this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }

        /// <summary>
        /// Gets if the current linknavigator can navigate to Source
        /// </summary>
        public bool CanNavigate
        {
            get => (bool)this.GetValue(CanNavigateProperty);
            protected set => this.SetValue(CanNavigatePropertyKey, value);
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
            protected set => this.SetValue(IsNavigatedToPropertyKey, value);
        }

        /// <summary>
        /// LinkCommands updates this
        /// </summary>
        bool ILink.IsNavigatedTo
        {
            get => this.IsNavigatedTo;
            set => this.IsNavigatedTo = value;
        }

        /// <summary>
        ///
        /// </summary>
        public ILinkNavigator LinkNavigator
        {
            get => (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            set => this.SetValue(LinkNavigatorProperty, value);
        }

        public override string ToString() => $"{this.GetType().Name}, DisplayName: {this.DisplayName}, Source: {this.Source}, CanNavigate: {this.CanNavigate}, IsNavigatedTo: {this.IsNavigatedTo}";

        /// <summary>
        /// This method is called when button is clicked via IInvokeProvider.
        /// </summary>
        internal void AutomationButtonBaseClick()
        {
            this.OnClick();
        }

        /// <inheritdoc/>
        protected override AutomationPeer OnCreateAutomationPeer() => new LinkAutomationPeer(this);

        protected virtual void OnSourceChanged(Uri oldSource, Uri newSource)
        {
            if (newSource != null && this.Command != LinkCommands.NavigateLink)
            {
                this.SetCurrentValue(CommandProperty, LinkCommands.NavigateLink);
            }
        }

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Link)d).OnSourceChanged((Uri)e.OldValue, (Uri)e.NewValue);
        }
    }
}