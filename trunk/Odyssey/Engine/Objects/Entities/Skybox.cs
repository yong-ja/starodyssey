using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class Skybox : BaseEntity
    {
        public Skybox(EntityDescriptor entityDesc) : base(entityDesc)
        {
        }

        public override void Render()
        {
            Device device = Game.Device;

            device.SetRenderState(RenderState.ZEnable, false);
            device.SetRenderState(RenderState.ZWriteEnable, false);


            //Matrix mView = Game.CurrentScene.Camera.View;
            //mView.M41 = mView.M42 = mView.M43 = 0f;

            device.SetTransform(TransformState.World, //Matrix.Identity);//
                                Matrix.Translation(Game.CurrentScene.Camera.PositionV3));
            //device.SetTransform(TransformState.View, mView);

            meshObject.Render();

            device.SetRenderState(RenderState.ZEnable, true);
            device.SetRenderState(RenderState.ZWriteEnable, true);

            ////device.SetTexture(0, null);
        }
    }
}