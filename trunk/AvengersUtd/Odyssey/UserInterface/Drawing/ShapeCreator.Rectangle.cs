﻿using System;
using System.Drawing;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public static partial class ShapeCreator
    {

        public static ShapeDescription DrawFullRectangle(Vector3 position, Size size, IGradientShader linearShader, Color4 fillColor, Thickness borderSize, BorderStyle borderStyle, Color4 borderColor)
        {
            Color4[] shadedColors = linearShader.Method(linearShader, 4,Shape.Rectangle);
            Color4[] borderColors;

            switch (borderStyle)
            {
                case BorderStyle.None:
                    borderColors = LinearShader.FillColorArray(new Color4(0), 4);
                    break;
                case BorderStyle.Flat:
                    borderColors = LinearShader.FillColorArray(borderColor, 4);
                    break;
                case BorderStyle.Raised:
                    borderColors = LinearShader.BorderRaised(borderColor, 4);
                    break;
                case BorderStyle.Sunken:
                    borderColors = LinearShader.BorderSunken(borderColor, 4);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("borderStyle");
            }
            ShapeDescription inside = DrawRectangle(position, size, shadedColors);
            ShapeDescription outline = DrawRectangularOutline(position, size, borderSize.All, borderColors, borderStyle, Borders.All);

            ShapeDescription result = ShapeDescription.Join(inside, outline);
            result.Shape = Shape.RectangleWithOutline;
            return result;
        }


        public static ShapeDescription DrawRectangle(Vector3 position, Size size, Color4 color)
        {
            Color4[] shadedColors = LinearShader.FillColorArray(color, 4);
            return DrawRectangle(position, size, shadedColors);
        }

        public static ShapeDescription DrawRectangle(Vector3 position, Size size, Color4[] colors)
        {
            short[] indices;
            ColoredVertex[] vertices = PolyMesh.CreateQuad(position.ToVector4(), size.Width, size.Height,
                                                 colors, out indices);
            return new ShapeDescription
                       {
                           Vertices = vertices,
                           Indices = indices,
                           Primitives = 2,
                           Shape = Shape.Rectangle
                       };
        }

        public static ShapeDescription DrawRectangularOutline(Vector3 position, Size size, int borderSize, Color4[] colors, BorderStyle borderStyle, Borders borders)
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

            if ((borders & Borders.Top) != 0)
                vBorderTop = DrawRectangle(position, borderTop, cTop);
            if ((borders & Borders.Left) != 0)
                vBorderSideL = DrawRectangle(position, borderSide, cLeft);
            if ((borders & Borders.Right) != 0)
                vBorderSideR = DrawRectangle(borderPositionTopRight, borderSide, cRight);
            if ((borders & Borders.Bottom) != 0)
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
