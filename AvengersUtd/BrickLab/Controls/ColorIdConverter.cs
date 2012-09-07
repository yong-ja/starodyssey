using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace AvengersUtd.BrickLab.Controls
{
    public class ColorIdConverter :  IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int colorId = (int)value;
            return new SolidColorBrush(ColorFromCode(colorId));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        static Color ColorFromCode(int code)
        {
            string hexCode = "#00000000";
            switch (code)
            {

                case 11: // Black
                    hexCode = "#FF212121";
                    break;

                case 85: // DBG
                    hexCode = "#FF595D60";
                    break;
                case 86: // LBG
                    hexCode = "#FFAFB5C7";
                    break;
            }

            return (Color)ColorConverter.ConvertFromString(hexCode);
        }
       
    }
}
