using System;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.Utils;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class CameraAnchorNode : TransformNode
    {
        private static int count;
        private readonly ICamera camera;

        public CameraAnchorNode()
            : this(Game.CurrentRenderer.Camera)
        {
        }

        public CameraAnchorNode(ICamera camera)
            : base(Text.GetCapitalLetters(typeof(CameraAnchorNode).GetType().Name) + '_' + ++count, true)
        {
            this.camera = camera;
            camera.CameraMoved += CameraMoved;
        }

        void CameraMoved(object sender, EventArgs e)
        {
            UpdateLocalWorldMatrix();
        }

        public override void UpdateLocalWorldMatrix()
        {
            LocalWorldMatrix = Matrix.Translation(camera.PositionV3);
            //LocalWorldMatrix = Matrix.Invert(camera.View);
            //Matrix mTemp = camera.View;
            //mTemp.M41 = mTemp.M42 = mTemp.M43 = 0.0f;
            //mTemp.M44 = 1.0f;
            //LocalWorldMatrix = mTemp;
        }
    }

}
