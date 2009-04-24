using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class Options
    {
        public static int GridSpacing { get; set; }
        public static int MajorGridLinesFrequency { get; set;}
        public static bool SnapToGrid { get; set;}

        static Options()
        {
            GridSpacing = Grid.DefaultGridSpacing;
            MajorGridLinesFrequency = Grid.DefaultMajorGridLinesFrequency;
            SnapToGrid = true;
        }
    }
}