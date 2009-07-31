#region Disclaimer

/* 
 * BaseControl
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Websit http://www.avengersutd.com
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

using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Helpers;

namespace AvengersUtd.Odyssey.UserInterface
{
    public abstract partial class BaseControl
    {
        #region MouseEvents

        static readonly object EventMouseClick;
        static readonly object EventMouseDown;
        static readonly object EventMouseEnter;
        static readonly object EventMouseLeave;
        static readonly object EventMouseMove;
        static readonly object EventMouseWheel;
        static readonly object EventMouseUp;


        /// <summary>
        /// Occurs when mouse pointer is over the control and a mouse button is pressed..
        /// </summary>
        public event MouseEventHandler MouseDown
        {
            add { eventHandlerList.AddHandler(EventMouseDown, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseDown, value); }
        }

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        public event MouseEventHandler MouseClick
        {
            add { eventHandlerList.AddHandler(EventMouseClick, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseClick, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        public event MouseEventHandler MouseUp
        {
            add { eventHandlerList.AddHandler(EventMouseUp, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseUp, value); }
        }

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        public event MouseEventHandler MouseWheel
        {
            add { eventHandlerList.AddHandler(EventMouseWheel, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseWheel, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        public event MouseEventHandler MouseMove
        {
            add { eventHandlerList.AddHandler(EventMouseMove, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseMove, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        public event EventHandler MouseEnter
        {
            add { eventHandlerList.AddHandler(EventMouseEnter, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseEnter, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        public event EventHandler MouseLeave
        {
            add { eventHandlerList.AddHandler(EventMouseLeave, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseLeave, value); }
        }


        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseMove];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            
            isClicked = true;
            Hud hud = OdysseyUI.CurrentHud;
            hud.ClickedControl = this;
            hud.WindowManager.BringToFront(depth.WindowLayer);

            if (hud.FocusedControl != this)
                hud.FocusedControl.OnLostFocus(e);

            UpdateAppearance();

            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseDown];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseClick"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseClick(MouseEventArgs e)
        {
            UpdateAppearance();

            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseClick];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            isClicked = false;
            OdysseyUI.CurrentHud.ClickedControl = null;

            UpdateAppearance();

            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseUp];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="MouseWheel"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseWheel];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseEnter(MouseEventArgs e)
        {
            //DebugManager.LogToScreen(
            //    DateTime.Now.Millisecond + " Entering " +
            //    id + " H: " + isHighlighted);

            isInside = true;
            if (OdysseyUI.CurrentHud.EnteredControl.IsInside)
                OdysseyUI.CurrentHud.EnteredControl.OnMouseLeave(e);

            OdysseyUI.CurrentHud.EnteredControl = this;

            if (applyStatusChanges)
            {
                if (OdysseyUI.ClickButton == e.Button)
                {
                    if (OdysseyUI.CurrentHud.ClickedControl == this)
                    {
                        IsHighlighted = IsClicked = true;
                    }
                    else
                        return;
                }
                else
                {
                    IsHighlighted = true;
                }
            }

            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseEnter];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseLeave(MouseEventArgs e)
        {
            //DebugManager.LogToScreen(
            //    DateTime.Now.Millisecond + " Leaving " +
            //    id + " H: " + isHighlighted);

            isInside = false;
            if (applyStatusChanges)
            {
                if (OdysseyUI.ClickButton == e.Button)
                {
                    if (OdysseyUI.CurrentHud.ClickedControl == this)
                    {
                        IsHighlighted = IsClicked = false;
                    }
                    else
                        return;
                }
                else
                {
                    IsHighlighted = false;
                }
            }

            MouseEventHandler handler = (MouseEventHandler) eventHandlerList[EventMouseLeave];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Keyboard Events

        static readonly object EventKeyDown;
        static readonly object EventKeyPress;
        static readonly object EventKeyUp;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public event KeyEventHandler KeyDown
        {
            add { eventHandlerList.AddHandler(EventKeyDown, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseDown, value); }
        }

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public event KeyPressEventHandler KeyPress
        {
            add { eventHandlerList.AddHandler(EventKeyPress, value); }
            remove { eventHandlerList.RemoveHandler(EventKeyPress, value); }
        }

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        public event KeyEventHandler KeyUp
        {
            add { eventHandlerList.AddHandler(EventKeyUp, value); }
            remove { eventHandlerList.RemoveHandler(EventKeyUp, value); }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            KeyEventHandler handler = (KeyEventHandler) Events[EventKeyDown];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="KeyPress"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyPressEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyPress(KeyPressEventArgs e)
        {
            KeyPressEventHandler handler = (KeyPressEventHandler) Events[EventKeyPress];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="KeyUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyUp(KeyEventArgs e)
        {
            KeyEventHandler handler = (KeyEventHandler) Events[EventKeyUp];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Control Events

        static readonly object EventControlAdded;
        static readonly object EventControlStyleChanged;
        static readonly object EventDesignModeChanged;
        static readonly object EventDisposing;
        static readonly object EventGotFocus;
        static readonly object EventHighlightedChanged;
        static readonly object EventLostFocus;
        static readonly object EventMouseCaptureChanged;
        static readonly object EventMove;
        static readonly object EventParentChanged;
        static readonly object EventPositionChanged;
        static readonly object EventSelectedChanged;
        static readonly object EventSizeChanged;
        static readonly object EventTextStyleChanged;
        static readonly object EventVisibleChanged;

        #region Events declaration

        public event EventHandler Disposing
        {
            add { eventHandlerList.AddHandler(EventDisposing, value); }
            remove { eventHandlerList.RemoveHandler(EventDisposing, value); }
        }

        /// <summary>
        /// Occurs when the control receives focus..
        /// </summary>
        public event EventHandler GotFocus
        {
            add { eventHandlerList.AddHandler(EventGotFocus, value); }
            remove { eventHandlerList.RemoveHandler(EventGotFocus, value); }
        }

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        public event EventHandler LostFocus
        {
            add { eventHandlerList.AddHandler(EventLostFocus, value); }
            remove { eventHandlerList.RemoveHandler(EventLostFocus, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsVisible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged
        {
            add { eventHandlerList.AddHandler(EventVisibleChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventVisibleChanged, value); }
        }

        /// <summary>
        /// Occurs when <see cref="Size"/> property value changes.
        /// </summary>
        public event EventHandler SizeChanged
        {
            add { eventHandlerList.AddHandler(EventSizeChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventSizeChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="Position"/> property value changes.
        /// </summary>
        public event EventHandler PositionChanged
        {
            add { eventHandlerList.AddHandler(EventPositionChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventPositionChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsHighlighted"/> property value changes.
        /// </summary>
        public event EventHandler HighlightedChanged
        {
            add { eventHandlerList.AddHandler(EventHighlightedChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventHighlightedChanged, value); }
        }

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        public event EventHandler Move
        {
            add { eventHandlerList.AddHandler(EventMove, value); }
            remove { eventHandlerList.RemoveHandler(EventMove, value); }
        }

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        public event EventHandler MouseCaptureChanged
        {
            add { eventHandlerList.AddHandler(EventMouseCaptureChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventMouseCaptureChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="DesignMode"/> property value changes.
        /// </summary>
        public event EventHandler DesignModeChanged
        {
            add { eventHandlerList.AddHandler(EventDesignModeChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventDesignModeChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property value changes.
        /// </summary>
        public event EventHandler ParentChanged
        {
            add { eventHandlerList.AddHandler(EventParentChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventParentChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="TextStyle"/> property value changes.
        /// </summary>
        public event EventHandler TextStyleChanged
        {
            add { eventHandlerList.AddHandler(EventTextStyleChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventTextStyleChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsSelected"/> property value changes.
        /// </summary>
        protected event EventHandler SelectedChanged
        {
            add { eventHandlerList.AddHandler(EventSelectedChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventSelectedChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="ControlStyle"/> property value changes.
        /// </summary>
        public event EventHandler ControlStyleChanged
        {
            add { eventHandlerList.AddHandler(EventControlStyleChanged, value); }
            remove { eventHandlerList.RemoveHandler(EventControlStyleChanged, value); }
        }

        /// <summary>
        /// Occurs when a new control is added to the <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler ControlAdded
        {
            add { eventHandlerList.AddHandler(EventControlAdded, value); }
            remove { eventHandlerList.RemoveHandler(EventControlAdded, value); }
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="AvengersUtd.Odyssey.UserInterface.ControlEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnControlAdded(ControlEventArgs e)
        {
            EventHandler<ControlEventArgs> handler =
                (EventHandler<ControlEventArgs>) eventHandlerList[EventControlAdded];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ControlStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnControlStyleChanged(EventArgs e)
        {
            if (!OdysseyUI.CurrentHud.DesignMode)
                UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventControlStyleChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DesignModeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDesignModeChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) eventHandlerList[EventDesignModeChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Disposing"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDisposing(EventArgs e)
        {
            EventHandler handler = (EventHandler)eventHandlerList[EventDisposing];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            ComputeAbsolutePosition();
            EventHandler handler = (EventHandler) eventHandlerList[EventParentChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseCaptureChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseCaptureChanged(EventArgs e)
        {
            OdysseyUI.CurrentHud.CaptureControl = null;
            isClicked = false;

            EventHandler handler = (EventHandler) eventHandlerList[EventMouseCaptureChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="GotFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            if (this != OdysseyUI.CurrentHud.FocusedControl)
            {
                OdysseyUI.CurrentHud.FocusedControl.OnLostFocus(e);

                OdysseyUI.CurrentHud.FocusedControl = this;
                isFocused = true;

                UpdateAppearance();

                EventHandler handler = (EventHandler) eventHandlerList[EventGotFocus];
                if (handler != null)
                    handler(this, e);
            }
            else return;
        }

        /// <summary>
        /// Raises the <see cref="LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            isFocused = isClicked = false;
            OdysseyUI.CurrentHud.FocusedControl = OdysseyUI.CurrentHud;

            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventLostFocus];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="VisibleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
            if (!DesignMode)
                OdysseyUI.CurrentHud.EndDesign();

            EventHandler handler = (EventHandler) eventHandlerList[EventVisibleChanged];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="SizeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSizeChanged(EventArgs e)
        {
            UpdateSizeDependantParameters();
            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventSizeChanged];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="PositionChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnPositionChanged(EventArgs e)
        {
            if (parent != null)
                ComputeAbsolutePosition();

            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventPositionChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="HighlightedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnHighlightedChanged(EventArgs e)
        {
            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventHighlightedChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Move"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMove(EventArgs e)
        {
            UpdatePositionDependantParameters();
            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventMove];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="SelectedChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnSelectedChanged(EventArgs e)
        {
            UpdateAppearance();

            EventHandler handler = (EventHandler) eventHandlerList[EventSelectedChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="TextStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTextStyleChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) eventHandlerList[EventTextStyleChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}