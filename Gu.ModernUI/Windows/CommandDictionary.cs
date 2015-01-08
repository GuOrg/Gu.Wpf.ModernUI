namespace Gu.ModernUI.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    /// <summary>
    /// Represents a collection of commands keyed by a uri.
    /// </summary>
    public class CommandDictionary
        : Dictionary<Uri, ICommand>
    {
    }
}
