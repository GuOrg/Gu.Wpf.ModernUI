namespace Gu.Wpf.ModernUI.Demo.Content
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    using JetBrains.Annotations;

    using ModernUI;

    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class SettingsAppearanceViewModel : INotifyPropertyChanged
    {
        private const string PaletteMetro = "metro";
        private const string PaletteWP = "windows phone";

        // 9 accent colors from metro design principles
        private Color[] metroAccentColors =
        {
            Color.FromRgb(0x33, 0x99, 0xff),   // blue
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff),   // purple            
        };

        // 20 accent colors from Windows Phone 8
        private Color[] wpAccentColors =
        {
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
        };

        private string selectedPalette = PaletteWP;

        private Color selectedAccentColor;
        private Link selectedTheme;

        public SettingsAppearanceViewModel()
        {
            // add the default themes
            this.Themes.Add(new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource });
            this.Themes.Add(new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource });

            // add additional themes
            this.Themes.Add(new Link { DisplayName = "bing image", Source = new Uri("/Gu.Wpf.ModernUI.Demo;component/Assets/ModernUI.BingImage.xaml", UriKind.Relative) });
            this.Themes.Add(new Link { DisplayName = "hello kitty", Source = new Uri("/Gu.Wpf.ModernUI.Demo;component/Assets/ModernUI.HelloKitty.xaml", UriKind.Relative) });
            this.Themes.Add(new Link { DisplayName = "love", Source = new Uri("/Gu.Wpf.ModernUI.Demo;component/Assets/ModernUI.Love.xaml", UriKind.Relative) });
            this.Themes.Add(new Link { DisplayName = "snowflakes", Source = new Uri("/Gu.Wpf.ModernUI.Demo;component/Assets/ModernUI.Snowflakes.xaml", UriKind.Relative) });

            this.SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += this.OnAppearanceManagerPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            this.SelectedTheme = this.Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            this.SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                this.SyncThemeAndColor();
            }
        }

        public ObservableCollection<Link> Themes { get; } = new ObservableCollection<Link>();

        public FontSize[] FontSizes { get; } = { FontSize.Small, FontSize.Large };

        public string[] Palettes { get; } = { PaletteMetro, PaletteWP };

        public Color[] AccentColors => this.selectedPalette == PaletteMetro ? this.metroAccentColors : this.wpAccentColors;

        public string SelectedPalette
        {
            get => this.selectedPalette;
            set
            {
                if (this.selectedPalette != value)
                {
                    this.selectedPalette = value;
                    this.OnPropertyChanged(nameof(this.AccentColors));

                    this.SelectedAccentColor = this.AccentColors.FirstOrDefault();
                }
            }
        }

        public Link SelectedTheme
        {
            get => this.selectedTheme;
            set
            {
                if (!ReferenceEquals(this.selectedTheme, value))
                {
                    this.selectedTheme = value;
                    AppearanceManager.Current.ThemeSource = value.Source;
                    this.OnPropertyChanged();
                }
            }
        }

        public FontSize SelectedFontSize
        {
            get => AppearanceManager.Current.FontSize;
            set
            {
                if (AppearanceManager.Current.FontSize != value)
                {
                    AppearanceManager.Current.FontSize = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public Color SelectedAccentColor
        {
            get => this.selectedAccentColor;
            set
            {
                if (this.selectedAccentColor != value)
                {
                    this.selectedAccentColor = value;
                    this.OnPropertyChanged();

                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
