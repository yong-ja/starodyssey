using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
    public class Test
    {
        static int boxIndex=0, selectionIndex=15, bezierIndex =2;
        static int participant=3;

        public static int BoxIndex { get { return boxIndex; } set { boxIndex = value; } }
        public static int SelectionIndex { get { return selectionIndex; } set { selectionIndex = value; } }
        public static int BezierIndex { get { return bezierIndex; } set {bezierIndex = value;}}

        public static int Participant { get { return participant; } }

        static int count = 0;
        public static int Count { get { return count; } set { count = value; } }
    }
}
