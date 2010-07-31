using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Utils.Collections;
using AvengersUtd.Odyssey.Graphics.Meshes;


namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class SceneOrganizer
    {
        readonly RenderMapper renderMapper;

        public RenderManager CommandManager { get; private set; }
        public SceneGraph.SceneGraph Tree { get; private set; }

        public SceneOrganizer()
        {
            renderMapper = new RenderMapper();
            CommandManager = new RenderManager();
            Tree = new SceneGraph.SceneGraph();
        }

        public void BuildRenderScene()
        {
            renderMapper.Clear();

          
            foreach (SceneNode node in Node.PostOrderVisit(Tree.RootNode))
            {

                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                {
                    IRenderable rObject = rNode.RenderableObject;
                    AbstractMaterial currentMaterial = rNode.CurrentMaterial;
                    if (!renderMapper.ContainsKey(currentMaterial.TechniqueName))
                    {
                        
                        renderMapper.Add(currentMaterial.OwningNode, new RenderableCollection(currentMaterial.RenderableCollectionDescription));
                    }
                    renderMapper[currentMaterial.TechniqueName].Add(rNode);
                }
            }

            foreach (MaterialNode mNode in renderMapper.Keys)
            {
                if (mNode.Material.RequirePreRenderStateChange)
                    {
                        foreach (BaseCommand stateChangeCommand in mNode.Material.PreRenderStates)
                        {
                            CommandManager.AddCommand(stateChangeCommand);
                        }
                    }
                CommandManager.AddCommand(new RenderCommand(mNode, renderMapper[mNode]));

                if (mNode.Material.RequirePostRenderStateChange)
                {
                    foreach (BaseCommand stateChangeCommand in mNode.Material.PostRenderStates)
                    {
                        CommandManager.AddCommand(stateChangeCommand);
                    }
                } 
            }
        }


        public void Display()
        {
            foreach (BaseCommand command in CommandManager.Commands)
            {
                command.Execute();
            }
        }

        
        //public IRenderable CheckForCollisions(IRenderable collidingObject, BoundingSphere sphere)
        //{
        //    RenderableNode cNode = collidingObject.ParentNode;

        //    foreach (RenderableNode rNode in nodes)
        //    {
        //        if (cNode == rNode || !rNode.renderableObject.IsCollidable)
        //            continue;

        //        IRenderable entity = rNode.renderableObject;
        //        IAxisAlignedBox iBox = entity as IAxisAlignedBox;
        //        if (iBox != null)
        //        {
        //            BoundingBox box = iBox.BoundingBox;
        //            //if (!rNode.CurrentAbsoluteWorldMatrix.IsIdentity)
        //            //    box = GeometryHelper.TransformBoundingBox(box, rNode.CurrentAbsoluteWorldMatrix);

        //            if (BoundingBox.Intersects(box, sphere))
        //            {
        //                return rNode.renderableObject;
        //            }
        //        }
        //        else
        //        {
        //            IPlane iPlane = entity as IPlane;
        //            if (iPlane != null)
        //            {
        //                Plane p = iPlane.BoundingPlane;
        //                if (Plane.Intersects(p, sphere) == PlaneIntersectionType.Intersecting
        //                    && Vector3.Distance(rNode.renderableObject.PositionV3, cNode.renderableObject.PositionV3) <= 5)
        //                    return rNode.renderableObject;
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
