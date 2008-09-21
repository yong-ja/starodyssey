using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class PointLight : BaseLight
    {
        public PointLight(Vector3 position) : base(position)
        {
        }
    }
}