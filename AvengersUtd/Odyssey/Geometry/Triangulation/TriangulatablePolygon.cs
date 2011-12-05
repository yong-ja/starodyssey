/* Poly2Tri
 * Copyright (c) 2009-2010, Poly2Tri Contributors
 * http://code.google.com/p/poly2tri/
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * * Neither the name of Poly2Tri nor the names of its contributors may be
 *   used to endorse or promote products derived from this software without specific
 *   prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

/// Changes from the Java version
///   Triangulatable constructors sprused up, checks for 3+ polys
///   Naming of everything
///   getTriangulationMode() -> TriangulationMode { get; }
///   Exceptions replaced
/// Future possibilities
///   We have a lot of Add/Clear methods -- we may prefer to just expose the container
///   Some self-explanitory methods may deserve commenting anyways

using System;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Geometry.Triangulation.Delaunay;

namespace AvengersUtd.Odyssey.Geometry.Triangulation
{
    public class Triangulatable : ITriangulatable
    {
        readonly List<TriangulationPoint> points = new List<TriangulationPoint>();
         List<TriangulationPoint> steinerPoints;
         List<Triangulatable> holes;
         List<DelaunayTriangle> triangles;
         TriangulationPoint last;

         public Triangulatable(IPolygon polygon)
         {
             List<Vector2D> vPoints = new List<Vector2D>(polygon.VerticesArray);
             List<TriangulationPoint> tPoints = vPoints.ConvertAll(p => (TriangulationPoint) (p));

             if (tPoints.Count < 3) throw new ArgumentException("List has fewer than 3 points", "points");

             // Lets do one sanity check that first and last point haven't got same position
             // It's something that often happens when importing polygon data from other formats
             if (tPoints[0].Equals(tPoints[tPoints.Count - 1])) tPoints.RemoveAt(tPoints.Count - 1);

             points.AddRange(tPoints);
         }



        /// <summary>
        /// Create a polygon from a list of at least 3 points with no duplicates.
        /// </summary>
        /// <param name="points">A list of unique points</param>
        public Triangulatable(IList<TriangulationPoint> points)
        {
            if (points.Count < 3) throw new ArgumentException("List has fewer than 3 points", "points");

            // Lets do one sanity check that first and last point haven't got same position
            // It's something that often happens when importing polygon data from other formats
            if (points[0].Equals(points[points.Count - 1])) points.RemoveAt(points.Count - 1);

            this.points.AddRange(points);
        }

        /// <summary>
        /// Create a polygon from a list of at least 3 points with no duplicates.
        /// </summary>
        /// <param name="points">A list of unique points.</param>
        public Triangulatable(IEnumerable<TriangulationPoint> points) : this((points as IList<TriangulationPoint>) ?? points.ToArray()) { }

        /// <summary>
        /// Create a polygon from a list of at least 3 points with no duplicates.
        /// </summary>
        /// <param name="points">A list of unique points.</param>
        public Triangulatable(params TriangulationPoint[] points) : this((IList<TriangulationPoint>)points) { }

        public TriangulationMode TriangulationMode { get { return TriangulationMode.Polygon; } }

        public void AddSteinerPoint(TriangulationPoint point)
        {
            if (steinerPoints == null) steinerPoints = new List<TriangulationPoint>();
            steinerPoints.Add(point);
        }

        public void AddSteinerPoints(List<TriangulationPoint> sPoints)
        {
            if (steinerPoints == null) this.steinerPoints = new List<TriangulationPoint>();
            steinerPoints.AddRange(sPoints);
        }

        public void ClearSteinerPoints()
        {
            if (steinerPoints != null) steinerPoints.Clear();
        }

        /// <summary>
        /// Add a hole to the polygon.
        /// </summary>
        /// <param name="poly">A subtraction polygon fully contained inside this polygon.</param>
        public void AddHole(Triangulatable poly)
        {
            if (holes == null) holes = new List<Triangulatable>();
            holes.Add(poly);
            // XXX: tests could be made here to be sure it is fully inside
            //        addSubtraction( poly.getPoints() );
        }

        ///// <summary>
        ///// Inserts newPoint after point.
        ///// </summary>
        ///// <param name="point">The point to insert after in the polygon</param>
        ///// <param name="newPoint">The point to insert into the polygon</param>
        //public void InsertPointAfter(TriangulationPoint point, TriangulationPoint newPoint)
        //{
        //    // Validate that 
        //    int index = points.IndexOf(point);
        //    if (index == -1) throw new ArgumentException("Tried to insert a point into a Triangulatable after a point not belonging to the Triangulatable", "point");
        //    newPoint.Next = point.Next;
        //    newPoint.Previous = point;
        //    point.Next.Previous = newPoint;
        //    point.Next = newPoint;
        //    points.Insert(index + 1, newPoint);
        //}

        ///// <summary>
        ///// Inserts list (after last point in polygon?)
        ///// </summary>
        ///// <param name="list"></param>
        //public void AddPoints(IEnumerable<TriangulationPoint> list)
        //{
        //    TriangulationPoint first;
        //    foreach (TriangulationPoint p in list)
        //    {
        //        p.Previous = last;
        //        if (last != null)
        //        {
        //            p.Next = last.Next;
        //            last.Next = p;
        //        }
        //        last = p;
        //        points.Add(p);
        //    }
        //    first = (TriangulationPoint)points[0];
        //    last.Next = first;
        //    first.Previous = last;
        //}

        ///// <summary>
        ///// Adds a point after the last in the polygon.
        ///// </summary>
        ///// <param name="p">The point to add</param>
        //public void AddPoint(TriangulationPoint p)
        //{
        //    p.Previous = last;
        //    p.Next = last.Next;
        //    last.Next = p;
        //    points.Add(p);
        //}

        ///// <summary>
        ///// Removes a point from the polygon.
        ///// </summary>
        ///// <param name="p"></param>
        //public void RemovePoint(TriangulationPoint p)
        //{
        //    TriangulationPoint next, prev;

        //    next = p.Next;
        //    prev = p.Previous;
        //    prev.Next = next;
        //    next.Previous = prev;
        //    points.Remove(p);
        //}

        public IList<TriangulationPoint> Points { get { return points; } }
        public IList<DelaunayTriangle> Triangles { get { return triangles; } }
        public IList<Triangulatable> Holes { get { return holes; } }

        public void AddTriangle(DelaunayTriangle t)
        {
            triangles.Add(t);
        }

        public void AddTriangles(IEnumerable<DelaunayTriangle> list)
        {
            triangles.AddRange(list);
        }

        public void ClearTriangles()
        {
            if (triangles != null) triangles.Clear();
        }

        /// <summary>
        /// Creates constraints and populates the context with points
        /// </summary>
        /// <param name="tcx">The context</param>
        public void Prepare(TriangulationContext tcx)
        {
            if (triangles == null)
            {
                triangles = new List<DelaunayTriangle>(points.Count);
            }
            else
            {
                triangles.Clear();
            }

            // Outer constraints
            for (int i = 0; i < points.Count - 1; i++) tcx.NewConstraint(points[i], points[i + 1]);
            tcx.NewConstraint(points[0], points[points.Count - 1]);
            tcx.Points.AddRange(points);

            // Hole constraints
            if (holes != null)
            {
                foreach (Triangulatable p in holes)
                {
                    for (int i = 0; i < p.points.Count - 1; i++) tcx.NewConstraint(p.points[i], p.points[i + 1]);
                    tcx.NewConstraint(p.points[0], p.points[p.points.Count - 1]);
                    tcx.Points.AddRange(p.points);
                }
            }

            if (steinerPoints != null)
            {
                tcx.Points.AddRange(steinerPoints);
            }
        }

    }
}
