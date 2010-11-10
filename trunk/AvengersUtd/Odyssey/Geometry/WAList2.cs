using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Geometry
{
    public class WAList2
    {
        public int Length { get; set; }
        public WAPoint Head { get; set; }
        public WAPoint Tail { get; set; }
        public WAPoint Current { get; set; }

        public void Add(WAPoint point)
        {
            if (Length==0)
            {
                Head = Tail = point;
                Head.NextVertex = Head.PrevVertex = Tail;
                Tail.NextVertex = Tail.PrevVertex = Head;
                Current = Head;
                Length++;
            }
            else if (Length == 1)
            {
                Head.NextVertex = Head.PrevVertex = point;
                Tail = point;
                point.PrevVertex = point.NextVertex = Head;
                Current = point;
                Length++;
            }
            else
            {
                //WAPoint temp = Current.NextVertex;
                Current.NextVertex = point;
                point.PrevVertex = Current;
                point.NextVertex = Head;
                Tail = point;
                Current = point;
                Length++;
            }


        }

    }
}
