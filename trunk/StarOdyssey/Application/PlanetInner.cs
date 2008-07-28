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
    public class PlanetInner : BaseEntity
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));
            base.Render();
            
        }

        public PlanetInner() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                     new MeshDescriptor("Airplane", "Meshes\\Planets\\Large.X"),
                     new[] {

                     new MaterialDescriptor(typeof(SpecularBumpMaterial),
                                            new TextureDescriptor[]
                                                {
                                                    new TextureDescriptor(TextureType.Diffuse,
                                                                          "Textures\\Planets\\Terra01.png"),
                                                    new TextureDescriptor(TextureType.Normal,
                                                                          "Textures\\Planets\\TerraNormal.png"),
                                                    new TextureDescriptor(TextureType.Texture1,
                                                                          "Textures\\Planets\\TerraClouds.png"),
                                                                           new TextureDescriptor(TextureType.Texture2,
                                                                          "Textures\\Planets\\TerraSpecular.png")

                                                }),
                    new MaterialDescriptor(typeof(EffectMaterial))
                     }))
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
                Matrix.Translation(position) * Matrix.RotationZ(MathHelper.DegreeToRadian(-7.25f)) * Matrix.RotationY(MathHelper.DegreeToRadian(290.0f)));


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
