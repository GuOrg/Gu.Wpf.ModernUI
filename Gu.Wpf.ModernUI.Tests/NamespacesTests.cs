namespace Gu.Wpf.ModernUI.Tests
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Markup;
    using NUnit.Framework;

    public class NamespacesTests
    {
        [Test]
        public void CheckStrings()
        {
            string uri = @"http://gu.com/ModernUI";
            string[] skip = { "Annotations", "Properties", "Win32", "XamlGeneratedNamespace" };
            var assembly = typeof(ModernWindow).Assembly;
            var strings = assembly.GetTypes()
                                  .Select(x => x.Namespace)
                                  .Distinct()
                                  .Where(x => !skip.Any(x.EndsWith))
                                  .OrderBy(x=>x)
                                  .ToArray();
            var attributes = assembly.CustomAttributes.Where(x => x.AttributeType == typeof(XmlnsDefinitionAttribute))
                                     .ToArray();
            var actuals = attributes.Select(a => a.ConstructorArguments[1].Value)
                                                             .OrderBy(x => x);
            CollectionAssert.AreEqual(strings, actuals);
            foreach (var attribute in attributes)
            {
                Assert.AreEqual(uri, attribute.ConstructorArguments[0].Value);
            }
            foreach (var s in strings)
            {
                Console.WriteLine(@"[assembly: XmlnsDefinition(""http://gu.com/ModernUI"", ""{0}"")]", s);
            }
            var prefixAttribute = assembly.CustomAttributes.Single(x=>x.AttributeType == typeof(XmlnsPrefixAttribute));
            Assert.AreEqual(uri, prefixAttribute.ConstructorArguments[0].Value);
        }
    }
}
