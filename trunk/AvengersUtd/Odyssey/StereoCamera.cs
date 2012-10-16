using System;
using System.ComponentModel;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Utils.Logging;

namespace AvengersUtd.Odyssey
{
    enum StereoSide
    {
        Left, Right
    }

    public struct StereoParameters
    {
        const float DefaultViewerDistance = 24f;
        const float DefaultInterocularDistance = 1.25f;
        public float ViewportWidth { get; private set; }
        public float ViewportHeight { get; private set; }
        //public float WorldScale { get; private set; }
        //public float StereoExaggeration { get; private set; }
        public float ViewerDistance { get; private set; }
        public float InterocularDistance { get; private set; }

        public StereoParameters(float viewportWidth, float viewportHeight, float worldScale, float stereoExaggeration):this()
        {
            ViewportWidth = viewportWidth / worldScale;
            ViewportHeight = viewportHeight / worldScale;
            ViewerDistance = DefaultViewerDistance / worldScale;
            InterocularDistance = DefaultInterocularDistance / worldScale * stereoExaggeration;

        }
    }

    public class StereoCamera : QuaternionCam
    {

        public const float DefaultIncrement = 0.25f;
        const float DefaultConvergence = -6f;
        const float DefaultSeparation = 0.02f;
        float separation;


        float convergence;
        private Matrix mStereoLeftProjection;
        private Matrix mStereoRightProjection;

        public event EventHandler<EventArgs> StereoParametersChanged;        

        #region Properties

        public Matrix LeftProjection
        {
            get { return mStereoLeftProjection; }
        }

        public Matrix RightProjection
        {
            get { return mStereoRightProjection; }
        }

        public float Separation
        {
            get { return separation; }
            set {

                if (separation != value)
                    BuildStereoProjectionMatrices();
                separation = value;
            }
        }

        public float Convergence
        {
            get { return convergence; }
            set
            {
                if (convergence != value)
                    BuildStereoProjectionMatrices();
                convergence = value;
            }
        }

        #endregion

        protected void OnStereoParametersChanged(EventArgs e)
        {
            BuildStereoProjectionMatrices();
            EventHandler<EventArgs> handler = StereoParametersChanged;
            if (handler != null)
                handler(this, e);
        }

        protected override void OnCameraReset(object sender, EventArgs e)
        {
            base.OnCameraReset(sender, e);
            BuildStereoProjectionMatrices();
        }
      
        public StereoCamera()
        {
            separation = DefaultSeparation;
            convergence = DefaultConvergence;
        }

        public void BuildStereoProjectionMatrices()
        {
            Matrix mProjection = Matrix.PerspectiveFovLH((float)Math.PI / 4, Game.Context.Settings.AspectRatio, NearClip, FarClip);
            float m11 = mProjection.M11;
            float m13 = mProjection.M13;
            float m22 = mProjection.M22;
            float m23 = mProjection.M23;
            float m31 = mProjection.M31;
            float m32 = mProjection.M32;
            float m33 = mProjection.M33;
            float m34 = mProjection.M34;
            float m43 = mProjection.M43;

            mStereoLeftProjection = new Matrix
                                        {
                                            M11 = m11,
                                            M22 = m22,
                                            M23 = m23,
                                            M31 = m31 - separation,
                                            M33 = m33,
                                            M34 = 1,
                                            M41 = -separation * convergence,
                                            M43 = m43
                                        };

            mStereoRightProjection = new Matrix
            {
                M11 = m11,
                M22 = m22,
                M23 = m23,
                M31 = m31 + separation,
                M33 = m33,
                M34 = 1,
                M41 = separation * convergence,
                M43 = m43
            };
        }

        public void EnableLeftStereoProjection()
        {
            Projection = LeftProjection;
        }

        public void EnableRightStereoProjection()
        {
            Projection = RightProjection;
        }


        public void ModifyConvergence(float value)
        {
            convergence += value;
            LogEvent.Engine.Log(string.Format("Convergence set to {0:f4}", convergence));
            OnStereoParametersChanged(EventArgs.Empty);
        }

        public void ModifySeparation(float value)
        {
            separation += value;
            LogEvent.Engine.Log(string.Format("Separation set to {0:f4}", separation));
            OnStereoParametersChanged(EventArgs.Empty);
        }


        
    }


}