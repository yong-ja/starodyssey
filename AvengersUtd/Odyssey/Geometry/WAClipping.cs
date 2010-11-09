using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class WAClipping
    {
        WAList2 subjectPointList;
        WAList2 clipPointList;

        private WAClipping(IPolygon subject, IPolygon clip)
        {
            subjectPointList = FillPointList(subject);
            clipPointList = FillPointList(clip);
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

        public static void PerformClipping(Polygon subject, Polygon clip)
        {
            WAClipping clipper = new WAClipping(subject, clip);
            clipper.FindIntersections();
        }

        void FindIntersections(Polygon subject, Polygon clip)
        {
            Vector2D s = subject.Vertices[0];
            WAList2 spList = new WAList2();
            WAList2 cpList = new WAList2();
            for (int i = 1; i < subject.Vertices.Count; i++)
            {
                Vector2D p = subject.Vertices[i];
                Segment subjectEdge = new Segment(s, p);

                Vector2D t = clip.Vertices[0];

                for (int j = 1; j < clip.Vertices.Count - 1; j++ )
                {
                    Vector2D q = clip.Vertices[j];
                    Segment clipEdge = new Segment(t, q);

                    Vector2D intersectionPoint;
                    bool inboundIntersection;
                    i++;
                    if (Intersection.SegmentSegmentTest(subjectEdge, clipEdge, out intersectionPoint, out inboundIntersection))
                    {
                        WAPoint subjectPoint = new WAPoint
                        {
                            Vertex = intersectionPoint,
                            Visited = false,
                            IsEntryPoint = inboundIntersection,
                            IsIntersection = true,
                        };

                        WAPoint clipPoint = subjectPoint.Clone();

                        subjectPoint.JumpLink = clipPoint;
                        clipPoint.JumpLink = subjectPoint;

                        // Insert point in both list
                        WAPoint tempPoint = currentSubjectPoint.NextVertex;
                        currentSubjectPoint.NextVertex = subjectPoint;
                        subjectPoint.PrevVertex = currentSubjectPoint;
                        subjectPoint.NextVertex = tempPoint;

                        tempPoint = currentClipPoint.NextVertex;
                        currentClipPoint.NextVertex = subjectPoint;
                        clipPoint.PrevVertex = currentClipPoint;
                        clipPoint.NextVertex = tempPoint;
                    }

                    t = q;
                    currentClipPoint = currentClipPoint.NextVertex;
                }

                s = p;
                currentSubjectPoint = currentSubjectPoint.NextVertex;
            }

        }
    }
}
