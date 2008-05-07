using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Devices;
using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public enum CameraAction
    {
        MoveForward,
        MoveBackward,
        RotateLeft,
        RotateRight,
        StrafeLeft,
        StrafeRight,
        HoverUp,
        HoverDown
    }

    public struct CameraBinding
    {
        public delegate void CameraTransform(float amount);
        Keys key;
        CameraTransform Transform;
        CameraAction action;
        float amount;

        public Keys Key
        {
            get { return key; }
        }

        public void Apply()
        {
            Transform(amount);
        }

        public CameraBinding(Keys key, CameraAction action, CameraTransform transform, float amount)
        {
            this.key = key;
            this.action = action;
            this.Transform = transform;
            this.amount = amount;
        }
    }

    public abstract class CameraHostControl: ContainerControl
    {
        static Keyboard keyboard = Keyboard.Instance;
        SortedList<Keys, CameraBinding> keyBindings;

        public CameraHostControl()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
            keyBindings = new SortedList<Keys, CameraBinding>();
        }

        public void SetBinding(CameraBinding binding)
        {
            if (!keyBindings.ContainsKey(binding.Key))
                keyBindings.Add(key, binding);
        }

        public void ProcessInput()
        {
            foreach (CameraBinding binding in keyBindings.Values)
            {
                if (keyboard[binding.Key])
                    binding.Apply();
            }
        }

    }
}
