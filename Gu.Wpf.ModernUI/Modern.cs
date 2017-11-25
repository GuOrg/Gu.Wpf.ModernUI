namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    public static class Modern
    {
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.RegisterAttached(
            "ContentLoader",
            typeof(IContentLoader),
            typeof(Modern),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits));

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

        /// <summary>
        /// Helper for setting ContentLoader property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set ContentLoader property on.</param>
        /// <param name="value">ContentLoader property value.</param>
        public static void SetContentLoader(this DependencyObject element, IContentLoader value)
        {
            element.SetValue(ContentLoaderProperty, value);
        }

        /// <summary>
        /// Helper for reading ContentLoader property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read ContentLoader property from.</param>
        /// <returns>ContentLoader property value.</returns>
        public static IContentLoader GetContentLoader(this DependencyObject element)
        {
            return (IContentLoader)element.GetValue(ContentLoaderProperty);
        }

        /// <summary>
        /// Helper for setting LinkNavigator property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set LinkNavigator property on.</param>
        /// <param name="value">LinkNavigator property value.</param>
        public static void SetLinkNavigator(this DependencyObject element, ILinkNavigator value)
        {
            element.SetValue(LinkNavigatorProperty, value);
        }

        /// <summary>
        /// Helper for reading LinkNavigator property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read LinkNavigator property from.</param>
        /// <returns>LinkNavigator property value.</returns>
        public static ILinkNavigator GetLinkNavigator(this DependencyObject element)
        {
            return (ILinkNavigator)element.GetValue(LinkNavigatorProperty);
        }

        /// <summary>
        /// Helper for setting NavigationTarget property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set NavigationTarget property on.</param>
        /// <param name="value">NavigationTarget property value.</param>
        public static void SetNavigationTarget(this DependencyObject element, ModernFrame value)
        {
            element.SetValue(NavigationTargetProperty, value);
        }

        /// <summary>
        /// Helper for reading NavigationTarget property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read NavigationTarget property from.</param>
        /// <returns>NavigationTarget property value.</returns>
        public static ModernFrame GetNavigationTarget(this DependencyObject element)
        {
            return (ModernFrame)element.GetValue(NavigationTargetProperty);
        }

        /// <summary>
        /// Helper for setting LinkStyle property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set LinkStyle property on.</param>
        /// <param name="value">LinkStyle property value.</param>
        public static void SetLinkStyle(this DependencyObject element, Style value)
        {
            element.SetValue(LinkStyleProperty, value);
        }

        /// <summary>
        /// Helper for reading LinkStyle property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read LinkStyle property from.</param>
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
            var link = (Link) d;
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
