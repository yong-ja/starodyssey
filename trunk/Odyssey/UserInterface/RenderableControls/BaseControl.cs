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
using System.ComponentModel;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;

#if !(SlimDX)
    using Microsoft.DirectX;
#else
    using SlimDX;
#endif


namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    /// <summary>
    /// The <b>BaseControl</b> class is the root class of all controls in the library. It provides
    /// inheritors with a comprehensive range of properties and methods common to all controls.</summary>
    /// <remarks>This is the primary class that you derive from when designing a new control
    /// for the UI. 
    /// A tutorial on how to create a new control can be found
    /// <see href="http://www.avengersutd.com/wiki/Creating_new_controls">here</see>. Internally, 
    /// a control is rendered via the creation of at least one <see cref="ShapeDescriptor"/> object.
    /// To define a control's appearance, two xml files are used: <c>[ThemeName] ControlStyle.ocs</c> and
    /// <c>[ThemeName] TextStyles.ots</c>. In the first one each default control's appearance is defined.
    /// In the second one, defaul text styles are defined. The run-time appearance of a control is stored
    /// inside the <see cref="ControlStyle"/> object. Upon creation each control loads 
    /// (<i>you should specify which theme to load</i>: see <see cref="StyleManager"/>) from the theme file
    /// its default appearance. See <see href="http://www.avengersutd.com/wiki/Theming">this article</see> for
    /// more information about theming.
    /// <para>Controls that inherit from <b>BaseControl</b> should define both the <see cref="CreateShape"/>
    /// and <see cref="UpdateShape"/> methods. Controls inheriting from <see cref="SimpleShapeControl"/>
    /// can skip this step. Inherit from <b>BaseControl</b> when your control has to define a custom
    /// (non primitive) shape, in all other cases, derive from <see cref="SimpleShapeControl"/></para>.
    /// </remarks>
    /// <seealso cref="ShapeDescriptor"/>
    /// <seealso cref="Hud"/>
    /// <seealso cref="StyleManager"/>
    public abstract partial class BaseControl : IControl, IComparable<BaseControl>, IDisposable
    {
        #region Private fields

        Vector2 absolutePosition;
        bool applyStatusChanges = true;
        bool autosize;
        Color borderColor;
        int borderSize;
        BorderStyle borderStyle;
        bool canRaiseEvents = true;
        Size clientSize;
        ControlStyle controlStyle;
        Depth depth;
        bool designMode = true;
        bool disposed;
        EventHandlerList eventHandlerList;
        bool hasCaptured;
        string id;
        int index;

        Color innerAreaColor;
        bool isBeingUpdated;
        bool isClicked;
        bool isBeingRemoved;

        bool isEnabled = true;
        bool isFocusable = true;
        bool isFocused;
        bool isHighlighted;
        bool isInside;
        bool isSelected;
        bool isSubComponent;
        bool isVisible = true;
        Padding padding;
        BaseControl parent;

        Vector2 position;
        ShapeDescriptorCollection shapeDescriptors;

        Size size;
        TextStyle textStyle;
        Vector2 topLeftPosition;

        #endregion

        #region Constructors

        static BaseControl()
        {
            EventControlAdded = new object();
            EventControlStyleChanged = new object();
            EventDesignModeChanged = new object();
            EventDisposing = new object();
            EventGotFocus = new object();
            EventHighlightedChanged = new object();
            EventKeyDown = new object();
            EventKeyPress = new object();
            EventKeyUp = new object();
            EventLostFocus = new object();
            EventMouseCaptureChanged = new object();
            EventMouseClick = new object();
            EventMouseEnter = new object();
            EventMouseDown = new object();
            EventMouseLeave = new object();
            EventMouseMove = new object();
            EventMouseWheel = new object();
            EventMouseUp = new object();
            EventMove = new object();
            EventSelectedChanged = new object();
            EventSizeChanged = new object();
            EventParentChanged = new object();
            EventPositionChanged = new object();
            EventTextStyleChanged = new object();
            EventVisibleChanged = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseControl"/> class.
        /// </summary>
        /// <remarks>Controls are created with the properties <see cref="DesignMode"/> 
        /// and <see cref="IsVisible"/> set to <c>true</c>. Therefore when creating controls at run-time
        /// always place the creation code between <c>OdysseyUI.CurrentHud.BeginDesign()</c> and 
        /// <c>OdysseyUI.CurrentHud.EndDesign()</c> calls.</remarks>
        /// <seealso cref="Hud"/>
        /// <seealso cref="Hud.BeginDesign"/>
        /// <seealso cref="Hud.EndDesign"/>
        protected BaseControl()
        {
            eventHandlerList = new EventHandlerList();
        }

        /// <summary>
        /// This constructors sets to default values all the common parameters shared by all the 
        /// classes that derive from it. Since BaseControl is an abstract class, this constructor will 
        /// only be called by deriving classes. 
        /// </summary>
        /// <param name="id">A string detailing the ID of the control.</param>
        /// <param name="textStyleClass">The name of the <see cref="TextStyle"/> class to use.</param>
        protected BaseControl(string id, string controlStyleClass, string textStyleClass)
        {
            this.id = id;
            eventHandlerList = new EventHandlerList();
            ApplyControlStyle(StyleManager.GetControlStyle(controlStyleClass));
            textStyle = StyleManager.GetTextStyle(textStyleClass);
        }

        #endregion

        #region IRenderable members

        /// <summary>
        /// Computes the intersection between the cursor location and this control. It is called
        /// each time an event is fired on every control in the <see cref="Hud"/> to determine
        /// if the UI needs to react.</summary>
        /// <remarks>When overriding, use the methods contained in the <see cref="Intersection"/> 
        /// static class.</remarks> 
        /// <param name="cursorLocation">The location of the mouse cursor</param>
        /// <returns><b>True</b> if the cursor is inside the control's boundaries. <b>False</b>, otherwise.</returns>
        /// <seealso cref="Intersection"/>
        public abstract bool IntersectTest(Point cursorLocation);

        /// <summary>
        /// Computes the absolute position of the control, depending on the inherited position of the parent.
        /// This method is called when its position or the parent changes.
        /// </summary>
        public virtual void ComputeAbsolutePosition()
        {
            if (parent != null)
            {
                Vector2 oldAbsolutePosition = absolutePosition;
                Vector2 newAbsolutePosition = parent.AbsolutePosition + position;

                if (!isSubComponent)
                    newAbsolutePosition += parent.topLeftPosition;

                if (!newAbsolutePosition.Equals(oldAbsolutePosition))
                {
                    absolutePosition = newAbsolutePosition;

                    if (!designMode)
                        OnMove(EventArgs.Empty);
                }

                IContainer iContainer = this as IContainer;
                if (iContainer != null)
                {
                    foreach (BaseControl ctl in iContainer.PrivateControlCollection.AllControls)
                    {
                        ctl.ComputeAbsolutePosition();
                    }
                }
            }
        }

        #endregion

        #region IComparable<BaseControl> Members

        /// <summary>
        /// Compares this instance of the <see cref="BaseControl"/> class to another one.
        /// </summary>
        /// <param name="other">The other instance to compare to.</param>
        /// <returns>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Return Value</term>
        ///         <description>Description</description>
        ///     </listheader>
        ///         <item>
        ///             <term>Less than zero</term>
        ///             <description>This control is in the background.</description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>This control and <item>value</item> are on the same depth level.</description>
        ///         </item>
        ///         <item>
        ///             <term>Greater than 0</term>
        ///             <description>This control is in the foreground.</description>
        ///         </item>
        /// </list>
        /// <para></para>
        /// </returns>
        public int CompareTo(BaseControl other)
        {
            return depth.CompareTo(other.depth);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes all managed and unmanaged resources of this object.
        /// </summary>
        /// <remarks>Be sure to call this method when you don't need this control anymore.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// IDisposable pattern implementation. Override this method if your control 
        /// uses resources that should be freed as soon as possible.
        /// </summary>
        /// <param name="disposing">A value, that if <b>true</b>, indicates that the method
        /// has been called by the user. <b>False</b>, if it has been automatically
        /// called by the finalizer.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    eventHandlerList.Dispose();

                    // Raise the OnDisposing event
                    OnDisposing(EventArgs.Empty);
                }

                // dispose unmanaged components
            }
            disposed = true;
        }

        ~BaseControl()
        {
            Dispose(false);
        }
    }
}