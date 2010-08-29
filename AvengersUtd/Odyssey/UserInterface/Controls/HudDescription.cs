using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public struct HudDescription
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float ZNear { get; private set; }
        public float ZFar { get; private set; }
        public bool CameraEnabled { get; private set; }
        public bool Multithreaded { get; private set; }

        public HudDescription(int width, int height, float zNear, float zFar, bool cameraEnabled, bool multithreaded) : this()
        {
            Width = width;
            Height = height;
            ZNear = zNear;
            ZFar = zFar;
            CameraEnabled = cameraEnabled;
            Multithreaded = multithreaded;
        }
    }
}
