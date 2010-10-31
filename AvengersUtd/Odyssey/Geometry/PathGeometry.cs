using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class PathGeometry
    {
        private readonly List<Segment> segments;

        public PathGeometry()
        {
            segments = new List<Segment>();
        }

        public PathGeometry(IEnumerable<Segment> segments):this()
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
    }
}
