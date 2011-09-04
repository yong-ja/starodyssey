using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry
{
    public class PathFigure
    {
        protected internal List<Segment> SegmentList { get; private set; }

        public PathFigure()
        {
            SegmentList = new List<Segment>();
        }

        #region Public Properties
        public PathFigure(IEnumerable<Segment> segments)
            : this()
        {
            this.SegmentList.AddRange(segments);
        }

        public Segment this[int index]
        {
            get { return SegmentList[index]; }
        }

        public Segment StartSegment
        {
            get { return SegmentList[0]; }
        }

        public IEnumerable<Segment> Segments
        {
            get { return SegmentList; }
        }
        #endregion

        /// <summary>
        /// Removes vertices that are too close together and less than the supplied value.
        /// </summary>
        /// <param name="toleranceValue">The tolerance value.</param>
        public void Optimize(double toleranceValue)
        {
            List<Segment> optimizedSegments = new List<Segment>();
            Vector2D s = this[0].StartPoint;
            foreach (Segment segment in SegmentList)
            {
                double length = segment.Length;
                if (length < toleranceValue)
                    continue;

                Vector2D p = segment.EndPoint;
                optimizedSegments.Add(new Segment(s, p));
                s = p;
            }

            SegmentList = optimizedSegments;
        }

        /// <summary>
        /// Subdivides long segments in ones of smaller length. 
        /// </summary>
        /// <param name="subSegmentLength">An approximate value indicating the length of the individual subsegment.
        /// The segment will be divided into an integer number of parts. 
        ///</param>
        public void Detail(double subSegmentLength)
        {
            List<Segment> detailedSegments = new List<Segment>();

            foreach (Segment segment in SegmentList)
            {
                double length = segment.Length;
                if (length < 2 * subSegmentLength)
                {
                    detailedSegments.Add(segment);
                    continue;
                }
                detailedSegments.AddRange(Segment.Subdivide(segment, (int)Math.Round(length/subSegmentLength)));
            }

            SegmentList = detailedSegments;
        }

        public static explicit operator Polygon(PathFigure figure)
        {
            return new Polygon(figure.SegmentList.Select(s => s.StartPoint));
        }

        public static explicit operator Vertices(PathFigure figure)
        {
            return new Vertices(figure.SegmentList.Select(s => s.StartPoint));
        }

    }
}
