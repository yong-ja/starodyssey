using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.Graphics.Meshes;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Input;
using AvengersUtd.Odyssey.Utils.Logging;
using SlimDX;
using SlimDX.XInput;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class FreeMovementGamepadBehaviour : IGamepadBehaviour
    {
        private const short MinValue = -32767;
        private const short MaxValue = 32767;
        private Thread pollingThread;
        private readonly X360Gamepad controller;
        private IRenderable rObject;
        private State state;
        private readonly float speed;
        private Vector2 vDelta;
        private Vector2 vCurrentValue;
        private Vector2 vLastValue;

        public FreeMovementGamepadBehaviour(IRenderable rObject, float speed)
        {
            this.rObject = rObject;
            this.speed = speed;
            controller = new X360Gamepad();

            Start();

        }

        public void Start()
        {
            pollingThread = new Thread(Loop) { Name = "GamepadThread" };
            pollingThread.Start();
        }


        void Loop()
        {
            while (controller.IsConnected)
            {

                if (!Game.IsInputEnabled)
                {
                    Thread.Sleep(100);
                    continue;
                }

                state = controller.GetState();
                ComputeDelta();


                //LogEvent.UserInterface.Write(vDelta.ToString());
                ((TransformNode)(rObject.ParentNode.Parent)).Position += new Vector3(vCurrentValue.X, 0, vCurrentValue.Y);
                Thread.Sleep(10);
            }
        }

        void ComputeDelta()
        {
            float lX = state.Gamepad.LeftThumbX;
            float lY = state.Gamepad.LeftThumbY;

            lX = Math.Abs(lX) <= Gamepad.GamepadLeftThumbDeadZone ? 0 : lX;
            lY = Math.Abs(lY) <= Gamepad.GamepadLeftThumbDeadZone ? 0 : lY;

            float flX = (float) MathHelper.ConvertRange(MinValue, MaxValue, -1.0, 1.0, lX) * (float)Game.FrameTime * speed;
            float flY = (float)MathHelper.ConvertRange(MinValue, MaxValue, -1.0, 1.0, lY) * (float)Game.FrameTime * speed;

            vCurrentValue = new Vector2(flX, flY);
            //vDelta = vCurrentValue - vLastValue;
            //vLastValue = vCurrentValue;
        }

        void IGamepadBehaviour.OnButtonPress(object sender, Input.GamepadEventArgs e)
        {
            throw new NotImplementedException();
        }

        


    }
}
