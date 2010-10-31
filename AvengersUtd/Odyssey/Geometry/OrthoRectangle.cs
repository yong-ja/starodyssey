using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
