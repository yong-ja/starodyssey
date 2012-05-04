using System.Text;
using System.Linq;
using AvengersUtd.Odyssey.Utils.Collections;
using System.Collections.Generic;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class SceneTree
    {
        public SceneNode RootNode { get; private set; }
        public Renderer Renderer { get; private set; }

        /// <summary>
        /// Initializes a new scenegraph.
        /// </summary>
        public SceneTree() : this(new DummyNode()) {}

        public SceneTree(SceneNode rootNode)
        {
            RootNode = rootNode;
            rootNode.Init();
        }

        public bool IsEmpty
        {
            get { return RootNode != null; }
        }

        public SceneNodeCollection<RenderableNode> VisibleNodes
        {
            get
            {
                
                var visibleNodes = from rNode in Node.PostOrderVisit(RootNode).OfType<RenderableNode>()
                        where rNode.RenderableObject.IsInViewFrustum()
                        select rNode;
                return new SceneNodeCollection<RenderableNode>(visibleNodes);        
            }
        }

        public IEnumerable<SceneNode> Nodes
        {
            get
            {
                return from rNode in Node.PostOrderVisit(RootNode).OfType<RenderableNode>()
                       select rNode;
            }
        }


        public void Render()
        {
            foreach (SceneNode node in Node.PreOrderVisit(RootNode))
            {
                node.Update();

                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                    rNode.Render();
            }
        }

        public void UpdateAllNodes()
        {
            foreach (SceneNode node in Node.PreOrderVisit(RootNode))
            {
                node.Update();
            }
        }

        public void Reset()
        {
            foreach (SceneNode node in Node.PreOrderVisit(RootNode))
            {
                node.Init();
            }
        }

        public string DisplayAsText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (INode node in Node.PreOrderVisit(RootNode))
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