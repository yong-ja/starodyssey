using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Graphics.Meshes;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Input;
using AvengersUtd.Odyssey.Graphics;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class KeyboardBehaviour : IKeyboardBehaviour
    {
        const float Speed = 5.0f;

        SortedList<Keys, KeyBinding> keyBindings;
        bool[] actions;

        public KeyboardBehaviour()
        {
            keyBindings = new SortedList<Keys, KeyBinding>();
            KeyBinding[] keyArray = new KeyBinding[] {
                //new KeyBinding(KeyAction.MoveForward,  Keys.W, 5f),
                //new KeyBinding(KeyAction.MoveBackward, Keys.S, 5.0f),
                //new KeyBinding(KeyAction.StrafeLeft, Keys.A, 5f),
                //new KeyBinding(KeyAction.StrafeRight, Keys.D, 5.0f)
                 new KeyBinding(KeyAction.MoveForward,  Keys.W, MoveForward),
                new KeyBinding(KeyAction.MoveBackward, Keys.S, MoveBackard),
                new KeyBinding(KeyAction.StrafeLeft, Keys.A, MoveLeft),
                new KeyBinding(KeyAction.StrafeRight, Keys.D, MoveRight)
            };

            foreach (KeyBinding kb in keyArray)
                this.keyBindings.Add(kb.Key, kb);
            actions = new bool[keyBindings.Count()];

            
        }

        public KeyboardBehaviour(IEnumerable<KeyBinding> keyBindings)
        {
            this.keyBindings = new SortedList<Keys, KeyBinding>();
            foreach (KeyBinding kb in keyBindings)
                this.keyBindings.Add(kb.Key, kb);
            actions = new bool[keyBindings.Count()];
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public IRenderable RenderableObject
        {
            get;
            set;
        }


        void IKeyboardBehaviour.OnKeyDown(object sender, KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(true);
        }

        void IKeyboardBehaviour.OnKeyPress(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            return;
        }

        void IKeyboardBehaviour.OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            KeyBinding binding = null;
            if (keyBindings.ContainsKey(e.KeyCode))
                binding = keyBindings[e.KeyCode];

            if (binding != null)
                binding.Apply(false);
        }


        public void Update()
        {
            foreach (KeyValuePair<Keys,KeyBinding> kvp in keyBindings)
            {
                KeyBinding kb = kvp.Value;

                if (kb.State) kb.Operation();
            }
        }

        void IBehaviour.Add()
        {
            IKeyboardBehaviour kBehaviour = ((IKeyboardBehaviour)this);
            //RenderableObject.KeyUp += kBehaviour.OnKeyUp;
            //RenderableObject.KeyDown += kBehaviour.OnKeyDown;
            //RenderableObject.KeyPress += kBehaviour.OnKeyPress;

            Global.FormOwner.KeyDown += kBehaviour.OnKeyDown;
            Global.FormOwner.KeyUp += kBehaviour.OnKeyUp;
        }

        void IBehaviour.Remove()
        {
            IKeyboardBehaviour kBehaviour = ((IKeyboardBehaviour)this);
            //RenderableObject.KeyUp -= kBehaviour.OnKeyUp;
            //RenderableObject.KeyDown -= kBehaviour.OnKeyDown;
            //RenderableObject.KeyPress -= kBehaviour.OnKeyPress;

            Global.FormOwner.KeyDown -= kBehaviour.OnKeyDown;
            Global.FormOwner.KeyUp -= kBehaviour.OnKeyUp;
        }

        public void SetState(KeyAction action, bool state)
        {
            actions[(int)action] = state;
        }


        void MoveLeft()
        {
            RenderableObject.Move(-Speed, Vector3.UnitX);
        }
        void MoveRight()
        {
            RenderableObject.Move(Speed, Vector3.UnitX);
        }

        void MoveForward()
        {
            RenderableObject.Move(Speed, Vector3.UnitZ);
        }
        void MoveBackard()
        {
            RenderableObject.Move(-Speed, Vector3.UnitZ);
        }
    }
}
