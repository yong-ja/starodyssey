#region Disclaimer

/* 
 * SimpleShapeControl
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

using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;

#endregion

namespace AvengersUtd.Odyssey.UserInterface
{
    /// <summary>
    /// A <c>SimpleShapeControl</c> is a control that is rendered using
    /// a simple shape, such as a Rectangle, a Circle, etc. (see the
    /// <see cref="Shape"/> enumeration). If this is sufficient for you,
    /// inherit from this class, otherwise consider inheriting from the
    /// other abstract classes.
    /// </summary>
    /// <seealso cref="BaseControl"/>
    /// <seealso cref="BaseButton"/>
    /// <seealso cref="ContainerControl"/>
    public abstract class SimpleShapeControl : BaseControl
    {
        ShapeDescriptor shapeDescriptor;

        #region Constructors

        protected SimpleShapeControl()
        {
            ShapeDescriptors = new ShapeDescriptorCollection(1);
        }

        protected SimpleShapeControl(string id, string controlStyleClass, string textStyleClass)
            : base(id, controlStyleClass, textStyleClass)
        {
            ShapeDescriptors = new ShapeDescriptorCollection(1);
        }

        #endregion

        /// <summary>
        /// Gets or sets a reference to the <see cref="ShapeDescriptor"/> object 
        /// contained in this class.
        /// </summary>
        /// <value>The <see cref="ShapeDescriptor"/> object describing the shape of this control.</value>
        /// <seealso cref="ShapeDescriptor"/>
        protected ShapeDescriptor ShapeDescriptor
        {
            get { return shapeDescriptor; }
            set { shapeDescriptor = value; }
        }

        /// <summary>
        /// Overriden. Creates a simple shape, depending on the <see cref="Shape"/> value
        /// set in the accompanying <see cref="ControlStyle"/> object.
        /// </summary>
        /// <seealso cref="BaseControl.CreateShape"/>
        public override void CreateShape()
        {
            base.CreateShape();
            if (ControlStyle.Shape != Shape.Custom && ControlStyle.Shape != Shape.None)
            {
                shapeDescriptor = ShapeDescriptor.ComputeShape(this, ControlStyle.Shape);
                shapeDescriptor.Depth = Depth;
                ShapeDescriptors[0] = shapeDescriptor;
            }
        }

        /// <summary>
        /// Overriden. Updates this control's shape, according to the <see cref="Shape"/> value
        /// set in the accompanying <see cref="ControlStyle"/> object.
        /// </summary>
        /// <seealso cref="BaseControl.UpdateShape"/>
        public override void UpdateShape()
        {
            if (ControlStyle.Shape != Shape.Custom && ControlStyle.Shape != Shape.None)
            {
                shapeDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, ControlStyle.Shape));
                shapeDescriptor.Depth = new Depth(Depth.WindowLayer, Depth.ComponentLayer, Depth.ZOrder);
            }
        }

        /// <summary>
        /// Overriden. Computes the result of the intersection between the mouse cursor and this control's
        /// bounds, using the appropriate algorithm for the <see cref="Shape"/> of this control.
        /// </summary>
        /// <param name="cursorLocation">The location of the mouse cursor</param>
        /// <returns>
        /// 	<b>True</b> if the cursor is inside the control's boundaries. <b>False</b>, otherwise.
        /// </returns>
        /// <remarks>When overriding, use the methods contained in the <see cref="Intersection"/>
        /// static class.</remarks>
        /// <seealso cref="Intersection"/>
        public override bool IntersectTest(Point cursorLocation)
        {
            switch (ControlStyle.Shape)
            {
                default:
                case Shape.Rectangle:
                    return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);

                case Shape.Circle:
                    return
                        Intersection.CircleTest(AbsolutePosition, (this as ICircularControl).OutlineRadius,
                                                cursorLocation);
            }
        }

        /// <summary>
        /// Overriden. Nothing is done here because a <c>SimpleShapeControl</c>
        /// does not need to update any position dependant parameters.
        /// </summary>
        protected override void UpdatePositionDependantParameters()
        {
            return;
        }
    }
}