using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows;

namespace WpfTest
{
        public class CustomGrid : FrameworkElement
    {
        #region Dependency Properties

        private static double _minimumCellSize = 50;

        public double CellHeight
        {
            get { return (double)GetValue(CellHeightProperty); }
            set { SetValue(CellHeightProperty, value); }
        }

        public static readonly DependencyProperty CellHeightProperty =
            DependencyProperty.Register("CellHeight", typeof(double), typeof(CustomGrid), new UIPropertyMetadata(_minimumCellSize, VisualPropertyChangedCallback), new ValidateValueCallback(CellSizeValidateCallback));

        public double CellWidth
        {
            get { return (double)GetValue(CellWidthProperty); }
            set { SetValue(CellWidthProperty, value); }
        }

        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register("CellWidth", typeof(double), typeof(CustomGrid), new UIPropertyMetadata(_minimumCellSize, VisualPropertyChangedCallback), new ValidateValueCallback(CellSizeValidateCallback));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(CustomGrid), new UIPropertyMetadata(Brushes.Transparent, VisualPropertyChangedCallback));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(CustomGrid), new UIPropertyMetadata(0E, VisualPropertyChangedCallback));

        public DashStyle DashStyle
        {
            get { return (DashStyle)GetValue(DashStyleProperty); }
            set { SetValue(DashStyleProperty, value); }
        }

        public static readonly DependencyProperty DashStyleProperty =
            DependencyProperty.Register("DashStyle", typeof(DashStyle), typeof(CustomGrid), new UIPropertyMetadata(new DashStyle(), VisualPropertyChangedCallback));

        // all the dependency properties are tied to this callback so the visual updates itself when any of them changes
        private static void VisualPropertyChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            // this method invalidates the state of the visual, causing a render
            (o as CustomGrid).InvalidateVisual();
        }

        private static bool CellSizeValidateCallback(object target)
        {
            // just so the grid's height or width don't go below 50
            // this method is called by both Height and Width DependencyProperties
            if ((double)target < _minimumCellSize)
                return false;

            return true;
        }

        #endregion Dependency Properties

        #region Overrides

        // override drawing to draw the grid
        protected override void OnRender(DrawingContext drawingContext)
        {
            // get the RenderDesiredSize, and divide it by the size of its cells so we know how many cells to draw vertically
            int cellsVertically = (int)Math.Ceiling(this.RenderSize.Height / this.CellHeight);
            // same horizontally
            int cellsHorizontally = (int)Math.Ceiling(this.RenderSize.Width / this.CellWidth);

            // this offsets are used to move right or down to draw the next row or column
            double verticalOffset = 0;
            double horizontalOffset = 0;

            // the pen is defined here, with the stroke and thickness set in the xaml
            Pen pen = new Pen(this.Stroke, this.StrokeThickness);

            pen.DashStyle = this.DashStyle;

                        // draw vertical lines
            for (int i = 0; i <= cellsHorizontally; i++) {
                drawingContext.DrawLine(pen, 
                new Point(i * this.CellWidth, 0), 
                new Point(i * this.CellWidth,
                this.RenderSize.Height));
            }

            // draw horizontal lines
            for (int i = 0; i <= cellsVertically; i++) {
                drawingContext.DrawLine(pen, 
                new Point(0, i * this.CellHeight), 
                new Point(this.RenderSize.Width, 
                i * this.CellHeight));
            }

            //// draw vertical cells
            //for (int i = 0; i <= cellsHorizontally; i++)
            //{
            //    for (int j = 0; j < cellsVertically; j++)
            //    {
            //        drawingContext.DrawLine(pen,
            //            new Point(horizontalOffset, verticalOffset),
            //            new Point(horizontalOffset, this.CellHeight + verticalOffset));

            //        verticalOffset += this.CellHeight;
            //    }

            //    horizontalOffset += this.CellWidth;
            //    verticalOffset = 0;
            //}

            //horizontalOffset = 0;
            //verticalOffset = 0;

            //// draw horizontal cells
            //for (int i = 0; i <= cellsVertically; i++)
            //{
            //    for (int j = 0; j < cellsHorizontally; j++)
            //    {
            //        drawingContext.DrawLine(pen,
            //            new Point(horizontalOffset, verticalOffset),
            //            new Point(this.CellWidth + horizontalOffset, verticalOffset));

            //        horizontalOffset += this.CellWidth;
            //    }

            //    verticalOffset += this.CellHeight;
            //    horizontalOffset = 0;
            //}

            // and we're done! We're actally using the FrameworkElement's own DrawingContext,
            // so no need to add it anywhere or anything
        }

        #endregion Overrides
    }

}
