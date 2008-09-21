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
    public class Office : BaseEntity, ISphere
    {
        public Texture shadowMap;


        public void RenderWithTechnique(string technique)
        {
            Device device = Game.Device;
            //device.SetTransform(TransformState.World, Matrix.Translation(positionV3) * Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));
            device.SetTransform(TransformState.World, Matrix.Translation(positionV3)); //* Matrix.Scaling(new Vector3(0.1f, 0.1f, 0.1f)));

            Effect effect = this.Materials[0].EffectDescriptor.Effect;
            effect.Technique = new EffectHandle(technique);
            int passes = effect.Begin(FX.None);
            for (int pass = 0; pass < passes; pass++)
            {
                effect.BeginPass(pass);
                effect.SetTexture(new EffectHandle("t1"), shadowMap);
                MeshObject.DrawMesh();
                effect.EndPass();
            }
            effect.End();
        }

        public Office() :
            base(new AvengersUtd.Odyssey.Resources.EntityDescriptor("Office", new MeshDescriptor("Meshes\\Teapot.x"),
                new MaterialDescriptor(typeof(PhongMaterial))))
        {
            //((SpecularMaterial)mesh.Materials[0]).Diffuse = new Color4(0.0f, 0f, 1f);
            positionV3 = new Vector3(0, 3, 0);
        }

        public override bool Intersects(Ray ray)
        {
            float distance;
            return Ray.Intersects(ray, new BoundingSphere(positionV3, 6), out distance);
        }


        #region ISphere Members

        Vector3 ISphere.Center
        {
            get { return positionV3; }
        }

        float ISphere.Radius
        {
            get { return 6; }
        }

        BoundingSphere ISphere.BoundingSphere
        {
            get { return new BoundingSphere(positionV3, 6); }
        }

        #endregion

        
    }
}
