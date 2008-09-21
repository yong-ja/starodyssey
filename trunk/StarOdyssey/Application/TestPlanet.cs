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
    public class TestPlanet : BaseEntity,ISphere
    {
        float rotation=30;
        float pos;
        float h = 30;
        float k = 0;
        float t;
        float a = 50;
        float radius;

        public override void Render()
        {
            
            float x = h + a*(float)Math.Cos(t);
            float z = k + a*(float) Math.Sin(t);
            Device device = Game.Device;
            pos += 5.0f * (float)Game.FrameTime;
            //positionV3 = new Vector3(x, 0,z);
            device.SetTransform(TransformState.World, Matrix.RotationY(rotation)* Matrix.Translation(positionV3));
            rotation += -0.5f*(float)Game.FrameTime;
            t = rotation;
            
            base.Render();

        }

        public TestPlanet(EntityDescriptor eDesc) : base(eDesc)
        {
            radius = 15;
        }

        public TestPlanet() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor(
                "Large Planet",
                     new MeshDescriptor("Meshes\\Planets\\Large01.X"),
                     new[] {

                     new MaterialDescriptor(typeof(SurfaceAtmosphereMaterial),
                                            new TextureDescriptor[]
                                                {
                                                    new TextureDescriptor(TextureType.Diffuse,
                                                                          "Textures\\Planets\\TerraDiffuse.png"),
                                                    new TextureDescriptor(TextureType.Normal,
                                                                          "Textures\\Planets\\TerraNormal.png"),
                                                    new TextureDescriptor(TextureType.Texture1,
                                                                          "Textures\\Planets\\TerraClouds.png"),
                                                                           new TextureDescriptor(TextureType.Specular,
                                                                          "Textures\\Planets\\TerraSpecular.png")

                                                }),
                    new MaterialDescriptor(typeof(AtmosphereMaterial))
                     }))
        {
            //mesh.Materials[0].DiffuseMap = new Color4(0.0f, 0f, 1f);
            
        }

        #region ISphere Members

        Vector3 ISphere.Center
        {
            get { return PositionV3; }
        }

        float ISphere.Radius
        {
            get { return radius; }
        }
        BoundingSphere ISphere.BoundingSphere
        {
            get { return new BoundingSphere(positionV3, radius); }
        }



        #endregion

        
    }
}
