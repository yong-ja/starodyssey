using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace AvengersUtd.BrickLab.Controls
{
    public class SelectAllTextOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += AssociatedObjectGotKeyboardFocus;
            AssociatedObject.GotMouseCapture += AssociatedObjectGotMouseCapture;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= AssociatedObjectGotKeyboardFocus;
            AssociatedObject.GotMouseCapture -= AssociatedObjectGotMouseCapture;
        }

        private void AssociatedObjectGotKeyboardFocus(object sender,
            System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            AssociatedObject.SelectAll();
        }

        private void AssociatedObjectGotMouseCapture(object sender,
            System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.SelectAll();
        }
    }
}
