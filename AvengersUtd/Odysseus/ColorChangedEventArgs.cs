using System;

namespace AvengersUtd.Odysseus
{
    public class ColorChangedEventArgs:EventArgs 
    {

        private ColorHandler.ARGB mArgb;
        private ColorHandler.HSV mHSV;

        public ColorChangedEventArgs ( ColorHandler.ARGB argb,  ColorHandler.HSV HSV) 
        {
            mArgb = argb;
            mHSV = HSV;
        }

        public ColorHandler.ARGB argb
        {
            get 
            {
                return mArgb;
            }
        }

        public ColorHandler.HSV HSV  
        {
            get 
            {
                return mHSV;
            }
        }
    }
}