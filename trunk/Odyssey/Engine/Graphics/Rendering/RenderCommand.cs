using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderCommand : BaseCommand
    {
        public RenderCommand(SceneNodeCollection nodeCollection) : 
            base(CommandType.RenderScene, nodeCollection)
        {
        }

        public override void Execute()
        {
            return;
        }

        public override void PerformRender()
        {
            foreach (RenderableNode rNode in Items)
                rNode.Render();
        }

        protected override void OnDispose()
        {
            
        }
    }
}
