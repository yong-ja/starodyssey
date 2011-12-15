using AvengersUtd.Odyssey.Utils;
using AvengersUtd.Odyssey.Utils.Collections;
using SlimDX;

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

        public override void Init()
        {
            base.Init();
            UpdateLocalWorldMatrix();
        }

        public override void UpdateLocalWorldMatrix()
        {
            float yaw = Rotation.X;
            float pitch = Rotation.Y;
            float roll = Rotation.Z;

            Matrix mRotation = Matrix.RotationYawPitchRoll(yaw, pitch, roll);
            Matrix mTranslation = Matrix.Translation(Position);
            Matrix mScaling = Matrix.Scaling(Scaling);
            LocalWorldMatrix = mRotation*mTranslation*mScaling;
        }

        protected override void OnParentChanged(object sender, NodeEventArgs e)
        {
            base.OnParentChanged(sender, e);
            UpdateAbsoluteWorldMatrix();
        }

    }
}
