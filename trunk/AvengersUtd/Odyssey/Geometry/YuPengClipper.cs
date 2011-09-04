using System;
using System.Collections.Generic;

namespace AvengersUtd.Odyssey.Geometry
{
    internal enum PolyClipType
    {
        Intersect,
        Union,
        Difference
    }

    public enum PolyClipError
    {
        None,
        DegeneratedOutput,
        NonSimpleInput,
        BrokenResult
    }

    public static class YuPengClipper
    {
        private const double ClipperEpsilonSquared = 1.192092896e-07;

        public static List<Polygon> Union(Polygon polygon1, Polygon polygon2, out PolyClipError error)
        {
            return Execute(polygon1, polygon2, PolyClipType.Union, out error);
        }

        public static List<Polygon> Difference(Polygon polygon1, Polygon polygon2, out PolyClipError error)
        {
            return Execute(polygon1, polygon2, PolyClipType.Difference, out error);
        }

        public static List<Polygon> Intersect(Polygon polygon1, Polygon polygon2, out PolyClipError error)
        {
            return Execute(polygon1, polygon2, PolyClipType.Intersect, out error);
        }

        /// <summary>
        /// Implements "A new algorithm for Boolean operations on general polygons" 
        /// available here: http://liama.ia.ac.cn/wiki/_media/user:dong:dong_cg_05.pdf
        /// Merges two polygons, a subject and a clip with the specified operation. Polygons may not be 
        /// self-intersecting.
        /// 
        /// Warning: May yield incorrect results or even crash if polygons contain collinear points.
        /// </summary>
        /// <param name="subject">The subject polygon.</param>
        /// <param name="clip">The clip polygon, which is added, 
        /// substracted or intersected with the subject</param>
        /// <param name="clipType">The operation to be performed. Either
        /// Union, Difference or Intersection.</param>
        /// <param name="error">The error generated (if any)</param>
        /// <returns>A list of closed polygons, which make up the result of the clipping operation.
        /// Outer contours are ordered counter clockwise, holes are ordered clockwise.</returns>
        private static List<Polygon> Execute(Polygon subject, Polygon clip,
                                              PolyClipType clipType, out PolyClipError error)
        {
            //Debug.Assert(subject.IsSimple() && clip.IsSimple(), "Non simple input!", "Input polygons must be simple (cannot intersect themselves).");

            // Copy polygons
            Polygon slicedSubject;
            Polygon slicedClip;
            // Calculate the intersection and touch points between
            // subject and clip and add them to both
            CalculateIntersections(subject, clip, out slicedSubject, out slicedClip);

            // Translate polygons into upper right quadrant
            // as the algorithm depends on it
            Vector2D lbSubject = subject.GetCollisionBox().LowerBound;
            Vector2D lbClip = clip.GetCollisionBox().LowerBound;
            Vector2D translate;
            Vector2D.Min(ref lbSubject, ref lbClip, out translate);
            translate = Vector2D.One - translate;
            if (translate != Vector2D.Zero)
            {
                slicedSubject.Translate(ref translate);
                slicedClip.Translate(ref translate);
            }

            // Enforce counterclockwise contours
            slicedSubject.ForceCounterClockWise();
            slicedClip.ForceCounterClockWise();

            List<Edge> subjectSimplices;
            List<double> subjectCoeff;
            List<Edge> clipSimplices;
            List<double> clipCoeff;
            // Build simplical chains from the polygons and calculate the
            // the corresponding coefficients
            CalculateSimplicalChain(slicedSubject, out subjectCoeff, out subjectSimplices);
            CalculateSimplicalChain(slicedClip, out clipCoeff, out clipSimplices);

            List<Edge> resultSimplices;

            // Determine the characteristics function for all non-original edges
            // in subject and clip simplical chain and combine the edges contributing
            // to the result, depending on the clipType
            CalculateResultChain(subjectCoeff, subjectSimplices, clipCoeff, clipSimplices, clipType,
                                 out resultSimplices);

            List<Polygon> result;
            // Convert result chain back to polygon(s)
            error = BuildPolygonsFromChain(resultSimplices, out result);

            // Reverse the polygon translation from the beginning
            // and remove collinear points from output
            translate *= -1f;
            for (int i = 0; i < result.Count; ++i)
            {
                result[i].Translate(ref translate);
                SimplifyTools.CollinearSimplify(result[i].Vertices);
            }
            return result;
        }

