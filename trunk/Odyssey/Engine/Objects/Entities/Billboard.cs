using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;
using SlimDX.Direct3D9;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class Billboard : BaseEntity, IBillboard
    {
        public float BillboardSize
        {
            get { return ((TexturedBillboardMaterial) Materials[0]).BillboardSize; }
            set { ((TexturedBillboardMaterial) Materials[0]).BillboardSize = value; }
        }

        public Billboard(EntityDescriptor entityDesc, float billboardSize) : base(entityDesc)
        {
        }

        public override void Render()
        {
            Game.Device.SetTransform(TransformState.World, Matrix.Translation(positionV3));

            base.Render();
        }
    }
}