using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using AvengersUtd.Odysseus;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class OControlDescription
    {
        [Category("Style"),Description("Name of this control style.")]
        public string Name { get; set; }

        [Category("Style"),Description("ID of the associated text style class.")]
        public string TextStyleClass { get; set; }

        [Category("Appearance"),Description("Border Style.")]
        public BorderStyle BorderStyle { get; set; }

        [Category("Appearance"), Description("Border Size.")]
        public int BorderSize { get; set; }

        [Category("Appearance"), Description("Element shape.")]
        public Shape Shape { get; set; }

        [Category("Appearance"), Description("Element size.")]
        public Size Size { get; set; }

        public Padding Padding { get; set; }

        public ColorShader ColorShader { get; set; }
        
        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Enabled background color.")]
        public OColor Enabled
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Highlighted background color.")]
        public OColor Highlighted
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Clicked background color.")]
        public OColor Clicked
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Disabled background color.")]
        public OColor Disabled
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Focused background color.")]
        public OColor Focused
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Selected background color.")]
        public OColor Selected
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Default border color.")]
        public OColor BorderEnabled
        {
            get;
            set;
        }

        [EditorAttribute(typeof(OColorEditor), typeof(UITypeEditor))]
        [Category("Design"), DefaultValueAttribute(typeof(OColor), "0 0 0"), DescriptionAttribute("Highlighted background color.")]
        public OColor BorderHighlighted
        {
            get;
            set;
        }

       

        public OControlDescription(ControlDescription cDesc)
        {
            this.BorderEnabled = new OColor(cDesc.ColorArray.BorderEnabled.ToArgb());
            this.BorderHighlighted = new OColor(cDesc.ColorArray.BorderHighlighted.ToArgb());
            this.BorderSize = cDesc.BorderSize;
            this.BorderStyle = cDesc.BorderStyle;
            this.Clicked = new OColor(cDesc.ColorArray.Clicked.ToArgb());
            this.Disabled = new OColor(cDesc.ColorArray.Disabled.ToArgb());
            this.Enabled = new OColor(cDesc.ColorArray.Enabled.ToArgb());
            this.Focused = new OColor(cDesc.ColorArray.Focused.ToArgb());
            this.Highlighted = new OColor(cDesc.ColorArray.Highlighted.ToArgb());
            this.Name = cDesc.Name;
            this.Padding = cDesc.Padding;
            this.Selected = new OColor(cDesc.ColorArray.Selected.ToArgb());
            this.ColorShader = cDesc.ColorShader;
            this.Shape = cDesc.Shape;
            this.Size = cDesc.Size;
            this.TextStyleClass = cDesc.TextStyleClass;
        }
    }
}