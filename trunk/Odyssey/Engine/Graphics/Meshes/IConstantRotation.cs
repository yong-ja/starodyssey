using System;
using System.Collections.Generic;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IConstantRotation
    {
        float RotationDelta { get; }
        float RotationPosition { get; set; }
    }
}