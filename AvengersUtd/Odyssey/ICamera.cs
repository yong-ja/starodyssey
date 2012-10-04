using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace AvengersUtd.Odyssey
{
    public interface ICamera
    {
        event EventHandler CameraMoved;
        event EventHandler CameraReset;
        Matrix World { get; }
        Matrix Projection { get; }
        Matrix View { get; }
        Matrix WorldViewProjection { get; }
        Matrix OrthoProjection { get; }
        Matrix Rotation { get; }
        Viewport Viewport { get; }
        Vector3 ViewVector { get; }
        Vector3 PositionV3 { get; set; }

        float NearClip { get; }
        float FarClip { get; }

        void Reset();
        void ChangeScreenSize(float width, float height);
        void LookAt(Vector3 to, Vector3 from);
        void Update();
        void Move(float distance);
        void Strafe(float distance);
        void Hover(float distance);
        void Rotate(float angle, Vector3 vAxis);
        void RotateY(float angle);
    }
}
