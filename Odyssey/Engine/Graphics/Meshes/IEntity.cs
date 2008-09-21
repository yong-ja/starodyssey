using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public interface IEntity
    {
        Vector3 PositionV3 { get; set; }
        Vector4 PositionV4 { get; }

        Vector3 RotationDelta { get; set; }
        Quaternion CurrentRotation { get; set; }

        bool IsInViewFrustum();
    }
}