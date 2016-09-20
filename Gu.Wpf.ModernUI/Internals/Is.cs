namespace Gu.Wpf.ModernUI
{
    using System.ComponentModel;
    using System.Windows;

    internal class Is
    {
        private static readonly DependencyObject DependencyObject = new DependencyObject();
        private static bool? isInDesignMode; // We use reflection to set this in tests, slight smell perhaps.

        /// <summary>
        /// Determines whether the current code is executed in a design time environment such as Visual Studio or Blend.
        /// </summary>
        public static bool InDesignMode => isInDesignMode ?? (isInDesignMode = DesignerProperties.GetIsInDesignMode(DependencyObject)) == true;
    }
}