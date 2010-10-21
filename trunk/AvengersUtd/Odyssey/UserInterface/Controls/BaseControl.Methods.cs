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
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Input;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Xml;
using SlimDX;


namespace AvengersUtd.Odyssey.UserInterface.Controls
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
        public abstract void CreateShape();

        /// <summary>
        /// Abstract method that require inheritors to define how to update the
        /// shape of the control when reacting to appearance changing events.
        /// </summary>
        public abstract void UpdateShape();

        /// <summary>
        /// Updates this instance of the control by updating the current appearance
        /// status and rewriting the vertexbuffer if applicable.
        /// </summary>
        public virtual void UpdateAppearance(UpdateAction updateAction = UpdateAction.UpdateShape)
        {
            
            if (isVisible && !DesignMode 
                && (ApplyStatusChanges || updateAction == UpdateAction.Move)
                && !IsBeingRemoved && Description.Shape != Shape.None)
            {
                UpdateStatus();
                OdysseyUI.CurrentHud.EnqueueForUpdate(this, updateAction);
            }
            return;
        }

        public Designer GetDesigner()
        {
            return new Designer
            {
                Position = AbsoluteOrthoPosition,
                BorderSize = new Thickness(Description.BorderSize),
                Width = Size.Width,
                Height = Size.Height
            };
        }

        /// <summary>
        /// Programmatically focuses this <see cref="BaseControl"/> object, <b>if</b> it is focusable.
        /// </summary>
        public void Focus()
        {
            if (isFocusable)
                OnGotFocus(EventArgs.Empty);
        }

        public void SendToBack()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, OdysseyUI.CurrentHud.HudDescription.ZFar);
        }

        public void BringToFront()
        {
            Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, OdysseyUI.CurrentHud.HudDescription.ZNear); 
        }

        ///// <summary>
        ///// Returns the window that this control belongs to, if any.
        ///// </summary>
        ///// <returns>The <see cref="Window"/> reference the control belongs to; <c>null</c> if the control doesn't
        ///// belong to any window.</returns>
        //public Window FindWindow()
        //{
        //    if (depth.WindowLayer == 0)
        //        return null;
        //    else
        //        return OdysseyUI.CurrentHud.WindowManager[depth.WindowLayer - 1];
        //}

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
        protected abstract void UpdateSizeDependantParameters();

        /// <summary>
        /// Updates the status of the control.
        /// </summary>
        protected void UpdateStatus()
        {
            if (IsEnabled)
            {
                if (IsClicked)
                {
                    InnerAreaColor = Description.ColorArray.Clicked;
                    BorderColor = Description.ColorArray.BorderHighlighted;
                }
                else if (IsSelected)
                {
                    InnerAreaColor = Description.ColorArray.Selected;
                    BorderColor = Description.ColorArray.BorderHighlighted;
                }
                else if (IsFocused)
                {
                    InnerAreaColor = Description.ColorArray.Focused;
                    BorderColor = Description.ColorArray.BorderHighlighted;
                }
                else if (IsHighlighted)
                {
                    InnerAreaColor = Description.ColorArray.Highlighted;
                    BorderColor = Description.ColorArray.BorderHighlighted;
                }
                else
                {
                    InnerAreaColor = Description.ColorArray.Enabled;
                    BorderColor = Description.ColorArray.BorderEnabled;
                }
            }
            else
            {
                InnerAreaColor = Description.ColorArray.Disabled;
            }
        }

        #endregion [rgn]

        protected void ApplyControlDescription(ControlDescription newDescription)
        {
            description = newDescription;

            if (!description.Size.IsEmpty)
                Size = description.Size;

            ClientSize = new Size(Size.Width - (Description.BorderSize * 2 + Description.Padding.Horizontal),
                                  Size.Height - (Description.BorderSize * 2 + Description.Padding.Vertical));

            InnerAreaColor = description.ColorArray.Enabled;
            BorderColor = description.ColorArray.BorderEnabled;
            TopLeftPosition = new Vector2(description.Padding.Left + description.BorderSize,
                                          description.Padding.Top + description.BorderSize);

            textDescription = StyleManager.GetTextDescription(description.TextStyleClass);
        }

        #region Event Processing methods

        internal virtual bool ProcessMousePress(MouseEventArgs e)
        {
            if (!canRaiseEvents || !IntersectTest(e.Location))
                return false;

            if (e.Button == Mouse.ClickButton && !IsClicked && isEnabled)
            {
                OnMouseDown(e);

                if (isFocusable && !IsFocused)
                    OnGotFocus(EventArgs.Empty);
            }

            return true;
        }

        internal virtual bool ProcessMouseRelease(MouseEventArgs e)
        {
            
            if (canRaiseEvents && (HasCaptured || IntersectTest(e.Location)))
            {
                if (IsClicked)
                {
                    if (e.Button != MouseButtons.None)
                        OnMouseClick(e);
                }
                OnMouseUp(e);
                return true;
            }

            if (IsClicked)
                IsClicked = false;
            return false;
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
            if (!HasCaptured && !IntersectTest(e.Location))
                return false;

            if (canRaiseEvents)
            {
                if (!isInside)
                    OnMouseEnter(e);

                if (e.Delta != 0)
                    OnMouseWheel(e);

                OnMouseMove(e);
                return true;
            }
            return false;
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Computes the eight points bounding this control.
        /// <remarks>Index 0 is northwest corner.<br>Index 7 is west point.</br></remarks>
        /// </summary>
        /// <param name="control">The control whose bounds to compute.</param>
        /// <returns>The array of points, stored in clockwise order.</returns>
        public Vector2[] ComputeBounds(BaseControl control)
        {
            Vector2 cornerNE = control.AbsolutePosition;
            int width = control.Size.Width;
            int height = control.Size.Height;

            return new[]{
                           cornerNE,
                           new Vector2(cornerNE.X + width/2f, cornerNE.Y),
                           new Vector2(cornerNE.X + width, cornerNE.Y),
                           new Vector2(cornerNE.X + width, cornerNE.Y + height/2f),
                           new Vector2(cornerNE.X + width, cornerNE.Y + height),
                           new Vector2(cornerNE.X + width/2f, cornerNE.Y + height),
                           new Vector2(cornerNE.X, cornerNE.Y + height),
                           new Vector2(cornerNE.X, cornerNE.Y + height/2f)
                       };
        }

        #endregion
    }
}