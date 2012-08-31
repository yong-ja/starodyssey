using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.Controls
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Currency currency = (Currency)value;
            switch (currency)
            {
                default:
                case Currency.Unknown:
                    return "?";

                case Currency.GBP:
                    return "£";

                case Currency.USD:
                    return "$";

                case Currency.EUR:
                    return "€";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string currencySymbol = (string)value;
            switch (currencySymbol)
            {
                default:
                    return Currency.Unknown;

                case "£":
                    return Currency.GBP;
                case "$":
                    return Currency.USD;
                case "€":
                    return Currency.EUR;

            }
        }
    }
}
