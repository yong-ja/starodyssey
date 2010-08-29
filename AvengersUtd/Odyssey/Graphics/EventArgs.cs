using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics
{
    public class ResizeEventArgs : EventArgs
    {
        public Size PreviousSize { get; private set; }
        public Size NewSize { get; private set; }
        public bool IsFullScreen { get; private set; }

        public ResizeEventArgs(Size previousSize, Size newSize, bool isFullScreen)
        {
            PreviousSize = previousSize;
            NewSize = newSize;
            IsFullScreen = isFullScreen;
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public bool CancelEvent { get; set; }
        public Vector3 PreviousValue { get; private set; }
        public Vector3 NewValue { get; private set; }

        public new static PositionEventArgs Empty
        {
            get { return new PositionEventArgs(new Vector3(), new Vector3()); }
        }

        public PositionEventArgs(Vector3 previousValue, Vector3 newValue)
        {
            this.PreviousValue = previousValue;
            this.NewValue = newValue;
        }
    }

    public class CollisionEventArgs : EventArgs
    {
        public IRenderable CollidingObject
        {
            get;
            private set;
        }
        public IRenderable CollidedObject
        {
            get;
            private set;
        }

        public bool AllowMovement
        {
            get;
            set;
        }

        public CollisionEventArgs(IRenderable collidingObject, IRenderable collidedObject)
        {
            CollidingObject = collidingObject;
            CollidedObject = collidedObject;
        }
    }

    public class RotationEventArgs : EventArgs
    {
        public bool CancelEvent
        {
            get;
            set;
        }
        public Vector3 RotationDelta
        {
            get;
            private set;
        }

        public Quaternion CurrentRotation
        { get; private set; }

        public RotationEventArgs(Quaternion currentRotation, Vector3 rotationDelta)
        {
            CurrentRotation = currentRotation;
            RotationDelta = rotationDelta;
        }
    }
}
