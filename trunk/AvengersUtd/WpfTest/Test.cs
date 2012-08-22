using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
    public class Test
    {
        static int boxIndex=3, selectionIndex, bezierIndex;
        static int participant;

        public static int BoxIndex { get { return boxIndex; } set { boxIndex = value; } }
        public static int SelectionIndex { get { return selectionIndex; } set { selectionIndex = value; } }
        public static int BezierIndex { get { return bezierIndex; } set {bezierIndex = value;}}

        public static int Participant { get { return participant; } }

        public static int Count { get; set; }
    }
}
