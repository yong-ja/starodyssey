using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AvengersUtd.Odyssey.Objects.Entities;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.SceneGraph
{
    public class SceneNodeCollection : Collection<SceneNode>
    {

        public SceneNodeCollection(IList<SceneNode> collection):base(collection)
        {
        }

        public SceneNodeCollection()
        {
        
        }


        public void RenderWithMaterial(AbstractMaterial material)
        {
            material.ApplyDynamicParameters();
            Effect effect = material.EffectDescriptor.Effect;
            int passes = effect.Begin(FX.None);

            effect.BeginPass(0);

            foreach (SceneNode node in this)
            {
                node.Update();
                RenderableNode rNode = node as RenderableNode;
                if (rNode != null)
                { 
                    Game.Device.SetTransform(TransformState.World, rNode.CurrentAbsoluteWorldMatrix);
                    material.Apply();
                    
                    IRenderable entity = rNode.RenderableObject;
                    entity.DrawMesh();
                }
            }

            Game.Device.SetTransform(TransformState.World, Matrix.Identity);
            effect.EndPass();
            effect.End();
        }
    }
}