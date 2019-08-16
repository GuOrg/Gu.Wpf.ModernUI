namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Data;

    internal static class BindingHelper
    {
        public static BindingExpressionBase Bind(
            DependencyObject source,
            DependencyProperty path,
            DependencyObject target,
            DependencyProperty targetProp,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
        {
            return Bind(source, path.Name, target, targetProp, mode, updateSourceTrigger);
        }

        public static BindingExpressionBase Bind(
            DependencyObject source,
            DependencyProperty path1,
            DependencyProperty path2,
            DependencyObject target,
            DependencyProperty targetProp,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
        {
            var path = $"{path1.Name}.{path2.Name}";
            return Bind(source, new PropertyPath(path), target, targetProp, mode, updateSourceTrigger);
        }

        public static BindingExpressionBase Bind(
            DependencyObject source,
            string path,
            DependencyObject target,
            DependencyProperty targetProp,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
        {
            return Bind(source, new PropertyPath(path), target, targetProp, mode, updateSourceTrigger);
        }

        public static BindingExpressionBase Bind(
            DependencyObject source,
            PropertyPath path,
            DependencyObject target,
            DependencyProperty targetProp,
            BindingMode mode,
            UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.PropertyChanged)
        {
            var binding = new Binding()
            {
                Source = source,
                Path = path,
                Mode = mode,
                UpdateSourceTrigger = updateSourceTrigger,
            };
            var bindingExpression = BindingOperations.SetBinding(target, targetProp, binding);
            return bindingExpression;
        }
    }
}