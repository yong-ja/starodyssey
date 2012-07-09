using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Arrow : MeshGroup
    {
        public Arrow(float baseWidth, float arrowHeight, float lineLength, float lineWidth) : base(2)
        {
            float halfLength = (arrowHeight + lineLength) / 2;
            float arrowOffset = halfLength - arrowHeight;
            float lineOffset = arrowOffset - halfLength + arrowHeight/2;
            Objects[0] = new Pyramid(baseWidth, arrowHeight, baseWidth) { PositionV3 = new Vector3(0, arrowOffset + arrowHeight/2, 0) };
            Objects[1] = new Box(lineWidth, lineLength, lineWidth) { PositionV3 = new Vector3(0, lineOffset, 0)};
            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};

            foreach (IRenderable rObject in Objects)
                rObject.Material = Material;
        }

        protected override void OnPositionChanged(PositionEventArgs e)
        {
            base.OnPositionChanged(e);
            Objects[0].PositionV3 += e.NewValue;
            Objects[1].PositionV3 += e.NewValue;
        }
    }
}
