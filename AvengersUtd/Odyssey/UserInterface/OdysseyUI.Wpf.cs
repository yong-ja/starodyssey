using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Forms;
using System.Windows;
using SlimDX;
using AvengersUtd.Odyssey.Utils.Logging;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static partial class OdysseyUI
    {
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
            Point p = e.GetTouchPoint(Global.Window).Position;
            //TouchPointCollection tpColl = e.GetIntermediateTouchPoints(Global.Window);
            LogEvent.Engine.Write(p.ToString());
            if (CurrentHud.TouchOverlay != null)
                CurrentHud.TouchOverlay.ProcessTouchDown(new UserInterface.Input.TouchEventArgs(new Vector2((float)p.X, (float)p.Y)));
        }
    }
}
