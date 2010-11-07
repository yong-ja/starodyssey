using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public static class Clipping
    {
        static void FindIntersections(Polygon subject, Polygon clip)
        {
            PathFigure subjectEdges = (PathFigure) subject;
            PathFigure clipEdges = (PathFigure) clip;

            foreach (Segment subjectEdge in subjectEdges.Segments)
            {
                foreach (Segment clipEdge in clipEdges.Segments)
                {
                    // Check intersection between two segments.
                    // Determine the intersection point and whether it is
                    // an inbound intersection.
                    // 

                    Vector2D intersectionPoint;
                    bool inboundIntersection;
                    if (Intersection.SegmentSegmentTest(subjectEdge, clipEdge, out intersectionPoint, out inboundIntersection))
                    {
                        WAPoint point = new WAPoint
                                        {
                                            
                                        }
                    }


                }
            }
        }
    }
}
