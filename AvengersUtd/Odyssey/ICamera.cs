using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey
{
    public interface ICamera
    {
         Matrix World { get; }
         Matrix Projection { get; }
         Matrix View { get; }
         Matrix WorldViewProjection { get;  }
         Viewport Viewport { get;  }
         Vector3 ViewVector { get; }
    }
}
