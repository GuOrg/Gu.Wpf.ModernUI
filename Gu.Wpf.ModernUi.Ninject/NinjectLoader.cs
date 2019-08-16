namespace Gu.Wpf.ModernUI.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using global::Ninject;

    using Gu.Wpf.ModernUI;

    /// <summary>
    /// Loads XAML files using Application.LoadComponent and Ninject.
    /// </summary>
    public class NinjectLoader : DefaultContentLoader
    {
        private readonly Dictionary<string, Type> userControlTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectLoader"/> class.
        /// Creates a dictionary with all usercontrol types in <see cref="Assembly.GetCallingAssembly"/>.
        /// These are later used when loading content
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/></param>
        public NinjectLoader(IKernel kernel)
            : this(kernel, Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectLoader"/> class.
        /// Creates a dictionary with all usercontrol types in mainwindow's assembly.
        /// These are later used when loading content
        /// </summary>
        /// <param name="mainwindow">
        /// The main window of the application.
        /// </param>
        /// <param name="kernel">The <see cref="IKernel"/></param>
        public NinjectLoader(IKernel kernel, ModernWindow mainwindow)
            : this(kernel, mainwindow.GetType().Assembly)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectLoader"/> class.
        /// Creates a dictionary with all usercontrol types in Assembly
        /// These are later used when loading content
        /// </summary>
        /// <param name="kernel">The <see cref="IKernel"/></param>
        /// <param name="assembly">This must be the assembly containing usercontrols to resolve</param>
        /// <param name="assemblies">Other assemblies with usercontrols.</param>
        public NinjectLoader(IKernel kernel, Assembly assembly, params Assembly[] assemblies)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            this.Kernel = kernel;
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { assembly };
            }
            else
            {
                assemblies = assemblies.Concat(new[] { assembly })
                                       .ToArray();
            }

            this.userControlTypes = assemblies.SelectMany(a => a.GetTypes())
                                              .Where(x => x.IsSubclassOf(typeof(UserControl)))
                                              .OrderBy(x => x.Name)
                                              .ToDictionary(t => t.Name, t => t);
        }

        /// <summary>
        /// Gets the <see cref="IKernel"/>
        /// </summary>
        public IKernel Kernel { get; }

        /// <inheritdoc/>
        protected override object LoadContent(Uri uri)
        {
            // don't do anything in design mode
            if (Is.InDesignMode)
            {
                return null;
            }

            var key = GetKeyFromUri(uri);
            Type type;
            if (this.userControlTypes.TryGetValue(key, out type))
            {
                var content = this.Kernel.Get(type);
                return content;
            }

            throw new ArgumentException($"Could not find {key} in user control collection.");
        }

        private static string GetKeyFromUri(Uri uri)
        {
            var segments = uri.ToString().Split('/');
            var last = segments.Last();
            var key = System.IO.Path.GetFileNameWithoutExtension(last);
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"Failed getting key from {uri}", nameof(uri));
            }

            return key;
        }
    }
}
