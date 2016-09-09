namespace Gu.Wpf.ModernUI.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Provides addition visual tree helper methods.
    /// </summary>
    internal static class VisualTreeHelperEx
    {
        /// <summary>
        /// Gets specified visual state group.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        internal static VisualStateGroup TryGetVisualStateGroup(this DependencyObject dependencyObject, string groupName)
        {
            FrameworkElement root = GetImplementationRoot(dependencyObject);
            if (root == null) {
                return null;
            }
            return (VisualStateManager.GetVisualStateGroups(root)
                                     ?.OfType<VisualStateGroup>()
                                      .FirstOrDefault<VisualStateGroup>(group => string.CompareOrdinal(groupName, group.Name) == 0));
        }

        /// <summary>
        /// Gets the implementation root.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns></returns>
        internal static FrameworkElement GetImplementationRoot(this DependencyObject dependencyObject)
        {
            if (VisualTreeHelper.GetChildrenCount(dependencyObject) != 1) {
                return null;
            }
            return (VisualTreeHelper.GetChild(dependencyObject, 0) as FrameworkElement);
        }

        /// <summary>
        /// Returns a collection of the visual ancestor elements of specified dependency object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>
        /// A collection that contains the ancestors elements.
        /// </returns>
        internal static IEnumerable<DependencyObject> Ancestors(this DependencyObject dependencyObject)
        {
            var parent = dependencyObject;
            while (true) {
                parent = GetParent(parent);
                if (parent != null) {
                    yield return parent;
                }
                else {
                    break;
                }
            }
        }

        /// <summary>
        /// Returns a collection of visual elements that contain specified object, and the ancestors of specified object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns>
        /// A collection that contains the ancestors elements and the object itself.
        /// </returns>
        internal static IEnumerable<DependencyObject> AncestorsAndSelf(this DependencyObject dependencyObject)
        {
            if (dependencyObject == null) {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            var parent = dependencyObject;
            while (true) {
                if (parent != null) {
                    yield return parent;
                }
                else {
                    break;
                }
                parent = GetParent(parent);
            }
        }

        /// <summary>
        /// Gets the parent for specified dependency object.
        /// </summary>
        /// <param name="dependencyObject">The dependency object</param>
        /// <returns>The parent object or null if there is no parent.</returns>
        internal static DependencyObject GetParent(this DependencyObject dependencyObject)
        {
            if (dependencyObject == null) {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            var ce = dependencyObject as ContentElement;
            if (ce != null) {
                var parent = ContentOperations.GetParent(ce);
                if (parent != null) {
                    return parent;
                }

                var fce = ce as FrameworkContentElement;
                return fce?.Parent;
            }

            return VisualTreeHelper.GetParent(dependencyObject);
        }
    }
}
