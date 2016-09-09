namespace Gu.Wpf.ModernUI.Win32
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    internal struct RECT
    {
        public int left, top, right, bottom;
    }
}
