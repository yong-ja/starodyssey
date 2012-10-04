using System;
using System.ComponentModel;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Settings;
using SlimDX;
using SlimDX.Direct3D11;

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
        const float DefaultConvergence = -6f;
        const float DefaultSeparation = 0.02f;
        float separation;
        float convergence;
        private Matrix mStereoLeftProjection;
        private Matrix mStereoRightProjection;

        #region Properties

        public Matrix LeftProjection
        {
            get { return mStereoLeftProjection; }
        }

        public Matrix RightProjection
        {
            get { return mStereoRightProjection; }
        }

        #endregion
      
        public StereoCamera()
        {
            BuildStereoProjectionMatrices();
        }

        public void BuildStereoProjectionMatrices(float separation = DefaultSeparation, float convergence=DefaultConvergence)
        {
            Matrix mProjection = Projection;
            float m11 = mProjection.M11;
            float m13 = mProjection.M13;
            float m22 = mProjection.M22;
            float m23 = mProjection.M23;
            float m31 = mProjection.M31;
            float m32 = mProjection.M32;
            float m33 = mProjection.M33;
            float m34 = mProjection.M34;
            float m43 = mProjection.M43;

            StereoParameters parameters = new StereoParameters(1920f / 96f, 1080f / 96f, 12f, 1.0f);

            //float yScale = 2f * parameters.ViewerDistance / parameters.ViewportHeight;
            //float xScale = 2f * parameters.ViewerDistance / parameters.ViewportWidth;

            //float mFactor = -parameters.InterocularDistance / parameters.ViewportWidth;

            //float m22 = FarClip / (NearClip - FarClip);

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

            //xScale = yScale = 1;

            //mStereoLeftProjection = new Matrix
            //{
            //    M11 = xScale,
            //    M22 = yScale,
            //    M31 = -mFactor,
            //    M33 = m22,
            //    M34 = -1,
            //    M41 = parameters.ViewerDistance * -mFactor,
            //    M43 = NearClip * m22
            //};

            //mStereoRightProjection = new Matrix
            //{
            //    M11 = xScale,
            //    M22 = yScale,
            //    M31 = mFactor,
            //    M33 = m22,
            //    M34 = -1,
            //    M41 = parameters.ViewerDistance * mFactor,
            //    M43 = NearClip * m22
            //};

        }

        public void EnableLeftStereoProjection()
        {
            Projection = LeftProjection;
        }

        public void EnableRightStereoProjection()
        {
            Projection = RightProjection;
        }



        
    }


}