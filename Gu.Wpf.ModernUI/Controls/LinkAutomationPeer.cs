namespace Gu.Wpf.ModernUI
{
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using System.Windows.Threading;

    public class LinkAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
    {
        public LinkAutomationPeer(Link owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        protected override string GetClassNameCore()
        {
            return "Link";
        }

        /// <inheritdoc/>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        /// <inheritdoc/>
        public override object GetPattern(PatternInterface patternInterface)
        {
            return patternInterface == PatternInterface.Invoke
                       ? this
                       : base.GetPattern(patternInterface);
        }

        void IInvokeProvider.Invoke()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            // Async call of click event
            // In ClickHandler opens a dialog and suspend the execution we don't want to block this thread
            this.Dispatcher.BeginInvoke(
                    DispatcherPriority.Input,
                    new DispatcherOperationCallback(
                        param =>
                            {
                                ((Link)param).AutomationButtonBaseClick();
                                return null;
                            }),
                    this.Owner);
        }
    }
}