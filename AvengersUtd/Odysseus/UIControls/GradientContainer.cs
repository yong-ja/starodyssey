using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class GradientContainer : UserControl
    {
        const int TriangleHalfSize = 6;
        const int TriangleHeight = 12;

        private readonly Pen markerPen;
        public GradientContainer()
        {
            InitializeComponent();
            markerPen = new Pen(Brushes.Black, 1f);
        }

        internal SortedList<float, Marker> Markers { get;  set; }
        private Rectangle gradientRectangle;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (Markers == null || Markers.Count == 0)
            {
                return;
            }


            ColorBlend colorBlend = new ColorBlend(Markers.Count)
            {
                Colors = Markers.Select(m => m.Value.Color).ToArray(),
                Positions = Markers.Select(m => m.Value.Offset).ToArray()
            };

            gradientRectangle = new Rectangle(TriangleHalfSize, TriangleHalfSize, ClientSize.Width - 2 * TriangleHalfSize, ClientSize.Height - TriangleHeight - 2);

            using (LinearGradientBrush brush = new LinearGradientBrush(gradientRectangle, Color.Black, Color.Black,
                                                                    LinearGradientMode.Horizontal))
            {
                brush.InterpolationColors = colorBlend;
                e.Graphics.FillRectangle(brush, gradientRectangle);
                e.Graphics.DrawRectangle(Pens.Black, gradientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMarkers(e.Graphics);
        }

        void DrawMarkers(Graphics graphics)
        {
            if (Markers == null || Markers.Count == 0)
                return;


            for (int i = 0; i < Markers.Values.Count; i++)
            {
                Marker marker = Markers.Values[i];
                int markerXLocation = (int) (marker.Offset*gradientRectangle.Width) + gradientRectangle.Location.X;
                Point[] triangleArray = new[]
                                            {
                                                new Point(markerXLocation - TriangleHalfSize, ClientSize.Height - 2),
                                                new Point(markerXLocation + TriangleHalfSize, ClientSize.Height - 2),
                                                new Point(markerXLocation, ClientSize.Height - TriangleHeight)
                                            };

                Brush triangleFill = marker.Selected ? Brushes.Green : Brushes.Gray;
                graphics.FillPolygon(triangleFill, triangleArray);
                graphics.DrawPolygon(markerPen, triangleArray);
            }
        }


        private void GradientContainer_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            int xLocation = me.X - 2*TriangleHalfSize;
            float offset = xLocation/(float)gradientRectangle.Width;

            Marker newMarker = new Marker(Color.Red, offset);
            Markers.Add(newMarker.Offset, newMarker);
            Invalidate();

        }

        private void GradientContainer_MouseClick(object sender, MouseEventArgs e)
        {
            int xLocation = e.X - 2 * TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;

            Marker selectedMarker = Markers.FirstOrDefault(m => Math.Abs(m.Key - offset) <= 0.1f).Value;

            if (selectedMarker!= null)
            {
                selectedMarker.Selected = true;
                Invalidate();
            }


        }
    }
}
