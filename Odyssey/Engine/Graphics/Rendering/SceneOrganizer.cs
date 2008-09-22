using System;
using System.Collections.Generic;
using System.Text;
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
            SceneNodeCollection nodeCollection = new SceneNodeCollection();
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

        private BoundingBox TransformBoundingBox(BoundingBox origBox, Matrix matrix)
        {
            Vector3 origCorner1 = origBox.Minimum;
            Vector3 origCorner2 = origBox.Maximum;

            Vector4 transCorner1 = Vector3.Transform(origCorner1, matrix);
            Vector4 transCorner2 = Vector3.Transform(origCorner2, matrix);


            Vector3 newCorner1 = new Vector3(transCorner1.X, transCorner1.Y, transCorner1.Z);
            Vector3 newCorner2 = new Vector3(transCorner2.X, transCorner2.Y, transCorner2.Z);
            return new BoundingBox(newCorner1, newCorner2);
        }

        public bool CheckForCollisions(BoundingSphere sphere)
        {
            foreach (RenderableNode rNode in nodes)
            {
                IRenderable entity = rNode.RenderableObject;
                IAxisAlignedBox iBox = entity as IAxisAlignedBox;
                if (iBox != null)
                {
                    BoundingBox box = iBox.BoundingBox;
                    if (!rNode.CurrentAbsoluteWorldMatrix.IsIdentity)
                        box = TransformBoundingBox(box, rNode.CurrentAbsoluteWorldMatrix);

                    if (BoundingBox.Intersects(box, sphere))
                        return true;
                }
            }
            return false;
        }
    }
}
