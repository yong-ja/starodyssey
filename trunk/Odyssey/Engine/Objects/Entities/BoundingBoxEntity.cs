using System;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Resources;
using SlimDX;

namespace AvengersUtd.Odyssey.Objects.Entities
{
    public class BoundingBoxEntity : BaseEntity, IAxisAlignedBox
    {
        BoundingBox boundingBox;

        public BoundingBoxEntity(IAxisAlignedBox box)
            : base(new EntityDescriptor(Solid.Parallelepiped.ToString(),
                                        new MaterialDescriptor(typeof(WireframeMaterial))),
            box)
        {
            boundingBox = box.BoundingBox;
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
            boundingBox = GeometryHelper.TransformBoundingBox(boundingBox, ParentNode.CurrentAbsoluteWorldMatrix);
        }

        #region IAxisAlignedBox Members

        Vector3 IAxisAlignedBox.Minimum
        {
            get { return boundingBox.Minimum; }
        }

        Vector3 IAxisAlignedBox.Maximum
        {
            get { return boundingBox.Maximum;  }
        }

        BoundingBox IAxisAlignedBox.BoundingBox
        {
            get { return boundingBox; }
        }

        #endregion
    }
}