using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Geometry;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;

namespace WpfTest
{
    public class ScalingWidget : MeshGroup
    {
        const float ArrowUnit = 1f;
        const float ArrowBase = ArrowUnit/4;
        const float ArrowLength = ArrowUnit/4;
        const float ArrowLineLength = ArrowUnit/4;
        const float ArrowLineWidth = ArrowUnit/8;
        IRenderable target;
        Arrow YArrow;

        public ScalingWidget(IRenderable targetObject)
            : base(1)
        {
            target = targetObject;
            IBox box = targetObject as IBox;
            YArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth) { PositionV3 = new Vector3(0, box.Height, 0) };
            Objects[0] = YArrow;
            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};
        }

        public override IEnumerable<RenderableNode> ToNodes()
        {
            List<RenderableNode> nodes = new List<RenderableNode>();
            foreach (IRenderable rObject in Objects)
                nodes.AddRange(((MeshGroup)rObject).ToNodes());

            return nodes;
        }
    }
}
