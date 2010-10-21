﻿
using System;
using System.Linq;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
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
                    shape = DrawFullRectangle(ctl.AbsoluteOrthoPosition, ctl.Size, ctl.Description.FillShader,
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
                    shape = DrawRectangle(ctl);
                    break;
                default:
                    throw Error.WrongCase("shape", "ComputeData", shape);
            }
            shape.Tag = ctl.Id;
            shape.Depth = ctl.Depth;
            return shape;
        }

        static ShapeDescription DrawRectangle(BaseControl control)
        {
            ControlDescription desc = control.Description;
            Designer d = control.GetDesigner();
            
            d.Position = control.AbsoluteOrthoPosition;
            d.FillShader = desc.FillShader;
            d.BorderShader = desc.BorderShader;
            d.BorderSize = new Thickness(10);
            d.FillRectangle();
            d.DrawRectangle();
            
            return d.Output;
        }
    }
}
