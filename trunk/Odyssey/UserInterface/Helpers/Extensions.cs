using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using System.Drawing;

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    public static class Extensions
    {
        public static OColor ToColor(this Color color)
        {
            return new OColor(color.R, color.G, color.B);
        }
    }
}
