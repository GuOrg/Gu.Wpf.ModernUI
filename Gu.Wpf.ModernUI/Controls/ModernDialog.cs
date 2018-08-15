namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Input;

    using Gu.ModernUI.Interfaces;

    /// <summary>
    /// Represents a Modern UI styled dialog window.
    /// </summary>
    public class ModernDialog : DpiAwareWindow
    {
        /// <summary>Identifies the <see cref="BackgroundContent"/> dependency property.</summary>
        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register(nameof(BackgroundContent), typeof(object), typeof(ModernDialog));

        /// <summary>Identifies the <see cref="ClickCommand"/> dependency property.</summary>
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            nameof(ClickCommand),
            typeof(ICommand),
            typeof(ModernDialog),
            new PropertyMetadata(default(ICommand)));

        /// <summary>Identifies the <see cref="ButtonTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ButtonTemplateSelector),
            typeof(DialogButtonTemplateSelector),
            typeof(ModernDialog),
            new PropertyMetadata(new DialogButtonTemplateSelector()));

        /// <summary>Identifies the <see cref="IconTemplateSelector"/> dependency property.</summary>
        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(
            nameof(IconTemplateSelector),
            typeof(DialogIconTemplateSelector),
            typeof(ModernDialog),
            new PropertyMetadata(new DialogIconTemplateSelector()));

        static ModernDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernDialog), new FrameworkPropertyMetadata(typeof(ModernDialog)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModernDialog"/> class.
        /// </summary>
        public ModernDialog()
        {
            this.ClickCommand = new RelayCommand(this.OnClick, _ => true);
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            // set the default owner to the app main window (if possible)
            if (Application.Current != null && !ReferenceEquals(Application.Current.MainWindow, this))
            {
                this.Owner = Application.Current.MainWindow;
            }
        }

        /// <summary>
        /// Gets or sets the background content of this window instance.
        /// </summary>
        public object BackgroundContent
        {
            get => this.GetValue(BackgroundContentProperty);
            set => this.SetValue(BackgroundContentProperty, value);
        }

        public ICommand ClickCommand
        {
            get => (ICommand)this.GetValue(ClickCommandProperty);
            set => this.SetValue(ClickCommandProperty, value);
        }

        public DialogButtonTemplateSelector ButtonTemplateSelector
        {
            get => (DialogButtonTemplateSelector)this.GetValue(ButtonTemplateSelectorProperty);
            set => this.SetValue(ButtonTemplateSelectorProperty, value);
        }

        public DialogIconTemplateSelector IconTemplateSelector
        {
            get => (DialogIconTemplateSelector)this.GetValue(IconTemplateSelectorProperty);
            set => this.SetValue(IconTemplateSelectorProperty, value);
        }

        public DialogResult Result { get; private set; }

        /// <summary>
        /// Displays a messagebox.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="title">The title.</param>
        /// <param name="buttons">The button.</param>
        /// <param name="owner">The window owning the messagebox. The messagebox will be located at the center of the owner.</param>
        /// <returns>A <see cref="DialogResult"/> with info about the user</returns>
        public static DialogResult ShowMessage(string text, string title, MessageBoxButtons buttons, Window owner = null)
        {
            var dlg = new ModernDialog
            {
                Title = title,
                DataContext = new DialogViewModel(title, text, MessageBoxIcon.None, buttons),
                MinHeight = 0,
                MinWidth = 0,
                MaxHeight = 480,
                MaxWidth = 640,
            };
            if (owner != null)
            {
                dlg.Owner = owner;
            }

            dlg.ShowDialog();
            return dlg.Result;
        }

        private void OnClick(object obj)
        {
            this.Result = (DialogResult)obj;
            this.DialogResult = true;
            this.Close();
        }
    }
}
