using System;
using System.Drawing;
using System.Linq;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public static partial class ShapeCreator
    {

        public static ShapeDescription DrawFullRectangle(Vector3 position, Size size, ColorShader colorShader, Color4 fillColor, int borderSize, BorderStyle borderStyle, Color4 borderColor)
        {
            Color4[] shadedColors = colorShader.Method(colorShader, 4,Shape.Rectangle);
            Color4[] borderColors;

            switch (borderStyle)
            {
                case BorderStyle.None:
                    borderColors = ColorShader.FillColorArray(new Color4(0), 4);
                    break;
                case BorderStyle.Flat:
                    borderColors = ColorShader.FillColorArray(borderColor, 4);
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
            Color4[] shadedColors = ColorShader.FillColorArray(color, 4);
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

        public static ShapeDescription DrawSubdividedRectangleWithOutline(Vector3 position, Size size, ColorShader colorShader, int borderSize, BorderStyle borderStyle, Color4 borderColor)
        {
            int widthSegments;
            int heightSegments;
            float[] offsets = colorShader.Gradient.Select(g => g.Offset).ToArray();
            switch (colorShader.GradientType)
            {
                case GradientType.Uniform:
                    widthSegments = heightSegments = 1;
                    break;
                case GradientType.LinearVerticalGradient:
                    widthSegments = 1;
                    heightSegments = offsets.Length-1;
                    break;
                case GradientType.LinearHorizontalGradient:
                    widthSegments = offsets.Length-1;
                    heightSegments = 1;
                    break;
                default:
                    throw Error.WrongCase("colorshader.GradientType", "DrawSubdividedRectangleWithOutline",
                        colorShader.GradientType);
            }
            Color4[] shadedColors = colorShader.Method(colorShader, (1+widthSegments)*(1+heightSegments), Shape.Rectangle);
            Color4[] borderColors;

            switch (borderStyle)
            {
                case BorderStyle.None:
                    borderColors = ColorShader.FillColorArray(new Color4(0), 4);
                    break;
                case BorderStyle.Flat:
                    borderColors = ColorShader.FillColorArray(borderColor, 4);
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
            ShapeDescription outline = DrawRectangularOutline(position, size, borderSize, borderColors, BorderStyle.Flat,
                Border.All);
            
            ShapeDescription inside;

            switch (colorShader.GradientType)
            {
                case GradientType.LinearVerticalGradient:
                    inside = DrawSubdividedRectangle(position, size, widthSegments, heightSegments,
                shadedColors, heightOffsets: offsets);
                    break;
                case GradientType.LinearHorizontalGradient:
                    inside = DrawSubdividedRectangle(position, size, widthSegments, heightSegments,
                shadedColors, widthOffsets: offsets);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            

            return ShapeDescription.Join(inside,outline);
        }

        public static ShapeDescription DrawSubdividedRectangle(Vector3 position, Size size, int widthSegments, int heightSegments, Color4[] colors, float[] widthOffsets=null, float[] heightOffsets=null)
        {
            short[] indices;
            ColoredVertex[] vertices = Polygon.CreateRectangleMesh(position.ToVector4(), size.Width, size.Height,
                                                 widthSegments, heightSegments, colors, out indices, widthOffsets, heightOffsets);
            return new ShapeDescription
            {
                Vertices = vertices,
                Indices = indices,
                Primitives = indices.Length/3,
                Shape = Shape.SubdividedRectangle
            };
        }
    }
}
