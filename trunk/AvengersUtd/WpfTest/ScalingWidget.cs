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
using AvengersUtd.Odyssey;

namespace WpfTest
{
    public class ScalingWidget : MeshGroup
    {
        const float ArrowUnit = 1f;
        const float ArrowBase = ArrowUnit/6;
        const float ArrowLength = ArrowUnit/4;
        const float ArrowLineLength = ArrowUnit/4;
        const float ArrowLineWidth = ArrowUnit/16;
        IRenderable target;
        Arrow YArrow;
        Arrow XArrow;
        Arrow ZArrow;

        public ScalingWidget(IRenderable targetObject)
            : base(3)
        {
            target = targetObject;
            IBox box = targetObject as IBox;
            YArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth)
            {
                PositionV3 = new Vector3(-box.Width/2 - ArrowLineWidth/2, box.Height/2 , -box.Depth/2 - ArrowLineWidth/2),
                Name = "YArrow"
            };
            XArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth) {
                PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth/2, -box.Height / 2 + ArrowLineWidth/2, -box.Depth / 2 - ArrowLineWidth/2),
                CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI/2f),
                Name = "XArrow"
            };
            ZArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth)
            {
                PositionV3 = new Vector3(-box.Width/2 - ArrowLineWidth/2, -box.Height/2 + ArrowLineWidth/2, box.Depth/2 + ArrowLineWidth/2),
                CurrentRotation = Quaternion.RotationYawPitchRoll(0,(float)Math.PI / 2f, 0),
                Name = "ZArrow"
            };  

            
            Objects[0] = YArrow;
            Objects[1] = XArrow;
            Objects[2] = ZArrow;
            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};
        }

        public override IEnumerable<RenderableNode> ToNodes()
        {
            List<RenderableNode> nodes = new List<RenderableNode>();
            foreach (IRenderable rObject in Objects)
                nodes.AddRange(((MeshGroup)rObject).ToNodes());

            return nodes;
        }

        public IRenderable FindIntersection(Ray r)
        {
            foreach (IRenderable rObject in Objects)
            {
                ISphere s = rObject as ISphere;
                bool result = Intersection.RaySphereTest(r, s);
                if (result)
                    return rObject;
            }
            return null;
        }

        public IRenderable FindIntersection2D(Vector2 p)
        {
            foreach (IRenderable rObject in Objects)
            {
                Vector3 absolutePosition = ((MeshGroup)rObject).Objects[0].AbsolutePosition;
                Vector3 screenSpace = Vector3.Project(absolutePosition, 0, 0, Game.Context.Settings.ScreenWidth, Game.Context.Settings.ScreenHeight,
                    Game.CurrentRenderer.Camera.NearClip, Game.CurrentRenderer.Camera.FarClip, Game.CurrentRenderer.Camera.WorldViewProjection);

                Circle c = new Circle(new Vector2D(absolutePosition.X, absolutePosition.Y), 128);
                bool result = Intersection.CirclePointTest(c, new Vector2D(p.X, p.Y));
                if (result)
                    return rObject;
            }
            return null;
        }


    }
}
