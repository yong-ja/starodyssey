using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Input;
using SlimDX.Direct3D9;
using Timer=AvengersUtd.Odyssey.UserInterface.Helpers.Timer;
using SlimDX;

namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// Descrizione di riepilogo per Game.
    /// </summary>
    public class Game
    {
        static Device device;
        static Renderer currentScene;
        static double frameTime;
        static InputHandler input;
        static Timer timer = new Timer();

        public static double FrameTime
        {
            get { return frameTime; }
        }

        public static InputHandler Input
        {
            get
            {
                if (input == null)
                    input = new InputHandler(Global.FormOwner);
                return input;
            }
        }

        public static Renderer CurrentScene
        {
            get { return currentScene; }
            set { currentScene = value; }
        }

        public static Device Device
        {
            get { return device; }
            set { device = value; }
        }

        public static double GetFrameTime()
        {
            return frameTime;
        }

    

        public static void Loop()
        {
            //Clear the render target
            timer.GetElapsedTime();
            Application.DoEvents();
            if (device.Disposed)
                return;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,Color.Black,
                         1.0f, 0);

            currentScene.ProcessInput();

            device.BeginScene();

            currentScene.Render();

            //Signal the device that we're done with our scene
            device.EndScene();
            //Show the results to the user
            device.Present();
            frameTime = timer.GetElapsedTime();
        }
    }
}