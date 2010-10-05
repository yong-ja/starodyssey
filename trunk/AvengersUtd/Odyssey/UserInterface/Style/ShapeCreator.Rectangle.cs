﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static partial class ShapeCreator
    {

        public static ShapeDescription DrawFullRectangle(Vector3 position, Size size, ColorShader colorShader, Color4 fillColor, int borderSize, BorderStyle borderStyle, Color4 borderColor)
        {
            Color4[] shadedColors = colorShader.Method(fillColor, 4, colorShader.StartValue, colorShader.EndValue, Shape.Rectangle);
            Color4[] borderColors;

            switch (borderStyle)
            {
                case BorderStyle.None:
                    borderColors = ColorShader.Uniform(new Color4(0), 4);
                    break;
                case BorderStyle.Flat:
                    borderColors = ColorShader.Uniform(borderColor, 4);
                    break;
                case BorderStyle.Raised:
                    borderColors = ColorShader.BorderRaised(borderColor, 4);
                    break;
                case BorderStyle.Sunken:
                    borderColors = ColorShader.BorderSunken(borderColor, 4);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("borderStyle");
            }
            ShapeDescription inside = DrawRectangle(position, size, shadedColors);
            ShapeDescription outline = DrawRectangularOutline(position, size, borderSize, borderColors, borderStyle, Border.All);

            ShapeDescription result = ShapeDescription.Join(inside, outline);
            result.Shape = Shape.RectangleWithOutline;
            return result;
        }


        public static ShapeDescription DrawRectangle(Vector3 position, Size size, Color4 color)
        {
            Color4[] shadedColors = ColorShader.Uniform(color, 4);
            return DrawRectangle(position, size, shadedColors);
        }

        public static ShapeDescription DrawRectangle(Vector3 position, Size size, Color4[] colors)
        {
            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateQuad(position.ToVector4(), size.Width, size.Height,
                                                 colors, out indices);
            return new ShapeDescription
                       {
                           Vertices = vertices,
                           Indices = indices,
                           Primitives = 2,
                           Shape = Shape.Rectangle
                       };
        }

        public static ShapeDescription DrawRectangularOutline(Vector3 position, Size size, int borderSize, Color4[] colors, BorderStyle borderStyle, Border borders)
        {
            ShapeDescription vBorderTop = default(ShapeDescription);
            ShapeDescription vBorderSideL = default(ShapeDescription);
            ShapeDescription vBorderSideR = default(ShapeDescription);
            ShapeDescription vBorderBottom = default(ShapeDescription);

            Vector3 innerPositionTopLeft = new Vector3(
                position.X + borderSize, position.Y - borderSize, position.Z);

            Vector3 borderPositionTopRight = new Vector3(
                position.X + size.Width - borderSize, position.Y, position.Z);

            Vector3 borderPositionBottomLeft = new Vector3(
                position.X, position.Y - size.Height + borderSize, position.Z);

            Size borderTop = new Size(size.Width, borderSize);
            Size borderSide = new Size(borderSize, size.Height);

            Color4 cLeft = colors[0];
            Color4 cTop = colors[1];
            Color4 cBottom = colors[2];
            Color4 cRight = colors[3];

            if ((borders & Border.Top) != 0)
                vBorderTop = DrawRectangle(position, borderTop, cTop);
            if ((borders & Border.Left) != 0)
                vBorderSideL = DrawRectangle(position, borderSide, cLeft);
            if ((borders & Border.Right) != 0)
                vBorderSideR = DrawRectangle(borderPositionTopRight, borderSide, cRight);
            if ((borders & Border.Bottom) != 0)
                vBorderBottom = DrawRectangle(borderPositionBottomLeft, borderTop, cBottom);

            switch (borderStyle)
            {
                case BorderStyle.Flat:
                case BorderStyle.Raised:
                    return ShapeDescription.Join(vBorderSideL, vBorderTop, vBorderSideR, vBorderBottom);
                case BorderStyle.Sunken:
                    return ShapeDescription.Join(vBorderSideL, vBorderSideR, vBorderTop, vBorderBottom);
                default:
                    throw Error.WrongCase("borderStyle", "DrawRectangularOutline", borderStyle);
            }
            
        }
    }
}