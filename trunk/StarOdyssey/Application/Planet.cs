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
        float rotation=40;
        float pos;
        float h = 0;
        float k = 0;
        float t;
        float a = 30;
        public override void Render()
        {
            
            float x = h + a*(float)Math.Cos(t);
            float z = k + a*(float) Math.Sin(t);
            Device device = Game.Device;
            pos += 1.5f * (float)Game.FrameTime;
            position = new Vector3(0, 0, 0);
            device.SetTransform(TransformState.World, Matrix.RotationY(rotation) * Matrix.Translation(position));
            rotation += -0.5f*(float)Game.FrameTime;
            //t = rotation;
            
            base.Render();

        }

        public Planet() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                     new MeshDescriptor("Airplane", "Meshes\\Planets\\Large01.X"),
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
