using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class OControlStyle
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

        public Style.Padding Padding { get; set; }

        public Style.Shading Shading { get; set; }
        
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

       

        public OControlStyle(ControlStyle cStyle)
        {
            this.BorderEnabled = new OColor(cStyle.ColorArray.BorderEnabled.ToArgb());
            this.BorderHighlighted = new OColor(cStyle.ColorArray.BorderHighlighted.ToArgb());
            this.BorderSize = cStyle.BorderSize;
            this.BorderStyle = cStyle.BorderStyle;
            this.Clicked = new OColor(cStyle.ColorArray.Clicked.ToArgb());
            this.Disabled = new OColor(cStyle.ColorArray.Disabled.ToArgb());
            this.Enabled = new OColor(cStyle.ColorArray.Enabled.ToArgb());
            this.Focused = new OColor(cStyle.ColorArray.Focused.ToArgb());
            this.Highlighted = new OColor(cStyle.ColorArray.Highlighted.ToArgb());
            this.Name = cStyle.Name;
            this.Padding = cStyle.Padding;
            this.Selected = new OColor(cStyle.ColorArray.Selected.ToArgb());
            this.Shading = cStyle.Shading;
            this.Shape = cStyle.Shape;
            this.Size = cStyle.Size;
            this.TextStyleClass = cStyle.TextStyleClass;
        }
    }
}