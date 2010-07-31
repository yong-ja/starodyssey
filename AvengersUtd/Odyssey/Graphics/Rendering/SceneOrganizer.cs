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
        SceneNodeCollection<MaterialNode> materials;
        CommandList<RenderCommand> renderList2;
        private RenderManager renderManager;
        RenderMapper renderMapper;
        SceneGraph.SceneGraph sceneGraph, renderGraph;

        public SceneOrganizer()
        {
            renderList2 = new CommandList<RenderCommand>();
            renderMapper = new RenderMapper();
            //stateManager = new StateManager();
            renderManager = new RenderManager();
        }

        public void BuildRenderScene(SceneGraph.SceneGraph graph)
        {
            renderList2.Clear();
            renderMapper.Clear();

            sceneGraph = graph;
            renderGraph = new SceneGraph.SceneGraph();
            
            foreach (SceneNode node in Node.PostOrderVisit(graph.RootNode))
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
                            renderManager.AddCommand(stateChangeCommand);
                        }
                    }
                renderManager.AddCommand(new RenderCommand(mNode, renderMapper[mNode]));

                if (mNode.Material.RequirePostRenderStateChange)
                {
                    foreach (BaseCommand stateChangeCommand in mNode.Material.PostRenderStates)
                    {
                        renderManager.AddCommand(stateChangeCommand);
                    }
                } 
            }
        }
 
        public void Display()
        {
            foreach (BaseCommand command in renderManager.Commands)
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
