﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX.Direct3D11;
using SlimDX;
using System.Windows.Forms;
using Effect = SlimDX.Direct3D11.Effect;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class SceneNodeCollection : Collection<SceneNode>
    {

        public SceneNodeCollection(IList<SceneNode> collection)
            : base(collection)
        {
        }

        public SceneNodeCollection()
        {

        }

        public void RenderGroup(MaterialNode mNode)
        {
            EffectTechnique technique = mNode.Material.EffectDescriptor.Technique;

            EffectPass pass = technique.GetPassByIndex(mNode.Material.EffectDescriptor.Pass);
            RenderForm11.Device.ImmediateContext.InputAssembler.InputLayout = new InputLayout(RenderForm11.Device, pass.Description.Signature,
                    TexturedVertex.InputElements);
            pass.Apply(RenderForm11.Device.ImmediateContext);
            mNode.Material.ApplyDynamicParameters();

            foreach (SceneNode node in this)
            {
                node.Update();
                RenderableNode rNode = node as RenderableNode;
                if (rNode != null && rNode.RenderableObject.IsVisible)
                {
                    IRenderable rObject = rNode.RenderableObject;
                    mNode.Material.ApplyInstanceParameters(rObject);
                    
                    rObject.Render();
                }
            }
            //string technique = mNode.Technique;
            //AbstractMaterial material = mNode.Materials[0];

            //Effect effect = material.EffectDescriptor.Effect;
            //if (effect.Technique != technique)
            //{
            //    try
            //    {
            //        effect.Technique = technique;
            //    }
            //    catch (Direct3D9Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}

            //int passes = effect.Begin(FX.None);
            //effect.BeginPass(0);


            //foreach (SceneNode node in this)
            //{
            //    node.Update();
            //    RenderableNode rNode = node as RenderableNode;
            //    if (rNode != null && rNode.renderableObject.IsVisible)
            //    {
            //        Game.Device.SetTransform(TransformState.World, rNode.CurrentAbsoluteWorldMatrix);

            //        rNode.renderableObject.Materials[0].Apply();

            //        IRenderable entity = rNode.renderableObject;
            //        entity.DrawMesh();
            //    }
            //}

            //Game.Device.SetTransform(TransformState.World, Matrix.Identity);
            //effect.EndPass();
            //effect.End();
        }

        public void RenderWithMaterial(AbstractMaterial material)
        {
            ////material.ApplyDynamicParameters();
            //Effect effect = material.EffectDescriptor.Effect;
            //int passes = effect.Begin(FX.None);

            //effect.BeginPass(0);

            //foreach (SceneNode node in this)
            //{
            //    node.Update();
            //    RenderableNode rNode = node as RenderableNode;
            //    if (rNode != null)
            //    {
            //        Game.Device.SetTransform(TransformState.World,rNode.CurrentAbsoluteWorldMatrix);
                    
            //        material.Apply();

            //        IRenderable entity = rNode.renderableObject;
            //        entity.DrawMesh();
            //    }
            //}

            //Game.Device.SetTransform(TransformState.World, Matrix.Identity);
            //effect.EndPass();
            //effect.End();
        }

        public void SortBy(Comparison<SceneNode> comparison)
        {
            List<SceneNode> nodeList = (List<SceneNode>)Items;
            nodeList.Sort(comparison);
        }
    }


}