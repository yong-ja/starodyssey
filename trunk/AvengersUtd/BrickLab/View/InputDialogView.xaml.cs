using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for InputDialogView.xaml
    /// </summary>
    public partial class InputDialogView : Window
    {

        public InputDialogView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tResponse.Focus();
        }

   }
}
