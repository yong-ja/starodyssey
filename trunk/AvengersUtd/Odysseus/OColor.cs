using System;
using System.ComponentModel;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using System.Text.RegularExpressions;

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

        #region Constructors
        public OColor()
        {
            Alpha = 0;
            Red = 0;
            Green = 0;
            Blue = 0;
        }
        public OColor(byte alpha, byte red, byte green, byte blue)
        {
            this.Alpha = alpha;
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }
        public OColor(byte[] rgb)
        {
            if (rgb.Length != 4)
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
            Regex regex = new Regex(@"A:(?<Alpha>\d+)\sR:(?<Red>\d+)\sG:(?<Green>\d+)\sB:(?<Blue>\d+)");
            Match match = regex.Match(rgb);
            
            if (match.Groups.Count!= 5)
                throw new Exception("Array must have a length of 4.");

            Alpha = Convert.ToByte(match.Groups["Alpha"].Value);
            Red = Convert.ToByte(match.Groups["Red"].Value);
            Green = Convert.ToByte(match.Groups["Green"].Value);
            Blue = Convert.ToByte(match.Groups["Blue"].Value);
        }
        #endregion

        #region Methods
        public new string ToString()
        {
            return String.Format("A:{0} R:{1} G:{2} B:{3}",  Alpha, Red, Green, Blue);
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

        public Color4 ToColor4()
        {
            return new Color4(GetARGB());
        }
        #endregion
    }
}