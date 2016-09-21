namespace Gu.Wpf.ModernUI.Win32
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global",Justification = "For interop")]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
