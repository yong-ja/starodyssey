using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        { get { return new Vector2(Bottom, X); } }


        public static explicit operator PathGeometry(OrthoRectangle rectangle)
        {
            Segment top = new Segment(rectangle.TopLeft, rectangle.TopRight);
            Segment right = new Segment(rectangle.TopRight, rectangle.BottomRight);
            Segment bottom = new Segment(rectangle.BottomLeft, rectangle.BottomRight);
            Segment left = new Segment(rectangle.TopLeft, rectangle.BottomLeft);

            return new PathGeometry(new[] { top, right, bottom, left });
        }
        
        public static explicit operator Polygon(OrthoRectangle rectangle)
        {
            return new Polygon(new[] { rectangle.TopLeft, rectangle.TopRight, rectangle.BottomRight, rectangle.BottomLeft }); 
        }
    }
}
