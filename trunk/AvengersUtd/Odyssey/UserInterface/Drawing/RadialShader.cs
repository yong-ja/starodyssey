using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public class RadialShader : LinearShader
    {
        public Vector2 Center { get; set; }
        public Vector2 GradientOrigin { get; set; }
        public float RadiusX { get; set; }
        public float RadiusY { get; set; }

    }
}
