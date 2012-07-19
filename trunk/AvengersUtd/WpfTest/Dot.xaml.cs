﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfTest
{
    public partial class Dot : UserControl, IDot
    {
        public const int Radius = 8;
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center",
                typeof(Point),
                typeof(Dot),
                new PropertyMetadata(new Point(), OnCenterChanged));

        public Dot()
        {
            InitializeComponent();
        }

        public Point Center
        {
            set { SetValue(CenterProperty, value); }
            get { return (Point)GetValue(CenterProperty); }
        }

        static void OnCenterChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as Dot).ellipseGeo.Center = (Point)args.NewValue;
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(Dot),
        new PropertyMetadata(OnFillChanged));

        public Brush Fill
        {
            set { SetValue(FillProperty, value); }
            get { return (Brush)GetValue(FillProperty); }

        }

        private static void OnFillChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Dot dot = sender as Dot;
            if (dot == null)
                return;

            dot.Path.Fill = (Brush)e.NewValue;
        }
    }


    public interface IDot
    {
        Point Center { get; set; }
        object Tag { get; set; }
    }
}
