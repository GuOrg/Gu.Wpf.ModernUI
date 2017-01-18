using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("Gu.Wpf.ModernUI")]
[assembly: AssemblyConfiguration("retail")]
[assembly: AssemblyCompany("Johan Larsson")]
[assembly: AssemblyProduct("Gu.Wpf.ModernUI")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("e467fe5a-70ee-4907-8d06-156e4c988e7d")]

[assembly: AssemblyVersion("1.0.8.1")]
[assembly: AssemblyFileVersion("1.0.8.1")]

[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.ModernUI", "Gu.Wpf.ModernUI")]
[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.ModernUI", "Gu.Wpf.ModernUI.Navigation")]
[assembly: XmlnsDefinition("https://github.com/JohanLarsson/Gu.Wpf.ModernUI", "Gu.Wpf.ModernUI.BBCode")]
[assembly: XmlnsPrefix("https://github.com/JohanLarsson/Gu.Wpf.ModernUI", "mui")]

[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: InternalsVisibleTo("Gu.Wpf.ModernUI.Tests", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Gu.Wpf.ModernUI.Demo", AllInternalsVisible = true)]
[assembly: InternalsVisibleTo("Gu.Wpf.ModernUI.Ninject", AllInternalsVisible = true)]