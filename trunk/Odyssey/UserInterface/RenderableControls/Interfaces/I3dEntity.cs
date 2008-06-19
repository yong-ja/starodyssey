using System;
using System.Collections.Generic;
using System.Text;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls.Interfaces
{
    public interface I3dEntity
    {
        Vector3 Position { get; set; }
        Mesh Mesh { get; }

    }
}
