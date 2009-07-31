using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class PaddingConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                  Type destinationType)
        {
            if (destinationType == typeof(Padding))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                               CultureInfo culture,
                               object value,
                               System.Type destinationType)
        {
            if (destinationType == typeof(String) &&
                 value is Padding)
            {

                Padding padding = (Padding)value;

                return string.Format("{0};{1};{2};{3}", padding.Top, padding.Right, padding.Bottom, padding.Left);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context,
                              Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
                              CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;
                    string[] paddings = s.Split(
                        new[] { ';', ',' }, 4);

                    Padding padding = new Padding
                                          {
                                              Top = Int32.Parse(paddings[0]),
                                              Right = Int32.Parse(paddings[1]),
                                              Bottom = Int32.Parse(paddings[2]),
                                              Left = Int32.Parse(paddings[3])
                                          };

                    return padding;
                }

                catch
                {
                    throw new ArgumentException(
                        "Can not convert '" + (string)value +
                        "' to type Padding");
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
