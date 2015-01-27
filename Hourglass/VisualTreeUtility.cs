using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Hourglass
{
    public class VisualTreeUtility
    {
        public static IEnumerable<T> GetVisualChildren<T>(DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null)
                yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T)
                    yield return (T)child;

                foreach (T childOfChild in GetVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }
    }
}
