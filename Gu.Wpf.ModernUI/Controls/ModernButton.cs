namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Adds icon content to a standard button.
    /// </summary>
    public class ModernButton
        : Button
    {
        /// <summary>
        /// Identifies the EllipseDiameter property.
        /// </summary>
        public static readonly DependencyProperty EllipseDiameterProperty = DependencyProperty.Register(nameof(EllipseDiameter), typeof(double), typeof(ModernButton), new PropertyMetadata(22D));

        /// <summary>
        /// Identifies the EllipseStrokeThickness property.
        /// </summary>
        public static readonly DependencyProperty EllipseStrokeThicknessProperty = DependencyProperty.Register(nameof(EllipseStrokeThickness), typeof(double), typeof(ModernButton), new PropertyMetadata(1D));

        /// <summary>
        /// Identifies the IconData property.
        /// </summary>
        public static readonly DependencyProperty IconDataProperty = DependencyProperty.Register(nameof(IconData), typeof(Geometry), typeof(ModernButton));

        /// <summary>
        /// Identifies the IconHeight property.
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(ModernButton), new PropertyMetadata(12D));

        /// <summary>
        /// Identifies the IconWidth property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(ModernButton), new PropertyMetadata(12D));

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernButton"/> class.
        /// </summary>
        public ModernButton()
        {
            this.DefaultStyleKey = typeof(ModernButton);
        }

        /// <summary>
        /// Gets or sets the ellipse diameter.
        /// </summary>
        public double EllipseDiameter
        {
            get => (double)this.GetValue(EllipseDiameterProperty);
            set => this.SetValue(EllipseDiameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the ellipse stroke thickness.
        /// </summary>
        public double EllipseStrokeThickness
        {
            get => (double)this.GetValue(EllipseStrokeThicknessProperty);
            set => this.SetValue(EllipseStrokeThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the icon path data.
        /// </summary>
        /// <value>
        /// The icon path data.
        /// </value>
        public Geometry IconData
        {
            get => (Geometry)this.GetValue(IconDataProperty);
            set => this.SetValue(IconDataProperty, value);
        }

        /// <summary>
        /// Gets or sets the icon height.
        /// </summary>
        /// <value>
        /// The icon height.
        /// </value>
        public double IconHeight
        {
            get => (double)this.GetValue(IconHeightProperty);
            set => this.SetValue(IconHeightProperty, value);
        }

        /// <summary>
        /// Gets or sets the icon width.
        /// </summary>
        /// <value>
        /// The icon width.
        /// </value>
        public double IconWidth
        {
            get => (double)this.GetValue(IconWidthProperty);
            set => this.SetValue(IconWidthProperty, value);
        }
    }
}
