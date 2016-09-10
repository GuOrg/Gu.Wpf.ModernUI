namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Xml.Linq;
    using NUnit.Framework;

    public class Dump
    {
        [TestCase("ModernUI.Dark.xaml")]
        [TestCase("ModernUI.Light.xaml")]
        public void Theme(string name)
        {
            const string dir = "C:\\Git\\Gu.ModernUI\\Gu.Wpf.ModernUI\\Assets";
            var element = XDocument.Parse(File.ReadAllText(Path.Combine(dir, name)));
            var brushElements = element.Root
                                       .Elements()
                                       .Where(x => x.Name.LocalName == "SolidColorBrush")
                                       .ToArray();
            var colors = brushElements.Select(x => x.Attributes()
                                                    .Single(a => a.Name.LocalName == "Color")
                                                    .Value)
                                      .Distinct()
                                      .ToArray();
            //Console.WriteLine(colors.Length);
            foreach (var color in colors)
            {
                Console.WriteLine(color);
                var colorBrushes = brushElements.Where(x => x.Attributes()
                                                          .Single(a => a.Name.LocalName == "Color")
                                                          .Value == color);
                foreach (var key in colorBrushes.Select(e => e.Attributes().Single(x => x.Name.LocalName == "Key").Value).OrderBy(x => x))
                {
                    Console.WriteLine(key);
                }

                Console.WriteLine();
            }
        }

        [Test]
        public void DumpSystemColorKeys()
        {
            var keys = typeof(SystemColors).GetProperties(BindingFlags.Static | BindingFlags.Public)
                                                    .Where(x => typeof(ResourceKey).IsAssignableFrom(x.PropertyType))
                                                    .Select(x => x.Name)
                                                    .OrderBy(x => x)
                                                    .ToArray();
            foreach (var key in keys)
            {
                Console.WriteLine(key);
            }
        }
    }
}
