using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class SceneOrganizer
    {
        SceneNodeCollection nodes; // temp;
        SceneNodeCollection materials;
        CommandList<BaseCommand> preprocessList;
        CommandList<RenderCommand> renderList;
        RenderMapper renderMapper;
        SceneGraph.SceneGraph sceneGraph, renderGraph;

        public SceneOrganizer()
        {
            preprocessList = new CommandList<BaseCommand>();
            renderList = new CommandList<RenderCommand>();
            renderMapper = new RenderMapper();
            materials = new SceneNodeCollection();
        }

        public void BuildRenderScene3(SceneGraph.SceneGraph graph)
        {
            renderList.Clear();
            renderMapper.Clear();

            sceneGraph = graph;
            renderGraph = new SceneGraph.SceneGraph();
            
            foreach (SceneNode node in Node.PostOrderVisit(graph.RootNode))
            {
                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                {
                    IRenderable entity = rNode.RenderableObject;
                    MaterialNode currentNode = entity.MaterialNode;
                    if (!renderMapper.ContainsKey(currentNode.Technique))
                    {
                        renderMapper.Add(currentNode, new SceneNodeCollection());
                        materials.Add(currentNode);
                    }
                    renderMapper[currentNode.Technique].Add(rNode);
                }
            }

            foreach (MaterialNode mNode in renderMapper.Keys)
                renderList.Add(new RenderCommand(mNode, renderMapper[mNode]));

            //Predicate<RenderableNode> p = rNode => rNode.RenderableObject.CastsShadows;
            nodes = graph.RootNode.SelectNodes<RenderableNode>();
        }

        /// <summary>
        /// Provvisorio.
        /// </summary>
        //public void BuildRenderScene2(SceneGraph.SceneGraph sceneGraph)
        //{
        //    renderList.Clear();
        //    MaterialCollection materialCollection = new MaterialCollection();
        //    foreach (SceneNode node in Node.PreOrderVisit(sceneGraph.RootNode))
        //    {
        //        RenderableNode rNode = node as RenderableNode;
        //        if (rNode != null)
        //            materialCollection.AddRange(rNode.RenderableObject.Materials);
        //    }
        //    materialCollection.SortBy(MaterialCollection.CompareNodesByTechnique);
        //    materials = materialCollection;

        //    MaterialCollection[] materialGroups = materialCollection.SplitByTechnique();
        //    foreach (MaterialCollection materialGroup in materialGroups)
        //    {
        //        renderList.Add(new RenderCommand(materialGroup));
        //    }
            
        //}
        
        /// 
        //public void BuildRenderScene(SceneGraph.SceneGraph sceneGraph)
        //{
        //    renderList.Clear();
        //    SceneNodeCollection nodeCollection = new SceneNodeCollection();
        //    renderList.Clear();
        //    foreach (SceneNode node in Node.PreOrderVisit(sceneGraph.RootNode))
        //    {
        //        RenderableNode rNode = node as RenderableNode;
        //        if (rNode != null)
        //            nodeCollection.Add(rNode);
        //    }

        //    renderList.Add(new RenderCommand(nodeCollection));
        //    nodes = nodeCollection;
        //}

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
            //renderList[1].PerformRender();
        }

        public void AddPreprocessEffect(CommandType commandType)
        {
            BaseCommand command;
            switch (commandType)
            {
                case CommandType.ComputeShadows:
                    //Predicate<RenderableNode> p = rNode => rNode.RenderableObject.CastsShadows;
                    //command = new ShadowMappingCommand(sceneGraph.SelectNodes(p));
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
                else
                {
                    IPlane iPlane = entity as IPlane;
                    if (iPlane != null)
                    {
                        Plane p = iPlane.BoundingPlane;
                        if (Plane.Intersects(p, sphere) == PlaneIntersectionType.Intersecting
                            && Vector3.Distance(rNode.RenderableObject.PositionV3, cNode.RenderableObject.PositionV3) <= 5)
                            return rNode.RenderableObject;
                    }
                }
            }
            return null;
        }
    }
}
