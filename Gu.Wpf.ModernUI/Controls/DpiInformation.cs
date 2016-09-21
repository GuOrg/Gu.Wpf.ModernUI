namespace Gu.Wpf.ModernUI
{
    using System.Windows;

    /// <summary>
    /// Provides DPI information for a window.
    /// </summary>
    public class DpiInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DpiInformation"/> class.
        /// </summary>
        /// <param name="wpfDpiX">The horizontal resolution of the WPF rendering DPI.</param>
        /// <param name="wpfDpiY">The vertical resolution of the WPF rendering DPI.</param>
        internal DpiInformation(double wpfDpiX, double wpfDpiY)
        {
            this.WpfDpiX = wpfDpiX;
            this.WpfDpiY = wpfDpiY;
            this.ScaleX = 1;
            this.ScaleY = 1;
        }

        /// <summary>
        /// Default value 96*96
        /// </summary>
        public static DpiInformation Identity { get; } = new DpiInformation(96, 96);

        /// <summary>
        /// Gets the horizontal resolution of the WPF rendering DPI.
        /// </summary>
        public double WpfDpiX { get; }

        /// <summary>
        /// Gets the vertical resolution of the WPF rendering DPI.
        /// </summary>
        public double WpfDpiY { get; }

        /// <summary>
        /// Gets the horizontal resolution of the current monitor DPI.
        /// </summary>
        /// <remarks>Null when the process is not per monitor DPI aware.</remarks>
        public double? MonitorDpiX { get; private set; }

        /// <summary>
        /// Gets the vertical resolution of the current monitor DPI.
        /// </summary>
        /// <remarks>Null when the process is not per monitor DPI aware.</remarks>
        public double? MonitorDpiY { get; private set; }

        /// <summary>
        /// Gets the x-axis scale factor.
        /// </summary>
        public double ScaleX { get; private set; }

        /// <summary>
        /// Gets the y-axis scale factor.
        /// </summary>
        public double ScaleY { get; private set; }

        /// <summary>
        /// Calculate the vector of the current to new dpi.
        /// This method has side effects, it sets:
        ///  <see cref="MonitorDpiX"/> to <paramref name="dpiX"/>
        ///  <see cref="ScaleX"/> to <paramref name="dpiX"/> / <see cref="WpfDpiX"/>
        ///  <see cref="MonitorDpiY"/> to <paramref name="dpiY"/>
        ///  <see cref="ScaleY"/> to <paramref name="dpiY"/> / <see cref="WpfDpiY"/>
        /// </summary>
        /// <param name="dpiX">The new horizontal monitor DPI</param>
        /// <param name="dpiY">The new vertical monitor DPI</param>
        /// <returns>The quota DPI / oldDPI</returns>
        internal Vector UpdateMonitorDpi(double dpiX, double dpiY)
        {
            var oldDpiX = this.MonitorDpiX ?? this.WpfDpiX;
            var oldDpiY = this.MonitorDpiY ?? this.WpfDpiY;

            this.MonitorDpiX = dpiX;
            this.MonitorDpiY = dpiY;

            this.ScaleX = dpiX / this.WpfDpiX;
            this.ScaleY = dpiY / this.WpfDpiY;

            return new Vector(dpiX / oldDpiX, dpiY / oldDpiY);
        }
    }
}
