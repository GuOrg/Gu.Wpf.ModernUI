namespace Gu.ModernUI.Presentation
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public interface ILink
    {
        /// <summary>
        /// The displayname
        /// </summary>
        string DisplayName { get; }
        
        /// <summary>
        /// The source
        /// </summary>
        Uri Source { get; }
    }
}