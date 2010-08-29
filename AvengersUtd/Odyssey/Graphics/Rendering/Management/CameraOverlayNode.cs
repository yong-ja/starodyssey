using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class CameraOverlayNode : TransformNode
    {
        private const string NodeTag = "CO_";
        private static int count;
        private readonly QuaternionCam camera;

        public CameraOverlayNode()
            : this(Game.CurrentRenderer.Camera)
        {
        }

        public CameraOverlayNode(QuaternionCam camera)
            : base(NodeTag + ++count, true)
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
            LocalWorldMatrix = Matrix.Invert(camera.View);
        }
    }

}
