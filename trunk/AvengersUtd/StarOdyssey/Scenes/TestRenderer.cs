using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey11.Graphics.Materials;
using AvengersUtd.Odyssey11.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey11.Rendering;

namespace AvengersUtd.StarOdyssey.Scenes
{
    public class TestRenderer : Renderer
    {
        public override void Init()
        {
            MaterialNode mNode = new MaterialNode(new FunctionalMaterial());
            SceneGraph sceneGraph = new SceneGraph();
            //sceneGraph.RootNode.AppendChild(new RenderableNode(P));
        }

        public override void Render()
        {
            throw new NotImplementedException();
        }

        public override void ProcessInput()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