        /// <summary>
        /// Calculates all intersections between two polygons.
        /// </summary>
        /// <param name="polygon1">The first polygon.</param>
        /// <param name="polygon2">The second polygon.</param>
        /// <param name="slicedPoly1">Returns the first polygon with added intersection points.</param>
        /// <param name="slicedPoly2">Returns the second polygon with added intersection points.</param>
        private static void CalculateIntersections(IPolygon polygon1, IPolygon polygon2,
                                                   out Polygon slicedPoly1, out Polygon slicedPoly2)
        {
            slicedPoly1 = new Polygon(polygon1.Vertices);
            slicedPoly2 = new Polygon(polygon2.Vertices);

            Vertices poly1Vs = polygon1.Vertices;
            Vertices poly2Vs = polygon2.Vertices;
            Vertices slicedPoly1Vs = slicedPoly1.Vertices;
            Vertices slicedPoly2Vs = slicedPoly2.Vertices;

            // Iterate through polygon1's edges
            for (int i = 0; i < poly1Vs.Count; i++)
            {
                // Get edge Vertices
                Vector2D a = poly1Vs[i];
                Vector2D b = poly1Vs[poly1Vs.NextIndex(i)];

                // Get intersections between this edge and polygon2
                for (int j = 0; j < poly2Vs.Count; j++)
                {
                    Vector2D c = poly2Vs[j];
                    Vector2D d = poly2Vs[poly2Vs.NextIndex(j)];

                    Vector2D intersectionPoint;
                    // Check if the edges intersect
                    if (Intersection.SegmentSegmentTest(new Segment(a,b), new Segment(c, d), out intersectionPoint))
                    {
                        // calculate alpha values for sorting multiple intersections points on a edge
                        double alpha;
                        // Insert intersection point into first polygon
                        alpha = GetAlpha(a, b, intersectionPoint);
                        if (alpha > 0f && alpha < 1f)
                        {
                            int index = slicedPoly1Vs.IndexOf(a) + 1;
                            while (index < slicedPoly1Vs.Count &&
                                   GetAlpha(a, b, slicedPoly1Vs[index]) <= alpha)
                            {
                                ++index;
                            }
                            slicedPoly1Vs.Insert(index, intersectionPoint);
                        }
                        // Insert intersection point into second polygon
                        alpha = GetAlpha(c, d, intersectionPoint);
                        if (alpha > 0f && alpha < 1f)
                        {
                            int index = slicedPoly2Vs.IndexOf(c) + 1;
                            while (index < slicedPoly2Vs.Count &&
                                   GetAlpha(c, d, slicedPoly2Vs[index]) <= alpha)
                            {
                                ++index;
                            }
                            slicedPoly2Vs.Insert(index, intersectionPoint);
                        }
                    }
                }
            }
            // Check for very small edges
            for (int i = 0; i < slicedPoly1Vs.Count; ++i)
            {
                int iNext = slicedPoly1Vs.NextIndex(i);
                //If they are closer than the distance remove vertex
                if ((slicedPoly1Vs[iNext] - slicedPoly1Vs[i]).LengthSquared() <= ClipperEpsilonSquared)
                {
                    slicedPoly1Vs.RemoveAt(i);
                    --i;
                }
            }
            for (int i = 0; i < slicedPoly2Vs.Count; ++i)
            {
                int iNext = slicedPoly2Vs.NextIndex(i);
                //If they are closer than the distance remove vertex
                if ((slicedPoly2Vs[iNext] - slicedPoly2Vs[i]).LengthSquared() <= ClipperEpsilonSquared)
                {
                    slicedPoly2Vs.RemoveAt(i);
                    --i;
                }
            }
        }

        /// <summary>
        /// Calculates the simplical chain corresponding to the input polygon.
        /// </summary>
        /// <remarks>Used by method <c>Execute()</c>.</remarks>
        private static void CalculateSimplicalChain(IPolygon poly, out List<double> coeff,
                                                    out List<Edge> simplicies)
        {
            simplicies = new List<Edge>();
            coeff = new List<double>();
            Vertices polyVertices = poly.Vertices;
            for (int i = 0; i < polyVertices.Count; ++i)
            {
                simplicies.Add(new Edge(polyVertices[i], polyVertices[polyVertices.NextIndex(i)]));
                coeff.Add(CalculateSimplexCoefficient(Vector2D.Zero, polyVertices[i], polyVertices[polyVertices.NextIndex(i)]));
            }
        }

