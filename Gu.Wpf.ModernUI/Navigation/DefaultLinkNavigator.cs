namespace Gu.Wpf.ModernUI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Input;
    using Properties;

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
            this.Commands.Add(new CommandKey(@"cmd:/accentcolor"), AppearanceManager.Current.AccentColorCommand);
            this.Commands.Add(new CommandKey(@"cmd:/darktheme"), AppearanceManager.Current.DarkThemeCommand);
            this.Commands.Add(new CommandKey(@"cmd:/largefontsize"), AppearanceManager.Current.LargeFontSizeCommand);
            this.Commands.Add(new CommandKey(@"cmd:/lighttheme"), AppearanceManager.Current.LightThemeCommand);
            this.Commands.Add(new CommandKey(@"cmd:/settheme"), AppearanceManager.Current.SetThemeCommand);
            this.Commands.Add(new CommandKey(@"cmd:/smallfontsize"), AppearanceManager.Current.SmallFontSizeCommand);

            // register navigation commands
            this.commands.Add(new CommandKey(@"cmd:/browseback"), NavigationCommands.BrowseBack);
            this.commands.Add(new CommandKey(@"cmd:/refresh"), NavigationCommands.Refresh);

            // register application commands
            this.commands.Add(new CommandKey(@"cmd:/copy"), ApplicationCommands.Copy);
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
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        /// <returns></returns>
        public bool CanNavigate(ModernFrame target, Uri uri)
        {
            if (uri == null)
            {
                return false;
            }
            ICommand command;
            if (this.commands != null && this.commands.TryGetValue(uri, out command))
            {
                // note: not executed within BBCodeBlock context, Hyperlink instance has Command and CommandParameter set
                return command.CanExecute(uri);
            }
            if (uri.IsAbsoluteUri && this.externalSchemes.Any(s => uri.Scheme.Equals(s, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }
            if (target == null)
            {
                return false;
            }
            return !Equals(target.Source, uri);
        }

        /// <summary>
        /// Performs navigation to specified link.
        /// </summary>
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        public virtual void Navigate(ModernFrame target, Uri uri)
        {
            if (uri == null)
            {
                return;
            }

            // first check if uri refers to a command
            ICommand command;
            if (this.commands != null && this.commands.TryGetValue(uri, out command))
            {
                // note: not executed within BBCodeBlock context, Hyperlink instance has Command and CommandParameter set
                if (command.CanExecute(uri))
                {
                    command.Execute(uri);
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
            if (target == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, Resources.NavigationFailedSourceNotSpecified, uri));
            }
            // delegate navigation to the frame
            target.Source = uri;
        }
    }
}
