#region Disclaimer

/* 
 * OdysseyUI
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

#region Using directives
		using System;
		using System.Drawing;
		using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX.Direct3D;
#else
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.UserInterface.Devices;
#endif 
	#endregion

namespace AvengersUtd.Odyssey.UserInterface
{
    /// <summary>
    /// This static class internally handles user interaction with the interface.
    /// It also provides to the user general configurable settings.
    /// </summary>
    /// <remarks>
    /// For more information, see the <see href="http://www.avengersutd.com/wiki">Odyssey UI wiki.</see>
    /// </remarks>
    /// <example>Remember to use this call chain, before you start designing controls:
    /// <code>
    /// OdysseyUI.SetupHooks(form);
    /// StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
    /// StyleManager.LoadTextStyles("Odyssey TextStyles.ots");
    ///
    /// Hud hud = new Hud();
    /// hud.Id = "MainMenu";
    /// hud.Size = Settings.ScreenSize;
    /// OdysseyUI.CurrentHud = hud;
    /// 
    /// hud.BeginDesign();
    /// // Design your controls here 
    /// hud.EndDesign();
    /// </code></example>
    public static class OdysseyUI
    {
        static Hud currentHUD;
        static Device device;
        public static Control Owner
        { get; private set; }

        static readonly MouseButtons clickButton;

        static OdysseyUI()
        {
            if (SystemInformation.MouseButtonsSwapped)
                clickButton = MouseButtons.Right;
            else
                clickButton = MouseButtons.Left;
        }


        /// <summary>
        /// Gets or sets the DirectX device object.
        /// </summary>
        /// <remarks>Since OdysseyUI needs a reference to the device object to render the UI,
        /// to avoid you having to pass it as a parameter in each method that requires it, 
        /// a reference is stored and used when needed. Remember to assign it before you start
        /// designing the UI.</remarks>
        /// <value>The DirectX device object.</value>
        public static Device Device
        {
            get { return device; }
            set { device = value; }
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
            get { return currentHUD; }
            set { currentHUD = value; }
        }

        #region UI Input
        static void MouseMovementHandler(object sender, MouseEventArgs e)
        {
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (currentHUD.CaptureControl != null)
            {
                currentHUD.CaptureControl.ProcessMouseMovement(e);
                return;
            }

            // Check whether a modal window is displayed
            if (currentHUD.WindowManager.Foremost != null && currentHUD.WindowManager.Foremost.IsModal)
            {
                foreach (
                    BaseControl control in
                        TreeTraversal.PostOrderControlInteractionVisit(currentHUD.WindowManager.Foremost))
                {
                    handled = control.ProcessMouseMovement(e);
                    if (handled)
                    {
                        return;
                    }
                }
                currentHUD.WindowManager.Foremost.ProcessMouseMovement(e);

                return;
            }


            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(currentHUD))
            {
                handled = control.ProcessMouseMovement(e);
                if (handled)
                    break;
            }
            if (!handled)
                currentHUD.ProcessMouseMovement(e);
        }

         static void ProcessMouseInputPress(object sender, MouseEventArgs e)
        {
            bool handled = false;

            // Checks whether a modal window is displayed
            if (currentHUD.WindowManager.Foremost != null && currentHUD.WindowManager.Foremost.IsModal)
            {
                foreach (
                    BaseControl control in
                        TreeTraversal.PostOrderControlInteractionVisit(currentHUD.WindowManager.Foremost))
                {
                    handled = control.ProcessMousePress(e);
                    if (handled)
                        return;
                }
                currentHUD.WindowManager.Foremost.ProcessMousePress(e);
                return;
            }

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(currentHUD))
            {
                handled = control.ProcessMousePress(e);
                if (handled)
                    break;
            }

            if (!handled)
                currentHUD.ProcessMousePress(e);
        }

        static void ProcessMouseInputRelease(object sender, MouseEventArgs e)
        {
            bool handled = false;

            // Checks whether a control has captured the mouse pointer
            if (currentHUD.CaptureControl != null)
            {
                currentHUD.CaptureControl.ProcessMouseRelease(e);
                return;
            }

            // Checks whether a modal window is displayed
            if (currentHUD.WindowManager.Foremost != null && currentHUD.WindowManager.Foremost.IsModal)
            {
                foreach (
                    BaseControl control in
                        TreeTraversal.PostOrderControlInteractionVisit(currentHUD.WindowManager.Foremost))
                {
                    handled = control.ProcessMouseRelease(e);
                    if (handled)
                        return;
                }
                currentHUD.WindowManager.Foremost.ProcessMouseRelease(e);
                return;
            }

            // Proceeds with the rest
            foreach (BaseControl control in TreeTraversal.PostOrderControlInteractionVisit(currentHUD))
            {
                handled = control.ProcessMouseRelease(e);
                if (handled)
                    break;
            }

            if (!handled)
                currentHUD.ProcessMouseRelease(e);
        }

        static void ProcessKeyDown(object sender, KeyEventArgs e)
        {
            if (currentHUD.FocusedControl != null)
                currentHUD.FocusedControl.ProcessKeyDown(e);
        }

        static void ProcessKeyUp(object sender, KeyEventArgs e)
        {
            if (currentHUD.FocusedControl != null)
                currentHUD.FocusedControl.ProcessKeyUp(e);
        }

        static void ProcessKeyPress(object sender, KeyPressEventArgs e)
        {
            if (currentHUD.FocusedControl != null)
                currentHUD.FocusedControl.ProcessKeyPress(e);
        }

        /// <summary>
        /// This method hooks the events fired by the container control (a <see cref="System.Windows.Forms"/>
        /// for example) and allows them to be processed by the Odyssey UI.
        /// </summary>
        /// <param name="target">The container control that hosts the DirectX viewport.</param>
        public static void SetupHooks(Control target)
        {
            if (target == null)
                throw new ArgumentNullException("target", "The target control must not be null.");


            target.KeyDown += Keyboard.Instance.KeyDown;
            target.KeyUp += Keyboard.Instance.KeyUp;

            target.KeyDown += new
                KeyEventHandler(ProcessKeyDown);
            target.KeyUp += new
                KeyEventHandler(ProcessKeyUp);
            target.KeyPress += new
                KeyPressEventHandler(ProcessKeyPress);
            target.MouseDown += new
                MouseEventHandler(ProcessMouseInputPress);
            target.MouseUp += new
                MouseEventHandler(ProcessMouseInputRelease);
            target.MouseMove += new
                MouseEventHandler(MouseMovementHandler);

            Owner = target;
        }
        #endregion

        public static void ReleaseResources()
        {
            FontManager.Clear();
            DebugManager.Instance.Dispose();

            if (currentHUD != null)
                currentHUD.Dispose();
        }

        #region Configurable Settings

        /// <summary>
        /// Returns the mouse button used to function as the left mouse button. On some systems 
        /// </summary>
        /// <value>The button used to function as the left mouse button.</value>
        public static MouseButtons ClickButton
        {
            get { return clickButton; }
        }

        #endregion
    }
}