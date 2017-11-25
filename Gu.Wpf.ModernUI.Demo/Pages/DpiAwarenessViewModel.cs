namespace Gu.Wpf.ModernUI.Demo.Pages
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using ModernUI;

    public class DpiAwarenessViewModel : INotifyPropertyChanged
    {
        private readonly DpiAwareWindow wnd;

        /// <summary>
        /// Initializes a new instance of the <see cref="DpiAwarenessViewModel" /> class.
        /// </summary>
        public DpiAwarenessViewModel()
        {
            this.wnd = (DpiAwareWindow)Application.Current.MainWindow;
            this.wnd.DpiChanged += this.OnWndDpiChanged;
            this.wnd.SizeChanged += this.OnWndSizeChanged;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnWndDpiChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged(string.Empty);        // refresh all properties
        }

        private void OnWndSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.OnPropertyChanged(string.Empty);        // refresh all properties
        }

        public string DpiAwareMessage => string.Format(CultureInfo.InvariantCulture, "The DPI awareness of this process is [b]{0}[/b]", ModernUIHelper.GetDpiAwereness());

        public string WpfDpi
        {
            get
            {
                var info = this.wnd.DpiInformation;
                return string.Format(CultureInfo.InvariantCulture, "{0} x {1}", info.WpfDpiX, info.WpfDpiY);
            }
        }

        public string MonitorDpi
        {
            get
            {
                var info = this.wnd.DpiInformation;
                if (info.MonitorDpiX.HasValue)
                {
                    return string.Format(CultureInfo.InvariantCulture, "{0} x {1}", info.MonitorDpiX, info.MonitorDpiY);
                }

                return "n/a";
            }
        }

        public string LayoutScale
        {
            get
            {
                var info = this.wnd.DpiInformation;
                return string.Format(CultureInfo.InvariantCulture, "{0} x {1}", info.ScaleX, info.ScaleY);
            }
        }

        public string WindowSize
        {
            get
            {
                var info = this.wnd.DpiInformation;
                var width = this.wnd.ActualWidth * info.WpfDpiX / 96D;
                var height = this.wnd.ActualHeight * info.WpfDpiY / 96D;

                return string.Format(CultureInfo.InvariantCulture, "{0} x {1}", width, height);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
