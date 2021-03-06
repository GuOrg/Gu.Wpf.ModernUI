namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Windows.Navigation;
    using Gu.Wpf.ModernUI.BBCode;

    using Gu.ModernUI.Interfaces;

    using Gu.Wpf.ModernUI.Navigation;

    /// <summary>
    /// A lighweight control for displaying small amounts of rich formatted BBCode content.
    /// </summary>
    [ContentProperty("BBCode")]
    public class BBCodeBlock
        : TextBlock
    {
        /// <summary>Identifies the <see cref="BBCode"/> dependency property.</summary>
        public static readonly DependencyProperty BBCodeProperty = DependencyProperty.Register(nameof(BBCode), typeof(string), typeof(BBCodeBlock), new PropertyMetadata(OnBBCodeChanged));

        /// <summary>Identifies the <see cref="LinkNavigator"/> dependency property.</summary>
        public static readonly DependencyProperty LinkNavigatorProperty = Modern.LinkNavigatorProperty.AddOwner(typeof(BBCodeBlock), new FrameworkPropertyMetadata(new DefaultLinkNavigator(), OnLinkNavigatorChanged));

        private bool dirty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BBCodeBlock"/> class.
        /// </summary>
        public BBCodeBlock()
        {
            // ensures the implicit BBCodeBlock style is used
            this.DefaultStyleKey = typeof(BBCodeBlock);

            this.AddHandler(FrameworkContentElement.LoadedEvent, new RoutedEventHandler(this.OnLoaded));
            this.AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(this.OnRequestNavigate));
        }

        /// <summary>
        /// Gets or sets the BB code.
        /// </summary>
        /// <value>The BB code.</value>
        public string BBCode
        {
            get => (string)this.GetValue(BBCodeProperty);
            set => this.SetValue(BBCodeProperty, value);
        }

        /// <summary>
        /// Gets or sets the link navigator.
        /// </summary>
        /// <value>The link navigator.</value>
        public ILinkNavigator LinkNavigator
        {
            get => (ILinkNavigator)this.GetValue(LinkNavigatorProperty);
            set => this.SetValue(LinkNavigatorProperty, value);
        }

        private static void OnBBCodeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((BBCodeBlock)o).UpdateDirty();
        }

        private static void OnLinkNavigatorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((BBCodeBlock)o).UpdateDirty();
        }

        private void OnLoaded(object o, EventArgs e)
        {
            this.Update();
        }

        private void UpdateDirty()
        {
            this.dirty = true;
            this.Update();
        }

        private void Update()
        {
            if (!this.IsLoaded || !this.dirty || this.LinkNavigator == null)
            {
                return;
            }

            var bbcode = this.BBCode;

            this.Inlines.Clear();

            if (!string.IsNullOrWhiteSpace(bbcode))
            {
                Inline inline;
                try
                {
                    var parser = new BBCodeParser(bbcode, this)
                    {
                        Commands = this.LinkNavigator.Commands,
                    };
                    inline = parser.Parse();
                }
                catch (Exception)
                {
                    // parsing failed, display BBCode value as-is
                    inline = new Run { Text = bbcode };
                }

                this.Inlines.Add(inline);
            }

            this.dirty = false;
        }

        private void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                // perform navigation using the link navigator
                var frame = NavigationHelper.FindFrame(e.Target, this);
                this.LinkNavigator.Navigate(frame, e.Uri);
            }
            catch (Exception error)
            {
                // display navigation failures
                ModernDialog.ShowMessage(error.Message, Properties.Resources.NavigationFailed, MessageBoxButtons.OK);
            }
        }
    }
}
