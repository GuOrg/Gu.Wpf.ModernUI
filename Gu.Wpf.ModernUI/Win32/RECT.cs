#pragma warning disable SA1307, 1600 // Accessible fields must begin with upper-case letter
namespace Gu.Wpf.ModernUI.Win32
{
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global", Justification = "For interop")]
    [SuppressMessage("ReSharper", "InconsistentNaming", Justification = "For interop")]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
