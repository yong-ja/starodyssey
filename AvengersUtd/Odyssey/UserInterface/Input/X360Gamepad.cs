using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SlimDX.XInput;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public class X360Gamepad
    {
        private static Controller[] controllers;
        private static int count;
        private readonly int index;
        private readonly Thread pollingThread;
        private State state;

        protected Controller Controller
        {
            get { return controllers[index]; }
        }

        public bool IsConnected
        {
            get { return Controller.IsConnected; }
        }

        static X360Gamepad()
        {
            controllers = new Controller[4];
        }

        public X360Gamepad() : this(UserIndex.Any)
        {
        }

        static int FindUnactivatedIndex()
        {
            for (int i = 0; i < controllers.Length; i++)
            {
                if (controllers[i] == null)
                    return i;
            }
            return -1;
        }

        public X360Gamepad(UserIndex userIndex)
        {
            switch (userIndex)
            {
                case UserIndex.Any:
                    index = FindUnactivatedIndex();
                    index = index == -1 ? 0 : index;
                    userIndex = (UserIndex)index;
                    break;
                default:
                    index = (int)userIndex;
                    count++;
                    break;
            }

            index = count;
            controllers[count] = new Controller(userIndex);
        }

        

        

        public State GetState()
        {
            return Controller.GetState(); 
        }


        
    }
}