        /// <summary>
        /// Calculates the characteristics function for all edges of
        /// the given simplical chains and builds the result chain.
        /// </summary>
        /// <remarks>Used by method <c>Execute()</c>.</remarks>
        private static void CalculateResultChain(List<double> poly1Coeff, List<Edge> poly1Simplicies,
                                                 List<double> poly2Coeff, List<Edge> poly2Simplicies,
                                                 PolyClipType clipType, out List<Edge> resultSimplices)
        {
            resultSimplices = new List<Edge>();

            for (int i = 0; i < poly1Simplicies.Count; ++i)
            {
                double edgeCharacter = 0;
                if (poly2Simplicies.Contains(poly1Simplicies[i]) ||
                    (poly2Simplicies.Contains(-poly1Simplicies[i]) && clipType == PolyClipType.Union))
                {
                    edgeCharacter = 1;
                }
                else
                {
                    for (int j = 0; j < poly2Simplicies.Count; ++j)
                    {
                        if (!poly2Simplicies.Contains(-poly1Simplicies[i]))
                        {
                            edgeCharacter += CalculateBeta(poly1Simplicies[i].GetCenter(),
                                                           poly2Simplicies[j], poly2Coeff[j]);
                        }
                    }
                }
                if (clipType == PolyClipType.Intersect)
                {
                    if (Math.Abs(edgeCharacter - 1) < ClipperEpsilonSquared)
                    {
                        resultSimplices.Add(poly1Simplicies[i]);
                    }
                }
                else
                {
                    if (Math.Abs(edgeCharacter - 0) < ClipperEpsilonSquared)
                    {
                        resultSimplices.Add(poly1Simplicies[i]);
                    }
                }
            }
            for (int i = 0; i < poly2Simplicies.Count; ++i)
            {
                if (!resultSimplices.Contains(poly2Simplicies[i]) &&
                    !resultSimplices.Contains(-poly2Simplicies[i]))
                {
                    double edgeCharacter = 0f;
                    if (poly1Simplicies.Contains(poly2Simplicies[i]) ||
                        (poly1Simplicies.Contains(-poly2Simplicies[i]) && clipType == PolyClipType.Union))
                    {
                        edgeCharacter = 1f;
                    }
                    else
                    {
                        for (int j = 0; j < poly1Simplicies.Count; ++j)
                        {
                            if (!poly1Simplicies.Contains(-poly2Simplicies[i]))
                            {
                                edgeCharacter += CalculateBeta(poly2Simplicies[i].GetCenter(),
                                                               poly1Simplicies[j], poly1Coeff[j]);
                            }
                        }
                    }
                    if (clipType == PolyClipType.Intersect || clipType == PolyClipType.Difference)
                    {
                        if (Math.Abs(edgeCharacter - 1) < ClipperEpsilonSquared)
                        {
                            resultSimplices.Add(-poly2Simplicies[i]);
                        }
                    }
                    else
                    {
                        if (Math.Abs(edgeCharacter) < ClipperEpsilonSquared)
                        {
                            resultSimplices.Add(poly2Simplicies[i]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the polygon(s) from the result simplical chain.
        /// </summary>
        /// <remarks>Used by method <c>Execute()</c>.</remarks>
        private static PolyClipError BuildPolygonsFromChain(IList<Edge> simplicies, out List<Polygon> result)
        {
            result = new List<Polygon>();
            PolyClipError errVal = PolyClipError.None;

            while (simplicies.Count > 0)
            {
                Vertices output = new Vertices {simplicies[0].EdgeStart, simplicies[0].EdgeEnd};

                simplicies.RemoveAt(0);
                bool closed = false;
                int index = 0;
                int count = simplicies.Count; // Needed to catch infinite loops
                while (!closed && simplicies.Count > 0)
                {
                    if (VectorEqual(output[output.Count - 1], simplicies[index].EdgeStart))
                    {
                        if (VectorEqual(simplicies[index].EdgeEnd, output[0]))
                        {
                            closed = true;
                        }
                        else
                        {
                            output.Add(simplicies[index].EdgeEnd);
                        }
                        simplicies.RemoveAt(index);
                        --index;
                    }
                    else if (VectorEqual(output[output.Count - 1], simplicies[index].EdgeEnd))
                    {
                        if (VectorEqual(simplicies[index].EdgeStart, output[0]))
                        {
                            closed = true;
                        }
                        else
                        {
                            output.Add(simplicies[index].EdgeStart);
                        }
                        simplicies.RemoveAt(index);
                        --index;
                    }
                    if (!closed)
                    {
                        if (++index == simplicies.Count)
                        {
                            if (count == simplicies.Count)
                            {
                                result = new List<Polygon>();
                                //Debug.WriteLine("Undefined error while building result polygon(s).");
                                return PolyClipError.BrokenResult;
                            }
                            index = 0;
                            count = simplicies.Count;
                        }
                    }
                }
                if (output.Count < 3)
                {
                    errVal = PolyClipError.DegeneratedOutput;
                    //Debug.WriteLine("Degenerated output polygon produced (Vertices < 3).");
                }
                result.Add(new Polygon(output));
            }
            return errVal;
        }

        /// <summary>
        /// Needed to calculate the characteristics function of a simplex.
        /// </summary>
        /// <remarks>Used by method <c>CalculateEdgeCharacter()</c>.</remarks>
        private static double CalculateBeta(Vector2D point, Edge e, double coefficient)
        {
            double result = 0;
            if (PointInSimplex(point, e))
            {
                result = coefficient;
            }
            if (PointOnLineSegment(Vector2D.Zero, e.EdgeStart, point) ||
                PointOnLineSegment(Vector2D.Zero, e.EdgeEnd, point))
            {
                result = .5*coefficient;
            }
            return result;
        }

        /// <summary>
        /// Needed for sorting multiple intersections points on the same edge.
        /// </summary>
        /// <remarks>Used by method <c>CalculateIntersections()</c>.</remarks>
        private static double GetAlpha(Vector2D start, Vector2D end, Vector2D point)
        {
            return (point - start).LengthSquared()/(end - start).LengthSquared();
        }

        /// <summary>
        /// Returns the coefficient of a simplex.
        /// </summary>
        /// <remarks>Used by method <c>CalculateSimplicalChain()</c>.</remarks>
        private static double CalculateSimplexCoefficient(Vector2D a, Vector2D b, Vector2D c)
        {
            double isLeft = MathHelper.Area(ref a, ref b, ref c);
            if (isLeft < 0)
            {
                return -1;
            }

            return isLeft > 0 ? 1 : 0;
        }

        /// <summary>
        /// Winding number test for a point in a simplex.
        /// </summary>
        /// <param name="point">The point to be tested.</param>
        /// <param name="edge">The edge that the point is tested against.</param>
        /// <returns>False if the winding number is even and the point is outside
        /// the simplex and True otherwise.</returns>
        private static bool PointInSimplex(Vector2D point, Edge edge)
        {
            Vertices polygon = new Vertices {Vector2D.Zero, edge.EdgeStart, edge.EdgeEnd};

            //return Intersection.PolygonPointTest(polygon, point);
            return (polygon.PointInPolygon(ref point) == 1);
        }

        /// <summary>
        /// Tests if a point lies on a line segment.
        /// </summary>
        /// <remarks>Used by method <c>CalculateBeta()</c>.</remarks>
        private static bool PointOnLineSegment(Vector2D start, Vector2D end, Vector2D point)
        {
            Vector2D segment = end - start;
            return Math.Abs(MathHelper.Area(ref start, ref end, ref point) - 0) < ClipperEpsilonSquared &&
                   Vector2D.Dot(point - start, segment) >= 0 &&
                   Vector2D.Dot(point - end, segment) <= 0;
        }

        private static bool VectorEqual(Vector2D vec1, Vector2D vec2)
        {
            return (vec2 - vec1).LengthSquared() <= ClipperEpsilonSquared;
        }

        #region Nested type: Edge

        /// <summary>Specifies an Edge. Edges are used to represent simplicies in simplical chains</summary>
        private sealed class Edge
        {
            public Edge(Vector2D edgeStart, Vector2D edgeEnd)
            {
                EdgeStart = edgeStart;
                EdgeEnd = edgeEnd;
            }

            public Vector2D EdgeStart { get; private set; }
            public Vector2D EdgeEnd { get; private set; }

            public Vector2D GetCenter()
            {
                return (EdgeStart + EdgeEnd)/2f;
            }

            public static Edge operator -(Edge e)
            {
                return new Edge(e.EdgeEnd, e.EdgeStart);
            }

            public override bool Equals(Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                return Equals(obj as Edge);
            }

            private bool Equals(Edge e)
            {
                // If parameter is null return false:
                if (e == null)
                {
                    return false;
                }

                // Return true if the fields match
                return VectorEqual(EdgeStart, e.EdgeStart) && VectorEqual(EdgeEnd, e.EdgeEnd);
            }

            public override int GetHashCode()
            {
                return EdgeStart.GetHashCode() ^ EdgeEnd.GetHashCode();
            }
        }

        #endregion
    }

}
