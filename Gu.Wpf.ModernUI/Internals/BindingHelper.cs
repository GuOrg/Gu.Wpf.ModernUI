namespace Gu.Wpf.ModernUI.Internals
{
    using System.Windows;
    using System.Windows.Data;

    internal static class BindingHelper
    {
        public static BindingExpressionBase BindOneWay(
            DependencyObject source,
            DependencyProperty path,
            DependencyObject target,
            DependencyProperty targetProp)
        {
            return BindOneWay(source, path.Name, target, targetProp);
        }

        public static BindingExpressionBase BindOneWay(
            DependencyObject source,
            string path,
            DependencyObject target,
            DependencyProperty targetProp)
        {
            var binding = new Binding(path)
                              {
                                  Source = source,
                                  Mode = BindingMode.OneWay,
                                  UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                              };
            var bindingExpression = BindingOperations.SetBinding(target, targetProp, binding);
            return bindingExpression;
        }
    }
}