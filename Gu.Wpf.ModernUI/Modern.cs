namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    public static class Modern
    {
        /// <summary>
        /// The <see cref="IContentLoader"/> to use when navigating links.
        /// </summary>
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.RegisterAttached(
            "ContentLoader",
            typeof(IContentLoader),
            typeof(Modern),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// The <see cref="ILinkNavigator"/> to use when navigating links.
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = DependencyProperty.RegisterAttached(
            "LinkNavigator",
            typeof(ILinkNavigator),
            typeof(Modern),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty NavigationTargetProperty =
            DependencyProperty.RegisterAttached(
                "NavigationTarget",
                typeof(ModernFrame),
                typeof(Modern),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty LinkStyleProperty = DependencyProperty.RegisterAttached(
            "LinkStyle",
            typeof(Style),
            typeof(Modern),
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits, OnLinkStyleChanged, CoerceLinkStyle));

        /// <summary>Helper for setting <see cref="ContentLoaderProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="ContentLoaderProperty"/> on.</param>
        /// <param name="value">ContentLoader property value.</param>
        public static void SetContentLoader(this DependencyObject element, IContentLoader value)
        {
            element.SetValue(ContentLoaderProperty, value);
        }

        /// <summary>Helper for getting <see cref="ContentLoaderProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="ContentLoaderProperty"/> from.</param>
        /// <returns>ContentLoader property value.</returns>
        public static IContentLoader GetContentLoader(this DependencyObject element)
        {
            return (IContentLoader)element.GetValue(ContentLoaderProperty);
        }

        /// <summary>Helper for setting <see cref="LinkNavigatorProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="LinkNavigatorProperty"/> on.</param>
        /// <param name="value">LinkNavigator property value.</param>
        public static void SetLinkNavigator(this DependencyObject element, ILinkNavigator value)
        {
            element.SetValue(LinkNavigatorProperty, value);
        }

        /// <summary>Helper for getting <see cref="LinkNavigatorProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="LinkNavigatorProperty"/> from.</param>
        /// <returns>LinkNavigator property value.</returns>
        public static ILinkNavigator GetLinkNavigator(this DependencyObject element)
        {
            return (ILinkNavigator)element.GetValue(LinkNavigatorProperty);
        }

        /// <summary>Helper for setting <see cref="NavigationTargetProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="NavigationTargetProperty"/> on.</param>
        /// <param name="value">NavigationTarget property value.</param>
        public static void SetNavigationTarget(this DependencyObject element, ModernFrame value)
        {
            element.SetValue(NavigationTargetProperty, value);
        }

        /// <summary>Helper for getting <see cref="NavigationTargetProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="NavigationTargetProperty"/> from.</param>
        /// <returns>NavigationTarget property value.</returns>
        public static ModernFrame GetNavigationTarget(this DependencyObject element)
        {
            return (ModernFrame)element.GetValue(NavigationTargetProperty);
        }

        /// <summary>Helper for setting <see cref="LinkStyleProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="LinkStyleProperty"/> on.</param>
        /// <param name="value">LinkStyle property value.</param>
        public static void SetLinkStyle(this DependencyObject element, Style value)
        {
            element.SetValue(LinkStyleProperty, value);
        }

        /// <summary>Helper for getting <see cref="LinkStyleProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="LinkStyleProperty"/> from.</param>
        /// <returns>LinkStyle property value.</returns>
        public static Style GetLinkStyle(this DependencyObject element)
        {
            return (Style)element.GetValue(LinkStyleProperty);
        }

        private static object CoerceLinkStyle(DependencyObject d, object basevalue)
        {
            var link = d as Link;
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
            var link = (Link)d;
            if (link != null)
            {
                var newValue = e.NewValue as Style;
                if (!Equals(link.Style, newValue))
                {
                    link.SetCurrentValue(FrameworkElement.StyleProperty, newValue);
                }
            }
        }
    }
}
