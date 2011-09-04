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
    public struct OrthoRectangle : IPolygon
    {
        #region Fields
        public double X;
        public double Y;
        public double Width;
        public double Height;
        #endregion

        #region Properties
        public double Top
        {
            get { return Y; }
        }

        public double Bottom
        {
            get { return Y - Height; }
        }

        public double Left
        {
            get { return X; }
        }

        public double Right
        { get { return X + Width; } }

        public Vector2D TopLeft
        {
            get { return new Vector2D(X, Y); }
        }

        public Vector2D TopRight
        {
            get { return new Vector2D(Right, Y); }
        }

        public Vector2D BottomRight
        {
            get { return new Vector2D(Right, Bottom); }
        }

        public Vector2D BottomLeft
        { get { return new Vector2D(X, Bottom); } } 
        #endregion

        public OrthoRectangle(double x, double y, double width, double height)
            : this()
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        #region Type conversion operators
        public static explicit operator PathFigure(OrthoRectangle rectangle)
        {
            Segment top = new Segment(rectangle.TopLeft, rectangle.TopRight);
            Segment right = new Segment(rectangle.TopRight, rectangle.BottomRight);
            Segment bottom = new Segment(rectangle.BottomRight, rectangle.BottomLeft);
            Segment left = new Segment(rectangle.BottomLeft, rectangle.TopLeft);

            return new PathFigure(new[] { top, left, bottom, right });
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
        #endregion

        public static bool IsPointInside(OrthoRectangle bounds, Borders edge, Vector2D p)
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
                    throw Error.WrongCase("edge", "IsPointInside", edge);
            }
        }

        public static Vector2D LineIntercept(OrthoRectangle bounds, Borders edge, Vector2D a, Vector2D b)
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

                    return new Vector2D(a.X + (((b.X - a.X) * (bounds.Bottom - a.Y)) / (b.Y - a.Y)), bounds.Bottom);

                case Borders.Left:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2D(bounds.Left, a.Y + (((b.Y - a.Y) * (bounds.Left - a.X)) / (b.X - a.X)));

                case Borders.Right:
                    if (b.X == a.X)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2D(bounds.Right, a.Y + (((b.Y - a.Y) * (bounds.Right - a.X)) / (b.X - a.X)));

                case Borders.Top:
                    if (b.Y == a.Y)
                    {
                        throw new ArgumentException("no intercept found");
                    }

                    return new Vector2D(a.X + (((b.X - a.X) * (bounds.Top - a.Y)) / (b.Y - a.Y)), bounds.Top);
            }

            throw new ArgumentException("no intercept found");
        }

        #region IPolygon Members

        public Vertices Vertices
        {
            get; set;}


        public Vector2D Centroid
        {
            get { return new Vector2D(X+ Width/2, Y - Height/2); }
        }

        public double Area
        {
            get { return Width*Height; }
        }

        public Vector4[] ComputeVector4Array(float zIndex)
        {
            return ((Polygon) this).ComputeVector4Array(zIndex);
        }

        public bool IsPointInside(Vector2D point)
        {
            return IsPointInside(this, Borders.Top, point) || IsPointInside(this, Borders.Right, point)
                   || IsPointInside(this, Borders.Bottom, point) || IsPointInside(this, Borders.Left, point);
        }

        #endregion
    }
}
