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

            target.MouseDown += WpfProcessMouseDown;
            Global.Window = target;

        }

        static void WpfProcessMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ProcessMouseInputPress(sender, (MouseEventArgs)e);
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
            Point p = e.GetTouchPoint(Global.Window).Position;
            Vector2 vP = new Vector2((float)p.X, (float)p.Y);
            LogEvent.Engine.Write(p.ToString());

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessTouchDown( new TouchEventArgs(vP));
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessTouchDown(new TouchEventArgs(vP));

        }

        static void ProcessTouchUp(object sender, System.Windows.Input.TouchEventArgs e)
        {
            bool handled = false;
            Point p = e.GetTouchPoint(Global.Window).Position;
            Vector2 vP = new Vector2((float)p.X, (float)p.Y);
            LogEvent.Engine.Write(p.ToString());

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessTouchUp(new TouchEventArgs(vP));
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessTouchUp(new TouchEventArgs(vP));
        }
    }
}
