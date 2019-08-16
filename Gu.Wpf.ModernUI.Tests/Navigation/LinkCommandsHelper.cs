namespace Gu.Wpf.ModernUI.Tests.Navigation
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using Gu.Wpf.ModernUI.Navigation;

    internal static class LinkCommandsHelper
    {
        private static readonly ConstructorInfo CanExecuteRoutedEventArgsCtor = typeof(CanExecuteRoutedEventArgs).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ICommand), typeof(object) }, null);
        private static readonly ConstructorInfo ExecutedRoutedEventArgsCtor = typeof(ExecutedRoutedEventArgs).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(ICommand), typeof(object) }, null);
          private static readonly FieldInfo OriginalSourceField = typeof(RoutedEventArgs).GetField("_originalSource", BindingFlags.NonPublic | BindingFlags.Instance);

        internal static CanExecuteRoutedEventArgs CreateCanExecuteRoutedEventArgs(ILink originalSource, object paramater = null)
        {
            var args = (CanExecuteRoutedEventArgs)CanExecuteRoutedEventArgsCtor.Invoke(new[] { LinkCommands.NavigateLink, paramater });
            OriginalSourceField.SetValue(args, originalSource);
            args.RoutedEvent = CommandManager.CanExecuteEvent;
            return args;
        }

        internal static ExecutedRoutedEventArgs CreateExecutedRoutedEventArgs(ILink originalSource, object paramater = null)
        {
            var args = (ExecutedRoutedEventArgs)ExecutedRoutedEventArgsCtor.Invoke(new[] { LinkCommands.NavigateLink, paramater });
            OriginalSourceField.SetValue(args, originalSource);
            args.RoutedEvent = CommandManager.ExecutedEvent;
            return args;
        }
    }
}
