using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static partial class ShapeCreator
    {
        public static ShapeDescription ComputeData(BaseControl ctl)
        {
            ShapeDescription shape = default(ShapeDescription);

            switch (ctl.Description.Shape)
            {
                case Shape.None:
                    break;
                case Shape.Custom:
                    break;

                case Shape.Triangle:
                    break;

                case Shape.Rectangle:
                    shape = DrawFullRectangle(ctl.AbsoluteOrthoPosition, ctl.Size, ctl.Description.ColorShader,
                                                       ctl.InnerAreaColor, ctl.Description.BorderSize, ctl.Description.BorderStyle,
                                                       ctl.BorderColor);
                    break;
                case Shape.Circle:
                    break;
                case Shape.LeftTrapezoidUpside:
                    break;
                case Shape.LeftTrapezoidDownside:
                    break;
                case Shape.RightTrapezoidUpside:
                    break;
                case Shape.RightTrapezoidDownside:
                    break;

                case Shape.SubdividedRectangle:
                    shape = DrawSubdividedRectangleWithOutline(ctl.AbsoluteOrthoPosition, ctl.Size,
                        ctl.Description.ColorShader, ctl.Description.BorderSize, ctl.Description.BorderStyle, ctl.BorderColor);
                    break;
                default:
                    throw Error.WrongCase("shape", "ComputeData", shape);
            }
            shape.Tag = ctl.Id;
            shape.Depth = ctl.Depth;
            return shape;
        }
    }
}
