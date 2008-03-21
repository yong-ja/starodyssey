using System;
using AvengersUtd.Odyssey.Engine.Input;
using SlimDX;
using SlimDX.Direct3D9;


namespace AvengersUtd.Odyssey.Engine
{
    public class QuaternionCam
    {
        Device device;
        Vector3 vPosition;
        Quaternion qOrientation;
        Matrix mView;
        Matrix mProjection;

        int zoomLevel = 0;
        CameraAction states;

        public const float DefaultSpeed = 20f;
        public const float DefaultRotationSpeed = 0.005f;
        public const float DefaultSlerpSpeed = 0.01f;
        public const float DefaultZoomSpeed = 200f;
        static Vector3 YAxis = new Vector3(0f, 1f, 0f);
        static Vector3 XAxis = new Vector3(1f, 0f, 0f);
        static Vector3 ZAxis = new Vector3(0f, 0f, 1f);

        public int ZoomLevel
        {
            get { return zoomLevel; }
            set { zoomLevel = value; }
        }

        public CameraAction States
        {
            get { return states; }
        }

        public Matrix View
        {
            get { return mView; }
        }

        public Vector4 PositionV4
        {
            get
            {
                Vector4 vPos = new Vector4(0, 0, 0, 1f);
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


        public QuaternionCam()
        {
            mProjection = Matrix.PerspectiveFovLH((float) Math.PI/4, 4/3f, 0.1f, 1000.0f);
            Game.Device.SetTransform(TransformState.Projection, mProjection);

            states = new CameraAction();
            Reset();
        }

        public void Reset()
        {
            vPosition = new Vector3();
            qOrientation = Quaternion.Identity;
            mView = Matrix.Identity;
        }

        public void SetCamera(Vector3 vNewPos)
        {
            vPosition = vNewPos;
        }

        public void Update()
        {
            Process(states);
            device = Game.Device;

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
            vPosition += ZAxis*distance*(float) Game.FrameTime;
        }

        public void Strafe(float distance)
        {
            vPosition += XAxis*distance*(float) Game.FrameTime;
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

        public void Slerp(Quaternion targetOrientation)
        {
            qOrientation = Quaternion.Slerp(qOrientation, targetOrientation, DefaultSlerpSpeed);
            qOrientation = Quaternion.Normalize(qOrientation);
        }

        void Process(CameraAction states)
        {
            //System.Diagnostics.Debug.WriteLine("X: " + PositionV4.X + " -Y: " + PositionV4.Y);

            if (states.MoveForward)
                Move(DefaultSpeed);
            if (states.MoveBackward)
                Move(-DefaultSpeed);
            if (states.StrafeLeft)
                Strafe(-DefaultSpeed);
            if (states.StrafeRight)
                Strafe(DefaultSpeed);
            if (states.HoverUp)
                Hover(DefaultSpeed);
            if (states.HoverDown)
                Hover(-DefaultSpeed);
            if (states.RotateLeft)
                Rotate(DefaultRotationSpeed, YAxis);
            if (states.RotateRight)
                Rotate(-DefaultRotationSpeed, YAxis);
        }


        /*	bool CCamera::Slerp(D3DXQUATERNION *pOrientation)
		{ 
			bool bSuccess = false;

			if(pOrientation) // This is the orientation of the target
		{

			if (m_quatOrientation == *pOrientation)
			return false;

			D3DXQUATERNION quatInterpotedRotation;

			// Calculate SLERP 
		
			D3DXQuaternionSlerp( &quatInterpotedRotation , 
			&m_quatOrientation , 
			pOrientation , 
			m_fSlerpSpeed );

			// Apply interpolted rotation
			m_quatOrientation = quatInterpotedRotation;

			D3DXQuaternionNormalize(&m_quatOrientation , &m_quatOrientation);

			bSuccess = true;
			m_bNeedUpdated = true;
		}

			return(bSuccess);
		}*/
    }
}