using System;
using System.Windows;
using System.Windows.Controls;

namespace FirstFloor.ModernUI.Presentation
{
    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link
        : Button
    {
        /// <summary>
        /// Identifies the DisplayNameProperty property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = Displayable.DisplayNameProperty.AddOwner(
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(string),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                OnDisplayNameChanged));

        /// <summary>
        /// Identifies the SourceProperty property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source",
            typeof(Uri),
            typeof(Link),
            new FrameworkPropertyMetadata(
                default(Uri),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [Obsolete("Kept it for compatibility, it just sets content")]
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
            get
            {
                return (Uri)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        private static void OnDisplayNameChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((Link)o).Content = e.NewValue;
        }
    }
}
