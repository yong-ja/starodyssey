#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System.Drawing;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    [XmlType(TypeName = "TextDescription")]
    public class XmlTextDescription
    {
        public XmlTextDescription()
        {
            Name = FontFamily = string.Empty;
            IsBold = IsItalic = ApplyShadowing = ApplyHighlight = false;
            StandardColor = new XmlColor();
            StandardColor = new XmlColor();
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Size = 0;
        }

        public XmlTextDescription(TextDescription textDescription)
        {
            if (textDescription == default(TextDescription))
                throw Error.InCreatingFromObject("TextDescription", GetType(), typeof (TextDescription));

            Name = textDescription.Name;
            IsBold = textDescription.IsBold;
            IsItalic = textDescription.IsItalic;
            IsOutlined = textDescription.IsOutlined;

            StandardColor = new XmlColor
                                {
                                    StateIndex = StateIndex.Enabled,
                                    ColorValue = textDescription.Color.ToArgb().ToString("{0:X8}")
                                };
            HighlightedColor = (ApplyHighlight)
                                   ? new XmlColor
                                         {
                                             StateIndex = StateIndex.Highlighted,
                                             ColorValue = textDescription.HighlightedColor.ToArgb().ToString("{0:X8}")
                                         }
                                   : default(XmlColor);
            FontFamily = textDescription.FontFamily;
            Size = textDescription.Size;
        }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsBold { get; set; }

        [XmlAttribute]
        public bool IsItalic { get; set; }

        [XmlAttribute]
        public bool IsOutlined { get; set; }

        [XmlAttribute]
        public bool ApplyShadowing { get; set; }

        [XmlAttribute]
        public bool ApplyHighlight { get; set; }

        [XmlElement]
        public XmlColor StandardColor { get; set; }

        [XmlElement]
        public XmlColor HighlightedColor { get; set; }

        [XmlAttribute]
        public int Size { get; set; }

        [XmlAttribute]
        public string FontFamily { get; set; }

        [XmlAttribute]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [XmlAttribute]
        public VerticalAlignment VerticalAlignment { get; set; }

        FontStyle FontStyle
        {
            get
            {
                FontStyle style = FontStyle.Regular;
                if (IsBold)
                    style |= FontStyle.Bold;
                if (IsItalic)
                    style |= FontStyle.Italic;

                return style;
            }
        }
        public TextDescription ToTextDescription()
        {
            return new TextDescription
                       {
                           Name = Name,
                           Color = StandardColor.ToColor4(),
                           HighlightedColor = HighlightedColor.ColorValue != null ? HighlightedColor.ToColor4() : new Color4(),
                           FontFamily = FontFamily,
                           FontStyle = FontStyle,
                           Size = Size,
                           HorizontalAlignment = HorizontalAlignment,
                           VerticalAlignment = VerticalAlignment,
                           IsOutlined = IsOutlined
                       };

        }
    }
}