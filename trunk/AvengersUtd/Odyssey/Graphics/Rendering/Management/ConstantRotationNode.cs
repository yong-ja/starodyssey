using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Utils;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public class ConstantRotationNode : TransformNode
    {
        static int count;
        float rotationDelta;
        double currentRotation;
        Vector3 axis;

        #region Properties
       
        public float RotationDelta
        {
            get { return rotationDelta; }
            set {
                if (rotationDelta != value)
                {
                    rotationDelta = value;
                    UpdateLocalWorldMatrix();
                }
            }
        }

        public Vector3 Axis
        {
            get { return axis; }
            set
            {
                if (axis != value)
                {
                    axis = value;
                    UpdateLocalWorldMatrix();
                }
            }
        }
        #endregion

        public ConstantRotationNode(Vector3 axis, float rotationDelta)
            : base(Text.GetCapitalLetters(typeof(ConstantRotationNode).GetType().Name) + '_' + ++count, true)
        {
            this.rotationDelta = rotationDelta;
            this.axis = axis;
        }

        public override void UpdateLocalWorldMatrix()
        {
            currentRotation += rotationDelta;//* Game.FrameTime;
            LocalWorldMatrix = Matrix.RotationAxis(axis, (float)currentRotation % 360.0f);
        }

    }
}
