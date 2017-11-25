namespace Gu.Wpf.ModernUI
{
    using System.Security.Permissions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Threading;

    using Gu.ModernUI.Interfaces;

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

        public static readonly DependencyProperty ButtonTemplateSelectorProperty = DependencyProperty.Register(
            "ButtonTemplateSelector",
            typeof(DialogButtonTemplateSelector),
            typeof(ModernPopup),
            new PropertyMetadata(new DialogButtonTemplateSelector()));

        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(
            "IconTemplateSelector",
            typeof(DialogIconTemplateSelector),
            typeof(ModernPopup),
            new PropertyMetadata(new DialogIconTemplateSelector()));

        static ModernPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernPopup), new FrameworkPropertyMetadata(typeof(ModernPopup)));
        }

        public ModernPopup()
        {
            this.ClickCommand = new RelayCommand(this.OnClick, _ => true);
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

        public DialogResult? Result { get; private set; }

        internal DialogResult RunDialog(ModernWindow owner, IDialogHandler dialogHandler, DialogViewModel viewModel)
        {
            this.Result = null;
            var decorator = owner.AdornerDecorator;
            if (decorator == null || !owner.IsActive)
            {
                return this.ShowDialog(viewModel);
            }

            AdornerLayer adornerLayer = decorator.AdornerLayer;
            var uiElement = decorator.Child;
            if (adornerLayer == null || uiElement == null)
            {
                return this.ShowDialog(viewModel);
            }

            var adorner = new ContentAdorner(uiElement, this);
            adornerLayer.Add(adorner);

            while (this.Result == null)
            {
                this.DoEvents();
            }

            adornerLayer.Remove(adorner);
            return this.Result.Value;
        }

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var dispatcherOperationCallback = new DispatcherOperationCallback(this.ExitFrame);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, dispatcherOperationCallback, frame);
            Dispatcher.PushFrame(frame);
        }

        private object ExitFrame(object f)
        {
            ((DispatcherFrame)f).Continue = false;
            return null;
        }

        private void OnClick(object obj)
        {
            this.Result = (DialogResult)obj;
        }

        /// <summary>
        /// Showing a standard dialog as fallback.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        private DialogResult ShowDialog(DialogViewModel vm)
        {
            var dialog = new ModernDialog
                                            {
                                                Title = vm.Title,
                                                DataContext = vm
                                            };
            dialog.ShowDialog();
            switch (dialog.Result)
            {
                case DialogResult.OK:
                    return DialogResult.OK;
                case DialogResult.Cancel:
                    return DialogResult.Cancel;
                case DialogResult.Yes:
                    return DialogResult.Yes;
                case DialogResult.No:
                    return DialogResult.No;
                default:
                    return DialogResult.None;
            }
        }
    }
}
