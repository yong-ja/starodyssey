#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Utils.Collections;
#endregion

namespace AvengersUtd.Odyssey.Graphics.Rendering
{
    public class SceneManager
    {
        private readonly RenderMapper renderMapper;

        public SceneManager()
        {
            renderMapper = new RenderMapper();
            CommandManager = new CommandManager();
            Tree = new SceneTree();
        }

        public CommandManager CommandManager { get; private set; }
        public SceneTree Tree { get; private set; }

        public void BuildRenderScene()
        {
            CommandManager.Updater.AddRecurrentTask(new RecurrentTask(TaskType.SceneTreeUpdate, Tree.UpdateAllNodes, 1));
            renderMapper.Clear();

            foreach (SceneNode node in Node.PostOrderVisit(Tree.RootNode))
            {
                RenderableNode rNode = node as RenderableNode;
                if (rNode == null) continue;

                AbstractMaterial currentMaterial = rNode.CurrentMaterial;
                if (!renderMapper.ContainsKey(currentMaterial.TechniqueName))
                {
                    renderMapper.Add(currentMaterial.OwningNode,
                        new RenderableCollection(currentMaterial.RenderableCollectionDescription));
                }
                renderMapper[currentMaterial.TechniqueName].Add(rNode);
            }

            foreach (MaterialNode mNode in renderMapper.OpaqueToTransparent)
            {
                if (mNode.Material.RequirePreRenderStateChange)
                    CommandManager.AddBaseCommands(mNode.Material.PreRenderStates);

                CommandManager.AddRenderCommand(mNode, renderMapper[mNode]);

                if (mNode.Material.RequirePostRenderStateChange)
                    CommandManager.AddBaseCommands(mNode.Material.PostRenderStates);
            }
        }


        public void Display()
        {
            foreach (BaseCommand command in CommandManager.Commands)
            {
                command.Execute();
            }
        }

        public void Dispose()
        {
            CommandManager.Dispose();
            renderMapper.Dispose();
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

        public void Update()
        {
            foreach (IUpdateCommand updateCommand in CommandManager.UpdateCommands)
                updateCommand.Execute();
        }

        public void SuspendUpdates()
        {
            foreach (IUpdateCommand updateCommand in
                CommandManager.UpdateCommands.Where(updateCommand => updateCommand.IsThreaded))
            {
                updateCommand.TerminateThread();
            }
        }

        public void ResumeUpdates()
        {
            foreach (IUpdateCommand updateCommand in
                CommandManager.UpdateCommands.Where(updateCommand => updateCommand.IsThreaded))
            {
                updateCommand.Resume();
            }
        }
    }
}