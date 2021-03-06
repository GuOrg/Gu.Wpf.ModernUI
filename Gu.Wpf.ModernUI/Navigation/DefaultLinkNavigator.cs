﻿namespace Gu.Wpf.ModernUI.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using Gu.Wpf.ModernUI.Properties;

    /// <summary>
    /// The default link navigator with support for loading frame content, external link navigation using the default browser and command execution.
    /// </summary>
    public class DefaultLinkNavigator
        : ILinkNavigator
    {
        private IEnumerable<string> externalSchemes = new[] { Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeMailto };

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLinkNavigator"/> class.
        /// </summary>
        public DefaultLinkNavigator()
        {
            // register all ApperanceManager commands
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/accentcolor"), AppearanceManager.Current.AccentColorCommand);
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/darktheme"), AppearanceManager.Current.DarkThemeCommand);
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/largefontsize"), AppearanceManager.Current.LargeFontSizeCommand);
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/lighttheme"), AppearanceManager.Current.LightThemeCommand);
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/settheme"), AppearanceManager.Current.SetThemeCommand);
            this.Commands.Add(CommandKey.GetOrCreate(@"cmd:/smallfontsize"), AppearanceManager.Current.SmallFontSizeCommand);

            // register application commands
            foreach (var cmd in GetRoutedUiCommandsFrom(typeof(ApplicationCommands)))
            {
                this.Commands.Add(CommandKey.GetOrCreate($@"cmd:/{cmd.Name}"), cmd);
            }

            foreach (var cmd in GetRoutedUiCommandsFrom(typeof(SystemCommands)))
            {
                this.Commands.Add(CommandKey.GetOrCreate($@"cmd:/{cmd.Name}"), cmd);
            }

            //// foreach (var cmd in GetRoutedUiCommandsFrom(typeof(MediaCommands)))
            //// {
            ////    this.Commands.Add(new CommandKey(string.Format(@"cmd:/{0}", cmd.Name)), cmd);
            //// }

            foreach (var cmd in GetRoutedUiCommandsFrom(typeof(NavigationCommands)))
            {
                this.Commands.Add(CommandKey.GetOrCreate($@"cmd:/{cmd.Name}"), cmd);
            }

            this.NavigatesToContentOnLoad = true;
        }

        /// <summary>
        /// Gets or sets the schemes for external link navigation.
        /// </summary>
        /// <remarks>
        /// Default schemes are http, https and mailto.
        /// </remarks>
        public IEnumerable<string> ExternalSchemes
        {
            get => this.externalSchemes;
            set => this.externalSchemes = value;
        }

        /// <summary>
        /// Gets or sets the navigable commands.
        /// </summary>
        public CommandDictionary Commands { get; } = new CommandDictionary();

        public bool NavigatesToContentOnLoad { get; set; }

        /// <summary>
        /// Checks if navigation can be performed to the link
        /// </summary>
        /// <param name="target">The target frame, can be null</param>
        /// <param name="uri">Used when the link is a command</param>
        /// <returns></returns>
        public virtual bool CanNavigate(ModernFrame target, Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            if (this.Commands != null && this.Commands.TryGetValue(uri, out var command))
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

            var canNavigate = !Equals(target.CurrentSource, uri);
            return canNavigate;
        }

        public virtual void CanNavigate(INavigator navigator, ILink link, CanExecuteRoutedEventArgs e)
        {
            if (link.Command != LinkCommands.NavigateLink)
            {
                e.Handled = false;
                return;
            }

            e.Handled = true;
            var frame = this.GetNavigationTarget(link, navigator);
            if (frame == null)
            {
                link.CanNavigate = this.CanNavigate(null, link.Source);
                link.IsNavigatedTo = false;
                e.CanExecute = link.CanNavigate;
                if (!link.Source.IsResourceUri())
                {
                    return;
                }
            }

            if (frame != null && frame.CurrentSource == null && this.CanNavigate(frame, link.Source) &&
                e.Command == LinkCommands.NavigateLink &&
                this.NavigatesToContentOnLoad)
            {
                // This happens when contentframe.ContentSource is not bound in template.
                // A bit of a hack but better than blank page I think
                this.Navigate(frame, link.Source);
                e.CanExecute = false; // We just did
            }

            if (this.SelectedLinkNeedsUpdate(navigator, frame))
            {
                ILink match = null;
                if (frame != null && frame.CurrentSource != null && navigator.Links != null)
                {
                    match = navigator.Links.FirstOrDefault(l => Equals(l.Source, frame.CurrentSource));
                }

                if (match == null && navigator.Links != null)
                {
                    match = navigator.Links.FirstOrDefault(l => l.Source.IsResourceUri());
                }

                if (match != null)
                {
                    navigator.SelectedLink = match;
                    if (!ReferenceEquals(navigator.SelectedSource, match.Source))
                    {
                        navigator.SelectedSource = match.Source;
                    }
                }
            }

            link.IsNavigatedTo = frame != null && Equals(frame.CurrentSource, link.Source);
            link.CanNavigate = this.CanNavigate(frame, link.Source);
            e.CanExecute = link.CanNavigate;
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
            if (this.Commands != null && this.Commands.TryGetValue(uri, out var command))
            {
                // note: not executed within BBCodeBlock context, Hyperlink instance has Command and CommandParameter set
                if (command.CanExecute(uri))
                {
                    command.Execute(uri);
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
            target.SetCurrentValue(ModernFrame.CurrentSourceProperty, uri);
        }

        public virtual void Navigate(INavigator navigator, ILink link, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var frame = this.GetNavigationTarget(link, navigator);
            this.Navigate(frame, link.Source);
        }

        protected virtual bool SelectedLinkNeedsUpdate(INavigator navigator, ModernFrame frame)
        {
            var selectedLink = navigator.SelectedLink;
            if (selectedLink == null)
            {
                var navigatorHasCandidateLinks = navigator.Links.Any(l => l.Source.IsResourceUri());
                return navigatorHasCandidateLinks;
            }

            if (frame == null || frame.CurrentSource == null)
            {
                return false;
            }

            if (navigator is ModernMenu)
            {
                // Debugger.Break();
            }

            if (selectedLink.Source != null)
            {
                if (Equals(frame.CurrentSource, selectedLink.Source))
                {
                    return false;
                }

                if (navigator.Links.Any(l => Equals(l.Source, frame.CurrentSource)))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual ModernFrame GetNavigationTarget(ILink link, INavigator navigator)
        {
            if (link?.CommandTarget != null)
            {
                if (link.CommandTarget is ModernFrame frame)
                {
                    return frame;
                }

                var target = link.CommandTarget as DependencyObject;
                return target.GetNavigationTarget();
            }

            return navigator.NavigationTarget;
        }

        private static IReadOnlyList<RoutedUICommand> GetRoutedUiCommandsFrom(Type typeContainingCommands)
        {
            var commands = typeContainingCommands.GetProperties(BindingFlags.Static | BindingFlags.Public)
                                                 .Where(p => p.PropertyType == typeof(RoutedUICommand))
                                                 .Select(p => (RoutedUICommand)p.GetValue(null))
                                                 .ToArray();
            return commands;
        }
    }
}
