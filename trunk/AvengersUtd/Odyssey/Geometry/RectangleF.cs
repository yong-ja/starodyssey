using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class OrthoRectangleF
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Heigth { get; set; }

        public OrthoRectangleF(float x, float y, float width, float heigth)
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
