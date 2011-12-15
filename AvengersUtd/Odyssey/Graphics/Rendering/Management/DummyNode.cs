using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class DummyNode : SceneNode
    {
        static int count;

        public DummyNode()
            : base(Text.GetCapitalLetters(typeof(DummyNode).GetType().Name) + '_' + ++count, SceneNodeType.Dummy)
        {}

        public override void Update()
        {
            return;
        }

    }
}
