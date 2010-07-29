using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public struct VertexDescription
    {
        public VertexFormat Format { get; private set; }
        public int Stride { get; private set; }

        public VertexDescription(VertexFormat format, int stride) : this()
        {
            Format = format;
            Stride = stride;
        }
    }
}
