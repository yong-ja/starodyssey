#region Disclaimer

/* 
 * DecoratedButton
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
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    public enum DecorationType
    {
        Cross,
        UpsideTriangle,
        DownsideTriangle
    }

    public class DecoratedButton : BaseButton
    {
        const string controlTag = "DecoratedButton";
        const int DefaultCrossPaddingX = 10;
        const int DefaultCrossPaddingY = 7;
        const int DefaultCrossWidth = 14;
        const int DefaultTriangleSideLength = 50;
        static int count;

        ShapeDescriptor buttonDescriptor;
        ShapeDescriptor decorationDescriptor;
        DecorationType decorationType;
        Vector2 decorationVertex;
        Vector2 decorationVertexAbsolutePosition;

        public DecoratedButton() : base(controlTag + count, controlTag, controlTag)
        {
            count++;
            ShapeDescriptors = new ShapeDescriptorCollection(2);
        }


        public DecorationType DecorationType
        {
            get { return decorationType; }
            set
            {
                OdysseyUI.CurrentHud.BeginDesign();
                decorationType = value;
                decorationVertex = GetDecorationVertex();
                OdysseyUI.CurrentHud.EndDesign();
            }
        }

        /// <summary>
        /// Gets the first vertex for the decoration descriptors, used to compute all the others. It may be
        /// a triangle's topmost vertex if a UpsideTriangle decoration is chosen, a cross' top left vertex, etc.
        /// </summary>
        /// <returns>The computed vertex, depending on the decorationType.</returns>
        Vector2 GetDecorationVertex()
        {
            switch (decorationType)
            {
                default:
                case DecorationType.DownsideTriangle:
                    return new Vector2(Size.Width/2 - DefaultTriangleSideLength/2,
                                       (Size.Height - (float) (DefaultTriangleSideLength/2*Math.Sqrt(3)))/2);

                case DecorationType.Cross:
                    return new Vector2(Size.Width/2 - DefaultCrossWidth/2,
                                       DefaultCrossPaddingY);
            }
        }

        protected override void OnMove(EventArgs e)
        {
            if (decorationDescriptor != null)
                decorationDescriptor.IsDirty = true;
            base.OnMove(e);
        }

        public override void CreateShape()
        {
            base.CreateShape();
            buttonDescriptor = ShapeDescriptor.ComputeShape(this, Shape.Rectangle);

            switch (decorationType)
            {
                default:
                case DecorationType.DownsideTriangle:
                    decorationDescriptor = Shapes.DrawFullEquilateralTriangle(decorationVertexAbsolutePosition,
                                                                          DefaultTriangleSideLength,
                                                                          Color.Silver,
                                                                          false,
                                                                          false, ControlStyle.ColorArray.BorderEnabled,
                                                                          ControlStyle.Shading, ControlStyle.BorderSize);
                    break;

                case DecorationType.Cross:
                    decorationDescriptor = ShapeDescriptor.Join(
                        Shapes.DrawLine(4, Color.AntiqueWhite,
                                        decorationVertexAbsolutePosition,
                                        decorationVertexAbsolutePosition +
                                        new Vector2(DefaultCrossWidth, DefaultCrossWidth)),
                        Shapes.DrawLine(4, Color.AntiqueWhite,
                                        decorationVertexAbsolutePosition + new Vector2(0, DefaultCrossWidth),
                                        decorationVertexAbsolutePosition + new Vector2(DefaultCrossWidth, 0)
                            )
                        );

                    break;
            }


            buttonDescriptor.Depth = Depth;
            decorationDescriptor.Depth = Depth.AsChildOf(Depth);

            ShapeDescriptors[0] = buttonDescriptor;
            ShapeDescriptors[1] = decorationDescriptor;
        }

        public override void UpdateShape()
        {
            buttonDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, Shape.Rectangle));

            if (!decorationDescriptor.IsDirty)
                return;

            switch (decorationType)
            {
                default:
                case DecorationType.DownsideTriangle:
                    decorationDescriptor.UpdateShape(Shapes.DrawFullEquilateralTriangle(decorationVertexAbsolutePosition,
                                                                          DefaultTriangleSideLength,
                                                                          ColorOperator.Scale(Color.Black, 0.5f), false,
                                                                          false, ControlStyle.ColorArray.BorderEnabled,
                                                                          ControlStyle.Shading, ControlStyle.BorderSize));
                    break;

                case DecorationType.Cross:
                    decorationDescriptor.UpdateShape(ShapeDescriptor.Join(
                                                         Shapes.DrawLine(4, Color.AntiqueWhite,
                                                                         decorationVertexAbsolutePosition,
                                                                         decorationVertexAbsolutePosition +
                                                                         new Vector2(DefaultCrossWidth,
                                                                                     DefaultCrossWidth)),
                                                         Shapes.DrawLine(4, Color.AntiqueWhite,
                                                                         decorationVertexAbsolutePosition +
                                                                         new Vector2(0, DefaultCrossWidth),
                                                                         decorationVertexAbsolutePosition +
                                                                         new Vector2(DefaultCrossWidth, 0)
                                                             )
                                                         ));

                    break;
            }
        }

        public override void ComputeAbsolutePosition()
        {
            base.ComputeAbsolutePosition();
            decorationVertex = GetDecorationVertex();
        }

        protected override void UpdatePositionDependantParameters()
        {
            decorationVertexAbsolutePosition = AbsolutePosition + decorationVertex;
            if (decorationDescriptor != null)
                decorationDescriptor.IsDirty = true;
        }

        protected override void UpdateSizeDependantParameters()
        {
            decorationVertex = GetDecorationVertex();
            decorationVertexAbsolutePosition = AbsolutePosition + decorationVertex;
            if (decorationDescriptor != null)
                decorationDescriptor.IsDirty = true;
        }


        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }
    }
}