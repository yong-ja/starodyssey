// CanonicalSpline.cs (c) 2009 by Charles Petzold (WPF version)
// www.charlespetzold.com/blog/2009/01/Canonical-Splines-in-WPF-and-Silverlight.html
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfTest
{
    public class CanonicalSpline : Shape
    {
        // Cached PathGeometry
        PathGeometry pathGeometry;

        // Dependency Properties
        public static readonly DependencyProperty PointsProperty =
            Polyline.PointsProperty.AddOwner(typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(null, OnMeasurePropertyChanged));

        public static readonly DependencyProperty TensionProperty =
            DependencyProperty.Register("Tension",
                typeof(double),
                typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(0.5, OnMeasurePropertyChanged));

        public static readonly DependencyProperty TensionsProperty =
            DependencyProperty.Register("Tensions",
                typeof(DoubleCollection),
                typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(null, OnMeasurePropertyChanged));

        public static readonly DependencyProperty IsClosedProperty =
            PathFigure.IsClosedProperty.AddOwner(typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(false, OnMeasurePropertyChanged));

        public static readonly DependencyProperty IsFilledProperty =
            PathFigure.IsFilledProperty.AddOwner(typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(false, OnMeasurePropertyChanged));

        public static readonly DependencyProperty FillRuleProperty =
            Polyline.FillRuleProperty.AddOwner(typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(FillRule.EvenOdd, OnRenderPropertyChanged));

        public static readonly DependencyProperty ToleranceProperty =
            DependencyProperty.Register("Tolerance",
                typeof(double),
                typeof(CanonicalSpline),
                new FrameworkPropertyMetadata(Geometry.StandardFlatteningTolerance, OnMeasurePropertyChanged));

        // CLR properties
        public PointCollection Points
        {
            set { SetValue(PointsProperty, value); }
            get { return (PointCollection)GetValue(PointsProperty); }
        }

        public double Tension
        {
            set { SetValue(TensionProperty, value); }
            get { return (double)GetValue(TensionProperty); }
        }

        public DoubleCollection Tensions
        {
            set { SetValue(TensionsProperty, value); }
            get { return (DoubleCollection)GetValue(TensionsProperty); }
        }

        public bool IsClosed
        {
            set { SetValue(IsClosedProperty, value); }
            get { return (bool)GetValue(IsClosedProperty); }
        }

        public bool IsFilled
        {
            set { SetValue(IsFilledProperty, value); }
            get { return (bool)GetValue(IsFilledProperty); }
        }

        public FillRule FillRule
        {
            set { SetValue(FillRuleProperty, value); }
            get { return (FillRule)GetValue(FillRuleProperty); }
        }

        public double Tolerance
        {
            set { SetValue(ToleranceProperty, value); }
            get { return (double)GetValue(ToleranceProperty); }
        }

        // Required DefiningGeometry override
        protected override Geometry DefiningGeometry
        {
            get { return pathGeometry; }
        }

        // Property-Changed handlers
        static void OnMeasurePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as CanonicalSpline).OnMeasurePropertyChanged(args);
        }

        void OnMeasurePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            pathGeometry = CanonicalSplineHelper.CreateSpline(Points, Tension, Tensions, IsClosed, IsFilled, Tolerance);
            InvalidateMeasure();
            OnRenderPropertyChanged(args);
        }

        static void OnRenderPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as CanonicalSpline).OnRenderPropertyChanged(args);
        }

        void OnRenderPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            if (pathGeometry != null)
                pathGeometry.FillRule = FillRule;

            InvalidateVisual();
        }
    }
}
