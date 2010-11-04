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
        private List<Segment> segments;

        protected internal List<Segment> Segments
        {
            get { return segments; }
        }

        public PathFigure()
        {
            segments = new List<Segment>();
        }

        #region Public Properties
        public PathFigure(IEnumerable<Segment> segments)
            : this()
        {
            this.segments.AddRange(segments);
        }

        public Segment this[int index]
        {
            get { return segments[index]; }
        }

        public Segment StartSegment
        {
            get { return segments[0]; }
        }

        public IEnumerable<Segment> SegmentCollection
        {
            get { return segments; }
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
            foreach (Segment segment in segments)
            {
                double length = segment.Length;
                if (length < toleranceValue)
                    continue;

                Vector2D p = segment.EndPoint;
                optimizedSegments.Add(new Segment(s, p));
                s = p;
            }

            segments = optimizedSegments;
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

            foreach (Segment segment in segments)
            {
                double length = segment.Length;
                if (length < 2 * subSegmentLength)
                {
                    detailedSegments.Add(segment);
                    continue;
                }
                detailedSegments.AddRange(Segment.Subdivide(segment, (int)Math.Round(length/subSegmentLength)));
            }

            segments = detailedSegments;
        }

        public static explicit operator Polygon(PathFigure figure)
        {
            return new Polygon(figure.SegmentCollection.Select(s => s.StartPoint));
        }

    }
}
