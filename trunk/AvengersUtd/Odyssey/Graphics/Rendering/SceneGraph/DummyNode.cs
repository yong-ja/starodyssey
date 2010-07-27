using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class DummyNode : SceneNode
    {
        static int count;
        const string nodeTag = "DN_";

        public DummyNode(string label):
            base(label, SceneNodeType.Dummy)
        {}

        public DummyNode()
            : this (nodeTag + count++)
        {}

        public override void Update()
        {
            return;
        }

        protected override object OnClone()
        {
            return new DummyNode(Label);
        }
    }
}
