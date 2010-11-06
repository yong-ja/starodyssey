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

        public void AddSp(WAPoint Vector2D)
        {
            if (countSp == 0)
            {
                HeadSp = TailSp = Vector2D;
                HeadSp.NextVertex = HeadSp.PrevVertex = Vector2D;
                TailSp.NextVertex = TailSp.PrevVertex = Vector2D;
                current = HeadSp;
                countSp++;
            }
            else if (countSp == 1)
            {
                if (!HeadSp.Equals(Vector2D))
                {
                    HeadSp.NextVertex = HeadSp.PrevVertex = Vector2D;
                    Vector2D.NextVertex = Vector2D.PrevVertex = HeadSp;
                    TailSp = Vector2D;
                    countSp++;
                }
                else
                {
                    if (Vector2D.IsIntersection)
                    {
                        Vector2D.PrevVertex = TailSp;
                        Vector2D.NextVertex = TailSp;

                        TailSp.NextVertex = Vector2D;
                        TailSp.PrevVertex = Vector2D;
                        HeadSp = Vector2D;
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

                    if (Vector2D.Between(start, end))
                    {
                        if (!start.Equals(Vector2D) && !end.Equals(Vector2D))
                        {
                            Vector2D.NextVertex = end;
                            Vector2D.PrevVertex = start;
                            start.NextVertex = Vector2D;
                            end.PrevVertex = Vector2D;
                            countSp++;
                            //Okay, inserted and exit from while.
                            break;
                        }
                        else
                        {
                            //Clone
                            if (Vector2D.IsIntersection)
                            {
                                if (start.Equals(Vector2D))
                                {
                                    Vector2D.PrevVertex = start.PrevVertex;
                                    Vector2D.NextVertex = start.NextVertex;

                                    start.PrevVertex.NextVertex = Vector2D;
                                    start.NextVertex.PrevVertex = Vector2D;

                                    if (HeadSp.Equals(Vector2D))
                                    {
                                        HeadSp = Vector2D;
                                        current = HeadSp;
                                    }
                                    if (TailSp.Equals(Vector2D))
                                        TailSp = Vector2D;

                                    start = Vector2D;
                                }
                                else
                                {
                                    Vector2D.PrevVertex = end.PrevVertex;
                                    Vector2D.NextVertex = end.NextVertex;

                                    end.PrevVertex.NextVertex = Vector2D;
                                    end.NextVertex.PrevVertex = Vector2D;

                                    if (HeadSp.Equals(Vector2D))
                                        HeadSp = Vector2D;
                                    if (TailSp.Equals(Vector2D))
                                        TailSp = Vector2D;

                                    end = Vector2D;
                                }
                            }
                            return;
                        }
                    }
                    currItem = currItem.NextVertex;
                }

                if (countSp == oldCounter)
                {
                    Vector2D.NextVertex = HeadSp;
                    Vector2D.PrevVertex = TailSp;
                    TailSp.NextVertex = Vector2D;
                    HeadSp.PrevVertex = Vector2D;
                    TailSp = Vector2D;
                    countSp++;
                }
            }
        }

        public void AddCP(WAPoint Vector2D)
        {
            if (countCp == 0)
            {
                HeadCp = TailCp = Vector2D;
                HeadCp.NextVertex = HeadCp.PrevVertex = Vector2D;
                TailCp.NextVertex = TailCp.PrevVertex = Vector2D;
                countCp++;
            }
            else if (countCp == 1)
            {
                if (!HeadCp.Equals(Vector2D))
                {
                    HeadCp.NextVertex = HeadCp.PrevVertex = Vector2D;
                    Vector2D.NextVertex = Vector2D.PrevVertex = HeadCp;
                    TailCp = Vector2D;
                    countCp++;
                }
                else
                {
                    if (Vector2D.IsIntersection)
                    {
                        Vector2D.PrevVertex = TailCp;
                        Vector2D.NextVertex = TailCp;

                        TailCp.NextVertex = Vector2D;
                        TailCp.PrevVertex = Vector2D;
                        HeadCp = Vector2D;
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

                    if (Vector2D.Between(start, end))
                    {
                        if (!start.Equals(Vector2D) && !end.Equals(Vector2D))
                        {
                            Vector2D.NextVertex = end;
                            Vector2D.PrevVertex = start;
                            start.NextVertex = Vector2D;
                            end.PrevVertex = Vector2D;
                            countCp++;
                            //Okay, inserted and exit from while.
                            break;
                        }
                        else
                        {
                            //Clone
                            if (Vector2D.IsIntersection)
                            {
                                if (start.Equals(Vector2D))
                                {
                                    Vector2D.PrevVertex = start.PrevVertex;
                                    Vector2D.NextVertex = start.NextVertex;

                                    start.PrevVertex.NextVertex = Vector2D;
                                    start.NextVertex.PrevVertex = Vector2D;

                                    if (HeadCp.Equals(Vector2D))
                                    {
                                        HeadCp = Vector2D;
                                        current = HeadCp;
                                    }
                                    if (TailCp.Equals(Vector2D))
                                        TailCp = Vector2D;

                                    start = Vector2D;
                                }
                                else
                                {
                                    Vector2D.PrevVertex = end.PrevVertex;
                                    Vector2D.NextVertex = end.NextVertex;

                                    end.PrevVertex.NextVertex = Vector2D;
                                    end.NextVertex.PrevVertex = Vector2D;

                                    if (HeadCp.Equals(Vector2D))
                                        HeadCp = Vector2D;
                                    if (TailCp.Equals(Vector2D))
                                        TailCp = Vector2D;

                                    end = Vector2D;
                                }
                            }
                            return;
                        }
                    }
                    currItem = currItem.NextVertex;
                }

                if (countCp == oldCounter)
                {
                    Vector2D.NextVertex = HeadCp;
                    Vector2D.PrevVertex = TailCp;
                    TailCp.NextVertex = Vector2D;
                    HeadCp.PrevVertex = Vector2D;
                    TailCp = Vector2D;
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

        public void SetEntryPoints(PointCollection subjectPoly, PointCollection clippingPoly)
        {
            WAPoint curr = HeadCp;
            for (int i = 0; i < countCp; i++)
            {
                if (curr.IsIntersection)
                {
                    if (curr.NextVertex.IsIntersection)
                    {
                        //Check if a subject polygon edge lies on clip poly edge
                        Vector2D A = curr.JumpLink.PrevVertex.Vertex;
                        Vector2D B = curr.NextVertex.Vertex;
                        Vector2D C = curr.Vertex;

                        bool test1 = A.X == B.X && B.X == C.X;
                        bool test2 = A.Y == B.Y && B.Y == C.Y;

                        bool result1 = test1 || test2;

                        A = curr.JumpLink.NextVertex.Vertex;

                        test1 = A.X == B.X && B.X == C.X;
                        test2 = A.Y == B.Y && B.Y == C.Y;

                        bool result2 = test1 || test2;

                        bool result = result1 || result2;

                        if (result)
                        {
                            //Check if ext or internal edge
                            if (
                                !(Geometry.PolygonHitTest(subjectPoly, curr.PrevVertex.Vertex) &&
                                  Geometry.PolygonHitTest(subjectPoly, curr.NextVertex.NextVertex.Vertex)))
                            {
                                //coincident egde a clip polygon edge
                                curr.IsIntersection = false;
                                curr.JumpLink.IsIntersection = false;
                            }
                            else
                            {
                                //Set New Entry Vector2D
                                curr.IsEntryPoint = true;
                                curr.JumpLink.IsEntryPoint = true;
                                CounterEntryPoint++;
                            }
                        }
                        else
                        {
                            //Entry Vector2D
                            curr.IsEntryPoint = true;
                            curr.JumpLink.IsEntryPoint = true;
                            CounterEntryPoint++;
                        }
                    }
                    else
                    {
                        bool inside = Geometry.PolygonHitTest(subjectPoly, curr.NextVertex.Vertex);
                        if (inside)
                        {
                            //Entry Vector2D
                            curr.IsEntryPoint = true;
                            curr.JumpLink.IsEntryPoint = true;
                            CounterEntryPoint++;
                        }
                        else
                        {
                            if (!curr.PrevVertex.IsIntersection)
                            {
                                bool discr = Geometry.PolygonHitTest(subjectPoly, curr.PrevVertex.Vertex);
                                if (!discr)
                                {
                                    //coincident wia a clip polygon edge
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

        public static int ComputeIntersectArea(Polygon subjectPoly, Polygon clippingPoly)
        {
            WAList list = new WAList();
            VerticesCollection intersectedPoly = new VerticesCollection();

            for (int i = 0; i < subjectPoly.Count; i++)
            {
                Vector2D startSP = subjectPoly[i];
                Vector2D endSP = subjectPoly[(i + 1)%subjectPoly.Count];

                WAPoint spPoint = new WAPoint
                                  {Vertex = startSP, IsIntersection = false, IsActive = false, IsEntryPoint = false};

                list.AddSp(spPoint);

                for (int j = 0; j < clippingPoly.Count; j++)
                {
                    Vector2D startCP = clippingPoly[j];
                    Vector2D endCP = clippingPoly[(j + 1)%clippingPoly.Count];

                    WAPoint cpPoint = new WAPoint
                                      {Vertex = startCP, IsIntersection = false, IsActive = false, IsEntryPoint = false};
                    if (i == 0)
                        list.AddCP(cpPoint);

                    Vector2D intersectSPCP;
                    if (Geometry.LineHitLine(startSP, endSP, startCP, endCP, out intersectSPCP))
                    {
                        Vector2D p = new Vector2D(intersectSPCP.X, intersectSPCP.Y);

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
                        list.AddCP(cpIntersectionPoint);
                    }
                }
            }

            list.SetEntryPoints(subjectPoly, clippingPoly);

            if (list.CounterEntryPoint == 0)
            {
                bool ace = false;
                int c = 0;
                foreach (Vector2D p in subjectPoly)
                {
                    if (Geometry.PolygonHitTest(clippingPoly, p))
                    {
                        ace = true;
                        c++;
                    }
                }
                if (ace)
                    return 0;
                else
                    return -1;
            }
            else
            {
                int count = 0;
                double area = 0.0;
                WAPoint curr = list.MoveNext();
                while (count < list.CounterEntryPoint)
                {
                    PointCollection points = new PointCollection();
                    //Find firs valid intersection
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
                            //Found exit Vector2D
                            list.Jump();
                            cp.IsActive = false;
                            cp.JumpLink.IsActive = false;
                        }
                        cp = list.MoveNext();
                    } while (!ep.Equals(cp));

                    list.ResetJump();
                    area += Geometry.PolygonArea(points);

                    intersectedPoly.Add(points);
                }

                return (int) area;
            }
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