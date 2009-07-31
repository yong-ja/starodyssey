#region Disclaimer

/* 
 * Shapes
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
#if (!SlimDX)
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TransformedColored = Microsoft.DirectX.Direct3D.CustomVertex.TransformedColored;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static class Shapes
    {
        #region Circle

        /// <summary>
        /// Draws a circle with an outline.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="outlineRadius">The outline radius.</param>
        /// <param name="slices">The slices.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <returns>A full Circular ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawFullCircle(Vector2 center, float radius, float outlineRadius,
                                                     int slices, int borderSize, Color color, Color borderColor)
        {
            return ShapeDescriptor.Join(
                DrawCircle(center, radius, slices, color),
                DrawCircularOutline(center, outlineRadius, slices, borderSize, borderColor));
        }

        /// <summary>
        /// Draws a circle without an outline.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="slices">The slices.</param>
        /// <param name="color">The color.</param>
        /// <returns>A Circular ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawCircle(Vector2 center, float radius, int slices, Color color)
        {
            TransformedColored[] vertices = new TransformedColored[slices + 2];
            int[] indices = new int[slices*3];
            int col1;
            float x, y;
            x = center.X;
            y = center.Y;
            col1 = color.ToArgb();
            
            float deltaRad = Geometry.DegreeToRadian(360)/slices;
            float delta = 0;

            vertices[0] = new TransformedColored(x, y, 0, 1, col1);

            for (int i = 1; i < slices + 2; i++)
            {
                vertices[i] = new TransformedColored(
                    (float) Math.Cos(delta)*radius + x,
                    (float) Math.Sin(delta)*radius + y,
                    0, 1, col1);
                delta += deltaRad;
            }

            indices[0] = 0;
            indices[1] = 1;

            for (int i = 0; i < slices; i++)
            {
                indices[3*i] = 0;
                indices[(3*i) + 1] = i + 1;
                indices[(3*i) + 2] = i + 2;
            }
            return new ShapeDescriptor(slices, vertices, indices);
        }

        /// <summary>
        /// Draws a circular outline.
        /// </summary>
        /// <param name="center">The center.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="slices">The slices.</param>
        /// <param name="width">The width.</param>
        /// <param name="color">The color.</param>
        /// <returns>A Circular outline ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawCircularOutline(Vector2 center, float radius, int slices, int width,
                                                          Color color)
        {
            Vector2[] points = new Vector2[slices];
            float deltaRad = Geometry.DegreeToRadian(360)/slices;
            float delta = 0;

            for (int i = 0; i < slices; i++)
            {
                points[i] = new Vector2(
                    (float) Math.Cos(delta)*radius + center.X,
                    (float) Math.Sin(delta)*radius + center.Y);

                delta += deltaRad;
            }

            return DrawPolyLine(width, color, true, points);
        }

        #endregion

        #region Ellipse

        public static ShapeDescriptor DrawEllipse(
            Vector2 center, int radius1, int radius2, int slices, Color color)
        {
            return
                DrawEllipse(center, radius1, radius2, Geometry.DegreeToRadian(0), Geometry.DegreeToRadian(360), slices,
                            color);
        }

        public static ShapeDescriptor DrawEllipse(Vector2 center, int radius1, int radius2, float radFrom, float radTo,
                                                  int slices, Color color)
        {
            TransformedColored[] vertices = new TransformedColored[slices + 2];
            int[] indices = new int[slices*3];
            int col1;
            float x, y;
            x = center.X;
            y = center.Y;
            col1 = color.ToArgb();

            float deltaRad = radTo/slices;
            float delta = radFrom;

            vertices[0] = new TransformedColored(x, y, 0, 1, col1);

            for (int i = 1; i < slices + 2; i++)
            {
                vertices[i] = new TransformedColored(
                    (float) Math.Cos(delta)*radius1 + x,
                    (float) Math.Sin(delta)*radius2 + y,
                    0, 1, col1);
                delta -= deltaRad;
            }

            indices[0] = 0;
            indices[1] = 1;

            for (int i = 0; i < slices; i++)
            {
                indices[3*i] = 0;
                indices[(3*i) + 1] = i + 2;
                indices[(3*i) + 2] = i + 1;
            }
            return new ShapeDescriptor(slices, vertices, indices);
        }

        #endregion

        #region Lines

        public static ShapeDescriptor DrawLine(float width, Color color, Vector2 v1, Vector2 v2)
        {
            TransformedColored[] vertices = new TransformedColored[4];
            int col1 = color.ToArgb();

            Vector2 vDir = (v1 - v2);
            vDir = new Vector2(-vDir.Y, vDir.X);
            //vDir.Normalize();
            float vLength = (float)Math.Sqrt(vDir.X*vDir.X + vDir.Y*vDir.Y);
            vDir = new Vector2(vDir.X / vLength, vDir.Y / vLength);
            width /= 2;

            Vector2 vTopLeft = v1 + (-width*vDir);
            Vector2 vTopRight = v1 + (width*vDir);
            Vector2 vBottomLeft = v2 + (-width*vDir);
            Vector2 vBottomRight = v2 + (width*vDir);
            vertices[0] = new TransformedColored(vTopLeft.X, vTopLeft.Y, 0, 1, col1);
            vertices[1] = new TransformedColored(vBottomLeft.X, vBottomLeft.Y, 0, 1, col1);
            vertices[2] = new TransformedColored(vBottomRight.X, vBottomRight.Y, 0, 1, col1);
            vertices[3] = new TransformedColored(vTopRight.X, vTopRight.Y, 0, 1, col1);

            int[] indices = new int[6];

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 1;
            indices[3] = 2;
            indices[4] = 0;
            indices[5] = 3;

            return new ShapeDescriptor(2, vertices, indices);
        }

        public static ShapeDescriptor DrawPolyLine(int width, Color color, bool closed, params Vector2[] points)
        {
            TransformedColored[] vertices = new TransformedColored[4];
            ShapeDescriptor[] segments;

            int col1 = color.ToArgb();

            if (closed)
            {
                segments = new ShapeDescriptor[points.Length];
                for (int i = 0; i < points.Length - 1; i++)
                    segments[i] = DrawLine(width, color, points[i], points[i + 1]);
                segments[points.Length - 1] = DrawLine(width, color, points[points.Length - 1], points[0]);
            }
            else
            {
                segments = new ShapeDescriptor[points.Length - 1];
                for (int i = 0; i < points.Length - 1; i++)
                    segments[i] = DrawLine(width, color, points[i], points[i + 1]);
            }
            return ShapeDescriptor.Join(segments);
        }

        #endregion

        #region Rectangle

        public static ShapeDescriptor DrawRectangle(Vector2 topLeft, Size size, Color color)
        {
            return DrawRectangle(topLeft, size, color, new Shading(ShadingType.RectangleFlat));
        }

        public static ShapeDescriptor DrawFullRectangle(Vector2 position, Size size, Color innerAreaColor,
                                                        Color borderColor, Shading shading, int borderSize,
                                                        BorderStyle borderStyle)
        {
            return DrawFullRectangle(position, size, innerAreaColor, borderColor, shading,
                                     borderSize, borderStyle, Border.All);
        }

        public static ShapeDescriptor DrawFullRectangle(Vector2 position, Size size, Color innerAreaColor,
                                                        Color borderColor, Shading shading, int borderSize,
                                                        BorderStyle borderStyle, Border borders)
        {
            ShapeDescriptor rectangle;
            ShapeDescriptor border;

            if (innerAreaColor != Color.Empty)
            {
                rectangle = DrawRectangle(new Vector2(position.X + borderSize, position.Y + borderSize),
                                          new Size(size.Width - borderSize*2, size.Height - borderSize*2),
                                          innerAreaColor,
                                          shading);
            }
            else
                rectangle = null;

            if (borderStyle != BorderStyle.None)
            {
                border = DrawRectangularOutline(position, size, borderSize, borderColor, borderStyle, borders);
            }
            else
                border = null;

            if (rectangle == null && border == null)
            {
                throw new InvalidOperationException();
            }
            else if (rectangle != null && border == null)
                return rectangle;
            else if (rectangle == null)
                return border;
            else
                return ShapeDescriptor.Join(rectangle, border);
        }

        public static ShapeDescriptor DrawRectangle(Vector2 topLeft, Size size, Color color, Shading shading)
        {
            // We need four vertices for a rectangle (!)
            TransformedColored[] vertices = new TransformedColored[4];

            int width = size.Width;
            int height = size.Height;
            
            // Apply the "shader": the delegate will return an array of colors in
            // argb format, with the resulting color values from the shader method.
            int[] colors = shading.Mode(color, shading.Values);

            int colorTopLeft = colors[0];
            int colorTopRight = colors[1];
            int colorBottomleft = colors[2];
            int colorBottomRight = colors[3];

            // Compute the position of the vertices
            vertices[0] = new TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);
            vertices[1] = new TransformedColored(topLeft.X, topLeft.Y + size.Height, 0, 1, colorBottomleft);
            vertices[2] = new TransformedColored(topLeft.X + size.Width, topLeft.Y + size.Height, 0, 1, colorBottomRight);
            vertices[3] = new TransformedColored(topLeft.X + size.Width, topLeft.Y, 0, 1, colorTopRight);

            // We need six vertices because two of the vertex are shared
            // between the two triangles that make up the rectangle.
            int[] indices = new int[6];

            indices[0] = 0;
            indices[1] = 3;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 1;

            // The number 2 means of number of primitives.
            // This number is equal to the number of triangles we used 
            // to assemble this shape (number of indices / 3)
            return new ShapeDescriptor(2, vertices, indices);
        }

        public static ShapeDescriptor DrawRectangularOutline(Vector2 position,
                                                             Size size, int borderSize, Color borderColor,
                                                             BorderStyle style, Border borders)
        {
            switch (style)
            {
                case BorderStyle.Raised:
                    return DrawRectangularOutline(position, size,
                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, 1f)),
                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, 0.5f)),
                                                  borderSize, borders);

                case BorderStyle.Sunken:
                    return DrawRectangularOutline(position, size,
                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, 0.5f)),
                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, 1f)),
                                                  borderSize, borders);

                case BorderStyle.Flat:
                default:
                    return DrawRectangularOutline(position, size,
                                                  borderColor, borderColor,
                                                  borderSize, borders);
            }
        }

        public static ShapeDescriptor DrawRectangularOutline(Vector2 position,
                                                             Size size, Color borderTopAndLeft,
                                                             Color borderBottomAndRight, int borderSize,
                                                             Border borders)
        {
            ShapeDescriptor vBorderTop = ShapeDescriptor.Empty;
            ShapeDescriptor vBorderSideL = ShapeDescriptor.Empty;
            ShapeDescriptor vBorderSideR = ShapeDescriptor.Empty;
            ShapeDescriptor vBorderBottom = ShapeDescriptor.Empty;

            Vector2 innerPositionTopLeft = new Vector2(
                position.X + borderSize, position.Y + borderSize);

            Vector2 borderPositionTopRight = new Vector2(
                position.X + size.Width - borderSize, position.Y);

            Vector2 borderPositionBottomLeft = new Vector2(
                position.X, position.Y + size.Height - borderSize);

            Size borderTop = new Size(size.Width, borderSize);
            Size borderSide = new Size(borderSize, size.Height);

            if ((borders & Border.Top) != 0)
                vBorderTop = DrawRectangle(
                    position, borderTop, borderTopAndLeft);
            if ((borders & Border.Left) != 0)
                vBorderSideL = DrawRectangle(
                    position, borderSide, borderTopAndLeft);
            if ((borders & Border.Right) != 0)
                vBorderSideR = DrawRectangle(
                    borderPositionTopRight, borderSide, borderBottomAndRight);
            if ((borders & Border.Bottom) != 0)
                vBorderBottom = DrawRectangle(
                    borderPositionBottomLeft, borderTop, borderBottomAndRight);

            return ShapeDescriptor.Join(vBorderTop, vBorderSideL, vBorderSideR, vBorderBottom);
        }

        #endregion

        #region Trapezoids

        /// <summary>
        /// Draws a left trapezoidal outline.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="Size">The Size.</param>
        /// <param name="triangleWidth">Width of the triangle.</param>
        /// <param name="isTriangleUpside">Ff set to <c>true</c> the triangle will be upside.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <param name="style">The style.</param>
        /// <param name="rectangleBorders">The rectangle borders.</param>
        /// <param name="triangleBorders">The triangle borders.</param>
        /// <returns>A left trapezoidal outline ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawLeftTrapezoidalOutline(Vector2 position, Size size,
                                                                 int triangleWidth, bool isTriangleUpside, Color color,
                                                                 int borderSize, Color borderColor,
                                                                 BorderStyle style, Border rectangleBorders,
                                                                 Border triangleBorders)
        {
            Vector2 topLeft = new Vector2(position.X + triangleWidth, position.Y);
            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

            ShapeDescriptor sTriangleSide = ShapeDescriptor.Empty;
            ShapeDescriptor sTriangleBase = ShapeDescriptor.Empty;
            ShapeDescriptor sRectangleOutline =
                DrawRectangularOutline(topLeft, size, borderSize, borderColor, style,
                                       rectangleBorders);

            Color lightShadedBorder = Color.FromArgb(255, ColorOperator.Scale(borderColor, 1f));
            Color darkShadeBorder = Color.FromArgb(255, ColorOperator.Scale(borderColor, 0.5f));
            Color triangleSideColor = Color.Empty;
            Color triangleBaseColor = Color.Empty;

            // Compute border color depending on border style.
            switch (style)
            {
                case BorderStyle.Flat:
                    triangleSideColor = triangleBaseColor = borderColor;
                    break;

                case BorderStyle.Raised:
                    if (isTriangleUpside)
                    {
                        triangleBaseColor = darkShadeBorder;
                        triangleSideColor = lightShadedBorder;
                    }
                    else
                    {
                        triangleBaseColor = lightShadedBorder;
                        triangleSideColor = darkShadeBorder;
                    }
                    break;

                case BorderStyle.Sunken:
                    if (isTriangleUpside)
                    {
                        triangleBaseColor = lightShadedBorder;
                        triangleSideColor = darkShadeBorder;
                    }
                    else
                    {
                        triangleBaseColor = darkShadeBorder;
                        triangleSideColor = lightShadedBorder;
                    }
                    break;
            }

            if (isTriangleUpside)
            {
                if ((triangleBorders & Border.Left) == Border.Left)
                    sTriangleSide = DrawLine(borderSize,
                                             triangleSideColor,
                                             new Vector2(topLeft.X - triangleWidth, topLeft.Y + size.Height),
                                             new Vector2(topLeft.X, topLeft.Y));

                if ((triangleBorders & Border.Bottom) == Border.Bottom)
                    sTriangleBase = DrawRectangle(
                        new Vector2(topLeft.X - triangleWidth, topLeft.Y + size.Height - borderSize),
                        new Size(triangleWidth, borderSize),
                        triangleBaseColor);
            }
            else
            {
                if ((triangleBorders & Border.Left) == Border.Left)
                    sTriangleSide = DrawLine(borderSize,
                                             triangleSideColor,
                                             new Vector2(topLeft.X - triangleWidth, // + borderSize,
                                                         topLeft.Y), // + (borderSize/2)),
                                             new Vector2(topLeft.X, //+ borderSize,
                                                         topLeft.Y + size.Height)); // + (borderSize/2)));


                if ((triangleBorders & Border.Top) == Border.Top)
                    sTriangleBase = DrawRectangle(
                        new Vector2(topLeft.X - triangleWidth + borderSize, topLeft.Y),
                        new Size(triangleWidth, borderSize),
                        triangleBaseColor);
            }

            return ShapeDescriptor.Join(sRectangleOutline, sTriangleSide, sTriangleBase);
        }


        /// <summary>
        /// This methods draws a trapezoid whose 90° angle is in the left side.
        /// </summary>
        /// <param name="position">The topLeft point of the trapezoid</param>
        /// <param name="Size">The <b>whole Size</b> of the trapezoid</param>
        /// <param name="triangleWidth">The extra width of the greater base side</param>
        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
        /// delegate will then proceed to create a shaded version.</param>
        /// <param name="shading">ShadeVertices delegate used.</param>
        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawLeftTrapezoid(Vector2 position, Size Size,
                                                        int triangleWidth, bool isTriangleUpside,
                                                        Color color, int borderSize,
                                                        Shading shading)
        {
            TransformedColored[] vertices = new TransformedColored[5];
            int[] indices = new int[9];
            Vector2 topLeft = new Vector2(position.X + triangleWidth, position.Y);

            Size innerSize = new Size(Size.Width - borderSize, Size.Height - borderSize);

            ShapeDescriptor sTrapezoid;

            int width = innerSize.Width;
            int height = innerSize.Height;

            int[] colors = shading.Mode(color, shading.Values);

            int colorTopLeft = colors[0];
            int colorTopRight = colors[1];
            int colorBottomleft = colors[2];
            int colorBottomRight = colors[3];

            vertices[0] =
                new TransformedColored(topLeft.X + innerSize.Width, topLeft.Y, 0, 1, colorTopRight);
            vertices[1] =
                new TransformedColored(topLeft.X + innerSize.Width, topLeft.Y + innerSize.Height, 0, 1,
                                                    colorBottomRight);
            vertices[2] =
                new TransformedColored(topLeft.X, topLeft.Y + innerSize.Height, 0, 1, colorBottomleft);
            vertices[4] = new TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);


            if (isTriangleUpside)
                vertices[3] =
                    new TransformedColored(topLeft.X - triangleWidth, topLeft.Y + innerSize.Height, 0, 1,
                                                        colorBottomleft);
            else
                vertices[3] =
                    new TransformedColored(topLeft.X - triangleWidth, topLeft.Y, 0, 1, colorTopLeft);

            indices[0] = 0;
            indices[1] = 2;
            indices[2] = 4;
            indices[3] = 0;
            indices[4] = 1;
            indices[5] = 2;
            indices[6] = 3;
            indices[7] = 4;
            indices[8] = 2;


            sTrapezoid = new ShapeDescriptor(3, vertices, indices);
            return sTrapezoid;
        }

        /// <summary>
        /// This methods draws a trapezoid whose 90° angle is in the left side.
        /// </summary>
        /// <param name="position">The topLeft point of the trapezoid</param>
        /// <param name="Size">The <b>whole Size</b> of the trapezoid</param>
        /// <param name="triangleWidth">The extra width of the greater base side</param>
        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
        /// delegate will then proceed to create a shaded version.</param>
        /// <param name="borderSize">Size in pixels of the border.</param>
        /// <param name="borderColor">Average color of the border.</param>
        /// <param name="rectangleBorders">Which borders of the rectangular part of the outline to draw.</param>
        /// <param name="triangleBorders">Which borders of the triangular part of the outline to draw.</param>
        /// <param name="shading">ShadeVertices delegate used.</param>
        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawFullLeftTrapezoid(Vector2 position, Size size,
                                                            int triangleWidth, bool isTriangleUpside,
                                                            Color color,
                                                            int borderSize, Color borderColor,
                                                            BorderStyle style, Border rectangleBorders,
                                                            Border triangleBorders,
                                                            Shading shading)
        {
            ShapeDescriptor sTrapezoid = DrawLeftTrapezoid(position, size, triangleWidth, isTriangleUpside,
                                                           color, borderSize, shading);

            if (style != BorderStyle.None)
                return ShapeDescriptor.Join(sTrapezoid,
                                            DrawLeftTrapezoidalOutline(position, size, triangleWidth, isTriangleUpside,
                                                                       color, borderSize, borderColor, style,
                                                                       rectangleBorders, triangleBorders));
            else
                return sTrapezoid;
        }

        /// <summary>
        /// This methods draws a trapezoid whose 90° angle is in the right side.
        /// </summary>
        /// <param name="position">The topLeft point of the trapezoid</param>
        /// <param name="Size">The <b>whole Size</b> of the trapezoid</param>
        /// <param name="triangleWidth">The extra width of the greater base side</param>
        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
        /// delegate will then proceed to create a shaded version.</param>
        /// <param name="borderSize">Size in pixels of the border.</param>
        /// <param name="borderColor">Average color of the border.</param>
        /// <param name="rectangleBorders">Which borders of the rectangular part of the outline to draw.</param>
        /// <param name="triangleBorders">Which borders of the triangular part of the outline to draw.</param>
        /// <param name="shading">ShadeVertices delegate used.</param>
        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawFullRightTrapezoid(Vector2 position, Size size,
                                                             int triangleWidth, bool isTriangleUpside,
                                                             Color color,
                                                             int borderSize, Color borderColor,
                                                             BorderStyle style, Border rectangleBorders,
                                                             Border triangleBorders,
                                                             Shading shading)
        {
            ShapeDescriptor sTrapezoid = DrawRightTrapezoid(position, size, triangleWidth, isTriangleUpside,
                                                            color, borderSize, shading);

            if (style != BorderStyle.None)
            {
                ShapeDescriptor sOutline = DrawRightTrapezoidalOutline(position, size, triangleWidth, isTriangleUpside,
                                                                       color, borderSize, borderColor, style,
                                                                       rectangleBorders, triangleBorders);
                return ShapeDescriptor.Join(sTrapezoid, sOutline);
            }
            else
                return sTrapezoid;
        }

        /// <summary>
        /// Draws a right trapezoidal outline.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The Size.</param>
        /// <param name="triangleWidth">Width of the triangle.</param>
        /// <param name="isTriangleUpside">Ff set to <c>true</c> the triangle will be upside.</param>
        /// <param name="color">The color.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <param name="style">The style.</param>
        /// <param name="rectangleBorders">The rectangle borders.</param>
        /// <param name="triangleBorders">The triangle borders.</param>
        /// <returns>A right trapezoidal outline ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawRightTrapezoidalOutline(Vector2 position, Size size,
                                                                  int triangleWidth, bool isTriangleUpside, Color color,
                                                                  int borderSize, Color borderColor,
                                                                  BorderStyle style, Border rectangleBorders,
                                                                  Border triangleBorders)
        {
            Vector2 topLeft = position;
            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

            ShapeDescriptor sTriangleSide = ShapeDescriptor.Empty;
            ShapeDescriptor sTriangleBase = ShapeDescriptor.Empty;
            ShapeDescriptor sRectangleOutline =
                DrawRectangularOutline(topLeft, size, borderSize, borderColor, style,
                                       rectangleBorders);

            Color lightShadedBorder = Color.FromArgb(255, ColorOperator.Scale(borderColor, 1f));
            Color darkShadeBorder = Color.FromArgb(255, ColorOperator.Scale(borderColor, 0.5f));
            Color triangleSideColor = Color.Empty;
            Color triangleBaseColor = Color.Empty;

            // Compute border color depending on border style.
            switch (style)
            {
                case BorderStyle.Flat:
                    triangleSideColor = triangleBaseColor = borderColor;
                    break;

                case BorderStyle.Raised:
                    if (isTriangleUpside)
                    {
                        triangleBaseColor = darkShadeBorder;
                        triangleSideColor = lightShadedBorder;
                    }
                    else
                    {
                        triangleBaseColor = lightShadedBorder;
                        triangleSideColor = darkShadeBorder;
                    }
                    break;

                case BorderStyle.Sunken:
                    if (isTriangleUpside)
                    {
                        triangleBaseColor = lightShadedBorder;
                        triangleSideColor = darkShadeBorder;
                    }
                    else
                    {
                        triangleBaseColor = darkShadeBorder;
                        triangleSideColor = lightShadedBorder;
                    }
                    break;
            }

            if (isTriangleUpside)
            {
                if ((triangleBorders & Border.Right) == Border.Right)
                    sTriangleSide = DrawLine(borderSize,
                                             triangleSideColor,
                                             new Vector2(topLeft.X + size.Width, position.Y + (borderSize/2)),
                                             new Vector2(topLeft.X + size.Width + triangleWidth,
                                                         topLeft.Y + size.Height - (borderSize/2)));

                if ((triangleBorders & Border.Bottom) == Border.Bottom)
                    sTriangleBase =
                        DrawRectangle(new Vector2(topLeft.X + size.Width, topLeft.Y + size.Height - borderSize),
                                      new Size(triangleWidth, borderSize),
                                      triangleBaseColor);
            }
            else
            {
                if ((triangleBorders & Border.Left) == Border.Left)
                    sTriangleSide = DrawLine(borderSize,
                                             triangleSideColor,
                                             new Vector2(topLeft.X + innerSize.Width,
                                                         position.Y + size.Height - (borderSize/2)),
                                             new Vector2(topLeft.X + innerSize.Width + triangleWidth,
                                                         position.Y + (borderSize/2)));


                if ((triangleBorders & Border.Top) == Border.Top)
                    sTriangleBase = DrawRectangle(new Vector2(topLeft.X + innerSize.Width - borderSize, topLeft.Y),
                                                  new Size(triangleWidth, borderSize),
                                                  triangleBaseColor);
            }

            return ShapeDescriptor.Join(sRectangleOutline, sTriangleSide, sTriangleBase);
        }

        /// <summary>
        /// Draws a right trapezoid.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="Size">The Size.</param>
        /// <param name="triangleWidth">Width of the triangle.</param>
        /// <param name="isTriangleUpside">if set to <c>true</c> [is triangle upside].</param>
        /// <param name="color">The color.</param>
        /// <param name="borderSize">Size of the border.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <param name="style">The style.</param>
        /// <param name="rectangleBorders">The rectangle borders.</param>
        /// <param name="triangleBorders">The triangle borders.</param>
        /// <param name="shading">The shade mode.</param>
        /// <returns>A right trapezoidal ShapeDescriptor object.</returns>
        public static ShapeDescriptor DrawRightTrapezoid(Vector2 position, Size size, int triangleWidth,
                                                         bool isTriangleUpside,
                                                         Color color,
                                                         int borderSize,
                                                         Shading shading)
        {
            ShapeDescriptor sTrapezoid;

            TransformedColored[] vertices = new TransformedColored[5];
            int[] indices = new int[9];

            Vector2 topLeft = position;

            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

            int[] colors = shading.Mode(color, shading.Values);

            int colorTopLeft = colors[0];
            int colorTopRight = colors[1];
            int colorBottomleft = colors[2];
            int colorBottomRight = colors[3];


            vertices[0] = new TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);
            vertices[1] =
                new TransformedColored(topLeft.X, topLeft.Y + innerSize.Height, 0, 1, colorBottomleft);
            vertices[2] =
                new TransformedColored(topLeft.X + innerSize.Width, topLeft.Y + innerSize.Height, 0, 1,
                                                    colorBottomRight);
            vertices[3] =
                new TransformedColored(topLeft.X + innerSize.Width, topLeft.Y, 0, 1, colorTopRight);

            if (isTriangleUpside)

                vertices[4] =
                    new TransformedColored(topLeft.X + innerSize.Width + triangleWidth,
                                                        topLeft.Y + innerSize.Height, 0, 1, colorBottomRight);
            else
                vertices[4] =
                    new TransformedColored(topLeft.X + innerSize.Width + triangleWidth, topLeft.Y, 0, 1,
                                                        colorTopRight);

            indices[0] = 0;
            indices[1] = 3;
            indices[2] = 2;
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 1;
            indices[6] = 3;
            indices[7] = 4;
            indices[8] = 2;

            sTrapezoid = new ShapeDescriptor(3, vertices, indices);
            return sTrapezoid;
        }

        #endregion

        #region Triangles

        public static ShapeDescriptor DrawEquilateralTriangleRL(Vector2 topLeft, float sideLength, Color color,
                                                              bool isShaded, bool isTriangleUpside)
        {
            Vector2 triangleVertex;
            TransformedColored[] vertices = new TransformedColored[3];
            Color shaded;
            float heightOffset = (float)(sideLength / 2 * Math.Sqrt(3));
           
            int col1 = color.ToArgb();
            int col2;

            if (isShaded)
            {
                shaded = Color.FromArgb(color.A, ColorOperator.Scale(color, 0.5f));
                col2 = shaded.ToArgb();
            }
            else
                col2 = col1;


            if (!isTriangleUpside)
            {
                triangleVertex = new Vector2(topLeft.X, topLeft.Y + sideLength/2);
                vertices[0] = new TransformedColored(triangleVertex.X, triangleVertex.Y, 0, 1, col1);
                vertices[1] = new TransformedColored(triangleVertex.X + heightOffset, triangleVertex.Y - sideLength / 2, 0, 1, col2);
                vertices[2] = new TransformedColored(triangleVertex.X + heightOffset, triangleVertex.Y + sideLength / 2, 0, 1, col2);
            }
            else
            {
                triangleVertex = new Vector2(topLeft.X + heightOffset, topLeft.Y + sideLength / 2);
                vertices[0] = new TransformedColored(triangleVertex.X, triangleVertex.Y, 0, 1, col1);
                vertices[1] = new TransformedColored(triangleVertex.X - heightOffset, triangleVertex.Y-sideLength/2, 0, 1, col2);
                vertices[2] = new TransformedColored(triangleVertex.X - heightOffset, triangleVertex.Y + sideLength/2, 0, 1, col2);
            }

            int[] indices = new int[3];

            if (isTriangleUpside)
            {
                indices[0] = 0;
                indices[1] = 1;
                indices[2] = 2;
            }
            else
            {
                indices[0] = 2;
                indices[1] = 0;
                indices[2] = 1;
            }

            


            return new ShapeDescriptor(1, vertices, indices);
        }

        public static ShapeDescriptor DrawEquilateralTriangle(Vector2 leftVertex, float sideLength, Color color,
                                                              bool isShaded, bool isTriangleUpside)
        {
            TransformedColored[] vertices = new TransformedColored[3];
            Color shaded;
            float heightOffset = (float) (sideLength/2*Math.Sqrt(3));

            int col1 = color.ToArgb();
            int col2;

            if (isShaded)
            {
                shaded = Color.FromArgb(color.A, ColorOperator.Scale(color, 0.5f));
                col2 = shaded.ToArgb();
            }
            else
                col2 = col1;

            vertices[0] = new TransformedColored(leftVertex.X, leftVertex.Y, 0, 1, col2);
            vertices[1] = new TransformedColored(leftVertex.X + sideLength, leftVertex.Y, 0, 1, col2);

            int[] indices = new int[3];

            if (isTriangleUpside)
            {
                heightOffset *= -1;
                indices[0] = 0;
                indices[1] = 1;
                indices[2] = 2;
            }
            else
            {
                indices[0] = 2;
                indices[1] = 0;
                indices[2] = 1;
            }

            vertices[2] =
                new TransformedColored(leftVertex.X + sideLength/2, leftVertex.Y + heightOffset, 0, 1, col1);


            return new ShapeDescriptor(1, vertices, indices);
        }

        public static ShapeDescriptor DrawFullEquilateralTriangle(Vector2 leftVertex, float sideLength, Color color,
                                                              bool isShaded, bool isTriangleUpside, Color borderColor, Shading shading, int borderSize)
        {
            float heightOffset = (float) (sideLength/2*Math.Sqrt(3));
            if (isTriangleUpside)
                heightOffset *= -1;
            ShapeDescriptor triangle = DrawEquilateralTriangleRL(leftVertex, sideLength, color, isShaded, isTriangleUpside);
            ShapeDescriptor triangleOutline =
                DrawPolyLine(2, borderColor, true,
                             new Vector2[]
                                 {
                                     new Vector2(leftVertex.X, leftVertex.Y),
                                     new Vector2(leftVertex.X + sideLength, leftVertex.Y),
                                     new Vector2(leftVertex.X + sideLength/2, leftVertex.Y + heightOffset)
                                 });
            return ShapeDescriptor.Join(triangle, triangleOutline);
        }

        #endregion

        public static ShapeDescriptor DrawWindowFrame(Vector2 position, Vector2 clientPosition,
                                                      Size size, Size captionBarSize, Size innerSize, Padding padding,
                                                      Color innerAreaColor,
                                                      Color borderColor, Shading shading, int borderSize,
                                                      BorderStyle borderStyle)
        {
            TransformedColored[] vertices = new TransformedColored[16];

            int[] colors = shading.Mode(innerAreaColor, shading.Values);

            int colorTopLeft = colors[0];
            int colorTopRight = colors[1];
            int colorBottomLeft = colors[2];
            int colorBottomRight = colors[3];


            /*
             * On the left is the map of the vertices, showing how the window
             * fram is constructed.
             * 
             * ( 0)-( 3)-----------( 4)-( 7)
             *  |  N  |      N      |  N  | 
             *  |   W |             |   E |
             * ( 1)-( 2)-----------( 5)-( 6)
             *  |     |             |     |
             *  |     |  Client     |     |
             *  |  W  |       Area  |  E  |
             *  |     |             |     |
             *  |     |             |     |
             * ( 8)-(11)-----------(12)-(15)
             *  |  S  |             |  S  |
             *  |   W |      S      |   E |
             * ( 9)-(10)-----------(13)-(14)                              
             */

            //northWest equals to Vertex 0
            Vector2 northWest = new Vector2(position.X + borderSize, position.Y + borderSize);
            // clientNorthWest equals to Vertex 2
            Vector2 clientNorthWest =
                new Vector2(northWest.X + padding.Left, northWest.Y + padding.Top + captionBarSize.Height);
            // clientNorthEast equals to Vertex 5
            Vector2 clientNorthEast = new Vector2(clientNorthWest.X + innerSize.Width, clientNorthWest.Y);
            // northEast equals to Vertex 7
            Vector2 northEast = new Vector2(northWest.X + size.Width - borderSize, northWest.Y);
            // southWest equals to Vertex 9
            Vector2 southWest = new Vector2(northWest.X, position.Y + size.Height - borderSize);
            // clientSouthWest equals to Vertex 11
            Vector2 clientSouthWest = new Vector2(clientNorthWest.X, clientNorthWest.Y + innerSize.Height);

            // Vertices 0 through 3 represent NW quadrant.
            vertices[0] = new TransformedColored(northWest.X, northWest.Y, 0, 1, colorTopLeft);
            vertices[1] = new TransformedColored(northWest.X, clientNorthWest.Y, 0, 1, colorTopLeft);
            vertices[2] = new TransformedColored(clientNorthWest.X, clientNorthWest.Y, 0, 1, colorTopLeft);
            vertices[3] = new TransformedColored(clientNorthWest.X, northWest.Y, 0, 1, colorTopLeft);

            // Vertices 4 through 7 represent NE quadrant
            vertices[4] = new TransformedColored(clientNorthEast.X, northEast.Y, 0, 1, colorTopRight);
            vertices[5] = new TransformedColored(clientNorthEast.X, clientNorthEast.Y, 0, 1, colorTopRight);
            vertices[6] = new TransformedColored(northEast.X, clientNorthEast.Y, 0, 1, colorTopRight);
            vertices[7] = new TransformedColored(northEast.X, northEast.Y, 0, 1, colorTopRight);

            // Vertices 8 through 11 represent SW quadrant
            vertices[8] = new TransformedColored(southWest.X, clientSouthWest.Y, 0, 1, colorBottomLeft);
            vertices[9] = new TransformedColored(southWest.X, southWest.Y, 0, 1, colorBottomLeft);
            vertices[10] = new TransformedColored(clientSouthWest.X, southWest.Y, 0, 1, colorBottomLeft);
            vertices[11] =
                new TransformedColored(clientSouthWest.X, clientSouthWest.Y, 0, 1, colorBottomLeft);

            // Vertices 12 throuh 15 represent SE quadrant
            vertices[12] =
                new TransformedColored(clientNorthEast.X, clientSouthWest.Y, 0, 1, colorBottomRight);
            vertices[13] = new TransformedColored(clientNorthEast.X, southWest.Y, 0, 1, colorBottomRight);
            vertices[14] = new TransformedColored(northEast.X, southWest.Y, 0, 1, colorBottomRight);
            vertices[15] = new TransformedColored(northEast.X, clientSouthWest.Y, 0, 1, colorBottomRight);


            int[] indices = new int[48]
                {
                    1, 0, 3, 2, 1, 3,
                    2, 3, 4, 5, 2, 4,
                    5, 4, 7, 6, 5, 7,
                    12, 5, 6, 15, 12, 6,
                    13, 12, 15, 14, 13, 15,
                    10, 11, 12, 13, 10, 12,
                    9, 8, 11, 10, 9, 11,
                    8, 1, 2, 11, 8, 2,
                };

            ShapeDescriptor frame = new ShapeDescriptor(16, vertices, indices);
            if (borderColor.A == 0)
                return frame;
            else
                return ShapeDescriptor.Join(frame,
                                            DrawRectangularOutline(position, size, borderSize, borderColor, borderStyle,
                                                                   Border.All));
        }
    }
}