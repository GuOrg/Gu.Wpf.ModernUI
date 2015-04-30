namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Represents a displayable link.
    /// </summary>
    public class Link : LinkBase
    {
        static Link()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Link), new FrameworkPropertyMetadata(typeof(Link)));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            Navigate();
            base.OnMouseLeftButtonDown(e);
        }

        internal void Navigate()
        {
            var target = this.GetNavigationTarget();
            if (this.LinkNavigator != null && this.LinkNavigator.CanNavigate(target, this.Source))
            {
                this.LinkNavigator.Navigate(target, this.Source);
            }
        }

        protected override void OnSourceChanged(Uri oldSource, Uri newSource)
        {
            var frame = this.GetNavigationTarget();
            this.IsNavigatedTo = frame != null && Equals(frame.Source, newSource);
            this.CanNavigate = CanNavigatorNavigate();
        }

        protected override void OnNavigationTargetSourceChanged(Uri oldSource, Uri newSource)
        {
            this.IsNavigatedTo = Equals(this.Source, newSource);
            this.CanNavigate = CanNavigatorNavigate();
        }
    }
}
