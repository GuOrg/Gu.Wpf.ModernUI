namespace Gu.Wpf.ModernUI
{
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Threading;

    public class LinkGroupAutomationPeer : ButtonBaseAutomationPeer, IInvokeProvider
    {
        public LinkGroupAutomationPeer(LinkGroup owner)
            : base(owner)
        {
        }

        /// <inheritdoc/>
        protected override string GetClassNameCore()
        {
            return "LinkGroup";
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
                                ((LinkGroup)param).AutomationButtonBaseClick();
                                return null;
                            }),
                    this.Owner);
        }
    }
}