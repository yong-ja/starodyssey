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
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;
using AvengersUtd.Odyssey.UserInterface.Xml;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    /// <summary>
    /// The <b>BaseControl</b> class is the root class of all controls in the library. It provides
    /// inheritors with a comprehensive range of properties and methods common to all controls.</summary>
    /// <remarks>This is the primary class that you derive from when designing a new control
    /// for the UI. 
    /// A tutorial on how to create a new control can be found
    /// <see href="http://www.avengersutd.com/wiki/Creating_new_controls">here</see>. Internally, 
    /// a control is rendered via the creation of at least one <see cref="ShapeDescription"/> object.
    /// To define a control's appearance, two xml files are used: <c>[ThemeName] ControlDescription.ocs</c> and
    /// <c>[ThemeName] TextStyles.ots</c>. In the first one each default control's appearance is defined.
    /// In the second one, defaul text styles are defined. The run-time appearance of a control is stored
    /// inside the <see cref="Description"/> object. Upon creation each control loads 
    /// (<i>you should specify which theme to load</i>: see <see cref="StyleManager"/>) from the theme file
    /// its default appearance. See <see href="http://www.avengersutd.com/wiki/Theming">this article</see> for
    /// more information about theming.
    /// <para>Controls that inherit from <b>BaseControl</b> should define both the <see cref="CreateShape"/>
    /// and <see cref="UpdateShape"/> methods. Controls inheriting from <see cref="SimpleShapeControl"/>
    /// can skip this step. Inherit from <b>BaseControl</b> when your control has to define a custom
    /// (non primitive) shape, in all other cases, derive from <see cref="SimpleShapeControl"/></para>.
    /// </remarks>
    /// <seealso cref="ShapeDescription"/>
    /// <seealso cref="Hud"/>
    /// <seealso cref="StyleManager"/>
    public abstract partial class BaseControl : IControl, IDisposable, IComparable<BaseControl>
    {
        #region Private fields

        bool canRaiseEvents = true;
        bool designMode = true;
        bool disposed;

        bool isEnabled = true;
        bool isFocusable = true;
        bool isHighlighted;
        bool isInside;
        bool isSelected;
        bool isVisible = true;
        BaseControl parent;

        ControlDescription description;
        TextDescription textDescription;
        Vector2 position;
        Size size;
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
        protected BaseControl(string id, string controlDescriptionClass)
        {
            ApplyStatusChanges = true;
            Events = new EventHandlerList();
            Id = id;
            ApplyControlDescription(StyleManager.GetControlDescription(controlDescriptionClass));
        }

        #endregion

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
                Vector2 oldAbsolutePosition = AbsolutePosition;
                Vector2 newAbsolutePosition = 
                    new Vector2(parent.AbsolutePosition.X + position.X, parent.AbsolutePosition.Y + position.Y);

                if (!IsSubComponent)
                    newAbsolutePosition += parent.TopLeftPosition;

                if (!newAbsolutePosition.Equals(oldAbsolutePosition))
                {
                    AbsolutePosition = newAbsolutePosition;
                    AbsoluteOrthoPosition = OrthographicTransform(AbsolutePosition, Depth.ZOrder);
                    if (!DesignMode)
                        UpdatePositionDependantParameters();
                }
            }

            IContainer iContainer = this as IContainer;
            if (iContainer == null) return;
            foreach (BaseControl ctl in iContainer.PrivateControlCollection.AllControls)
            {
                ctl.ComputeAbsolutePosition();
            }

        }

        public override string ToString()
        {
            return string.Format("{0}: '{1}' [{2}] D:{3}", GetType().Name, Id, AbsolutePosition, Depth);
        }

        public static Vector3 OrthographicTransform(Vector2 screenPosition, float zOrder)
        {
            return new Vector3
            {
                X =(float)Math.Floor(((Game.Context.Settings.ScreenWidth / 2f) * -1f) + screenPosition.X),
                Y =(float)Math.Floor((Game.Context.Settings.ScreenHeight / 2f) - screenPosition.Y),
                Z = zOrder,
            };
        }

        #region IComparable Members
        public int CompareTo(BaseControl other)
        {
            return Depth.CompareTo(other.Depth);
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
                    Events.Dispose();

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
        #endregion

    }
}