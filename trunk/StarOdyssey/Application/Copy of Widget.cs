using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public class Platform : BaseEntity
    {

        float rotation = 0;
        public Texture shadowMap;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, //Matrix.Scaling(2.0f, 2.0f, 2.0f)*Matrix.RotationX(rotation) * 
                Matrix.Translation(positionV3));
            //device.SetTransform(TransformState.World, Matrix.Translation(positionV3)); //* Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            base.Render();
            
        }

        public void RenderWithTechnique(string technique)
        {
            Device device = Game.Device;
            //device.SetTransform(TransformState.World, Matrix.Translation(positionV3) * Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            //device.SetTransform(TransformState.World, Matrix.Translation(positionV3)); //* Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            device.SetTransform(TransformState.World, //Matrix.Scaling(4.0f, 4.0f, 4.0f) *//
                //Matrix.RotationX(rotation) * 
                Matrix.Translation(positionV3));


          Effect effect = this.Materials[0].EffectDescriptor.Effect;
            effect.Technique = new EffectHandle(technique);
            int passes = effect.Begin(FX.None);
            for (int pass = 0; pass < passes; pass++)
            {
                effect.BeginPass(pass);
                effect.SetValue(new EffectHandle("t1"), shadowMap);
                MeshObject.DrawMesh();
                effect.EndPass();
            }
            effect.End();
        }

        public Platform() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor("ciao", new MeshDescriptor("Meshes\\platform.X"),
                 new MaterialDescriptor(typeof(PhongMaterial))))
        {
            
           //((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0.0f, 1f, 0f);
            PositionV3 = new Vector3(0, -2, 0);
        }
    }
}
