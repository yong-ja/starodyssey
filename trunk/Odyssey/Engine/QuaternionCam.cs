using System;
using AvengersUtd.Odyssey.Input;
using SlimDX;
using SlimDX.Direct3D9;
using SlimDX.XInput;
using AvengersUtd.Odyssey.UserInterface.Devices;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;


namespace AvengersUtd.Odyssey
{
    public class QuaternionCam
    {
        bool[] actions;
        Device device;
        Vector3 vPosition;

        Quaternion qOrientation;
        Matrix mView;
        Matrix mProjection;

        int zoomLevel = 0;

        public const float DefaultSpeed = 20f;
        public const float DefaultRotationSpeed = 0.005f;
        public const float DefaultSlerpSpeed = 0.01f;
        public const float DefaultZoomSpeed = 200f;
        static Vector3 YAxis = new Vector3(0f, 1f, 0f);
        static Vector3 XAxis = new Vector3(1f, 0f, 0f);
        static Vector3 ZAxis = new Vector3(0f, 0f, 1f);

        #region Properties
        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }

        public Matrix World
        {
            get { return device.GetTransform(TransformState.World); }
        }

        public Matrix View
        {
            get { return mView; }
        }

        public Vector4 PositionV4
        {
            get
            {
                Vector4 vPos = new Vector4(vPosition.X, vPosition.Y, vPosition.Z, 1f);
                //Vector4 vPos = new Vector4(0, 0, 0, 1f);
                //vPos = Vector4.Transform(vPos, Matrix.Invert(mView));
                return vPos;
            }
        }

        public Vector3 PositionV3
        {
            get { return vPosition; }
            set { vPosition = value; }
        }

        public Matrix Projection
        {
            get { return mProjection; }
        }

        Vector3 ViewVector
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
        #endregion


        public QuaternionCam()
        {
            actions = new bool[8];
            mProjection = Matrix.PerspectiveFovLH((float) Math.PI/4, 4/3f, 0.01f, 100000.0f);
            Game.Device.SetTransform(TransformState.Projection, mProjection);

            Reset();
        }

        public void Reset()
        {
            device = Game.Device;
            vPosition = new Vector3();
            qOrientation = Quaternion.Identity;
            mView = Matrix.Identity;
        }

        public void SetCamera(Vector3 vNewPos)
        {
            vPosition = vNewPos;
        }

        public void UpdateStates()
        {
            if (actions[(int)CameraAction.MoveForward])
                Move(DefaultSpeed);
            if (actions[(int)CameraAction.MoveBackward])
                Move(-DefaultSpeed);
            if (actions[(int)CameraAction.StrafeLeft])
                Strafe(-DefaultSpeed);
            if (actions[(int)CameraAction.StrafeRight])
                Strafe(DefaultSpeed);
            if (actions[(int)CameraAction.HoverUp])
                Hover(-DefaultSpeed);
            if (actions[(int)CameraAction.HoverDown])
                Hover(DefaultSpeed);
            if (actions[(int)CameraAction.RotateLeft])
                RotateY(DefaultRotationSpeed);
            if (actions[(int)CameraAction.RotateRight])
                RotateY(-DefaultRotationSpeed);
        }

        public void Update()
        {

            Matrix mTranslation = Matrix.Translation(-vPosition.X, -vPosition.Y, -vPosition.Z);
            Matrix mRotation = Matrix.RotationQuaternion(qOrientation);
            mView = mTranslation*mRotation;
            device.SetTransform(TransformState.View,mView);
        }

        public void LookAt(Vector3 vTo, Vector3 vFrom)
        {
            vPosition = vFrom;
            Matrix r = Matrix.LookAtLH(vFrom, vTo, GetCameraAxis(YAxis));

            qOrientation = Quaternion.RotationMatrix(r);
        }

        public void Move(float distance)
        {
            vPosition += ViewVector * distance * (float)Game.FrameTime;
        }

        public void Strafe(float distance)
        {
            vPosition +=AxisX * distance * (float)Game.FrameTime;
        }

        public void Hover(float distance)
        {
            vPosition += YAxis*distance*(float) Game.FrameTime;
        }

        public void Rotate(float angle, Vector3 vAxis)
        {
            //Vector4 vNewAxis = Vector3.Transform(vAxis, Matrix.RotationQuaternion(qOrientation));
            //Quaternion qRotation = Quaternion.RotationAxis(new Vector3(
            //    vNewAxis.X, vNewAxis.Y, vNewAxis.Z), (float)(angle * Game.FrameTime));

            Quaternion qRotation = Quaternion.RotationAxis(GetCameraAxis(vAxis), angle);
            qOrientation *= qRotation;
            //qOrientation.Normalize();
        }

        public void RotateY(float angle)
        {
            Rotate(angle, YAxis);
        }

        public Vector3 GetCameraAxis(Vector3 axisVector)
        {
            Vector3 cameraAxis = new Vector3();
            Matrix cameraRotation;

            cameraRotation = Matrix.RotationQuaternion(qOrientation);

            cameraAxis.X = axisVector.X*cameraRotation.M11 + axisVector.Y*cameraRotation.M21 +
                           axisVector.Z*cameraRotation.M31 + cameraRotation.M41;
            cameraAxis.Y = axisVector.X*cameraRotation.M12 + axisVector.Y*cameraRotation.M22 +
                           axisVector.Z*cameraRotation.M32 + cameraRotation.M42;
            cameraAxis.Z = axisVector.X*cameraRotation.M13 + axisVector.Y*cameraRotation.M23 +
                           axisVector.Z*cameraRotation.M33 + cameraRotation.M43;

            return cameraAxis;
        }

        public void SetState(CameraAction action, bool state)
        {
            actions[(int)action] = state;   
        }

        public void FirstPersonCameraWithGamepad(XBox360Controller gamepad)
        {
            Vector3 vView = ViewVector;
            Vector3 vDelta = new Vector3(vView.X * gamepad.LeftThumbstick.X,
                vView.Y, vView.Z* gamepad.LeftThumbstick.Y);
            vPosition += vDelta * DefaultSpeed *  (float)Game.FrameTime;
        }

        public void Slerp(Quaternion targetOrientation)
        {
            qOrientation = Quaternion.Slerp(qOrientation, targetOrientation, DefaultSlerpSpeed);
            qOrientation = Quaternion.Normalize(qOrientation);
        }

       
    }
}