using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Properties;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Input;
using System.Windows;
using System.Windows.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using SlimDX.RawInput;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static partial class OdysseyUI
    {
        public static AvengersUtd.Odyssey.UserInterface.Input.Keyboard Keyboard { get; private set;  }
        public static AvengersUtd.Odyssey.UserInterface.Input.Mouse Mouse { get; private set; }

        static OdysseyUI()
        {
            Keyboard = new AvengersUtd.Odyssey.UserInterface.Input.Keyboard();
            Mouse = new AvengersUtd.Odyssey.UserInterface.Input.Mouse();
        }

        /// <summary>
        /// Gets or sets a reference to current <see cref="Hud"/> overlayed on the screen. Since
        /// you can theoretically build many different Huds for each game screen, this allows you
        /// to switch between them with ease. As soon as the value changes, from the next frame
        /// onwards, the new Hud will be rendered. 
        /// </summary>
        /// <value>The current <see cref="Hud"/> object.</value>
        public static Hud CurrentHud
        {
            get;
            set;
        }

        #region UI Input
        static void MouseMovementHandler(object sender, MouseEventArgs e)
        {

            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (CurrentHud.CaptureControl != null)
            {
                CurrentHud.CaptureControl.ProcessMouseMovement(e);
                return;
            }

            //// Check whether a modal window is displayed
            //if (CurrentHud.WindowManager.Foremost != null && CurrentHud.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentHud.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMouseMovement(e);
            //        if (handled)
            //        {
            //            return;
            //        }
            //    }
            //    CurrentHud.WindowManager.Foremost.ProcessMouseMovement(e);

            //    return;
            //}


            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessMouseMovement(e);
                if (handled)
                    break;
            }
            if (!handled)
                CurrentHud.ProcessMouseMovement(e);
        }

        static void ProcessMouseInputPress(object sender, MouseEventArgs e)
        {
            bool handled = false;

            //// Checks whether a modal window is displayed
            //if (CurrentHud.WindowManager.Foremost != null && CurrentHud.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentHud.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMousePress(e);
            //        if (handled)
            //            return;
            //    }
            //    CurrentHud.WindowManager.Foremost.ProcessMousePress(e);
            //    return;
            //}

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessMousePress(e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessMousePress(e);
        }

        static void ProcessMouseInputRelease(object sender, MouseEventArgs e)
        {
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (CurrentHud.CaptureControl != null)
            {
                CurrentHud.CaptureControl.ProcessMouseRelease(e);
                return;
            }

            //// Checks whether a modal window is displayed
            //if (CurrentHud.WindowManager.Foremost != null && CurrentHud.WindowManager.Foremost.IsModal)
            //{
            //    foreach (
            //        BaseControl control in
            //            TreeTraversal.PostOrderControlInteractionVisit(CurrentHud.WindowManager.Foremost))
            //    {
            //        handled = control.ProcessMouseRelease(e);
            //        if (handled)
            //            return;
            //    }
            //    CurrentHud.WindowManager.Foremost.ProcessMouseRelease(e);
            //    return;
            //}

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(CurrentHud))
            {
                handled = control.ProcessMouseRelease(e);
                if (handled)
                    break;
            }

            if (!handled)
                CurrentHud.ProcessMouseRelease(e);
        }

        static void ProcessKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (CurrentHud.FocusedControl != null)
                CurrentHud.FocusedControl.ProcessKeyDown(e);
        }

        static void ProcessKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (CurrentHud.FocusedControl != null)
                CurrentHud.FocusedControl.ProcessKeyUp(e);
        }

        static void ProcessKeyPress(object sender, KeyPressEventArgs e)
        {
            if (CurrentHud.FocusedControl != null)
                CurrentHud.FocusedControl.ProcessKeyPress(e);
        }

        /// <summary>
        /// This method hooks the events fired by the container control (a <see cref="System.Windows.Forms"/>
        /// for example) and allows them to be processed by the Odyssey UI.
        /// </summary>
        /// <param name="target">The container control that hosts the DirectX viewport.</param>
        public static void SetupHooks(Control target)
        {
            Contract.Requires<ArgumentNullException>(target != null,Resources.ERR_UI_TargetControlNull );

            //target.KeyDown += Keyboard.KeyDown;
            //target.KeyUp += Keyboard.KeyUp;
            //target.MouseDown += Mouse.MouseDown;
            //target.MouseUp += Mouse.MouseUp;
            //target.MouseMove += Mouse.MouseMove;

            target.KeyDown += ProcessKeyDown;
            target.KeyUp += ProcessKeyUp;
            target.KeyPress += ProcessKeyPress;
            target.MouseDown += ProcessMouseInputPress;
            target.MouseUp += ProcessMouseInputRelease;
            target.MouseMove += MouseMovementHandler;
        }

        public static void SetupHooksWpf(Window target)
        {
            Contract.Requires<ArgumentNullException>(target != null,Resources.ERR_UI_TargetControlNull );
            target.KeyDown += ProcessKeyDown;
            target.KeyUp += ProcessKeyUp;
            target.TouchDown += ProcessTouchDown;
            Global.Window = target;

            //target.KeyDown += ProcessKeyDown;
            
            //target.KeyPress += ProcessKeyPress;
            //target.MouseDown += ProcessMouseInputPress;
            //target.MouseUp += ProcessMouseInputRelease;
            //target.MouseMove += MouseMovementHandler;
        }

        public static void RemoveHooks(Control target)
        {
            Contract.Requires<ArgumentNullException>(target != null, Resources.ERR_UI_TargetControlNull);

            //target.KeyDown -= Keyboard.KeyDown;
            //target.KeyUp -= Keyboard.KeyUp;
            target.MouseDown -= Mouse.MouseDown;
            target.MouseUp -= Mouse.MouseUp;
            target.MouseMove -= Mouse.MouseMove;

            target.KeyDown -= ProcessKeyDown;
            target.KeyUp -= ProcessKeyUp;
            target.KeyPress -= ProcessKeyPress;
            target.MouseDown -= ProcessMouseInputPress;
            target.MouseUp -= ProcessMouseInputRelease;
            target.MouseMove -= MouseMovementHandler;

            Game.IsInputEnabled = false;
        }
        #endregion

        internal static void OnDispose(object sender, EventArgs e)
        {
//            CurrentHud.Dispose();
        }

        public static void Dispose()
        {
            OnDispose(null, EventArgs.Empty);
        }
    }
}
