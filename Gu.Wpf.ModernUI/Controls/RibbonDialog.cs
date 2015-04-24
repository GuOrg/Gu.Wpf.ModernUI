namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Controls;
    using ModernUi.Interfaces;

    /// <summary>
    /// A control for showing messages ribbon style
    /// </summary>
    public class RibbonDialog : Control
    {
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.Register(
            "ClickCommand",
            typeof(ICommand),
            typeof(RibbonDialog),
            new PropertyMetadata(default(ICommand)));

        private DispatcherFrame dispatcherFrame;

        static RibbonDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RibbonDialog), new FrameworkPropertyMetadata(typeof(RibbonDialog)));
        }

        public RibbonDialog()
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
                throw new NotImplementedException("Refactor dialog to use buttons & result from interfaces");
            }
            AdornerLayer adornerLayer = decorator.AdornerLayer;
            var uiElement = decorator.Child;
            if (adornerLayer == null || uiElement == null)
            {
                throw new NotImplementedException("Refactor dialog to use buttons & result from interfaces");
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
            Result = (DialogResult)obj;
            this.dispatcherFrame.Continue = false; // stops the frame
        }

        private void Wait()
        {
            while (this.Result == DialogResult.None)
            {
                System.Windows.Threading.Dispatcher.Yield(DispatcherPriority.Background).GetAwaiter().GetResult();
            }
        }
    }
}
