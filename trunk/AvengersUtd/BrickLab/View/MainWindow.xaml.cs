﻿using System;
using System.Collections.ObjectModel;
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
            InitializeComponent();
            //applicationMenu = new ObservableCollection<MenuItem>();
            //applicationMenu.CollectionChanged += (sender, e) => OnPropertyChanged(new DependencyPropertyChangedEventArgs("MenuItems"));
            //CreateMenuItems();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);

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