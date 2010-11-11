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
            WAList2 list = new WAList2 {Head = pointList[0], Tail = pointList[pointList.Count - 1]};

            list.Head.PrevVertex = list.Tail;
            list.Tail.NextVertex = list.Head;

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
                        cpList.Add(clipPoint);
                        
                    }

                    t = q;
                }

                s = p;
            }
        }

        void ComputeIntersectedPolygon()
        {
            WAPoint currentPoint = spList.Head;
            Polygon clippedPolygon = new Polygon();
            do
            {
                currentPoint = spList.FindNextIntersectionPoint();

                if (currentPoint.IsEntryPoint && currentPoint.IsIntersection && !currentPoint.Visited)
                {
                    WAPoint clipPoint = currentPoint;
                    do
                    {
                        
                        do
                        {
                            clipPoint.Visited = true;
                            clippedPolygon.Vertices.Add(clipPoint.Vertex);
                            clipPoint = clipPoint.NextVertex;
                        } while (!clipPoint.IsIntersection) ;

                        clipPoint.Visited = clipPoint.JumpLink.Visited = true;
                        clipPoint = clipPoint.JumpLink;
                        

                    } while (clipPoint.Index != currentPoint.Index);
                }

                currentPoint.Visited = true;
                currentPoint = currentPoint.NextVertex;

            } while (currentPoint.Index != spList.Head.Index);

            Result = clippedPolygon;
        }
    }
}
