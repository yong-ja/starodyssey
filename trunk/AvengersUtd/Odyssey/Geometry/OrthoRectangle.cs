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
        /// <summary>
        /// Gets the top Y-coordinate.
        /// </summary>
        public double Top
        {
            get { return Y; }
        }

        /// <summary>
        /// Gets the bottom Y-coordinate.
        /// </summary>
        public double Bottom
        {
            get { return Y - Height; }
        }

        /// <summary>
        /// Gets the leftmost X-coordinate.
        /// </summary>
        public double Left
        {
            get { return X; }
        }

        /// <summary>
        /// Gets the rightmost X-coordinate.
        /// </summary>
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


        public bool IsPointInside(Vector2D point)
        {
            return IsPointInside(this, Borders.Top, point) || IsPointInside(this, Borders.Right, point)
                   || IsPointInside(this, Borders.Bottom, point) || IsPointInside(this, Borders.Left, point);
        }

        #endregion

        public Vector2D[] VerticesArray
        {
            get { return new[] {TopLeft, BottomLeft, BottomRight, TopRight}; }
        }
    }
}
