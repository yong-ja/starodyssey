using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public class ShapeDescription : IEquatable<ShapeDescription>, IComparable<ShapeDescription>
    {
        public Shape Shape { get; set; }
        public string Tag { get; set; }
        public int Primitives { get; set; }
        public int ArrayOffset {get; set;}
        public ColoredVertex[] Vertices { get; set; }
        public short[] Indices { get; set; }
        public Depth Depth { get; set; }

        public bool IsDirty { get; set; }

        public ShapeDescription()
        {
            Depth = OdysseyUI.CurrentHud.Depth;
        }

        public bool Equals(ShapeDescription other)
        {
            return (Shape == other.Shape && Primitives == other.Primitives && Vertices == other.Vertices &&
                    Indices == other.Indices && Depth == other.Depth && Tag==other.Tag);

        }

        public void UpdateVertices(ColoredVertex[] vertices)
        {
            Vertices = vertices;
            IsDirty = true;
        }

        public static ShapeDescription Join(params ShapeDescription[] shapes)
        {
            int vbTotal = 0;
            int ibTotal = 0;
            int numPrimitives = 0;

            int arrayOffset = 0;

            for (int i = 0; i < shapes.Length; i++)
            {
                ShapeDescription shape = shapes[i];
                vbTotal += shape.Vertices.Length;
                ibTotal += shape.Indices.Length;
                numPrimitives += shape.Primitives;
                shape.ArrayOffset = arrayOffset;
                arrayOffset += shape.Vertices.Length;
            }

            ColoredVertex[] vertices = new ColoredVertex[vbTotal];
            short[] indices = new short[ibTotal];

            int vbOffset = 0;
            int ibOffset = 0;

            foreach (ShapeDescription shape in shapes)
            {
                Array.Copy(shape.Vertices, 0, vertices, vbOffset, shape.Vertices.Length);

                for (int j = 0; j < shape.Indices.Length; j++)
                    indices[j + ibOffset] = (short) (shape.Indices[j] + vbOffset);

                vbOffset += shape.Vertices.Length;
                ibOffset += shape.Indices.Length;
            }

            ShapeDescription newSDesc = new ShapeDescription
                                            {
                                                Primitives = numPrimitives,
                                                Indices = indices,
                                                Vertices = vertices,
                                                Shape = Shape.Custom,
                                                ArrayOffset = 0,
                                                Tag = shapes[0].Tag
                                            };
            return newSDesc;
        
        }

        public int CompareTo(ShapeDescription other)
        {
            return Depth.CompareTo(other.Depth);
        }

        public override string ToString()
        {
            return string.Format("Id: {0} S: {1} D: {2}", Tag, Shape, Depth);
        }
    }
}
