#region Disclaimer
// /* 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com/blog
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */
#endregion

#region Using Directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace AvengersUtd.Odysseus.UIControls
{
    public delegate void MarkerEventHandler(object sender, MarkerEventArgs e);

    public partial class GradientContainer : UserControl
    {
        private const int TriangleHalfSize = 6;
        private const int TriangleHeight = 15;
        private const int CircleRadius = 10;

        private readonly Pen markerPen;
        private Rectangle gradientRectangle;
        private bool isDragMode;

        private Marker selectedMarker;

        public GradientContainer()
        {
            InitializeComponent();
            markerPen = new Pen(Brushes.Black, 1f);
        }

        internal List<Marker> Markers { get; set; }

        public Marker SelectedMarker
        {
            get { return selectedMarker; }
            set
            {
                if (selectedMarker == value) return;
                if (selectedMarker != default(Marker))
                    selectedMarker.Selected = false;
                selectedMarker = value;
                selectedMarker.Selected = true;
                OnSelectedMarkerChanged(new MarkerEventArgs(value));
            }
        }


        #region Events
        public event MarkerEventHandler SelectedMarkerChanged;
        public event MarkerEventHandler OffsetChanging;
        public event MarkerEventHandler OffsetChanged;
        public event EventHandler MarkersChanged;

        protected void OnMarkersChanged(EventArgs e)
        {
            EventHandler handler = MarkersChanged;
            if (handler != null) handler(this, e);
        }

        protected void OnOffsetChanged(MarkerEventArgs e)
        {
            MarkerEventHandler handler = OffsetChanged;
            if (handler != null) handler(this, e);
        }

        protected void OnOffsetChanging(MarkerEventArgs e)
        {
            MarkerEventHandler handler = OffsetChanging;
            if (handler != null) handler(this, e);
        }

        protected void OnSelectedMarkerChanged(MarkerEventArgs e)
        {
            MarkerEventHandler handler = SelectedMarkerChanged;
            if (handler != null) handler(this, e);
        } 
        #endregion

        private void DrawMarkers(Graphics graphics)
        {
            if (Markers == null || Markers.Count == 0)
                return;

            // Draw Start and End Markers
            Rectangle startRectangle = new Rectangle(gradientRectangle.X - CircleRadius / 2,
                                               ClientSize.Height - (int)(CircleRadius * 1.5f),
                                               CircleRadius,
                                               CircleRadius);
            Rectangle endRectangle =
                new Rectangle(gradientRectangle.X + gradientRectangle.Width - CircleRadius / 2,
                              ClientSize.Height - (int)(CircleRadius * 1.5f),
                              CircleRadius,
                              CircleRadius);
            graphics.FillRectangle(Markers[0].Selected ? Brushes.Green : Brushes.Gray,
                                   startRectangle);
            graphics.DrawRectangle(markerPen, startRectangle);

            graphics.FillRectangle(Markers[Markers.Count - 1].Selected ? Brushes.Green : Brushes.Gray, endRectangle);
            graphics.DrawRectangle(markerPen, endRectangle);

            for (int i = 1; i < Markers.Count - 1; i++)
            {
                Marker marker = Markers[i];
                int markerXLocation = (int)(marker.Offset * gradientRectangle.Width) +
                                      gradientRectangle.Location.X;
                Point[] triangleArray = new[]
                                        {
                                            new Point(markerXLocation - TriangleHalfSize,
                                                      ClientSize.Height - 5),
                                            new Point(markerXLocation + TriangleHalfSize,
                                                      ClientSize.Height - 5),
                                            new Point(markerXLocation,
                                                      ClientSize.Height - TriangleHeight)
                                        };

                Brush triangleFill = marker.Selected ? Brushes.Green : Brushes.Gray;
                graphics.FillPolygon(triangleFill, triangleArray);
                graphics.DrawPolygon(markerPen, triangleArray);
            }
        }

        public void DeleteSelectedMarker()
        {
            Markers.Remove(SelectedMarker);
            SelectedMarker = Markers[0];
            Invalidate();
            OnMarkersChanged(EventArgs.Empty);
        }

        public void SortMarkers()
        {
            Markers.Sort((m1, m2) => m1.Offset.CompareTo(m2.Offset));
        }

        #region Events
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            gradientRectangle = new Rectangle(TriangleHalfSize,
                                              TriangleHalfSize,
                                              ClientSize.Width - 2 * TriangleHalfSize,
                                              ClientSize.Height - TriangleHeight - 4);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (Markers == null || Markers.Count == 0)
            {
                return;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(gradientRectangle,
                                                    Color.Black,
                                                    Color.Black,
                                                    LinearGradientMode.Horizontal))
            {
                brush.InterpolationColors = GradientBuilder.ConvertToColorBlend(Markers);
                e.Graphics.FillRectangle(brush, gradientRectangle);
                e.Graphics.DrawRectangle(Pens.Black, gradientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawMarkers(e.Graphics);
        }

        private void GradientContainerDoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            int xLocation = me.X - 2 * TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;

            Marker newMarker = new Marker(Color.Red, offset);
            Markers.Add(newMarker);
            SelectedMarker = newMarker;
            SortMarkers();
            Invalidate();
            OnMarkersChanged(e);
        }

        private void GradientContainer_MouseDown(object sender, MouseEventArgs e)
        {
            int xLocation = e.X - TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;

            Marker newSelectedMarker =
                Markers.FirstOrDefault(m => Math.Abs(m.Offset - offset) <= 0.01f);

            if (newSelectedMarker == null) return;

            if (SelectedMarker == null)
            {
                SelectedMarker = newSelectedMarker;
            }
            if (SelectedMarker != newSelectedMarker)
            {
                SelectedMarker.Selected = false;
                SelectedMarker = newSelectedMarker;
            }

            SelectedMarker.Selected = true;
            if (SelectedMarker.Offset != 0.0f && SelectedMarker.Offset != 1.0f)
            {
                isDragMode = true;
            }

            Invalidate();
        }

        private void GradientContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragMode)
                return;

            int xLocation = e.X - TriangleHalfSize;
            float offset = xLocation / (float)gradientRectangle.Width;
            if (offset < 0.01f)
                offset = 0.01f;
            if (offset > 0.99f)
                offset = 0.99f;
            SelectedMarker.Offset = offset;
            OnOffsetChanging(new MarkerEventArgs(selectedMarker));
            SortMarkers();
            Invalidate();
        }

        private void GradientContainer_MouseUp(object sender, MouseEventArgs e)
        {
            isDragMode = false;
            OnOffsetChanged(new MarkerEventArgs(SelectedMarker));

        }

        private void GradientContainer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Cancel)
                DeleteSelectedMarker();
        } 
        #endregion


    }
}