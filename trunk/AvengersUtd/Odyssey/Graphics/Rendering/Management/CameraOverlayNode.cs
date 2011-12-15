using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class CameraOverlayNode : TransformNode
    {
        private static int count;
        private readonly QuaternionCam camera;


        public CameraOverlayNode(QuaternionCam camera)
            : base(Text.GetCapitalLetters(typeof(CameraOverlayNode).GetType().Name) + '_' + ++count, true)
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
