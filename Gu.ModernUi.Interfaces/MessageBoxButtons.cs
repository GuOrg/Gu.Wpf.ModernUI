namespace Gu.ModernUI.Interfaces
{
    /// <summary>Same as System.Windows.MessageBoxButton </summary>
    public enum MessageBoxButtons
    {
        /// <summary>Specifies that the dialog contains an OK button.</summary>
        OK = 0x00000000,

        /// <summary>Specifies that the dialog contains OK and Cancel buttons.</summary>
        OKCancel = 0x00000001,

        /// <summary>Specifies that the dialog contains Yes, No, and Cancel buttons.</summary>
        YesNoCancel = 0x00000003,

        /// <summary>Specifies that the dialog contains Yes, and No buttons.</summary>
        YesNo = 0x00000004,

        /// <summary>Specifies that the dialog contains Abort, Retry, and Ignore buttons.</summary>
        AbortRetryIgnore = 0x00000005,

        /// <summary>Specifies that the dialog contains Retry, and Cancel buttons.</summary>
        RetryCancel = 0x00000006,
    }
}