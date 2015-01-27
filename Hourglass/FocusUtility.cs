using System.Windows;
using System.Windows.Input;

namespace Hourglass
{
    public static class FocusUtility
    {
        public static void RemoveFocus(FrameworkElement element)
        {
            var parent = (FrameworkElement)element.Parent;
            while (parent != null && !((IInputElement)parent).Focusable)
                parent = (FrameworkElement)parent.Parent;
            var scope = FocusManager.GetFocusScope(element);
            FocusManager.SetFocusedElement(scope, parent);
        }
    }
}
