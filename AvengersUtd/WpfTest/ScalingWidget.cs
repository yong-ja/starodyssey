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
        const float ArrowLineLength = ArrowUnit/2;
        const float ArrowLineWidth = ArrowUnit/8;
        IRenderable target;
        Arrow YArrow;
        Arrow XArrow;

        public ScalingWidget(IRenderable targetObject)
            : base(2)
        {
            target = targetObject;
            IBox box = targetObject as IBox;
            YArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth) { PositionV3 = new Vector3(0, box.Height, 0) };
            XArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth) {
                //PositionV3 = new Vector3(box.Width, 0, 0),
                CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI/4f),//Quaternion.RotationAxis(-Vector3.UnitZ, (float)(Math.PI / 4f)),
                RotationCenter = new Vector3(0, 1, 0)
            };  
               // PositionV3 = new Vector3(box.Width, 0, 0),
               //CurrentRotation = Quaternion.RotationAxis(Vector3.UnitZ, (float)Math.PI/2)
                ;
            //YArrow.Move(box.Height, Vector3.UnitY);
            //XArrow.Move(box.Width, Vector3.UnitX);
            //XArrow.Rotate(-(float)(Math.PI / 2), new Vector3(0, 1, 0));
            
            Objects[0] = YArrow;
            Objects[1] = XArrow;
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
