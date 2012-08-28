using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
    public class Test
    {
        static int boxIndex=15, selectionIndex=15, bezierIndex =2;
        static int participant=6;

        public static int BoxIndex { get { return BoxConditions[Count]; } private set { boxIndex = value; } }
        public static int SelectionIndex { get { return selectionIndex; } set { selectionIndex = value; } }
        public static int BezierIndex { get { return bezierIndex; } set {bezierIndex = value;}}

        public static int Participant { get { return participant; } }

        static int count = 0;
        public static int Count { get { return count; } set { count = value; } }

        public static int[] CreateResizingSquare(int row)
        {
            const int conditions = 6;
            int[] order = new int[conditions];
            int n = row + (conditions - 1) % conditions;

            order[0] = row;
            order[1] = (row + 1) % conditions ;
            order[2] = ((n) % conditions);
            order[3] = (row + 2) % conditions;
            order[4] = ((n - 1) % conditions);
            order[5] = (row + 3) % conditions;

            return order;
        }

        public static int[] BoxConditions { get; set; }
    }

    
}
