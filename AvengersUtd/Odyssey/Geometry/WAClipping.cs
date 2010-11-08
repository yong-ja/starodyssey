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
                               IsActive = true,
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

        void FindIntersections()
        {

            //PathFigure subjectEdges = (PathFigure) subject;
            //PathFigure clipEdges = (PathFigure) clip;
            WAPoint currentPoint = subjectPointList.Head.NextVertex;
            Vector2D s = subjectPointList.Head.Vertex;
            while (currentPoint!= subjectPointList.Head)
            {

                Vector2D p = currentPoint.Vertex;
                Segment subjectEdge = new Segment(s, p);

                Vector2D t = clip.Vertices[0];

                for (int j = 1; j < clip.Vertices.Count; j++)
                {
                    Vector2D q = clip.Vertices[j];
                    Segment clipEdge = new Segment(t, q);
                    t = q;

                    Vector2D intersectionPoint;
                    bool inboundIntersection;
                    if (!Intersection.SegmentSegmentTest(subjectEdge, clipEdge, out intersectionPoint, out inboundIntersection))
                        continue;
                    
                        WAPoint point = new WAPoint
                                        {
                                            Vertex = intersectionPoint,
                                            IsActive = true,
                                            IsEntryPoint = inboundIntersection
                                            
                                        }
                    
                }

                s = p;
            }

            //foreach (Segment subjectEdge in subjectEdges.Segments)
            //{
            //    foreach (Segment clipEdge in clipEdges.Segments)
            //    {
                    // Check intersection between two segments.
                    // Determine the intersection point and whether it is
                    // an inbound intersection.
                    // 




        }
    }
}
