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
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Xml;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Controls
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
        #region Constructors

        protected SimpleShapeControl(string id, string controlDescriptionClass) : base(id, controlDescriptionClass)
        {
            Shapes = new ShapeCollection(1);
        }

        #endregion

        /// <summary>
        /// Gets or sets a reference to the <see cref="ShapeDescriptor"/> object 
        /// contained in this class.
        /// </summary>
        /// <value>The <see cref="ShapeDescriptor"/> object describing the shape of this control.</value>
        /// <seealso cref="ShapeDescriptor"/>
        protected ShapeDescription ShapeDescription { get; set; }

        /// <summary>
        /// Overriden. Creates a simple shape, depending on the <see cref="Shape"/> value
        /// set in the accompanying <see cref="ControlDescription"/> object.
        /// </summary>
        /// <seealso cref="BaseControl.CreateShape"/>
        public override void CreateShape()
        {
            if (Description.Shape == Shape.Custom || Description.Shape == Shape.None) return;

            ShapeDescription = ShapeCreator.ComputeData(this);
            Shapes[0] = ShapeDescription;
        }

        /// <summary>
        /// Overriden. Updates this control's shape, according to the <see cref="Shape"/> value
        /// set in the accompanying <see cref="ControlDescription"/> object.
        /// </summary>
        /// <seealso cref="BaseControl.UpdateShape"/>
        public override void UpdateShape()
        {
            if (Description.Shape != Shape.Custom && Description.Shape != Shape.None)
            {
                ShapeDescription.UpdateVertices(ShapeCreator.ComputeData(this).Vertices);
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
            switch (Description.Shape)
            {
                default:
                case Shape.Rectangle:
                    return Geometry.Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);

                //case Shape.Circle:
                //    return
                //        Intersection.CirclePointTest(AbsolutePosition, (this as ICircularControl).OutlineRadius,
                //                                cursorLocation);
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

        protected override void UpdateSizeDependantParameters()
        {
            return;
        }

    }
}