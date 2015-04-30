namespace Gu.Wpf.ModernUI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Threading;
    using ModernUi.Interfaces;

    /// <summary>
    /// A control for showing messages ribbon style
    /// </summary>
    public class ModernPopup : Control
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand",
            typeof(ICommand),
            typeof(ModernPopup),
            new PropertyMetadata(default(ICommand)));

        private DispatcherFrame dispatcherFrame;

        static ModernPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernPopup), new FrameworkPropertyMetadata(typeof(ModernPopup)));
        }

        public ModernPopup()
        {
            this.ClickCommand = new RelayCommand(OnClick, _ => true);
        }

        public ICommand ClickCommand
        {
            get { return (ICommand)GetValue(ClickCommandProperty); }
            set { SetValue(ClickCommandProperty, value); }
        }

        public DialogResult Result { get; private set; }

        internal DialogResult RunDialog(ModernWindow owner, IDialogHandler dialogHandler)
        {
            this.Result = DialogResult.None;
            var decorator = owner.AdornerDecorator;
            if (decorator == null)
            {
                return ShowDialog(dialogHandler);
            }
            AdornerLayer adornerLayer = decorator.AdornerLayer;
            var uiElement = decorator.Child;
            if (adornerLayer == null || uiElement == null)
            {
               return ShowDialog(dialogHandler);
            }
            this.dispatcherFrame = new DispatcherFrame();
            var adorner = new ContentAdorner(uiElement, this);
            adornerLayer.Add(adorner);
            // This will "block" execution of the current dispatcher frame
            // and run our frame until the dialog is closed.
            Dispatcher.PushFrame(this.dispatcherFrame);
            adornerLayer.Remove(adorner);
            return this.Result;
        }

        private void OnClick(object obj)
        {
            this.Result = (DialogResult)obj;
            this.dispatcherFrame.Continue = false; // stops the frame
        }

        private DialogResult ShowDialog(IDialogHandler dialogHandler)
        {
            var dialog = new ModernDialog
                                            {
                                                Title = dialogHandler.Title,
                                                Content = dialogHandler.Content
                                            };
            dialog.ShowDialog();
            switch (dialog.MessageBoxResult)
            {
                case MessageBoxResult.OK:
                   return DialogResult.OK;
                case MessageBoxResult.Cancel:
                   return DialogResult.Cancel;
                case MessageBoxResult.Yes:
                   return DialogResult.Yes;
                case MessageBoxResult.No:
                   return DialogResult.No;
                case MessageBoxResult.None:
                default:
                   return DialogResult.None;
            }
        }
    }
}
