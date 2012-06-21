using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace WpfTest
{
    public class GazeEventArgs: EventArgs
    {
        public Vector2 GazePoint { get; private set; }
        public GazeEventArgs(Vector2 gazePoint)
        {
            GazePoint = gazePoint;
        }
    }
}
