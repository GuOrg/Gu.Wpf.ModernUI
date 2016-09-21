namespace Gu.ModernUI.Interfaces
{
    /// <summary>
    /// Specifies the icon that is displayed by a message box.
    /// This is the same as <see cref="System.Windows.MessageBoxImage"/>
    /// </summary>
    public enum MessageBoxIcon
    {

        /// <summary>
        /// Specifies that the message box contain no symbols.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the message box contains a question mark symbol.
        /// </summary>
        Hand = 0x00000010,

        /// <summary>
        /// Specifies that the message box contains a question mark symbol.
        /// </summary>
        Error = Hand,

        /// <summary>
        /// Specifies that the message box contains a question mark symbol.
        /// </summary>
        Stop = Hand,

        /// <summary>
        /// Specifies that the message box contains a question mark symbol.
        /// </summary>
        Question = 0x00000020,

        /// <summary>
        /// Specifies that the message box contains an exclamation icon.
        /// </summary>
        Exclamation = 0x00000030,

        /// <summary>
        /// Specifies that the message box contains an exclamation icon.
        /// </summary>
        Warning = Exclamation,

        /// <summary>
        /// Specifies that the message box contains an asterisk icon.
        /// </summary>
        Asterisk = 0x00000040,

        /// <summary>
        /// Specifies that the message box contains an asterisk icon.
        /// </summary>
        Information = Asterisk,
    }
}