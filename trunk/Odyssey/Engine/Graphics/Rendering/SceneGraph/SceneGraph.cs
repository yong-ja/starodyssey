using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.Graphics.Meshes;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class SceneGraph
    {
        SceneNode rootNode;

        public SceneNode RootNode
        {
            get { return rootNode; }
        }


        /// <summary>
        /// Initializes a new scenegraph.
        /// </summary>
        public SceneGraph()
        {
            rootNode = new DummyNode();
        }
        

        public SceneGraph(SceneNode rootNode)
        {
            this.rootNode = rootNode;
            rootNode.Init();
        }


        public SceneNodeCollection VisibleNodes
        {
            get
            {
                SceneNodeCollection visibleNodes = new SceneNodeCollection();
                foreach (SceneNode node in Node.PostOrderVisit(rootNode))
                {
                    RenderableNode rNode = node as RenderableNode;
                    if (rNode != null && rNode.RenderableObject.IsInViewFrustum())
                        visibleNodes.Add(node);
                }
                return visibleNodes;
            }
        }

        public void Render()
        {
            foreach (SceneNode node in Node.PreOrderVisit(rootNode))
            {
                node.Update();

                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                    rNode.Render();
            }
        }

        public void UpdateAllNodes()
        {
            foreach (SceneNode node in Node.PreOrderVisit(rootNode))
            {
                node.Update();
            }
        }

        public string DisplayAsText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (INode node in Node.PreOrderVisit(rootNode))
            {
                string indent = string.Empty;
                for (int i = 0; i < node.Level; i++)
                    indent += '\t';
                sb.AppendLine(string.Format("{0}{1} N:{2}.{3}",indent,
                    node.GetType().Name,
                    node.Level,
                    node.Index));
            }
            return sb.ToString();

        }
    }
}