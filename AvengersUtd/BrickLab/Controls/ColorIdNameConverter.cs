using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace AvengersUtd.BrickLab.Controls
{
    public class ColorIdNameConverter :  IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int colorId = (int)value;
            return ColorNameFromCode(colorId);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static string ColorNameFromCode(int code)
        {
            switch (code)
            {
                default:
                    return "N/A";

                case 11: // Black
                    return "Black";

                case 85: // DBG
                    return "Dark Bluish Gray";
                case 86: // LBG
                    return "Light Bluish Gray";
            }

            
        }
       
    }
}
