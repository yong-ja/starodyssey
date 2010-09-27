#region Disclaimer

/* 
 * Xml Wrappers for basic controls
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

using System;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface.Xml
{
    //[XmlType(TypeName = "Label")]
    //public class XmlLabel : XmlBaseControl
    //{
    //    string text;

    //    public XmlLabel() : base()
    //    {
    //        text = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Text
    //    {
    //        get { return text; }
    //        set { text = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        Label label = control as Label;
    //        text = label.Text;
    //    }
    //}

    [XmlType(TypeName = "Button")]
    public class XmlButton : XmlBaseControl
    {

        public XmlButton(Button button) : base(button)
        {
            Content = button.Content;
        }

        [XmlAttribute]
        [Category("Appearance")]
        public string Content { get; set; }

        internal override void FromControl(BaseControl control)
        {
            base.FromControl(control);
            Button button = control as Button;
            if (button != null) Content = button.Content;

            else throw Error.ArgumentNull("control", GetType(), "FromControl");
        }

        protected override void WriteCustomCSCode(StringBuilder sb)
        {
            sb.AppendFormat("\t\tContent = {0},\n", Content);
        }
    }

    //[XmlType(TypeName = "DecoratedButton")]
    //public class XmlDecoratedButton : XmlBaseControl
    //{
    //    DecorationType decorationType;

    //    public XmlDecoratedButton() : base()
    //    {
    //        decorationType = DecorationType.DownsideTriangle;
    //    }

    //    [XmlAttribute]
    //    public DecorationType DecorationType
    //    {
    //        get { return decorationType; }
    //        set { decorationType = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        DecoratedButton decoratedButton = control as DecoratedButton;
    //        decorationType = decoratedButton.DecorationType;
    //    }
    //}

    //[XmlType(TypeName = "TextBox")]
    //public class XmlTextBox : XmlBaseControl
    //{
    //    bool acceptsReturn;
    //    KeyType keyMask;
    //    string text;


    //    public XmlTextBox() : base()
    //    {
    //        text = string.Empty;
    //    }

    //    [XmlAttribute]
    //    public string Text
    //    {
    //        get { return text; }
    //        set { text = value; }
    //    }

    //    [XmlAttribute]
    //    public bool AcceptsReturn
    //    {
    //        get { return acceptsReturn; }
    //        set { acceptsReturn = value; }
    //    }

    //    [XmlAttribute]
    //    public KeyType KeyMask
    //    {
    //        get { return keyMask; }
    //        set { keyMask = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        TextBox textBox = control as TextBox;
    //        text = textBox.Text;
    //        TextDescriptionClass = textBox.TextStyle.Name;
    //        acceptsReturn = textBox.AcceptsReturn;
    //        keyMask = textBox.KeyMask;
    //    }
    //}

    //[XmlType(TypeName = "TrackBar")]
    //public class XmlTrackBar : XmlBaseControl
    //{
    //    int maxValue;
    //    int minValue;
    //    int tickFrequency;

    //    public XmlTrackBar() : base()
    //    {
    //    }

    //    [XmlAttribute]
    //    public int MinimumValue
    //    {
    //        get { return minValue; }
    //        set { minValue = value; }
    //    }

    //    [XmlAttribute]
    //    public int MaximumValue
    //    {
    //        get { return maxValue; }
    //        set { maxValue = value; }
    //    }

    //    [XmlAttribute]
    //    public int TickFrequency
    //    {
    //        get { return tickFrequency; }
    //        set { tickFrequency = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        TrackBar trackBar = control as TrackBar;
    //        minValue = trackBar.MinimumValue;
    //        maxValue = trackBar.MaximumValue;
    //        tickFrequency = trackBar.TickFrequency;
    //    }
    //}

    //[XmlType(TypeName ="CheckBox")]
    //public class XmlCheckBox : XmlBaseControl
    //{
    //    bool isChecked;

    //    public XmlCheckBox() : base()
    //    {
    //    }

    //    [XmlAttribute]
    //    public bool IsChecked
    //    {
    //        get { return isChecked; }
    //        set { isChecked = value; }
    //    }

    //    public override void FromControl(BaseControl control)
    //    {
    //        base.FromControl(control);
    //        CheckBox checkBox = control as CheckBox;
    //        TextDescriptionClass = checkBox.TextStyle.Name;
    //        isChecked = checkBox.IsChecked;
    //    }
    // }
}