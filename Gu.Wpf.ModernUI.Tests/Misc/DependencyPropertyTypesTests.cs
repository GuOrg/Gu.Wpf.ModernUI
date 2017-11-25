namespace Gu.Wpf.ModernUI.Tests.Misc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using NUnit.Framework;

    [Apartment(ApartmentState.STA)]
    [SuppressMessage("ReSharper", "HeuristicUnreachableCode")]
    public class DependencyPropertyTypesTests
    {
        [TestCaseSource(typeof(DependencyPropertyTypesSource))]
        public void FieldNames(DependencyPropertyType source)
        {
            foreach (var field in source.DependencyPropertyFields)
            {
                Console.WriteLine(field.Name);
                var dp = (DependencyProperty)field.GetValue(null);
                Assert.AreEqual(field.Name, dp.Name + "Property");
            }
        }

        [Explicit]
        [TestCaseSource(typeof(DependencyPropertyTypesSource))]
        public void OwnerTypes(DependencyPropertyType source)
        {
            string result = string.Empty;
            foreach (var field in source.DependencyPropertyFields)
            {
                var dp = (DependencyProperty)field.GetValue(null);
                var descriptor = DependencyPropertyDescriptor.FromProperty(dp, source.ControlType);
                if (descriptor == null)
                {
                    result += dp.Name;
                    continue;
                }

                Assert.AreEqual(dp.OwnerType, source.ControlType);
            }

            if (!string.IsNullOrEmpty(result))
            {
                Assert.Inconclusive(result);
            }
        }

        [TestCaseSource(typeof(DependencyPropertyTypesSource))]
        public void BackingProperties(DependencyPropertyType source)
        {
            string result =string.Empty;
            foreach (var field in source.DependencyPropertyFields)
            {
                var dp = (DependencyProperty)field.GetValue(null);
                var descriptor = DependencyPropertyDescriptor.FromProperty(dp, source.ControlType);
                if (descriptor == null)
                {
                    result += dp.Name;
               }

                if (descriptor != null && !descriptor.IsAttached)
                {
                    var propertyInfo = source.ControlType.GetProperty(dp.Name);
                    Assert.IsNotNull(propertyInfo);
                    Assert.AreEqual(dp.PropertyType, propertyInfo.PropertyType);
                }
            }

            if (!string.IsNullOrEmpty(result))
            {
                Assert.Inconclusive(result);
            }
        }

        [Explicit]
        [TestCaseSource(typeof(DependencyPropertyTypesSource))]
        public void Values(DependencyPropertyType source)
        {
            Assert.Fail("Write to backing prop, then GetValue");
            Assert.Fail("SetValue then read from backing prop");
        }

        [TestCaseSource(typeof(DependencyPropertyTypesSource))]
        public void CreateInstanceOfControl(DependencyPropertyType source)
        {
            Assert.IsNotNull(source.Control);
        }

        [Test]
        public void NotEmpty()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var source = new DependencyPropertyTypesSource();
            CollectionAssert.IsNotEmpty(source);
            //foreach (var source in source)
            //{
            //    CollectionAssert.IsNotEmpty(source.DependencyPropertyFields);
            //}
        }

        private class DependencyPropertyTypesSource : List<DependencyPropertyType>
        {
            public DependencyPropertyTypesSource()
            {
                var controlTypes = typeof(ModernWindow).Assembly.GetTypes()
                                                       .Where(x => typeof(Control).IsAssignableFrom(x) && !x.IsAbstract)
                                                       .OrderBy(x => x.Name)
                                                       .ToArray();
                foreach (var controlType in controlTypes)
                {
                    this.Add(new DependencyPropertyType(controlType));
                }
            }
        }

        public class DependencyPropertyType
        {
            public Type ControlType;
            public IEnumerable<FieldInfo> DependencyPropertyFields;

            public DependencyPropertyType(Type controlType)
            {
                Console.WriteLine("Processing: {0}", controlType.Name);
                this.ControlType = controlType;
                this.DependencyPropertyFields =
                    controlType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                               .Where(f => f.FieldType == typeof(DependencyProperty))
                               .ToArray();
            }

            public Control Control => (Control)Activator.CreateInstance(this.ControlType);

            public override string ToString()
            {
                return this.ControlType.Name;
            }
        }
    }
}
