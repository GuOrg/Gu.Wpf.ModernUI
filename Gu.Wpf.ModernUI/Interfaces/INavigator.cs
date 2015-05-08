namespace Gu.Wpf.ModernUI
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is an interface for controls that responds to LinkCommands.NavigateLink
    /// </summary>
    public interface INavigator
    {
        ILink SelectedLink { get; set; }
      
        Uri SelectedSource { get; set; }
        
        IEnumerable<ILink> Links { get; }

        ILinkNavigator LinkNavigator { get; }

        ModernFrame NavigationTarget { get; set; }
    }
}