using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CBUpdateEveryFrame
    {
        //public Matrix World { get; set; }
        //public Matrix View { get; set; }
        public Color4 Color4 { get; set; }

        public const int Stride = 16;
    }
}
