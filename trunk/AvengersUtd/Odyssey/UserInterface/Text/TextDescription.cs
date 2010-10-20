using System;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Text
{
    public struct TextDescription : IEquatable<TextDescription>
    {
        public const string Error = "Error";

        public string Name { get; set; }
        public string FontFamily { get; set; }
        public int Size { get; set; }
        public FontStyle FontStyle { get; set; }
        public Color4 Color { get; set; }
        public Color4 SelectedColor { get; private set; }
        public Color4 HighlightedColor { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }

        public override string ToString()
        {
            string styleTag = string.Empty;
            if (IsBold) styleTag += "B";
            if (IsItalic) styleTag += "I";
            if (IsStrikeout) styleTag += "S";
            if (IsUnderlined) styleTag += "U";
            if (IsOutlined) styleTag += "O";

            if (styleTag == string.Empty)
                styleTag = "R";

            return string.Format("{0} {1} {2} {3} C:[{4}] H:[{5}] S:[{6}] Ha:{7} Va:{8}", 
                Name, FontFamily, Size,
                styleTag,
                Color.ToArgb().ToString("X8"),
                HighlightedColor.ToArgb().ToString("X8"),
                SelectedColor.ToArgb().ToString("X8"),
                HorizontalAlignment, VerticalAlignment);
        }

        internal string ActiveCode(StateIndex activeState)
        {
            unchecked
            {
                int result = (FontFamily != null ? FontFamily.GetHashCode() : 0);
                result = (result * 397) ^ Size;
                result = (result * 397) ^ FontStyle.GetHashCode();

                switch (activeState)
                {
                    case StateIndex.Highlighted:
                        result = (result * 397) ^ HighlightedColor.GetHashCode();
                        break;

                    case StateIndex.Selected:
                        result = (result * 397) ^ SelectedColor.GetHashCode();
                        break;

                    case StateIndex.Enabled:
                        result = (result * 397) ^ Color.GetHashCode();
                        break;

                    default:
                        throw Odyssey.Error.WrongCase("activeState", "ActiveCode", activeState);
                }
                return result.ToString("X8");
            }
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, FontStyle, GraphicsUnit.World);
        }

        public bool IsBold
        {
            get { return (FontStyle & FontStyle.Bold) == FontStyle.Bold; }
        }

        public bool IsItalic
        {
            get { return (FontStyle & FontStyle.Italic) == FontStyle.Italic; }
        }

        public bool IsStrikeout
        {
            get { return (FontStyle & FontStyle.Strikeout) == FontStyle.Strikeout; }
        }

        public bool IsUnderlined
        {
            get { return (FontStyle & FontStyle.Underline) == FontStyle.Underline; }
        }

        public bool IsOutlined { get; set; }
        
        public static TextDescription Default
        {
            get
            {
                return new TextDescription
                           {
                               Name = "Default",
                               Color = new Color4(System.Drawing.Color.White),
                               FontFamily = "Arial",
                               FontStyle = FontStyle.Regular,
                               Size = 16,
                               HorizontalAlignment = HorizontalAlignment.Left,
                               VerticalAlignment = VerticalAlignment.Center
                           };
            }
        }

        #region Equality
        #region IEquatable<ControlDescription>
        public static bool operator ==(TextDescription fDesc1, TextDescription fDesc2)
        {
            return fDesc1.Name == fDesc2.Name;
        }

        public static bool operator !=(TextDescription fDesc1, TextDescription fDesc2)
        {
            return !(fDesc1 == fDesc2);
        }


        #endregion

        public bool Equals(TextDescription other)
        {
            return Name == other.Name && FontFamily == other.FontFamily && Size == other.Size &&
                   FontStyle == other.FontStyle && Color == other.Color && SelectedColor == other.SelectedColor &&
                   HighlightedColor == other.HighlightedColor && HorizontalAlignment == other.HorizontalAlignment &&
                   VerticalAlignment == other.VerticalAlignment;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(TextDescription)) return false;
            return Equals((TextDescription)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Name != null ? Name.GetHashCode() : 0);
                result = (result * 397) ^ (FontFamily != null ? FontFamily.GetHashCode() : 0);
                result = (result * 397) ^ Size;
                result = (result * 397) ^ FontStyle.GetHashCode();
                result = (result * 397) ^ Color.GetHashCode();
                result = (result * 397) ^ SelectedColor.GetHashCode();
                result = (result * 397) ^ HighlightedColor.GetHashCode();
                result = (result * 397) ^ HorizontalAlignment.GetHashCode();
                result = (result * 397) ^ VerticalAlignment.GetHashCode();
                return result;
            }
        } 
        #endregion
    }
}