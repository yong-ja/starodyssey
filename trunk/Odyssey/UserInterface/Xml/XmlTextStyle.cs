#region Disclaimer

/* 
 * XmlTextStyle
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    [XmlType(TypeName = "TextStyle")]
    public class XmlTextStyle
    {
        bool applyHighlight;
        bool applyShadowing;
        string fontName;
        HorizontalAlignment horizontalAlignment;
        bool ignoreBounds;
        bool isBold;
        bool isItalic;
        string name;
        int size;
        VerticalAlignment verticalAlignment;
        XmlColor xmlHighlightedColor;
        XmlColor xmlStandardColor;


        public XmlTextStyle()
        {
            name = fontName = string.Empty;
            isBold = isItalic = applyShadowing = applyHighlight = ignoreBounds = false;
            xmlStandardColor = new XmlColor();
            xmlStandardColor = new XmlColor();
            horizontalAlignment = HorizontalAlignment.Left;
            verticalAlignment = VerticalAlignment.Top;
            Size = 0;
        }

        public XmlTextStyle(TextStyle textStyle)
        {
            if (textStyle == null)
                throw Error.InCreatingFromObject("textStyle", GetType(), typeof (TextStyle));

            name = textStyle.Name;
            isBold = textStyle.IsBold;
            isItalic = textStyle.IsItalic;
            applyHighlight = textStyle.ApplyHighlight;
            applyShadowing = textStyle.ApplyShadowing;
            ignoreBounds = textStyle.IgnoreBounds;
            xmlStandardColor = new XmlColor(ColorIndex.Enabled, textStyle.StandardColor);
            xmlHighlightedColor = (applyHighlight)
                                      ? new XmlColor(ColorIndex.Highlighted, textStyle.HighlightedColor)
                                      : null;
            fontName = textStyle.FontName;
            Size = textStyle.Size;
        }

        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute]
        public bool IsBold
        {
            get { return isBold; }
            set { isBold = value; }
        }

        [XmlAttribute]
        public bool IsItalic
        {
            get { return isItalic; }
            set { isItalic = value; }
        }

        [XmlAttribute]
        public bool ApplyShadowing
        {
            get { return applyShadowing; }
            set { applyShadowing = value; }
        }

        [XmlAttribute]
        public bool ApplyHighlight
        {
            get { return applyHighlight; }
            set { applyHighlight = value; }
        }

        [XmlAttribute]
        public bool IgnoreBounds
        {
            get { return ignoreBounds; }
            set { ignoreBounds = value; }
        }

        [XmlElement]
        public XmlColor StandardColor
        {
            get { return xmlStandardColor; }
            set { xmlStandardColor = value; }
        }

        [XmlElement]
        public XmlColor HighlightedColor
        {
            get { return xmlHighlightedColor; }
            set { xmlHighlightedColor = value; }
        }

        [XmlAttribute]
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

        [XmlAttribute]
        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        [XmlAttribute]
        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        [XmlAttribute]
        public VerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set { verticalAlignment = value; }
        }

        public TextStyle ToTextStyle()
        {
            return new TextStyle(name, isBold, isItalic, applyShadowing, ignoreBounds, xmlStandardColor.ToColor(),
                                 (xmlHighlightedColor == null)
                                     ? xmlStandardColor.ToColor()
                                     : xmlHighlightedColor.ToColor(), Size, fontName, horizontalAlignment,
                                 verticalAlignment);
        }
    }
}