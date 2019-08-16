namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    /// <summary>
    /// An adorner that can have content.
    /// </summary>
    public class ContentAdorner : Adorner
    {
        private readonly VisualCollection children;
        private readonly ContentPresenter contentPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element to place the adorner above.</param>
        /// <param name="child">the content</param>
        public ContentAdorner(UIElement adornedElement, ModernPopup child)
            : base(adornedElement)
        {
            this.children = new VisualCollection(this);
            this.contentPresenter = new ContentPresenter
            {
                Content = child,
            };

            this.children.Add(this.contentPresenter);
        }

        /// <inheritdoc/>
        protected override int VisualChildrenCount => this.children.Count;

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size constraint)
        {
            this.contentPresenter.Measure(constraint);
            return constraint;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.contentPresenter.RenderSize;
        }

        /// <inheritdoc/>
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.contentPresenter;
        }
    }
}
