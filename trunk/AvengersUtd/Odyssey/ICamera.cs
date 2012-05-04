using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey
{
    public interface ICamera
    {
         float NearClip { get; }
         float FarClip { get; }
         Matrix World { get; }
         Matrix Projection { get; }
         Matrix View { get; }

    }
}
