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
    public class QuaternionCam : ICamera
    {
        //BoundingFrustum frustum;
        Vector3 vPosition;
        Quaternion qOrientation;
        readonly float nearClip;
        readonly float farClip;

        private Matrix mWorld;
        Matrix mView;
        Matrix mProjection;
        private Matrix mOrthoProjection;

        public const float DefaultSpeed = 10f;
        public const float DefaultRotationSpeed = 1f;
        public const float DefaultSlerpSpeed = 0.01f;
        public const float DefaultZoomSpeed = 200f;
        static readonly Vector3 YAxis = new Vector3(0f, 1f, 0f);
        static Vector3 XAxis = new Vector3(1f, 0f, 0f);
        static Vector3 ZAxis = new Vector3(0f, 0f, 1f);
        
        #region Events
        public event EventHandler CameraMoved;
        public event EventHandler CameraReset;

        protected virtual void OnCameraMoved(object sender, EventArgs e)
        {
            EventHandler handler = CameraMoved;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnCameraReset(object sender, EventArgs e)
        {
            EventHandler handler = CameraReset;
            if (handler != null)
                handler(this, e);
        }
        #endregion

        #region Properties

        public int ZoomLevel { get; set; }
        internal bool ShouldUpdateFrustum { get; private set; }

        public float NearClip
        {
            get { return nearClip; }
        }

        public float FarClip
        {
            get { return farClip; }
        }

        public Viewport Viewport { get; private set; }

        //public BoundingFrustum Frustum
        //{
        //    get { return frustum; }
        //}

        public Matrix World
        {
            get { return mWorld; }
        }

        public Matrix WorldViewProjection
        {
            get { return mWorld*mView*mProjection; }
        }

        public Matrix View
        {
            get { return mView; }
        }

        public Matrix Rotation
        {
            get
            {
                Matrix mRotation = mView;
                mRotation.M41 = mRotation.M42 = mRotation.M43 = 0.0f;
                mRotation.M44 = 1.0f;
                return mRotation;
            }
        }

        public Vector4 PositionV4
        {
            get { return vPosition.ToVector4(); }
        }

        public Vector3 PositionV3
        {
            get { return vPosition; }
            set { vPosition = value; }
        }

        public Quaternion Orientation
        {
            get { return qOrientation; }
        }

        public Matrix Projection
        {
            get { return mProjection; }
            protected set { mProjection = value; }
        }

        public Vector3 ViewVector
        {
            get { return new Vector3(mView.M13, mView.M23, mView.M33); }
        }

        Vector3 AxisX
        {
            get { return new Vector3(mView.M11, mView.M21, mView.M31); }
        }

        Vector3 AxisY
        {
            get { return new Vector3(mView.M12, mView.M22, mView.M32); }
        }

        Vector3 AxisZ
        {
            get { return new Vector3(mView.M13, mView.M23, mView.M33); }
        }

        public Matrix OrthoProjection
        {
            get {
                return mOrthoProjection;
            }
        }

        #endregion

        public QuaternionCam()
        {
            ZoomLevel = 0;
            nearClip = 0.1f;
            farClip = 100.0f;
            mProjection = Matrix.PerspectiveFovLH((float) Math.PI/4, Game.Context.Settings.AspectRatio, nearClip, farClip);
            
            mOrthoProjection = Matrix.OrthoLH(Game.Context.Settings.ScreenWidth, Game.Context.Settings.ScreenHeight, nearClip, farClip);
            Viewport = new SlimDX.Direct3D11.Viewport(0, 0, Game.Context.Settings.ScreenWidth, Game.Context.Settings.ScreenHeight,
                                                      nearClip, farClip);
        }

        public void Reset()
        {
            vPosition = new Vector3();
            qOrientation = Quaternion.Identity;
            mWorld = mView = Matrix.Identity;
            OnCameraReset(this, EventArgs.Empty);
        }

        public void SetCamera(Vector3 vNewPos)
        {
            vPosition = vNewPos;
        }

        /// <summary>
        /// Updates the camera view matrix. Should be called once per frame.
        /// </summary>
        public void Update()
        {
            Matrix mTranslation = Matrix.Translation(-vPosition.X, -vPosition.Y, -vPosition.Z);
            Matrix mRotation = Matrix.RotationQuaternion(qOrientation);
            mView = mTranslation*mRotation;
            
            if (ShouldUpdateFrustum)
            {
                //frustum.SetMatrix(mView * mProjection);
                ShouldUpdateFrustum = false;
            }
        }

        public void LookAt(Vector3 vTo, Vector3 vFrom)
        {
            vPosition = vFrom;
            Matrix r = Matrix.LookAtLH(vFrom, vTo, GetCameraAxis(YAxis));

            qOrientation = Quaternion.RotationMatrix(r);
        }

        public void Move(float distance, Vector3 direction)
        {
            vPosition += direction * distance * (float)Game.FrameTime;
            ShouldUpdateFrustum = true;
        }


        public void Move(float distance)
        {
            Move(distance, ViewVector);
        }

        public void Strafe(float distance)
        {
            Move(distance, AxisX);
        }

        public void Hover(float distance)
        {
            Move(distance, YAxis);
        }

        public void Rotate(float angle, Vector3 vAxis)
        {
            Quaternion qRotation = Quaternion.RotationAxis(GetCameraAxis(vAxis), angle * (float)Game.FrameTime);
            qOrientation *= qRotation;
        }

        public void RotateY(float angle)
        {
            Rotate(angle, YAxis);
        }

        public Vector3 GetCameraAxis(Vector3 axisVector)
        {
            Vector3 cameraAxis = new Vector3();
            Matrix cameraRotation = Matrix.RotationQuaternion(qOrientation);

            cameraAxis.X = axisVector.X*cameraRotation.M11 + axisVector.Y*cameraRotation.M21 +
                           axisVector.Z*cameraRotation.M31 + cameraRotation.M41;
            cameraAxis.Y = axisVector.X*cameraRotation.M12 + axisVector.Y*cameraRotation.M22 +
                           axisVector.Z*cameraRotation.M32 + cameraRotation.M42;
            cameraAxis.Z = axisVector.X*cameraRotation.M13 + axisVector.Y*cameraRotation.M23 +
                           axisVector.Z*cameraRotation.M33 + cameraRotation.M43;

            return cameraAxis;
        }

        public void ChangeScreenSize(float width, float height)
        {
            mOrthoProjection = Matrix.OrthoLH(width, height, nearClip, farClip);
            mProjection = Matrix.PerspectiveFovLH((float)Math.PI / 4, width/height, nearClip, farClip);
        }

    }


}