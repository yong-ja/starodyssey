using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.BrickLab.Model;

namespace AvengersUtd.BrickLab.Controls
{
    public class AccountingCell : Control
    {

        static AccountingCell()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (AccountingCell),
                                                     new FrameworkPropertyMetadata(typeof (AccountingCell)));
        }

        public string Amount
        {
            get { return (string) GetValue(AmountProperty); }
            set { SetValue(AmountProperty, value); }
        }

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register("Amount", typeof (string), typeof (AccountingCell), new UIPropertyMetadata(null));

    }
}
