using System;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class BoundingSphereEntity : BaseEntity, ISphere
    {
        BoundingSphere boundingSphere;

        public BoundingSphereEntity(ISphere sphere)
            : base(new EntityDescriptor(Solid.Sphere.ToString(),
                                        new MaterialDescriptor(typeof(WireframeMaterial))),
            sphere)
        {
            boundingSphere = sphere.BoundingSphere ;
            CastsShadows = false;
          
        }

        public override void Init()
        {
            base.Init();
            Materials[0].DiffuseColor = new SlimDX.Color4(1, 1, 0);
        }

        protected override void OnPositionChanged(PositionEventArgs e)
        {
            base.OnPositionChanged(e);
            boundingSphere = GeometryHelper.TransformBoundingSphere(boundingSphere, e.NewValue);
        }

        #region ISphere Members

        Vector3 ISphere.Center
        {
            get { return boundingSphere.Center; }
        }

        float ISphere.Radius
        {
            get { return boundingSphere.Radius; }
        }

        BoundingSphere ISphere.BoundingSphere
        {
            get { return boundingSphere; }
        }

        #endregion

       
    }
}