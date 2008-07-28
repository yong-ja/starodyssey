using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Objects.Materials;
using AvengersUtd.Odyssey.Meshes;
using AvengersUtd.Odyssey.Resources;
using AvengersUtd.Odyssey;
using SlimDX.Direct3D9;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public class Office : BaseEntity
    {
        public Texture shadowMap;

        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));//, Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));


            MeshObject.DrawWithEffect();

        }

        public void RenderWithTechnique(string technique)
        {
            Device device = Game.Device;
            //device.SetTransform(TransformState.World, Matrix.Translation(position) * Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            device.SetTransform(TransformState.World, Matrix.Translation(position)); //* Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));


            MeshObject.DrawWithEffect();
            Effect effect = this.Materials[0].EffectDescriptor.Effect;
            effect.Technique = new EffectHandle(technique);
            int passes = effect.Begin(FX.None);
            for (int pass = 0; pass < passes; pass++)
            {
                effect.BeginPass(pass);
                effect.SetValue(new EffectHandle("t1"), shadowMap);
                MeshObject.Draw();
                effect.EndPass();
            }
            effect.End();
        }

        public Office() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Office", "Meshes\\office.X"),
                new MaterialDescriptor(typeof(SpecularMaterial))))
        {
            ((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0.0f, 1f, 0f);
        }
    }
}
