using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class SceneOrganizer
    {
        SceneNodeCollection nodes; // temp;
        CommandList<BaseCommand> preprocessList;
        CommandList<RenderCommand> renderList;

        public SceneOrganizer()
        {
            preprocessList = new CommandList<BaseCommand>();
            renderList = new CommandList<RenderCommand>();
        }

        /// <summary>
        /// Provvisorio.
        /// </summary>
        public void BuildRenderScene(SceneGraph.SceneGraph sceneGraph)
        {
            renderList.Clear();
            SceneNodeCollection nodeCollection = new SceneNodeCollection();
            renderList.Clear();
            foreach (SceneNode node in Node.PreOrderVisit(sceneGraph.RootNode))
            {
                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                    nodeCollection.Add(rNode);
            }

            renderList.Add(new RenderCommand(nodeCollection));
            nodes = nodeCollection;
        }

        public void Process()
        {
            foreach (BaseCommand command in preprocessList)
                command.Execute();
        }

        public void Display()
        {
            foreach (RenderCommand rCommand in renderList)
            {
                rCommand.PerformRender();
            }
        }

        public void AddPreprocessEffect(CommandType commandType)
        {
            BaseCommand command;
            switch (commandType)
            {
                case CommandType.ComputeShadows:
                    command = new ShadowMappingCommand(nodes);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("commandType", "Command not supported");
            }
            preprocessList.Insert(0, command);
        }

        public void ClearPreprocessEffects()
        {
            preprocessList.Clear();
        }

        

        public IRenderable CheckForCollisions(IRenderable collidingObject, BoundingSphere sphere)
        {
            RenderableNode cNode = collidingObject.ParentNode;

            foreach (RenderableNode rNode in nodes)
            {
                if (cNode == rNode || !rNode.RenderableObject.IsCollidable)
                    continue;

                IRenderable entity = rNode.RenderableObject;
                IAxisAlignedBox iBox = entity as IAxisAlignedBox;
                if (iBox != null)
                {
                    BoundingBox box = iBox.BoundingBox;
                    //if (!rNode.CurrentAbsoluteWorldMatrix.IsIdentity)
                    //    box = GeometryHelper.TransformBoundingBox(box, rNode.CurrentAbsoluteWorldMatrix);

                    if (BoundingBox.Intersects(box, sphere))
                    {
                        return rNode.RenderableObject;
                    }
                }
            }
            return null;
        }
    }
}
