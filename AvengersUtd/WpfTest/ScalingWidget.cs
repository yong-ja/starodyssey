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

        public const float ArrowIntersectionRadius = 256;
        IBox box;
        IBox frame;
        Arrow YArrow;
        Arrow XArrow;
        Arrow ZArrow;

        public bool XInverted { get; private set; }

        public int Configuration { get; private set; }

        public void SetFrame(IBox frame)
        {
            this.frame = frame;
        }

        public ScalingWidget(IBox targetObject)
            : base(3)
        {
            box = targetObject;
           
            YArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth)
            {
                Name = "YArrow"
            };
            XArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth) {
                
                Name = "XArrow"
            };
            ZArrow = new Arrow(ArrowBase, ArrowLength, ArrowLineLength, ArrowLineWidth)
            {
                Name = "ZArrow"
            };  

            
            Objects[0] = YArrow;
            Objects[1] = XArrow;
            Objects[2] = ZArrow;

            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};

            ChooseArrangement(0);
        }

        void ChooseArrangement(int configuration)
        {
            Configuration = configuration;
            switch (Configuration)
            {
                    // Tangent to the closest edge
                case 0:
                    YArrow.PositionV3 = new Vector3(-box.Width/2 - ArrowLineWidth/2, box.Height/2 , -box.Depth/2 - ArrowLineWidth/2);
                    XArrow.PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI/2f);
                    ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, box.Depth / 2 + ArrowLineWidth / 2);
                    ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0,(float)Math.PI / 2f, 0);
                break;

                    // Tangent to the rightmost edge
                case 1:
                    YArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    XArrow.PositionV3 = new Vector3(-box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, (float)Math.PI / 2f);
                    ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, box.Depth / 2 + ArrowLineWidth / 2);
                    ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0,(float)Math.PI / 2f, 0);
                    XInverted = true;
                    break;

                    // Tangent to the leftmost edge
                case 2:
                    YArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    XArrow.PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI / 2f);
                    ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
                    ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, -(float)Math.PI / 2f, 0);
                    break;
            }
        }

        //public Vector3 GetAxis(int axis)
        //{
        //    switch (Configuration)
        //    {
        //            // YAxis:
        //        case 0:

        //    }
        //}


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

                Circle c = new Circle(new Vector2D(screenSpace.X, screenSpace.Y), ArrowIntersectionRadius);
                bool result = Intersection.CirclePointTest(c, new Vector2D(p.X, p.Y));
                if (result)
                    return rObject;
            }
            return null;
        }

        public void ResetColors()
        {
            foreach (IRenderable rObject in Objects)
            {
                if (!((Arrow)rObject).IsSelected)
                    foreach (IRenderable rArrow in ((MeshGroup)rObject).Objects)
                        ((IColorMaterial)rArrow.Material).DiffuseColor = Color.Yellow;
            }
        }

        public Arrow SelectByName(string name)
        {
            Arrow arrow;
            switch (name)
            {
                case "XArrow":
                    arrow = XArrow;
                    break;
                case "YArrow":
                    arrow = YArrow;
                    break;
                case "ZArrow":
                    arrow = ZArrow;
                    break;
                default:
                    arrow = null;
                    break;
            }

            return arrow;
        }

        public void Select(string name, Color color)
        {
            Arrow arrow = SelectByName(name);
            if (arrow != null)
                foreach (IRenderable rObject in arrow.Objects)
                    ((IColorMaterial)rObject.Material).DiffuseColor = color;
        }

        public void SetColor(Arrow arrow, Color color)
        {
            if (arrow != null)
                foreach (IRenderable rObject in arrow.Objects)
                    ((IColorMaterial)rObject.Material).DiffuseColor = color;
        }


        public Vector3 GetBoxOffset()
        {
            switch (Configuration)
            {
                default:
                    return Vector3.Zero;

                case 0:
                    return new Vector3(-frame.Width / 2 + box.Width / 2, box.Height / 2, -frame.Depth / 2 + box.Depth / 2);
                    
                case 1:
                    return new Vector3(frame.Width / 2 - box.Width / 2, box.Height / 2, -frame.Depth / 2 + box.Depth / 2);

                case 2:
                    return new Vector3(-frame.Width / 2 + box.Width / 2, box.Height / 2, frame.Depth / 2 - box.Depth / 2);


            }
        }

        //public Vector3 GetAxisOffset(float frameSide, int axis)
        //{
        //    switch (Configuration)
        //    {
        //        case 0:
        //            switch (axis)
        //            {
        //                    // X
        //                case 0:
        //                    return 
        //            }
        //    }
        //}

        
    }

}
