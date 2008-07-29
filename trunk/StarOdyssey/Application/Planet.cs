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
    public class Planet : BaseEntity
    {
        public override void Render()
        {
            Device device = Game.Device;
            device.SetTransform(TransformState.World, Matrix.Translation(position));
            base.Render();
            
        }

        public Planet() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                     new MeshDescriptor("Airplane", "Meshes\\Planets\\Large.X"),
                     new[] {

                     new MaterialDescriptor(typeof(SpecularBumpMaterial),
                                            new TextureDescriptor[]
                                                {
                                                    new TextureDescriptor(TextureType.Diffuse,
                                                                          "Textures\\Planets\\Terra.png"),
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

        
    }
}
