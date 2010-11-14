using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class WAClipping
    {
        private Polygon subject;
        private Polygon clip;
        WAList2 spList;
        WAList2 cpList;
        public Polygon Result { get; private set; }

        private WAClipping(Polygon subject, Polygon clip)
        {
            this.subject = subject;
            this.clip = clip;
            //subjectPointList = FillPointList(subject);
            //clipPointList = FillPointList(clip);
        }

        static WAList2 FillPointList(IPolygon polygon)
        {
            List<WAPoint> pointList = polygon.Vertices.Select
                (vertex => new WAPoint
                           {
                               Vertex = vertex,
                               Visited = false,
                               IsIntersection = false,
                               IsEntryPoint = false,
                           }).ToList();
            WAList2 list = new WAList2 {First = pointList[0], Last = pointList[pointList.Count - 1]};

            list.First.PrevVertex = list.Last;
            list.Last.NextVertex = list.First;

            for (int i = 1; i < pointList.Count - 1; i++)
            {
                WAPoint prevPoint = pointList[i - 1];
                WAPoint currentPoint = pointList[i];
                WAPoint nextPoint = pointList[i + 1];

                prevPoint.NextVertex = currentPoint;
                currentPoint.NextVertex = nextPoint;
                currentPoint.PrevVertex = prevPoint;
                nextPoint.PrevVertex = currentPoint;
            }

            return list;
        }

        public static Polygon PerformClipping(Polygon subject, Polygon clip)
        {
            WAClipping clipper = new WAClipping(subject, clip);
            clipper.FindIntersections();
            clipper.HandleSpecialCases();
            clipper.ComputeIntersectedPolygon();

            return clipper.Result;
        }

        void FindIntersections()
        {
            int counter = 0;
            Vector2D s = subject.Vertices[subject.Vertices.Count -1];
            spList = new WAList2();
            cpList = new WAList2();
            for (int i = 0; i < subject.Vertices.Count; i++)
            {
                Vector2D p = subject.Vertices[i];
                Segment subjectEdge = new Segment(s, p);

                WAPoint sPoint = new WAPoint
                                 {
                                     Vertex = s,
                                     Visited = false,
                                     IsEntryPoint = false,
                                     IsIntersection = false,
                                     Index = counter
                                 };
                spList.Add(sPoint);
                counter++;

                Vector2D t = clip.Vertices[clip.Vertices.Count-1];

                for (int j = 0; j < clip.Vertices.Count; j++ )
                {
                    Vector2D q = clip.Vertices[j];
                    Segment clipEdge = new Segment(t, q);

                    Vector2D intersectionPoint;
                    bool inboundIntersection;

                    if (i == 0)
                    {
                        WAPoint cPoint = new WAPoint
                                         {
                                             Vertex = t,
                                             Visited = false,
                                             IsEntryPoint = false,
                                             IsIntersection = false,
                                             Index = counter
                                         };
                        counter++;
                        cpList.Add(cPoint);
                    }

                    if (Intersection.SegmentSegmentTest(subjectEdge, clipEdge, out intersectionPoint, out inboundIntersection))
                    {
                        WAPoint subjectPoint = new WAPoint
                                               {
                                                   Vertex = intersectionPoint,
                                                   Visited = false,
                                                   IsEntryPoint = inboundIntersection,
                                                   IsIntersection = true,
                                                   Index = counter
                                               };

                        WAPoint clipPoint = subjectPoint.Clone();

                        counter++;

                        // Insert point in both list
                        subjectPoint.JumpLink = clipPoint;
                        clipPoint.JumpLink = subjectPoint;
                        spList.Add(subjectPoint);
                        int cIndex1 = cpList.IndexOf(t);
                        cpList.AddAfter(cIndex1, clipPoint);
                        
                    }

                    t = q;
                }

                s = p;
            }
        }

        void HandleSpecialCases()
        {
            WAPoint currentPoint = spList.First;

            do
            {
                // Points are coincident
                Vector2D sA = currentPoint.Vertex;
                Vector2D sB = currentPoint.NextVertex.Vertex;

                sA.Round();
                sB.Round();

                if (sA == sB)
                {
                    Vector2D sC = currentPoint.Forward(2).Vertex;
                    sC.Round();
                    if (sB == sC)
                    {
                        // v1 == v2 == v3
                        Vector2D sD = currentPoint.Forward(3).Vertex;

                        Vector2D sA0 = currentPoint.PrevVertex.Vertex;
                        Segment sA0sD = new Segment(sA0, sD);

                        WAPoint cIntersection = currentPoint.NextVertex.JumpLink;

                        Vector2D cA = cIntersection.PrevVertex.Vertex;
                        Vector2D cB = sB;
                        Vector2D cC = cIntersection.NextVertex.Vertex;

                        Segment cAcC = new Segment(cA,cC);
                        bool intersection = Intersection.SegmentSegmentTest(sA0sD, cAcC);

                        if (!intersection)
                        {
                            spList.Remove(currentPoint.NextVertex.Index);
                            cpList.Remove(currentPoint.NextVertex.Index);
                        }
                    }
                }
               currentPoint = currentPoint.NextVertex;
            } while (currentPoint.Index != spList.First.Index);
        }

        void ComputeIntersectedPolygon()
        {
            int counter = 0;
            WAPoint currentPoint = spList.FindNextIntersectionPoint(spList.First);
            Polygon clippedPolygon = new Polygon();
            
            while (currentPoint.Index != spList.First.Index)
            {
                if (!currentPoint.Visited)
                {
                    clippedPolygon.Vertices.Add(currentPoint.Vertex);
                    counter++;
                    currentPoint.Visited = true;

                    WAPoint clipPoint = currentPoint.NextVertex;
                    while (clipPoint.Index != currentPoint.Index)
                    {
                        clippedPolygon.Vertices.Add(clipPoint.Vertex);
                        clipPoint.Visited = true;
                        counter++;

                        if (clipPoint.IsIntersection)
                        {
                            clipPoint.JumpLink.Visited = true;
                            clipPoint = clipPoint.JumpLink.NextVertex;
                        }
                        else
                            clipPoint = clipPoint.NextVertex;
                    }
                }

                currentPoint = spList.FindNextIntersectionPoint(currentPoint.NextVertex);
            }

            Result = clippedPolygon;
        }
    }
}
