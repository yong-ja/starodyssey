using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Drawing
{
    public partial class Designer
    {
        public void DrawPoints()
        {
            CheckParameters(Options.Shader);

            SaveState();
            Width = Height  = 4;
            foreach (Vector4 point in Points)
            {
                Position = new Vector3(point.X, point.Y, point.Z);
                FillRectangle();
            }
            RestoreState();
        }
    }
}
