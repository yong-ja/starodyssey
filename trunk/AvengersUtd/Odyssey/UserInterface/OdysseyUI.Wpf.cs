using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Logging;
using System.Diagnostics.Contracts;
using AvengersUtd.Odyssey.UserInterface.Controls;
using TouchEventArgs = AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs;
using MouseEventArgs = AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static partial class OdysseyUI
    {
        public static void SetupHooksWpf(Window target)
        {
            Contract.Requires(target != null);
            target.KeyDown += ProcessKeyDown;
            target.KeyUp += ProcessKeyUp;
            target.TouchDown += ProcessTouchDown;
            target.TouchUp += ProcessTouchUp;
            target.TouchMove += ProcessTouchMove;

            //target.MouseDown += WpfProcessMouseDown;
            //target.MouseUp += WpfProcessMouseUp;
            //target.MouseMove += WpfProcessMouseMove;
            //target.LostTouchCapture += new EventHandler<System.Windows.Input.TouchEventArgs>(target_LostTouchCapture);
            Global.Window = target;

        }

        static void target_LostTouchCapture(object sender, System.Windows.Input.TouchEventArgs e)
        {
            Global.Window.ReleaseTouchCapture(e.TouchDevice);
        }

        static void WpfProcessMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProcessMouseInputPress(sender, (MouseEventArgs)e);
        }

        static void WpfProcessMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProcessMouseInputRelease(sender, (MouseEventArgs) e);
        }

        static void WpfProcessMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseMovementHandler(sender, (MouseEventArgs) e);
        }

        static void ProcessKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (CurrentHud.FocusedControl != null)
                CurrentHud.FocusedControl.ProcessKeyDown(new System.Windows.Forms.KeyEventArgs((Keys)KeyInterop.VirtualKeyFromKey(e.Key)));
        }

        static void ProcessKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (CurrentHud.FocusedControl != null)
                CurrentHud.FocusedControl.ProcessKeyUp(new System.Windows.Forms.KeyEventArgs((Keys)KeyInterop.VirtualKeyFromKey(e.Key)));
        }

        static void ProcessTouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            bool handled = false;
            

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessTouchDown((TouchEventArgs)e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessTouchDown((TouchEventArgs)e);

        }

        static void ProcessTouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            bool handled = false;

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessTouchUp((TouchEventArgs)e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessTouchUp((TouchEventArgs)e);
        }

        static void ProcessTouchMove(object sender, System.Windows.Input.TouchEventArgs e)
        {
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (CurrentHud.CaptureControl != null)
            {
                CurrentHud.CaptureControl.ProcessTouchMove((TouchEventArgs)e);
                return;
            }

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessTouchMove((TouchEventArgs)e);
                if (handled)
                    break;
            }


            if (!handled)
                CurrentHud.ProcessTouchMove((TouchEventArgs)e);
        }
    }
}
