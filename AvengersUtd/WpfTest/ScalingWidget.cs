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

        public const float ArrowIntersectionRadius = 320;
        IBox box;
        IBox frame;
        Arrow YArrow;
        Arrow XArrow;
        Arrow ZArrow;
        Vector3 scaling;

        public bool XInverted { get; private set; }
        public bool ZInverted { get; private set; }
        public bool YInverted { get; private set; }

        public int Configuration { get; private set; }

        public void SetFrame(IBox frame)
        {
            this.frame = frame;
        }

        public ScalingWidget(Box targetObject)
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

            scaling = targetObject.ScalingValues;

            Material = new PhongMaterial() { DiffuseColor = Color.Yellow, AmbientCoefficient=1f};
            YArrow.PositionV3 = new Vector3(-scaling.X / 2 - ArrowLineWidth / 2, scaling.Y / 2, -scaling.Z / 2 - ArrowLineWidth / 2);
            XArrow.PositionV3 = new Vector3(scaling.X / 2 + ArrowLineWidth / 2, -scaling.Y / 2 + ArrowLineWidth / 2, -scaling.Z / 2 - ArrowLineWidth / 2);
            ZArrow.PositionV3 = new Vector3(-scaling.X / 2 - ArrowLineWidth / 2, -scaling.Y / 2 + ArrowLineWidth / 2, scaling.Z / 2 + ArrowLineWidth / 2);

            //YArrow.PositionV3 = new Vector3(-box.Scaling / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //XArrow.PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, box.Depth / 2 + ArrowLineWidth / 2);

            
        }

        public void ChooseArrangement(bool arrow1Fw, bool arrow2Fw, bool arrow3Fw)
        {

            if (arrow1Fw)
                XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI / 2f);
            else
                XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, (float)Math.PI / 2f);

            if (arrow2Fw)
                YArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0,0 );
            else
                YArrow.CurrentRotation = Quaternion.RotationYawPitchRoll((float)Math.PI/2, (float)Math.PI, 0);

            if (arrow3Fw)
                ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, (float)Math.PI/2f,0);
            else
                ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, -(float)Math.PI / 2f, 0f);

            XInverted = !arrow1Fw;
            YInverted = !arrow2Fw;
            ZInverted = !arrow3Fw;

            //switch (Configuration)
            //{
            //    default:
            //    case 0:
            //        YArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI / 2f);
            //        ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, box.Depth / 2 + ArrowLineWidth / 2);
            //        ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, (float)Math.PI / 2f, 0);
            //    break;

            //        // Tangent to the rightmost edge
            //    case 1:
            //        YArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.PositionV3 = new Vector3(-box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, (float)Math.PI / 2f);
            //        ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, box.Depth / 2 + ArrowLineWidth / 2);
            //        ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0,(float)Math.PI / 2f, 0);
            //        XInverted = true;
            //        break;

            //        // Tangent to the leftmost edge
            //    case 2:
            //        YArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, box.Height / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.PositionV3 = new Vector3(box.Width / 2 + ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        XArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, 0, -(float)Math.PI / 2f);
            //        ZArrow.PositionV3 = new Vector3(-box.Width / 2 - ArrowLineWidth / 2, -box.Height / 2 + ArrowLineWidth / 2, -box.Depth / 2 - ArrowLineWidth / 2);
            //        ZArrow.CurrentRotation = Quaternion.RotationYawPitchRoll(0, -(float)Math.PI / 2f, 0);
            //        break;
            //}
        
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
                if (!((Arrow)rObject).IsTouched)
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
                    return new Vector3(-frame.Width / 2 + scaling.X / 2, scaling.Y / 2, -frame.Depth / 2 + scaling.Z / 2);
                    //return new Vector3(-frame.Width / 2 + box.Width / 2, box.Height / 2, -frame.Depth / 2 + box.Depth / 2);
                    
                case 1:
                    return new Vector3(frame.Width / 2 - box.Width / 2, box.Height / 2, -frame.Depth / 2 + box.Depth / 2);

                case 2:
                    return new Vector3(-frame.Width / 2 + box.Width / 2, box.Height / 2, frame.Depth / 2 - box.Depth / 2);

                //case 1:
                //    return new Vector3(frame.Width / 2 - scaling.X / 2, scaling.Y / 2, -frame.Depth / 2 + scaling.Z / 2);

                //case 2:
                //    return new Vector3(-frame.Width / 2 + scaling.X / 2, scaling.Y / 2, frame.Depth / 2 - scaling.Z / 2);
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
