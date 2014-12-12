using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hourglass
{
    public static class FocusUtility
    {
        public static void RemoveFocus(FrameworkElement element)
        {
            FrameworkElement parent = (FrameworkElement)element.Parent;
            while (parent != null && parent is IInputElement && !((IInputElement)parent).Focusable)
                parent = (FrameworkElement)parent.Parent;
            DependencyObject scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, parent as IInputElement);
        }
    }
}
