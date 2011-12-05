using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct Ellipse : IEquatable<Ellipse>
    {
        public Vector2D Center;
        public double RadiusX;
        public double RadiusY;

        public Ellipse(Vector2D center, double radiusX, double radiusY)
        {
            Center = center;
            RadiusX = radiusX;
            RadiusY = radiusY;
        }

        public bool Equals(Ellipse other)
        {
            return (this.Center == other.Center) &&
                   (Math.Abs(RadiusX - other.RadiusX) < MathHelper.EpsilonD) &&
                    (Math.Abs(RadiusY - other.RadiusY) < MathHelper.EpsilonD);
        }

    }
}
