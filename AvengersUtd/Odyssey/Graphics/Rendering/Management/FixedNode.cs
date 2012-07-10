using AvengersUtd.Odyssey.Utils;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;
using System;

namespace AvengersUtd.Odyssey.Graphics.Rendering.Management
{
    public sealed class FixedNode : TransformNode
    {
        static int count;

        #region Constructors
        public FixedNode()
            : base(Text.GetCapitalLetters(typeof(FixedNode).GetType().Name) + '_' + ++count, false)
        {}
        #endregion

        public override void UpdateLocalWorldMatrix()
        {
            Matrix mRotationCenter = Matrix.Translation(RotationCenter);
            Matrix mRotation = Matrix.RotationQuaternion(Rotation);
            Matrix mTranslation = Matrix.Translation(Position);
            Matrix mScaling = Matrix.Scaling(Scaling);

            LocalWorldMatrix = mScaling*mRotationCenter* mRotation*mTranslation
        }

    }
}
