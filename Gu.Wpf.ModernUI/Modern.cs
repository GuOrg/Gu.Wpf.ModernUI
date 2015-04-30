namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using Navigation;

    public static class Modern
    {
        public static readonly DependencyProperty ContentLoaderProperty = DependencyProperty.RegisterAttached(
            "ContentLoader",
            typeof(IContentLoader),
            typeof(Modern),
            new FrameworkPropertyMetadata(
                default(IContentLoader), 
                FrameworkPropertyMetadataOptions.Inherits));
        /// <summary>
        /// Identifies the LinkNavigator dependency property.
        /// </summary>
        public static readonly DependencyProperty LinkNavigatorProperty = DependencyProperty.RegisterAttached(
            "LinkNavigator",
            typeof(ILinkNavigator),
            typeof(Modern),
            new FrameworkPropertyMetadata(
                new DefaultLinkNavigator(),
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
            new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.Inherits));

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
            element.SetValue(LinkBase.LinkStyleProperty, value);
        }

        public static Style GetLinkStyle(this DependencyObject element)
        {
            return (Style)element.GetValue(LinkBase.LinkStyleProperty);
        }
    }
}
