#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public abstract partial class BaseControl
    {
        #region Public properties

        /// <summary>
        ///   Gets the zero based index of this control in the <see cref = "ContainerControl" />.
        /// </summary>
        /// <value>The zero based index.</value>
        public int Index { get; internal set; }

        /// <summary>
        ///   Gets or sets the <see cref = "TextStyle" /> class to use for this control.
        /// </summary>
        /// <value>
        ///   The name of the <see cref = "TextStyle" /> class to use. Classes are defined in a
        ///   <c><b>[Theme Name]</b> TextStyles.ots</c> file.
        /// </value>
        /// <remarks>
        ///   If you <b>set</b> the class, it will cause that class to be recovered
        ///   from the static cache in memory. If there's no <see cref = "TextStyle" /> object present
        ///   in memory, the <b>Default</b> style will be used instead.
        /// </remarks>
        public string TextDescriptionClass
        {
            get { return textDescription.Name; }
            set
            {
                if (textDescription.Name != value)
                {
                    textDescription = StyleManager.GetTextDescription(value);
                }
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref = "TextDescription" /> to use for this control.
        /// </summary>
        /// <value>
        ///   The <see cref = "TextDescription" /> object that contains information on how to format the text of
        ///   this control.
        /// </value>
        public TextDescription TextDescription
        {
            get { return textDescription; }
            set
            {
                if (textDescription != value)
                {
                    textDescription = value;
                    OnTextDescriptionChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   Gets or sets the <see cref = "Description" /> class to use for this control.
        /// </summary>
        /// <value>The name of the <see cref = "TextStyle" /> class to use. Classes are defined
        ///   in a <c><b>[Theme Name]</b> TextStyles.ots</c> file.
        /// </value>
        public string ControlDescriptionClass
        {
            get { return description.Name; }
            set
            {
                if (description.Name != value)
                {
                    Description = StyleManager.GetControlDescription(value);
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the control is in design mode.
        /// </summary>
        /// <value><c>true</c> if the control is in design mode; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   While in design mode, certain events are not fired.
        /// </remarks>
        public virtual bool DesignMode
        {
            get { return designMode; }
            protected internal set
            {
                if (designMode != value)
                {
                    designMode = value;
                    OnDesignModeChanged(EventArgs.Empty);

                    IContainer container = this as IContainer;
                    if (container != null)
                    {
                        foreach (BaseControl childControl in container.PrivateControlCollection.AllControls)
                            childControl.DesignMode = value;
                    }
                }
            }
        }

        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        ///   Determines whether this control has captured the mouse pointer.
        /// </summary>
        /// <value><c>true</c> if the control has captured the mouse cursor, <c>false</c> otherwise.</value>
        /// <remarks>
        ///   When a control captures the mouse pointer, events are only sent to that control.
        /// </remarks>
        public bool HasCaptured { get; internal set; }

        public bool Autosize { get; set; }

        /// <summary>
        ///   Determines whether this control can be focused.
        /// </summary>
        /// <value><c>true</b> if the control can be focused; <c>false</c> otherwise.</value>
        public bool IsFocusable
        {
            get { return isFocusable; }
            internal set { isFocusable = value; }
        }

        /// <summary>
        ///   Gets the top left position in the client area of the control.
        /// </summary>
        /// <value>The top left position.</value>
        /// <remarks>
        ///   The top left position is computed considering the <see cref = "BorderSize" /> value
        ///   and the <see cref = "Thickness" />value.
        /// </remarks>
        protected internal Vector2 TopLeftPosition { get; private set; }

        /// <summary>
        ///   Gets or sets the height and width of the client area of the control.
        /// </summary>
        /// <value>A <see cref = "System.Drawing.Size" /> that represents the dimensions of the client area of the control. </value>
        public Size ClientSize { get; set; }

        public Size ContentAreaSize { get; set; }

        /// <summary>
        ///   Gets or sets the Color of the inner area.
        /// </summary>
        /// <value>The background <see cref = "Color4" /> for this control.</value>
        internal Color4 InnerAreaColor { get; set; }

        internal Color4 BorderColor { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to apply status changes.
        /// </summary>
        /// <value><c>true</c> if status changes are applied for this control; otherwise, <c>false</c>.</value>
        /// <remarks>
        ///   If the value is false, the control won't update its status.
        /// </remarks>
        public bool ApplyStatusChanges { get; set; }

        /// <summary>
        ///   Returns true if the control is being updated (ie, it is in the updateQueue collection),
        ///   false otherwise.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is being updated; otherwise, <c>false</c>.
        /// </value>
        protected internal bool IsBeingUpdated { get; set; }


        /// <summary>
        ///   Gets the event list.
        /// </summary>
        /// <value>The <see cref = "EventHandlerList" /> object for this control.</value>
        protected EventHandlerList Events { get; private set; }

        internal ShapeCollection Shapes { get; set; }
        internal Vector3 AbsoluteOrthoPosition { get; set; }

        /// <summary>
        ///   Gets or sets the Id of the control.
        /// </summary>
        /// <value>
        ///   The id of the control. The default is: <b>name of the control + number of instances of
        ///                                            that control generated</b>. Like, <b>Label82</b> for example.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        ///   Gets or sets the <see cref = "Description" /> to use for this control.
        /// </summary>
        /// <value>
        ///   The <see cref = "Description" /> object that contains information on how to style
        ///   the text appearance of this control.
        /// </value>
        public ControlDescription Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    ApplyControlDescription(value);
                    OnControlStyleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value that will be used by the interface to know whether that control
        ///   can raise events or not.
        /// </summary>
        /// <value><c>true</c> if the control can react to events; <c>false</c> otherwise.</value>
        /// <remarks>
        ///   Setting this property to <b>false</b> will lock the control in its current state.
        /// </remarks>
        public bool CanRaiseEvents
        {
            get { return canRaiseEvents; }
            set { canRaiseEvents = value; }
        }

        /// <summary>
        ///   Gets or sets a value that indicates whether the control is visible or not.
        /// </summary>
        /// <value><c>true</c> if the control is visible; <c>false</c> otherwise.</value>
        /// <remarks>
        ///   Setting this property to a different value that the one it had before the assignment,
        ///   will cause the UI to be recomputed if the control is not in <see cref = "DesignMode" />
        /// </remarks>
        public virtual bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value != isVisible)
                {
                    isVisible = value;
                    OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this control is highlighted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this control is highlighted; otherwise, <c>false</c>.
        /// </value>
        public bool IsHighlighted
        {
            get { return isHighlighted; }
            set
            {
                if (isHighlighted != value)
                {
                    isHighlighted = value;
                    OnHighlightedChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        ///   Determines whether this control is focused.
        /// </summary>
        /// <value><c>true</c> if this control is focused; <c>false</c> otherwise.</value>
        public bool IsFocused { get; private set; }

        /// <summary>
        ///   Gets or sets a value that indicates whether the control can be interacted with.
        /// </summary>
        /// <remarks>
        ///   This consequently causes the <see cref = "BaseControl.CanRaiseEvents" /> property to be set.
        /// </remarks>
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = canRaiseEvents = value; }
        }

        /// <summary>
        ///   Gets or Sets the parent control. When a new parent control is set
        ///   the absolute position of the child control is also computed.
        /// </summary>
        /// <value>The parent control.</value>
        public virtual BaseControl Parent
        {
            get { return parent; }
            internal set
            {
                if (parent == value) return;

                IContainer formerParent = parent as IContainer;
                if (formerParent != null)
                {
                    formerParent.Controls.Remove(value);
                }
                parent = value;

                OnParentChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        ///   Gets or sets the coordinates of the upper-left corner of the control 
        ///   relative to the upper-left corner of its container.
        /// </summary>
        /// <value>A Vector2 that represents the upper-left corner
        ///   of the control relative to the upper-left corner of its container.</value>
        /// <remarks>
        ///   If the controls's <see cref = "Parent" /> is the <see cref = "Hud" />, the <b>PositionV3</b>
        ///   property value represents the upper-left corner of the control in screen coordinates.
        /// </remarks>
        public Vector2 Position
        {
            get { return position; }
            set
            {
                if (position == value) return;

                position = value;

                if (DesignMode) return;

                OnPositionChanged(EventArgs.Empty);
                OnMove(EventArgs.Empty);
            }
        }

        /// <summary>
        ///   Gets the absolute position in screen coordinates of the upper-left corner
        ///   of this control.
        /// </summary>
        /// <value>A <see cref = "Microsoft.DirectX.Vector2" /> that represents the 
        ///   absolute position of the upper-left corner in screen coordinates
        ///   for this control. </value>
        public Vector2 AbsolutePosition { get; internal set; }


        /// <summary>
        ///   Gets or sets the height and width of the control.
        /// </summary>
        /// <value>The <see cref = "System.Drawing.Size">Size</see> that represents the height and width of the control in pixels. </value>
        public Size Size
        {
            get { return size; }
            set
            {
                if (size == value) return;
                size = value;
                ContentAreaSize = new Size(Size.Width - (Description.BorderSize.Horizontal + Description.Padding.Horizontal),
                                 Size.Height - (Description.BorderSize.Vertical + Description.Padding.Vertical));

                ClientSize = new Size(Size.Width - Description.BorderSize.Horizontal,
                                      Size.Height - Description.BorderSize.Vertical);

                if (DesignMode) return;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        public int Width
        {
            get { return size.Width; }
        }

        public int Height
        {
            get { return size.Height; }
        }

        #endregion

        #region Protected properties

        /// <summary>
        ///   Gets or sets a value indicating whether this control is clicked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this control is clicked; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        ///   This value stays <c>true</c> for as long as the user presses the mouse button.
        /// </remarks>
        protected internal bool IsClicked { get; set; }


        /// <summary>
        ///   Gets or sets a value indicating whether the mouse cursor is currently inside.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the mouse pointer is currently inside; otherwise, <c>false</c>.
        /// </value>
        protected internal bool IsInside
        {
            get { return isInside && canRaiseEvents; }
            set { isInside = value; }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this control is selected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this control is selected; otherwise, <c>false</c>.
        /// </value>
        protected internal bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnSelectedChanged(EventArgs.Empty);
            }
        }

        internal bool IsSubComponent { get; set; }
        internal bool IsBeingRemoved { get; set; }

        protected internal Depth Depth { get; set; }

        #endregion
    }
}