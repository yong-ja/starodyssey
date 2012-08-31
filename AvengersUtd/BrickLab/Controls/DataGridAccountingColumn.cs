using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AvengersUtd.BrickLab.Controls
{
    public class DataGridAccountingColumn : DataGridTextColumn
    {
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            AccountingCell accountingCell = new AccountingCell();

            Binding newBinding = new Binding(((Binding) Binding).Path.Path);

            string decimalPattern = new String('0', DecimalDigits);
            string accountingFormat = "{0:#,##0." + decimalPattern + "}";
            newBinding.StringFormat = accountingFormat;
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            newBinding.ConverterCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            accountingCell.SetBinding(AccountingCell.AmountProperty, newBinding);

            return accountingCell;
        }

        public int DecimalDigits
        {
            get { return (int) GetValue(DecimalDigitsProperty); }
            set { SetValue(DecimalDigitsProperty, value); }
        }

        public static readonly DependencyProperty DecimalDigitsProperty =
            DependencyProperty.Register("DecimalDigits", typeof (int), typeof (DataGridAccountingColumn),
                                        new UIPropertyMetadata(2));
    }

} 
