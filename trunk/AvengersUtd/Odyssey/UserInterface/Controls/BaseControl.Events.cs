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
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Input;
using System.Windows.Input;
using System.Windows.Forms;
using MouseEventHandler = System.Windows.Forms.MouseEventHandler;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace AvengersUtd.Odyssey.UserInterface.Controls
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
            add { Events.AddHandler(EventMouseDown, value); }
            remove { Events.RemoveHandler(EventMouseDown, value); }
        }

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        public event MouseEventHandler MouseClick
        {
            add { Events.AddHandler(EventMouseClick, value); }
            remove { Events.RemoveHandler(EventMouseClick, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        public event MouseEventHandler MouseUp
        {
            add { Events.AddHandler(EventMouseUp, value); }
            remove { Events.RemoveHandler(EventMouseUp, value); }
        }

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        public event MouseEventHandler MouseWheel
        {
            add { Events.AddHandler(EventMouseWheel, value); }
            remove { Events.RemoveHandler(EventMouseWheel, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        public event MouseEventHandler MouseMove
        {
            add { Events.AddHandler(EventMouseMove, value); }
            remove { Events.RemoveHandler(EventMouseMove, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        public event EventHandler MouseEnter
        {
            add { Events.AddHandler(EventMouseEnter, value); }
            remove { Events.RemoveHandler(EventMouseEnter, value); }
        }

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        public event EventHandler MouseLeave
        {
            add { Events.AddHandler(EventMouseLeave, value); }
            remove { Events.RemoveHandler(EventMouseLeave, value); }
        }

        /// <summary>
        /// Raises the <see cref="MouseMove"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseMove(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseMove];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="MouseDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseDown(MouseEventArgs e)
        {
            
            IsClicked = true;
            Hud hud = OdysseyUI.CurrentHud;
            hud.ClickedControl = this;
            //hud.WindowManager.BringToFront(depth.WindowLayer);

            if (hud.FocusedControl != this)
                hud.FocusedControl.OnLostFocus(e);

            UpdateAppearance();

            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseDown];
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

            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseClick];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="MouseUp"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseUp(MouseEventArgs e)
        {
            IsClicked = false;
            OdysseyUI.CurrentHud.ClickedControl = null;

            UpdateAppearance();

            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseUp];
            if (handler != null)
                handler(this, e);
        }


        /// <summary>
        /// Raises the <see cref="MouseWheel"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMouseWheel(MouseEventArgs e)
        {
            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseWheel];
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

            if (ApplyStatusChanges)
            {
                if (AvengersUtd.Odyssey.UserInterface.Input.Mouse.ClickButton == e.Button)
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

            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseEnter];
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
            if (ApplyStatusChanges)
            {
                if (AvengersUtd.Odyssey.UserInterface.Input.Mouse.ClickButton == e.Button)
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

            MouseEventHandler handler = (MouseEventHandler) Events[EventMouseLeave];
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
        public event EventHandler<KeyEventArgs> KeyDown
        {
            add { Events.AddHandler(EventKeyDown, value); }
            remove { Events.RemoveHandler(EventMouseDown, value); }
        }

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        public event KeyPressEventHandler KeyPress
        {
            add { Events.AddHandler(EventKeyPress, value); }
            remove { Events.RemoveHandler(EventKeyPress, value); }
        }

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        public event EventHandler<KeyEventArgs> KeyUp
        {
            add { Events.AddHandler(EventKeyUp, value); }
            remove { Events.RemoveHandler(EventKeyUp, value); }
        }

        /// <summary>
        /// Raises the <see cref="KeyDown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnKeyDown(KeyEventArgs e)
        {
            EventHandler<KeyEventArgs> handler = (EventHandler<KeyEventArgs>)Events[EventKeyDown];
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
            EventHandler<KeyEventArgs> handler = (EventHandler<KeyEventArgs>)Events[EventKeyUp];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Touch Events
        static readonly object EventTouchDown;
        static readonly object EventTouchUp;

        //public event EventHandler<TouchEventArgs> MouseDown
        //{
        //    add { Events.AddHandler(EventMouseDown, value); }
        //    remove { Events.RemoveHandler(EventMouseDown, value); }
        //}
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
            add { Events.AddHandler(EventDisposing, value); }
            remove { Events.RemoveHandler(EventDisposing, value); }
        }

        /// <summary>
        /// Occurs when the control receives focus..
        /// </summary>
        public event EventHandler GotFocus
        {
            add { Events.AddHandler(EventGotFocus, value); }
            remove { Events.RemoveHandler(EventGotFocus, value); }
        }

        /// <summary>
        /// Occurs when the control loses focus.
        /// </summary>
        public event EventHandler LostFocus
        {
            add { Events.AddHandler(EventLostFocus, value); }
            remove { Events.RemoveHandler(EventLostFocus, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsVisible"/> property value changes.
        /// </summary>
        public event EventHandler VisibleChanged
        {
            add { Events.AddHandler(EventVisibleChanged, value); }
            remove { Events.RemoveHandler(EventVisibleChanged, value); }
        }

        /// <summary>
        /// Occurs when <see cref="Size"/> property value changes.
        /// </summary>
        public event EventHandler SizeChanged
        {
            add { Events.AddHandler(EventSizeChanged, value); }
            remove { Events.RemoveHandler(EventSizeChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="Position"/> property value changes.
        /// </summary>
        public event EventHandler PositionChanged
        {
            add { Events.AddHandler(EventPositionChanged, value); }
            remove { Events.RemoveHandler(EventPositionChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsHighlighted"/> property value changes.
        /// </summary>
        public event EventHandler HighlightedChanged
        {
            add { Events.AddHandler(EventHighlightedChanged, value); }
            remove { Events.RemoveHandler(EventHighlightedChanged, value); }
        }

        /// <summary>
        /// Occurs when the control is moved.
        /// </summary>
        public event EventHandler Move
        {
            add { Events.AddHandler(EventMove, value); }
            remove { Events.RemoveHandler(EventMove, value); }
        }

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        public event EventHandler MouseCaptureChanged
        {
            add { Events.AddHandler(EventMouseCaptureChanged, value); }
            remove { Events.RemoveHandler(EventMouseCaptureChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="DesignMode"/> property value changes.
        /// </summary>
        public event EventHandler DesignModeChanged
        {
            add { Events.AddHandler(EventDesignModeChanged, value); }
            remove { Events.RemoveHandler(EventDesignModeChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="Parent"/> property value changes.
        /// </summary>
        public event EventHandler ParentChanged
        {
            add { Events.AddHandler(EventParentChanged, value); }
            remove { Events.RemoveHandler(EventParentChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="TextStyle"/> property value changes.
        /// </summary>
        public event EventHandler TextStyleChanged
        {
            add { Events.AddHandler(EventTextStyleChanged, value); }
            remove { Events.RemoveHandler(EventTextStyleChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="IsSelected"/> property value changes.
        /// </summary>
        protected event EventHandler SelectedChanged
        {
            add { Events.AddHandler(EventSelectedChanged, value); }
            remove { Events.RemoveHandler(EventSelectedChanged, value); }
        }

        /// <summary>
        /// Occurs when the <see cref="Description"/> property value changes.
        /// </summary>
        public event EventHandler ControlStyleChanged
        {
            add { Events.AddHandler(EventControlStyleChanged, value); }
            remove { Events.RemoveHandler(EventControlStyleChanged, value); }
        }

        /// <summary>
        /// Occurs when a new control is added to the <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler ControlAdded
        {
            add { Events.AddHandler(EventControlAdded, value); }
            remove { Events.RemoveHandler(EventControlAdded, value); }
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="ControlAdded"/> event.
        /// </summary>
        /// <param name="e">The <see cref="AvengersUtd.Odyssey.UserInterface.ControlEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnControlAdded(ControlEventArgs e)
        {
            EventHandler<ControlEventArgs> handler =
                (EventHandler<ControlEventArgs>) Events[EventControlAdded];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ControlStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnControlStyleChanged(EventArgs e)
        {
            if (!DesignMode)
                UpdateAppearance();

            EventHandler handler = (EventHandler) Events[EventControlStyleChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DesignModeChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDesignModeChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventDesignModeChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Disposing"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnDisposing(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EventDisposing];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            Depth = Style.Depth.AsChildOf(parent.Depth);

            IContainer iContainer = this as IContainer;
            if (iContainer == null) return;
            foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(iContainer))
            //foreach (BaseControl ctl in iContainer.PrivateControlCollection.AllControls)
            {
                ctl.Depth = Style.Depth.AsChildOf(ctl.Parent.Depth);
            }

            if (DesignMode) return;
            ComputeAbsolutePosition();
            UpdatePositionDependantParameters();

            EventHandler handler = (EventHandler) Events[EventParentChanged];
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
            IsClicked = false;

            EventHandler handler = (EventHandler) Events[EventMouseCaptureChanged];
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
                IsFocused = true;

                UpdateAppearance();

                EventHandler handler = (EventHandler) Events[EventGotFocus];
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
            IsFocused = IsClicked = false;
            OdysseyUI.CurrentHud.FocusedControl = OdysseyUI.CurrentHud;

            UpdateAppearance();

            EventHandler handler = (EventHandler) Events[EventLostFocus];
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
            {
                OdysseyUI.CurrentHud.EnqueueForUpdate(this, IsVisible ? UpdateAction.Add : UpdateAction.Remove);
            }

            EventHandler handler = (EventHandler) Events[EventVisibleChanged];
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

            EventHandler handler = (EventHandler) Events[EventSizeChanged];
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

            UpdateAppearance(UpdateAction.Move);

            EventHandler handler = (EventHandler) Events[EventPositionChanged];
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

            EventHandler handler = (EventHandler) Events[EventHighlightedChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Move"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMove(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventMove];
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

            EventHandler handler = (EventHandler) Events[EventSelectedChanged];
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// Raises the <see cref="TextStyleChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected virtual void OnTextDescriptionChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventTextStyleChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion
    }
}