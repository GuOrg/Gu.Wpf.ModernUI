namespace Gu.Wpf.ModernUI.UITests
{
    using TestStack.White.UIItems;

    internal static class ButtonExt
    {
        public static void ClickIfEnabled(this Button button)
        {
            if (button.Enabled)
            {
                button.Click();
            }
        }
    }
}
