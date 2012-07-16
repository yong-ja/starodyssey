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
        public int LeftValid { get; private set; }
        public int RightValid { get; private set; }

        public GazeEventArgs(Vector2 gazePoint, int left, int right)
        {
            GazePoint = gazePoint;
            LeftValid = left;
            RightValid = right;
        }
    }
}
