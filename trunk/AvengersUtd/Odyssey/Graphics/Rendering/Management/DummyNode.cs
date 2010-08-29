using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class DummyNode : SceneNode
    {
        static int count;
        const string NodeTag = "DN_";

        public DummyNode():
            base(NodeTag + (++count), SceneNodeType.Dummy)
        {}

        public override void Update()
        {
            return;
        }

    }
}
