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
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits, OnLinkStyleChanged, OnLinkStyleCoerce));

        public static void SetContentLoader(this DependencyObject element, IContentLoader value)
        {
            element.SetValue(ContentLoaderProperty, value);
        }

        public static IContentLoader GetContentLoader(this DependencyObject element)
        {
            return (IContentLoader)element.GetValue(ContentLoaderProperty);
        }

        public static void SetLinkNavigator(this DependencyObject element, ILinkNavigator value)
        {
            element.SetValue(LinkNavigatorProperty, value);
        }

        public static ILinkNavigator GetLinkNavigator(this DependencyObject element)
        {
            return (ILinkNavigator)element.GetValue(LinkNavigatorProperty);
        }

        public static void SetNavigationTarget(this DependencyObject element, ModernFrame value)
        {
            element.SetValue(NavigationTargetProperty, value);
        }

        public static ModernFrame GetNavigationTarget(this DependencyObject element)
        {
            return (ModernFrame)element.GetValue(NavigationTargetProperty);
        }

        public static void SetLinkStyle(this DependencyObject element, Style value)
        {
            element.SetValue(LinkStyleProperty, value);
        }

        public static Style GetLinkStyle(this DependencyObject element)
        {
            return (Style)element.GetValue(LinkStyleProperty);
        }

        private static object OnLinkStyleCoerce(DependencyObject d, object basevalue)
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
            var link = d as Link;
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
    }
}
