using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Devices;
using System.Windows.Forms;
using SlimDX.XInput;
using AvengersUtd.Odyssey.UserInterface.Helpers;

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

    public class CameraBinding
    {
        public delegate void CameraTransformState(CameraAction action, bool state);
        Keys key;
        CameraTransformState TransformState;
        CameraAction action;
        float amount;

        public Keys Key
        {
            get { return key; }
        }

        public void Apply(bool state)
        {
            TransformState(action,state);
        }

        public CameraBinding(Keys key, CameraAction action, CameraTransformState transform, float amount)
        {
            this.key = key;
            this.action = action;
            this.TransformState = transform;
            this.amount = amount;
        }
    }

    public class CameraHostControl: ContainerControl
    {
        static Keyboard keyboard = Keyboard.Instance;
        SortedList<Keys, CameraBinding> keyBindings;

        public CameraHostControl()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
            keyBindings = new SortedList<Keys, CameraBinding>();
            IsFocusable = true;
        }

        public void SetBinding(CameraBinding binding)
        {
            if (!keyBindings.ContainsKey(binding.Key))
                keyBindings.Add(binding.Key, binding);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            CameraBinding binding=null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(true);

       }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            CameraBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(false);
        }

    }
}
