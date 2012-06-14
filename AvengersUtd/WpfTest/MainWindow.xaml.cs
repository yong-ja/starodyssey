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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using SlimDX.Direct3D11;
using AvengersUtd.StarOdyssey.Scenes;
using AvengersUtd.Odyssey;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AvengersUtd.Odyssey.WpfWindow
    {
        TestRenderer scene;

        static MainWindow()
        {
            Game.InitWPF();
        }

        public MainWindow()
        {
            InitializeComponent();
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Maximized;
            
            Loaded += Window_Loaded;
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            scene = new TestRenderer(Game.Context);
            Game.ChangeRenderer(scene);
            BeginRenderingScene();
        }


    }
}
