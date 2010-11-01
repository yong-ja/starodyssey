using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    /// <summary>
    /// This struct represents a rectangle structure in 2D orthographic projection space.
    /// As a result, downward Y coordinates decrease since the center point of the space is at (0,0).
    /// Top left is at (-Width/2, Height/2) while bottom right is at (Width/2, -Height/2) with
    /// width and height referring to the screen size.
    /// </summary>
    public struct OrthoRectangle
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Heigth { get; set; }

        public OrthoRectangle(float x, float y, float width, float heigth) : this()
        {
            X = x;
            Y = y;
            Width = width;
            Heigth = heigth;
        }

        public float Top
        {
            get { return Y; }
        }

        public float Bottom
        {
            get { return Y - Heigth; }
        }

        public float Left
        {
            get { return X; }
        }

        public float Right
        { get { return X + Width; } }

        public Vector2 TopLeft
        {
            get { return new Vector2(X,Y);}
        }

        public Vector2 TopRight
        {
            get { return new Vector2(Right,Y);}
        }

        public Vector2 BottomRight
        {
            get { return new Vector2(Right, Bottom); }
        }

        public Vector2 BottomLeft
        { get { return new Vector2(X, Bottom); } }


        public static explicit operator PathGeometry(OrthoRectangle rectangle)
        {
            Segment top = new Segment(rectangle.TopLeft, rectangle.TopRight);
            Segment right = new Segment(rectangle.TopRight, rectangle.BottomRight);
            Segment bottom = new Segment(rectangle.BottomRight, rectangle.BottomLeft);
            Segment left = new Segment(rectangle.BottomLeft, rectangle.TopLeft);

            return new PathGeometry(new[] { top, left, bottom, right});
        }
        
        /// <summary>
        /// Returns a polygon composed of the rectangles' vertices arranged in
        /// counter-clockwise order;
        /// </summary>
        /// <param name="rectangle">The source OrthoRectangle.</param>
        /// <returns>A polygon structure.</returns>
        public static explicit operator Polygon(OrthoRectangle rectangle)
        {
            return new Polygon(new[] { rectangle.TopRight, rectangle.TopLeft, rectangle.BottomLeft, rectangle.BottomRight }); 
        }

        public static bool IsInside(OrthoRectangle bounds, Borders edge, Vector2 p)
        {
            switch (edge)
            {
                case Borders.Left:
                    return !(p.X <= bounds.Left);

                case Borders.Right:
                    return !(p.X >= bounds.Right);

                case Borders.Top:
                    return !(p.Y >= bounds.Top);

                case Borders.Bottom:
                    return !(p.Y <= bounds.Bottom);

                default:
                    throw Error.WrongCase("edge", "IsInside", edge);
            }
        }

        public static Vector2 LineIntercept(OrthoRectangle bounds, Borders edge, Vector2 a, Vector2 b)
        {
            if (a == b)
            {
                return a;
            }

            switch (edge)
            {
                case Borders.Bottom:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case Borders.Left:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case Borders.Right:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case Borders.Top:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }
    }
}
