using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class WAList2
    {
        public int Length { get; set; }
        public WAPoint First { get; set; }
        public WAPoint Last { get; set; }
        WAPoint current;

        public void Add(WAPoint point)
        {
            if (Length==0)
            {
                First = Last = point;
                First.NextVertex = First.PrevVertex = Last;
                Last.NextVertex = Last.PrevVertex = First;
                current = First;
                Length++;
            }
            else if (Length == 1)
            {
                First.NextVertex = First.PrevVertex = point;
                Last = point;
                point.PrevVertex = point.NextVertex = First;
                current = point;
                Length++;
             }
            else
            {
                //WAPoint temp = Current.NextVertex;
                current.NextVertex = point;
                point.PrevVertex = current;
                point.NextVertex = First;
                Last = point;
                First.PrevVertex = Last;
                current = point;
                Length++;
            }
        }

        public void AddAfter(int index, WAPoint point)
        {
            WAPoint currentPoint = Find(index);

            WAPoint tempVertex = currentPoint.NextVertex;
            currentPoint.NextVertex = point;
            point.PrevVertex = currentPoint;
            point.NextVertex = tempVertex;
            tempVertex.PrevVertex = point;
            Length++;
        }

        public void AddBefore(int index, WAPoint point)
        {
            WAPoint currentPoint = Find(index);

            WAPoint tempVertex = currentPoint.PrevVertex;
            currentPoint.PrevVertex = point;
            point.PrevVertex = tempVertex;
            point.NextVertex = currentPoint;
            tempVertex.NextVertex = point;
            Length++;
        }

        public WAPoint Find(int index)
        {
            WAPoint currentPoint = First;
            int counter = 0;
            while (currentPoint.Index != index && counter < Length)
            {
                currentPoint = currentPoint.NextVertex;
                counter++;
            }

            if (counter > Length)
                throw new InvalidOperationException("Exceeded the bounds of the list: index was not found.");

            return currentPoint;
        }

        public void Remove(int index)
        {


            if (index == First.Index)
            {
                First = First.NextVertex;
                First.PrevVertex = Last;
                Last.NextVertex = First;
            }
            else if (index == Last.Index)
            {
                Last = Last.PrevVertex;
                Last.NextVertex = First;
                First.PrevVertex = Last;
            }
            else 
            {
                WAPoint currentPoint = Find(index);
                currentPoint.PrevVertex.NextVertex = currentPoint.NextVertex;
                currentPoint.NextVertex.PrevVertex = currentPoint.PrevVertex;
            }
            Length--;
        }

        public int IndexOf(Vector2D v)
        {
            WAPoint currentPoint = First;
            do
            {
                if (currentPoint.Vertex == v)
                    return currentPoint.Index;
                currentPoint = currentPoint.NextVertex;

            } while (currentPoint.Index != First.Index);

            return -1;
        }

        public WAPoint FindNextIntersectionPoint(WAPoint startPoint)
        {
            WAPoint currentPoint = startPoint;
            do
            {
                if (currentPoint.IsIntersection && currentPoint.IsEntryPoint)
                    return currentPoint;

                currentPoint = currentPoint.NextVertex;
            } while (currentPoint.Index != First.Index);

            return First;
        }

        public Vector2D[] ToArray()
        {
            Vector2D[] array = new Vector2D[Length];

            WAPoint currentPoint = First;
            for (int i = 0; i < Length; i++)
            {
                array[i] = currentPoint.Vertex;
                currentPoint = currentPoint.NextVertex;
            }

            return array;
        }

        public WAPoint[] ToWAPointArray()
        {
            WAPoint[] array = new WAPoint[Length];

            WAPoint currentPoint = First;
            for (int i = 0; i < Length; i++)
            {
                array[i] = currentPoint;
                currentPoint = currentPoint.NextVertex;
            }

            return array;
        }
    }
}
