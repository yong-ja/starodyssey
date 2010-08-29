using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Primitive
    {
        public static Sphere CreateSphere(float radius, int numStrips)
        {
            Sphere sphere = new Sphere(radius, numStrips);
            sphere.Init();
            return sphere;
        }

        
    }
}
