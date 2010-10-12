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
    public delegate void MarkerEventHandler(object sender, MarkerEventArgs e);
    public partial class GradientContainer : UserControl
    {
        const int TriangleHalfSize = 6;
        const int TriangleHeight = 12;

        private bool isDragMode;
        private Point dragStartPosition;
        private readonly Pen markerPen;
        internal SortedList<float, Marker> Markers { get; set; }
        private Rectangle gradientRectangle;

        public Marker SelectedMarker { get; private set; }


        public event MarkerEventHandler SelectedMarkerChanged;

        public void OnSelectedMarkerChanged(MarkerEventArgs e)
        {
            MarkerEventHandler handler = SelectedMarkerChanged;
            if (handler != null) handler(this, e);
        }

        public GradientContainer()
        {
            InitializeComponent();
            markerPen = new Pen(Brushes.Black, 1f);
            
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            gradientRectangle = new Rectangle(TriangleHalfSize, TriangleHalfSize,
                                              ClientSize.Width - 2 * TriangleHalfSize,
                                              ClientSize.Height - TriangleHeight - 2);
        }

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


        private void GradientContainer_MouseDown(object sender, MouseEventArgs e)
        {
            int xLocation = e.X - TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;

            Marker newSelectedMarker = Markers.FirstOrDefault(m => Math.Abs(m.Key - offset) <= 0.1f).Value;

            if (newSelectedMarker == null) return;

            if (SelectedMarker == null)
            {
                SelectedMarker = newSelectedMarker;
                OnSelectedMarkerChanged(new MarkerEventArgs(newSelectedMarker));
            }
            if (SelectedMarker != newSelectedMarker)
            {
               SelectedMarker.Selected = false;
               SelectedMarker = newSelectedMarker;
               OnSelectedMarkerChanged(new MarkerEventArgs(newSelectedMarker));
            }
            
            SelectedMarker.Selected = true;
            if (SelectedMarker.Offset != 0.0f && SelectedMarker.Offset != 1.0f)
            {
                isDragMode = true;
                dragStartPosition = e.Location;
            }
            
            Invalidate();
        }

        private void GradientContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragMode)
                return;

            int xLocation = e.X -  TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;
            SelectedMarker.Offset = offset;
            Invalidate();
        }

        private void GradientContainer_MouseUp(object sender, MouseEventArgs e)
        {
            isDragMode = false;
        }
    }
}
