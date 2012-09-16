using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using AvengersUtd.BrickLab.Controls;
using AvengersUtd.BrickLab.Logging;
using MenuItem = AvengersUtd.BrickLab.Controls.MenuItem;

namespace AvengersUtd.BrickLab.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private readonly ObservableCollection<MenuItem> applicationMenu;


        public MainWindow()
        {
            BrickClient.Dispatcher = Dispatcher;
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            //Dispatcher.ShutdownStarted += (s, args) => FlushLog();

        }

        private void FlushLog()
        {
            foreach (TraceListener tl in Trace.Listeners)
            {
                tl.Flush();
                tl.Close();
            }
        }

        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.InnerException + "\n\n"+
                e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            LogEvent.System.Log(string.Format("{0}\n{1}\nStackTrace:\n{2}", e.Exception.GetType(),
                e.Exception.Message, e.Exception.StackTrace));
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return applicationMenu;
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Global.Init();
            Title += Global.Version;
            LogEvent.System.Log("App started");
        }


        public void SwitchTo(UserControl control)
        {
            //UserControl prevControl = (UserControl) Container.Content;
            if (Container.Children.Contains(control))
                control.Visibility = Visibility.Visible;
            else
                Container.Children.Add(control);

            foreach (UIElement ctl in Container.Children)
                if (ctl != control)
                    ctl.Visibility = Visibility.Collapsed;

            foreach (ImageButton imgButton in SidePanel.Children)
                if (imgButton.CommandParameter != control)
                    imgButton.IsChecked = false;

        }



    }
}
