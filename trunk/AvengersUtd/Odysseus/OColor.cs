using System;
using System.ComponentModel;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odysseus
{
    [TypeConverter(typeof(OColorConverter))]
    public class OColor
    {
        #region " The color channel variables w/ accessors/mutators. "

        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        #endregion

        #region " Constructors "
        public OColor()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
        }
        public OColor(byte red, byte green, byte blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
        public OColor(byte[] rgb)
        {
            if (rgb.Length != 3)
                throw new Exception("Array must have a length of 3.");
            Red = rgb[0];
            Green = rgb[1];
            Blue = rgb[2];
        }
        public OColor(int argb)
        {
            byte[] bytes = BitConverter.GetBytes(argb);
            Alpha = bytes[3];
            Red = bytes[2];
            Green = bytes[1];
            Blue = bytes[0];
        }
        public OColor(string rgb)
        {
            string[] parts = rgb.Split(' ');
            if (parts.Length != 3)
                throw new Exception("Array must have a length of 3.");
            Red = Convert.ToByte(parts[0]);
            Green = Convert.ToByte(parts[1]);
            Blue = Convert.ToByte(parts[2]);
        }
        #endregion

        #region " Methods "
        public new string ToString()
        {
            return String.Format("{0} {1} {2}", Red, Green, Blue);
        }
        public byte[] GetRGB()
        {
            return new[] { Red, Green, Blue };
        }
        public int GetARGB()
        {
            byte[] temp = new[] { Blue, Green, Red, Alpha };
            return BitConverter.ToInt32(temp, 0);
        }
        #endregion
    }
}