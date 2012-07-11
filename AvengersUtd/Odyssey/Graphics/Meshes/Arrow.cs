using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using AvengersUtd.Odyssey.Graphics.Materials;
using System.Drawing;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Graphics.Meshes
{
    public class Arrow : MeshGroup, ISphere
    {
        readonly float arrowRadius;
        readonly Vector3 arrowPointerCenter;

        public Arrow(float baseWidth, float arrowHeight, float lineLength, float lineWidth) : base(2)
        {
            this.arrowRadius = arrowHeight/2;
            float halfLength = (arrowHeight + lineLength) / 2;
            float arrowOffset = halfLength - arrowHeight / 2;//halfLength - arrowHeight;
            float lineOffset = halfLength - arrowHeight - lineLength/2;
            arrowPointerCenter = new Vector3(0, arrowOffset, 0);
            Objects[0] = new Pyramid(baseWidth, arrowHeight, baseWidth) { PositionV3 = arrowPointerCenter };
            Objects[1] = new Box(lineWidth, lineLength, lineWidth) { PositionV3 = new Vector3(0,lineOffset, 0)};
            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};

            foreach (IRenderable rObject in Objects)
                rObject.Material = Material;
        }



        Vector3 ISphere.AbsolutePosition
        {
            get { return Objects[0].AbsolutePosition; }
        }

        float ISphere.Radius
        {
            get { return arrowRadius; }
        }
    }
}
