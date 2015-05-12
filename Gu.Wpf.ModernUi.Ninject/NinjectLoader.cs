namespace Gu.Wpf.ModernUi.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Controls;
    using global::Ninject;

    using Gu.Wpf.ModernUI.Internals;

    using ModernUI;

    public class NinjectLoader : DefaultContentLoader
    {
        public static readonly string NinjectLoaderKey = "NinjectLoaderKey";
        private readonly IKernel kernel;
        private readonly Dictionary<string, Type> userControlTypes;

        /// <summary>
        /// Creates a dictionary with all usercontrol types in the CallingAssembly.
        /// These are later used when loading content
        /// </summary>
        /// <param name="kernel"></param>
        public NinjectLoader(IKernel kernel)
            : this(kernel, Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        /// Creates a dictionary with all usercontrol types in mainwindow's assembly.
        /// These are later used when loading content
        /// </summary>
        /// <param name="mainwindow"></param>
        /// <param name="kernel"></param>
        public NinjectLoader(IKernel kernel, ModernWindow mainwindow)
            : this(kernel, mainwindow.GetType().Assembly)
        {
        }

        /// <summary>
        /// Creates a dictionary with all usercontrol types in Assembly
        /// These are later used when loading content
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="assembly">This must be the assembly containing usercontrols to resolve</param>
        public NinjectLoader(IKernel kernel, Assembly assembly, params Assembly[] assemblies)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            this.kernel = kernel;
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
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            // don't do anything in design mode
            if (ModernUIHelper.IsInDesignMode)
            {
                return null;
            }
            var key = GetKeyFromUri(uri);
            Type type;
            if (this.userControlTypes.TryGetValue(key, out type))
            {
                var content = this.kernel.Get(type);
                return content;
            }

            throw new ArgumentException(string.Format("Could not find {0} in user control collection.", key));
        }

        private static string GetKeyFromUri(Uri uri)
        {
            var segments = uri.ToString().Split('/');
            var last = segments.Last();
            var key = System.IO.Path.GetFileNameWithoutExtension(last);
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(string.Format("Failed getting key from {1}", uri), "uri");
            }
            return key;
        }
    }
}
