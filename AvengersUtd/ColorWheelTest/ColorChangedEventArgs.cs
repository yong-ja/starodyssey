using System;

namespace AvengersUtd.ColorChooserTest
{
    public class ColorChangedEventArgs:EventArgs 
    {
        public ColorChangedEventArgs ( ColorHandler.ARGB argb,  ColorHandler.HSV HSV) 
        {
            this.ARGB = argb;
            this.HSV = HSV;
        }

        public ColorHandler.ARGB ARGB { get; private set; }

        public ColorHandler.HSV HSV { get; private set; }
    }
}