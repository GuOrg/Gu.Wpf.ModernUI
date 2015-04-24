namespace Gu.Wpf.ModernUI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;

    using Gu.Wpf.ModernUI;
    using Gu.Wpf.ModernUI.Properties;

    /// <summary>
    /// The default link navigator with support for loading frame content, external link navigation using the default browser and command execution.
    /// </summary>
    public class DefaultLinkNavigator
        : ILinkNavigator
    {
        private IEnumerable<string> externalSchemes = new[] { Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto };
        private readonly CommandDictionary commands = new CommandDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLinkNavigator"/> class.
        /// </summary>
        public DefaultLinkNavigator()
        {
            // register all ApperanceManager commands
            this.Commands.Add(new Uri(@"cmd:/accentcolor"), AppearanceManager.Current.AccentColorCommand);
            this.Commands.Add(new Uri(@"cmd:/darktheme"), AppearanceManager.Current.DarkThemeCommand);
            this.Commands.Add(new Uri(@"cmd:/largefontsize"), AppearanceManager.Current.LargeFontSizeCommand);
            this.Commands.Add(new Uri(@"cmd:/lighttheme"), AppearanceManager.Current.LightThemeCommand);
            this.Commands.Add(new Uri(@"cmd:/settheme"), AppearanceManager.Current.SetThemeCommand);
            this.Commands.Add(new Uri(@"cmd:/smallfontsize"), AppearanceManager.Current.SmallFontSizeCommand);

            // register navigation commands
            this.commands.Add(new Uri(@"cmd:/browseback"), NavigationCommands.BrowseBack);
            this.commands.Add(new Uri(@"cmd:/refresh"), NavigationCommands.Refresh);

            // register application commands
            this.commands.Add(new Uri(@"cmd:/copy"), ApplicationCommands.Copy);
        }

        /// <summary>
        /// Gets or sets the schemes for external link navigation.
        /// </summary>
        /// <remarks>
        /// Default schemes are http, https and mailto.
        /// </remarks>
        public IEnumerable<string> ExternalSchemes
        {
            get { return this.externalSchemes; }
            set { this.externalSchemes = value; }
        }

        /// <summary>
        /// Gets or sets the navigable commands.
        /// </summary>
        public CommandDictionary Commands
        {
            get { return this.commands; }
        }

        /// <summary>
        /// Checks if navigation can be performed to the link
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="current"></param>
        /// <param name="commandParameter">Used when the link is a command</param>
        /// <returns></returns>
        public bool CanNavigate(Uri uri, Uri current = null, object commandParameter = null)
        {
            if (uri == null)
            {
                return false;
            }
            ICommand command;
            if (this.commands != null && this.commands.TryGetValue(uri, out command))
            {
                // note: not executed within BBCodeBlock context, Hyperlink instance has Command and CommandParameter set
                return command.CanExecute(commandParameter);
            }
            if (uri.IsAbsoluteUri && this.externalSchemes.Any(s => uri.Scheme.Equals(s, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            return !Equals(current, uri);
        }

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="uri">The uri to navigate to.</param>
        /// <param name="frameNavigation">The action to invoke if it is a frame navigation i.e the uri is something like /content/settings.caml</param>
        /// <param name="commandParameter">Used when the link is a command</param>
        public virtual void Navigate(Uri uri, Action<Uri> frameNavigation = null, object commandParameter = null)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            // first check if uri refers to a command
            ICommand command;
            if (this.commands != null && this.commands.TryGetValue(uri, out command))
            {
                // note: not executed within BBCodeBlock context, Hyperlink instance has Command and CommandParameter set
                if (command.CanExecute(commandParameter))
                {
                    command.Execute(commandParameter);
                }
                else
                {
                    // do nothing
                }
                return;
            }
            if (uri.IsAbsoluteUri && this.externalSchemes.Any(s => uri.Scheme.Equals(s, StringComparison.OrdinalIgnoreCase)))
            {
                // uri is external, load in default browser
                Process.Start(uri.AbsoluteUri);
                return;
            }
            if (frameNavigation == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Resources.NavigationFailedSourceNotSpecified, uri));
            }
            // delegate navigation to the frame
            frameNavigation(uri);
        }
    }
}
