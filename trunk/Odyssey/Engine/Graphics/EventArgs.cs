﻿using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using SlimDX;

namespace AvengersUtd.Odyssey.Graphics
{
    public class PositionEventArgs : EventArgs
    {
        public bool CancelEvent { get; set; }
        public Vector3 PreviousValue { get; set; }

        public Vector3 NewValue { get; set; }

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