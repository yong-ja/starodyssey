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

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        public Start()
        {
            InitializeComponent();
        }

        private void BoxTask_Click(object sender, RoutedEventArgs e)
        {
            DXWindow window = new DXWindow();
            window.Show();
            this.Close();
        }

        private void BezierTask_Click(object sender, RoutedEventArgs e)
        {
            BezierWindow window = new BezierWindow();
            window.Show();
            this.Close();
        }

        private void GameTask_Click(object sender, RoutedEventArgs e)
        {
            GameTask window = new GameTask();
            window.Show();
            this.Close();
        }
    }
}
