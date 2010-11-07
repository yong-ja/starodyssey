#region #Disclaimer
// Copyright (c) 2008 Davide Carfì
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace AvengersUtd.Odyssey.Geometry
{
    // The follows classes implements Weiller-Atherton alghorithm a concave clipping regions
    // For more details view "Procedural Elements for Computer Graphics" David F. Rogers (pp.179)
    public class WAList
    {
        private int countCp;
        private int countSp;
        private WAPoint current;

        private bool jumpCp;

        public WAList()
        {
            countSp = 0;
            countCp = 0;
            CounterEntryPoint = 0;
            jumpCp = false;
        }

        public WAPoint HeadSp { get; set; }
        public WAPoint TailSp { get; set; }

        public WAPoint HeadCp { get; set; }
        public WAPoint TailCp { get; set; }

        public int CounterEntryPoint { get; set; }

        public void AddSp(WAPoint point)
        {
            if (countSp == 0)
            {
                HeadSp = TailSp = point;
                HeadSp.NextVertex = HeadSp.PrevVertex = point;
                TailSp.NextVertex = TailSp.PrevVertex = point;
                current = HeadSp;
                countSp++;
            }
            else if (countSp == 1)
            {
                if (!HeadSp.Equals(point))
                {
                    HeadSp.NextVertex = HeadSp.PrevVertex = point;
                    point.NextVertex = point.PrevVertex = HeadSp;
                    TailSp = point;
                    countSp++;
                }
                else
                {
                    if (point.IsIntersection)
                    {
                        point.PrevVertex = TailSp;
                        point.NextVertex = TailSp;

                        TailSp.NextVertex = point;
                        TailSp.PrevVertex = point;
                        HeadSp = point;
                        current = HeadSp;
                    }
                }
            }
            else
            {
                WAPoint currItem = HeadSp;
                int oldCounter = countSp;

                while (!currItem.Vertex.Equals(TailSp.Vertex))
                {
                    WAPoint start = currItem;
                    WAPoint end = currItem.NextVertex;

                    if (point.Between(start, end))
                    {
                        if (!start.Equals(point) && !end.Equals(point))
                        {
                            point.NextVertex = end;
                            point.PrevVertex = start;
                            start.NextVertex = point;
                            end.PrevVertex = point;
                            countSp++;
                            //Okay, inserted and exit from while.
                            break;
                        }
                        else
                        {
                            //Clone
                            if (point.IsIntersection)
                            {
                                if (start.Equals(point))
                                {
                                    point.PrevVertex = start.PrevVertex;
                                    point.NextVertex = start.NextVertex;

                                    start.PrevVertex.NextVertex = point;
                                    start.NextVertex.PrevVertex = point;

                                    if (HeadSp.Equals(point))
                                    {
                                        HeadSp = point;
                                        current = HeadSp;
                                    }
                                    if (TailSp.Equals(point))
                                        TailSp = point;

                                    start = point;
                                }
                                else
                                {
                                    point.PrevVertex = end.PrevVertex;
                                    point.NextVertex = end.NextVertex;

                                    end.PrevVertex.NextVertex = point;
                                    end.NextVertex.PrevVertex = point;

                                    if (HeadSp.Equals(point))
                                        HeadSp = point;
                                    if (TailSp.Equals(point))
                                        TailSp = point;

                                    end = point;
                                }
                            }
                            return;
                        }
                    }
                    currItem = currItem.NextVertex;
                }

                if (countSp == oldCounter)
                {
                    point.NextVertex = HeadSp;
                    point.PrevVertex = TailSp;
                    TailSp.NextVertex = point;
                    HeadSp.PrevVertex = point;
                    TailSp = point;
                    countSp++;
                }
            }
        }

        public void AddCp(WAPoint point)
        {
            if (countCp == 0)
            {
                HeadCp = TailCp = point;
                HeadCp.NextVertex = HeadCp.PrevVertex = point;
                TailCp.NextVertex = TailCp.PrevVertex = point;
                countCp++;
            }
            else if (countCp == 1)
            {
                if (!HeadCp.Equals(point))
                {
                    HeadCp.NextVertex = HeadCp.PrevVertex = point;
                    point.NextVertex = point.PrevVertex = HeadCp;
                    TailCp = point;
                    countCp++;
                }
                else
                {
                    if (point.IsIntersection)
                    {
                        point.PrevVertex = TailCp;
                        point.NextVertex = TailCp;

                        TailCp.NextVertex = point;
                        TailCp.PrevVertex = point;
                        HeadCp = point;
                        current = HeadCp;
                    }
                }
            }
            else
            {
                WAPoint currItem = HeadCp;
                int oldCounter = countCp;

                while (!currItem.Vertex.Equals(TailCp.Vertex))
                {
                    WAPoint start = currItem;
                    WAPoint end = currItem.NextVertex;

                    if (point.Between(start, end))
                    {
                        if (!start.Equals(point) && !end.Equals(point))
                        {
                            point.NextVertex = end;
                            point.PrevVertex = start;
                            start.NextVertex = point;
                            end.PrevVertex = point;
                            countCp++;
                            //Okay, inserted and exit from while.
                            break;
                        }
                        else
                        {
                            //Clone
                            if (point.IsIntersection)
                            {
                                if (start.Equals(point))
                                {
                                    point.PrevVertex = start.PrevVertex;
                                    point.NextVertex = start.NextVertex;

                                    start.PrevVertex.NextVertex = point;
                                    start.NextVertex.PrevVertex = point;

                                    if (HeadCp.Equals(point))
                                    {
                                        HeadCp = point;
                                        current = HeadCp;
                                    }
                                    if (TailCp.Equals(point))
                                        TailCp = point;

                                    start = point;
                                }
                                else
                                {
                                    point.PrevVertex = end.PrevVertex;
                                    point.NextVertex = end.NextVertex;

                                    end.PrevVertex.NextVertex = point;
                                    end.NextVertex.PrevVertex = point;

                                    if (HeadCp.Equals(point))
                                        HeadCp = point;
                                    if (TailCp.Equals(point))
                                        TailCp = point;

                                    end = point;
                                }
                            }
                            return;
                        }
                    }
                    currItem = currItem.NextVertex;
                }

                if (countCp == oldCounter)
                {
                    point.NextVertex = HeadCp;
                    point.PrevVertex = TailCp;
                    TailCp.NextVertex = point;
                    HeadCp.PrevVertex = point;
                    TailCp = point;
                    countCp++;
                }
            }
        }

        public WAPoint MoveNext()
        {
            WAPoint toReturn = current;

            if (jumpCp)
                current = current.PrevVertex;
            else
                current = current.NextVertex;

            return toReturn;
        }

        public void SetEntryPoints(Polygon subjectPoly, Polygon clippingPoly)
        {
            WAPoint curr = HeadCp;
            for (int i = 0; i < countCp; i++)
            {
                if (curr.IsIntersection)
                {
                    if (curr.NextVertex.IsIntersection)
                    {
                        //Check if a subject polygon edge lies on clip poly edge
                        Vector2D a = curr.JumpLink.PrevVertex.Vertex;
                        Vector2D b = curr.NextVertex.Vertex;
                        Vector2D c = curr.Vertex;

                        bool test1 = Math.Abs(a.X - b.X) < MathHelper.EpsilonD &&
                                     Math.Abs(b.X - c.X) < MathHelper.EpsilonD;

                        bool test2 = Math.Abs(a.Y - b.Y) < MathHelper.EpsilonD &&
                                     Math.Abs(b.Y - c.Y) < MathHelper.EpsilonD;

                        bool result1 = test1 || test2;

                        a = curr.JumpLink.NextVertex.Vertex;

                        test1 = Math.Abs(a.X - b.X) < MathHelper.EpsilonD &&
                                     Math.Abs(b.X - c.X) < MathHelper.EpsilonD;

                        test2 = Math.Abs(a.Y - b.Y) < MathHelper.EpsilonD &&
                                     Math.Abs(b.Y - c.Y) < MathHelper.EpsilonD;

                        bool result2 = test1 || test2;

                        bool result = result1 || result2;

                        if (result)
                        {
                            //Check if ext or internal edge
                            if (!subjectPoly.IsPointInside(curr.PrevVertex.Vertex) &&
                                subjectPoly.IsPointInside(curr.NextVertex.NextVertex.Vertex))
                            {
                                //coincident egde a clip polygon edge
                                curr.IsIntersection = false;
                                curr.JumpLink.IsIntersection = false;
                            }
                            else
                            {
                                //Set New Entry point
                                curr.IsEntryPoint = true;
                                curr.JumpLink.IsEntryPoint = true;
                                CounterEntryPoint++;
                            }
                        }
                        else
                        {
                            //Entry point
                            curr.IsEntryPoint = true;
                            curr.JumpLink.IsEntryPoint = true;
                            CounterEntryPoint++;
                        }
                    }
                    else
                    {
                        
                        if (subjectPoly.IsPointInside(curr.NextVertex.Vertex))
                        {
                            //Entry point
                            curr.IsEntryPoint = true;
                            curr.JumpLink.IsEntryPoint = true;
                            CounterEntryPoint++;
                        }
                        else
                        {
                            if (!curr.PrevVertex.IsIntersection)
                            {
                                if (!subjectPoly.IsPointInside(curr.PrevVertex.Vertex))
                                {
                                    //coincident via a clip polygon edge
                                    curr.IsIntersection = false;
                                    curr.JumpLink.IsIntersection = false;
                                }
                            }
                        }
                    }
                }
                curr = curr.NextVertex;
            }
        }

        public void ResetJump()
        {
            jumpCp = !jumpCp;

            if (jumpCp)
            {
                current = current.PrevVertex.JumpLink;
            }
            else
            {
                current = current.NextVertex.JumpLink;
            }
        }

        public void Jump()
        {
            jumpCp = !jumpCp;

            if (jumpCp)
            {
                current = current.PrevVertex.JumpLink;
                current = current.PrevVertex;
            }
            else
            {
                current = current.NextVertex.JumpLink;
                current = current.NextVertex;
            }
        }

        public static bool LineHitLine(Vector2D p1A, Vector2D p1B, Vector2D p2A, Vector2D p2B, out Vector2D intersect)
        {
            double denom = ((p2B.Y - p2A.Y) * (p1B.X - p1A.X)) - ((p2B.X - p2A.X) * (p1B.Y - p1A.Y));

            double nume_a = ((p2B.X - p2A.X) * (p1A.Y - p2A.Y)) - ((p2B.Y - p2A.Y) * (p1A.X - p2A.X));

            double nume_b = ((p1B.X - p1A.X) * (p1A.Y - p2A.Y)) - ((p1B.Y - p1A.Y) * (p1A.X - p2A.X));

            intersect = new Vector2D(0, 0);

            if (denom == 0.0)
            {
                return false;
            }
            else
            {
                double ua = nume_a / denom;
                double ub = nume_b / denom;

                if (ua >= 0.0 && ua <= 1.0 && ub >= 0.0 && ub <= 1.0)
                {
                    // Get the intersection point.
                    intersect.X = p1A.X + ua * (p1B.X - p1A.X);
                    intersect.Y = p1A.Y + ua * (p1B.Y - p1A.Y);

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static Polygon ComputeIntersectArea(Polygon subjectPoly, Polygon clippingPoly)
        {
            WAList list = new WAList();
            VerticesCollection intersectedPoints = new VerticesCollection();
            for (int i = 0; i < subjectPoly.VerticesCount; i++)
            {
                Vector2D startSp = subjectPoly[i];
                Vector2D endSp = subjectPoly[(i + 1)%subjectPoly.VerticesCount];

                WAPoint spPoint = new WAPoint
                                  {Vertex = startSp, IsIntersection = false, IsActive = false, IsEntryPoint = false};

                list.AddSp(spPoint);

                for (int j = 0; j < clippingPoly.VerticesCount; j++)
                {
                    Vector2D startCp = clippingPoly[j];
                    Vector2D endCp = clippingPoly[(j + 1)%clippingPoly.VerticesCount];

                    WAPoint cpPoint = new WAPoint
                                      {Vertex = startCp, IsIntersection = false, IsActive = false, IsEntryPoint = false};
                    if (i == 0)
                        list.AddCp(cpPoint);

                    Vector2D intersectSpcp;
                    Segment startSpToendSp = new Segment(startSp, endSp);
                    Segment startCpToendCp = new Segment(startCp, endCp);

                    //if (!Intersection.LineLineTest(startSpToendSp, startCpToendCp, out intersectSpcp)) continue;
                    if (!Intersection.SegmentSegmentIntersection(startSpToendSp, startCpToendCp, out intersectSpcp)) continue;

                    Vector2D p = new Vector2D(intersectSpcp.X, intersectSpcp.Y);

                    WAPoint spIntersectionPoint = new WAPoint
                                                  {
                                                      Vertex = p,
                                                      IsIntersection = true,
                                                      IsActive = true,
                                                      IsEntryPoint = false
                                                  };
                    WAPoint cpIntersectionPoint = new WAPoint
                                                  {
                                                      Vertex = p,
                                                      IsIntersection = true,
                                                      IsActive = true,
                                                      IsEntryPoint = false
                                                  };

                    spIntersectionPoint.JumpLink = cpIntersectionPoint;
                    cpIntersectionPoint.JumpLink = spIntersectionPoint;

                    list.AddSp(spIntersectionPoint);
                    list.AddCp(cpIntersectionPoint);
                }
            }

            list.SetEntryPoints(subjectPoly, clippingPoly);

            if (list.CounterEntryPoint == 0)
            {
                return null;
                //bool ace = false;
                //int c = 0;
                //foreach (Vector2D p in subjectPoly)
                //{
                //    if (!clippingPoly.IsPointInside(p)) continue;

                //    ace = true;
                //    c++;
                //}
                //if (ace)
                //    return 0;
                //else
                //    return -1;
            }
            int count = 0;
            WAPoint curr = list.MoveNext();
            while (count < list.CounterEntryPoint)
            {
                VerticesCollection points = new VerticesCollection();
                //Find first valid intersection
                while (!(curr.IsIntersection && curr.IsActive && curr.IsEntryPoint))
                {
                    curr = list.MoveNext();
                }
                WAPoint ep = curr;
                ep.IsActive = false;
                ep.JumpLink.IsActive = false;
                count++;
                WAPoint cp = ep;

                do
                {
                    points.Add(cp.Vertex);
                    if (cp.IsIntersection && cp.IsActive)
                    {
                        if (cp.IsEntryPoint)
                            count++;
                        //Found exit point
                        list.Jump();
                        cp.IsActive = false;
                        cp.JumpLink.IsActive = false;
                    }
                    cp = list.MoveNext();
                } while (!ep.Equals(cp));

                list.ResetJump();

                intersectedPoints.AddRange(points);
            }

            return new Polygon(intersectedPoints);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("SP[");
            WAPoint iter = HeadSp;
            if (countSp > 0)
            {
                do
                {
                    sb.Append(iter + ",\n ");
                    iter = iter.NextVertex;
                } while (!iter.Equals(HeadSp));
            }
            sb.Append("]\n");

            sb.Append("CP[");
            iter = HeadCp;
            if (countCp > 0)
            {
                do
                {
                    sb.Append(iter + ",\n ");
                    iter = iter.NextVertex;
                } while (!iter.Equals(HeadCp));
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}