using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Geometry.Triangulation.Delaunay;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Geometry.Triangulation
{
    public static class MeshMapper
    {
        public static ushort[] CreateIndexArray(Triangulatable t)
        {
            IList<TriangulationPoint> points = t.Points;
            List<ushort> indices = new List<ushort>();
            foreach (DelaunayTriangle triangle in t.Triangles)
            {
                int[] indexCCW = new[]
                                     {
                                         triangle.IndexCCWFrom(triangle.Points[0]),
                                         triangle.IndexCCWFrom(triangle.Points[1]),
                                         triangle.IndexCCWFrom(triangle.Points[2]),
                                     };

                ushort[] indexArray = new[]
                                       {
                                           (ushort) points.IndexOf(triangle.Points[indexCCW[0]]),
                                           (ushort) points.IndexOf(triangle.Points[indexCCW[1]]),
                                           (ushort) points.IndexOf(triangle.Points[indexCCW[2]]),
                                       };

                indices.AddRange(indexArray);
            }

            //indices.AddRange(new ushort[]
            //                     {
            //                         (ushort)(points.Count - 1),1,0
            //                     });

            return indices.ToArray();
        }


    }
}
