using System;

namespace AvengersUtd.Odysseus.UIControls
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