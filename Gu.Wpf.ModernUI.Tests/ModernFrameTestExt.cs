namespace Gu.Wpf.ModernUI.Tests
{
    using System.Reflection;
    using System.Windows.Media;

    using ModernUI;

    public static class ModernFrameTestExt
    {
        private static readonly MethodInfo AddVisualChildMethod = typeof(ModernFrame).GetMethod("AddVisualChild", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void AddVisualChild(this ModernFrame frame, Visual child)
        {
            _ = AddVisualChildMethod.Invoke(frame, new object[] { child });
        }
    }
}
