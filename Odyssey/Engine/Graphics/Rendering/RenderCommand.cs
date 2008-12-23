using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class RenderCommand : BaseCommand
    {
        readonly MaterialNode materialNode;

        public RenderCommand(MaterialNode mNode, SceneNodeCollection sceneNodeCollection) :
            base(CommandType.RenderScene, sceneNodeCollection)
        {
            materialNode = mNode;
        }

        public override void Execute()
        {
            return;
        }

        public override void PerformRender()
        {
            Items.RenderGroup(materialNode);
        }

        protected override void OnDispose()
        {
            
        }
    }
}
