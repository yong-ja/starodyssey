using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;

namespace AvengersUtd.Odysseus
{
    public static class Options
    {
        public static int GridSpacing { get; set; }
        public static int MajorGridLinesFrequency { get; set;}
        public static bool SnapToGrip { get; set;}

        static Options()
        {
            GridSpacing = Grid.DefaultGridSpacing;
            MajorGridLinesFrequency = Grid.DefaultMajorGridLinesFrequency;
        }
    }
}
