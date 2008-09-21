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
    public class PlanetClouds : BaseEntity<SpecularBumpMaterial>
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));
            MeshObject.DrawWithEffect();
            
        }

        public PlanetClouds() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                new MeshDescriptor("Airplane", "Meshes\\cloudCover.X"),
                new TextureDescriptor("Textures", "Textures\\Planets\\Terra.png",
                    "Textures\\Planets\\TerraNormal.png", "Textures\\Planets\\TerraClouds01.jpg")))
        {
            //mesh.Materials[0].Diffuse = new Color4(0.0f, 0f, 1f);
        }

        public void RenderWithTechnique(string technique)
        {
            Device device = Game.Device;
            //device.SetTransform(TransformState.World, Matrix.Translation(position) * Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            //device.SetTransform(TransformState.World, Matrix.Translation(position)); //* Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            device.SetTransform(TransformState.World, //Matrix.Scaling(4.0f, 4.0f, 4.0f) *//
                //Matrix.RotationX(rotation) * 
                Matrix.Translation(position));


            Effect effect = this.Materials[0].EffectDescriptor.Effect;
            effect.Technique = new EffectHandle(technique);
            int passes = effect.Begin(FX.None);
            for (int pass = 0; pass < passes; pass++)
            {
                effect.BeginPass(pass);
                MeshObject.Draw();
                effect.EndPass();
            }
            effect.End();
        }
    }
}
