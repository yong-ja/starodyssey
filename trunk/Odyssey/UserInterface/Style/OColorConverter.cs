using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class OColorConverter: TypeConverter
    {
        // This is used, for example, by DefaultValueAttribute to convert from string to MyColor.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
                return new OColor((string)value);
            return base.ConvertFrom(context, culture, value);
        }
        // This is used, for example, by the PropertyGrid to convert MyColor to a string.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
        {
            if ((destType == typeof(string)) && (value is OColor))
            {
                OColor color = (OColor)value;
                return color.ToString();
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
}