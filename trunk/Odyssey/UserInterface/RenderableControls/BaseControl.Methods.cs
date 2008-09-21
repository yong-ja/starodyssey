#region Disclaimer

/* 
 * BaseControl
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

using System;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public abstract partial class BaseControl
    {
        #region [rgn] Public methods (5)

        /// <summary>
        /// In the <c>BaseControl</c> class, this method just computes values for
        /// the <see cref="Size"/> and <see cref="Position"/> dependant parameters.
        /// </summary>
        /// <remarks>Each control has to override this method in order to define
        /// the kind of shape that has to be created when the user interface is
        /// built.</remarks>
        public virtual void CreateShape()
        {
            UpdateSizeDependantParameters();
            UpdatePositionDependantParameters();
        }

        /// <summary>
        /// Abstract method that require inheritors to define how to update the
        /// shape of the control when reacting to appearance changing events.
        /// </summary>
        public abstract void UpdateShape();

        /// <summary>
        /// Updates this instance of the control by updating the current appearance
        /// status and rewriting the vertexbuffer if applicable.
        /// </summary>
        public virtual void UpdateAppearance()
        {
            if (IsVisible)
            {
                if (applyStatusChanges)
                    UpdateStatus();

                if (!DesignMode && !IsBeingRemoved && ControlStyle.Shape != Shape.None)
                {
                    UpdateShape();
                    OdysseyUI.CurrentHud.EnqueueForUpdate(this);
                }
            }
            return;
        }

        /// <summary>
        /// Programmatically focuses this <see cref="BaseControl"/> object, <b>if</b> it is focusable.
        /// </summary>
        public void Focus()
        {
            if (isFocusable)
                OnGotFocus(EventArgs.Empty);
        }

        /// <summary>
        /// Returns the window that this control belongs to, if any.
        /// </summary>
        /// <returns>The <see cref="Window"/> reference the control belongs to; <c>null</c> if the control doesn't
        /// belong to any window.</returns>
        public Window FindWindow()
        {
            if (depth.WindowLayer == 0)
                return null;
            else
                return OdysseyUI.CurrentHud.WindowManager[depth.WindowLayer - 1];
        }

        #endregion [rgn]

        #region [rgn] Protected methods 

        /// <summary>
        /// This method should be overriden by inheritors to automatically
        /// update all position dependant parameters.
        /// </summary>
        /// <remarks>This method is called even in the constructor.</remarks>
        protected abstract void UpdatePositionDependantParameters();

        /// <summary>
        /// In the <c>BaseControl</c> class this method just computes
        /// the <see cref="ClientSize"/>. Inheritors should override this method
        /// to automatically update any <see cref="Size"/> dependant parameter.
        /// </summary>
        /// <remarks>This method is called even in the constructor.</remarks>
        protected virtual void UpdateSizeDependantParameters()
        {
            clientSize = new Size(Size.Width - (borderSize*2 + padding.Horizontal),
                                  Size.Height - (borderSize*2 + padding.Vertical));
        }

        /// <summary>
        /// Updates the status of the control.
        /// </summary>
        protected void UpdateStatus()
        {
            if (IsEnabled)
            {
                if (IsClicked)
                {
                    innerAreaColor = ControlStyle.ColorArray.Clicked;
                    borderColor = ControlStyle.ColorArray.BorderHighlighted;
                }
                else if (IsSelected)
                {
                    innerAreaColor = ControlStyle.ColorArray.Selected;
                    borderColor = ControlStyle.ColorArray.BorderHighlighted;
                }
                else if (IsFocused)
                {
                    innerAreaColor = ControlStyle.ColorArray.Focused;
                    borderColor = ControlStyle.ColorArray.BorderHighlighted;
                }
                else if (IsHighlighted)
                {
                    innerAreaColor = ControlStyle.ColorArray.Highlighted;
                    borderColor = ControlStyle.ColorArray.BorderHighlighted;
                }
                else
                {
                    innerAreaColor = ControlStyle.ColorArray.Enabled;
                    borderColor = ControlStyle.ColorArray.BorderEnabled;
                }
            }
            else
            {
                innerAreaColor = ControlStyle.ColorArray.Disabled;
            }
        }

        #endregion [rgn]

        protected void ApplyControlStyle(ControlStyle newStyle)
        {
            controlStyle = newStyle;

            if (!controlStyle.Size.IsEmpty)
                size = controlStyle.Size;

            if (!controlStyle.Padding.IsEmpty)
                padding = controlStyle.Padding;

            borderSize = controlStyle.BorderSize;
            borderStyle = controlStyle.BorderStyle;
            innerAreaColor = controlStyle.ColorArray.Enabled;
            borderColor = controlStyle.ColorArray.BorderEnabled;

            textStyle = StyleManager.GetTextStyle(controlStyle.TextStyleClass);

            topLeftPosition = new Vector2(Padding.Left + borderSize, Padding.Top + borderSize);
        }

        #region Event Processing methods

        internal virtual bool ProcessMousePress(MouseEventArgs e)
        {
            if (canRaiseEvents && IntersectTest(e.Location))
            {
                if (e.Button == OdysseyUI.ClickButton && !isClicked && isEnabled)
                {
                    OnMouseDown(e);

                    if (isFocusable && !isFocused)
                        OnGotFocus(EventArgs.Empty);
                }

                return true;
            }
            else
                return false;
        }

        internal virtual bool ProcessMouseRelease(MouseEventArgs e)
        {
            if (canRaiseEvents && (hasCaptured || IntersectTest(e.Location)))
            {
                if (isClicked)
                {
                    if (e.Button != MouseButtons.None)
                        OnMouseClick(e);
                }
                OnMouseUp(e);
                return true;
            }
            else
            {
                if (isClicked)
                    isClicked = false;
                return false;
            }
        }

        internal virtual void ProcessKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        internal virtual void ProcessKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        internal virtual void ProcessKeyPress(KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }

        internal virtual bool ProcessMouseMovement(MouseEventArgs e)
        {
            if (hasCaptured || IntersectTest(e.Location))
            {
                if (canRaiseEvents)
                {
                    if (!isInside)
                    {
                        OnMouseEnter(e);
                    }

                    if (e.Delta != 0)
                        OnMouseWheel(e);

                    OnMouseMove(e);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}