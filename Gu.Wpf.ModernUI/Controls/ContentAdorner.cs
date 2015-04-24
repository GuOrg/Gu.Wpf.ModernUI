namespace Gu.Wpf.ModernUI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// http://tech.pro/tutorial/856/wpf-tutorial-using-a-visual-collection
    /// </summary>
    public class ContentAdorner : Adorner
    {
        private readonly VisualCollection children;
        private readonly ContentPresenter contentPresenter;

        public ContentAdorner(UIElement adornedElement, RibbonDialog child)
            : base(adornedElement)
        {
            children = new VisualCollection(this);
            this.contentPresenter = new ContentPresenter
            {
                Content = child
            };
            children.Add(this.contentPresenter);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this.contentPresenter.Measure(constraint);
            return constraint;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.contentPresenter.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            return this.contentPresenter.RenderSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return contentPresenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }
    }
}
