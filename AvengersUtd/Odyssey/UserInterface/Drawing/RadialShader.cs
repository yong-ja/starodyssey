﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public class RadialShader : LinearShader
    {
        public Vector2 Center { get; set; }
        public Vector2 GradientOrigin { get; set; }
        public float RadiusX { get; set; }
        public float RadiusY { get; set; }

        internal int CenterXIndex { get; private set; }

        public RadialShader()
        {
            Center = GradientOrigin = new Vector2(0.5f, 0.5f);
            RadiusX = RadiusY = 0.5f;
        }

        float[] BuildWidthOffsets()
        {
            List<float> offsets = new List<float>();
            float center = Center.X;

            offsets.Add(center);
            foreach (GradientStop t in Gradient)
            {
                float offset = (t.Offset * RadiusX);
                float leftOffset = center - offset;
                float rightOffset = center + offset;
                
                offsets.AddRange(new[] {leftOffset, rightOffset});
            }

            offsets.Sort();
            CenterXIndex = offsets.IndexOf(center);
            return offsets.ToArray();
        }

        public static Color4[] RadialGradient(LinearShader shader, int numVertex, Shape shape)
        {
            RadialShader rs = (RadialShader) shader;
            Color4[] colors = new Color4[numVertex];
            switch (shape)
            {
                default:
                case Shape.RectangleMesh:
                    // A radial gradient needs  rectangle mesh composed by n*n segments.
                    // The total vertex count
                    //
                    break;
            }
        }

        
    }
}
