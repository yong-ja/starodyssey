using System.Windows.Forms;

namespace AvengersUtd.Odyssey.Engine.Input
{
    public class CameraAction : IActionStates
    {
        public const string MoveForwardID = "MoveForward";
        public const string MoveBackwardID = "MoveBackward";
        public const string RotateLeftID = "RotateLeft";
        public const string RotateRightID = "RotateRight";
        public const string StrafeLeftID = "StrafeLeft";
        public const string StrafeRightID = "StrafeRight";
        public const string HoverUpID = "HoverUp";
        public const string HoverDownID = "HoverDown";

        bool moveForward;
        bool moveBackward;
        bool rotateLeft;
        bool rotateRight;
        bool strafeLeft;
        bool strafeRight;
        bool hoverUp;
        bool hoverDown;

        #region Properties

        public bool MoveForward
        {
            get { return moveForward; }
            set { moveForward = value; }
        }

        public bool MoveBackward
        {
            get { return moveBackward; }
            set { moveBackward = value; }
        }

        public bool RotateLeft
        {
            get { return rotateLeft; }
            set { rotateLeft = value; }
        }

        public bool RotateRight
        {
            get { return rotateRight; }
            set { rotateRight = value; }
        }

        public bool StrafeLeft
        {
            get { return strafeLeft; }
            set { strafeLeft = value; }
        }

        public bool StrafeRight
        {
            get { return strafeRight; }
            set { strafeRight = value; }
        }

        public bool HoverUp
        {
            get { return hoverUp; }
            set { hoverUp = value; }
        }

        public bool HoverDown
        {
            get { return hoverDown; }
            set { hoverDown = value; }
        }

        #endregion

        public void Reset()
        {
            MoveForward = MoveBackward = RotateLeft = RotateRight = StrafeLeft = StrafeRight =
                                                                                 HoverUp = HoverDown = false;
        }

        public void ProcessEvent(bool[] keystate)
        {
            moveForward = keystate[(int) Keys.W];
            moveBackward = keystate[(int) Keys.S];
            strafeLeft = keystate[(int) Keys.A];
            strafeRight = keystate[(int) Keys.D];
            hoverUp = keystate[(int) Keys.Q];
            hoverDown = keystate[(int) Keys.E];
            rotateLeft = keystate[(int) Keys.Z];
            rotateRight = keystate[(int) Keys.C];
        }
    }
}