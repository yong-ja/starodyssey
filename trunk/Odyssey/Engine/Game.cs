using System.Drawing;
using System.Windows.Forms;
using AvengersUTD.Odyssey.Engine;
using AvengersUtd.Odyssey.Engine.Input;
using SlimDX.Direct3D9;
using Timer=AvengersUtd.Odyssey.UserInterface.Helpers.Timer;

namespace AvengersUtd.Odyssey.Engine
{
    /// <summary>
    /// Descrizione di riepilogo per Game.
    /// </summary>
    public class Game
    {
        static Device device;
        static Renderer currentScene;
        static InputHandler input;
        static double frameTime;
        static Point cursorLocation;
        static Timer timer = new Timer();

        public static double FrameTime
        {
            get { return frameTime; }
        }

        public static Point CursorLocation
        {
            get
            {
                cursorLocation = Global.FormOwner.PointToClient(Cursor.Position);
                return cursorLocation;
            }
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

        public static void SwitchScene(Renderer renderer)
        {
            Cursor.Current = Cursors.WaitCursor;
            renderer.Init();
            currentScene = renderer;

            /*
            Size progressBarSize = new Size(200, 30);
            ProgressBar pBar = new ProgressBar("PBar",
                new Microsoft.DirectX.Vector2(Settings.Width / 2 - progressBarSize.Width / 2,
                Settings.Height / 2 - progressBarSize.Height / 2), progressBarSize);
            HUD hud = UI.CurrentHud;

            pBar.BackgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs e)
            {
                hud.Controls.Clear();
                hud.BeginDesign();
                hud.Add(pBar);
                hud.EndDesign();
                pBar.ReportProgress(0.3f);

                renderer.Init();
                pBar.ReportProgress(0.9f);
                currentScene = renderer;
                pBar.ReportProgress(1f);
            };
            pBar.BackgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            };
            pBar.BackgroundWorker.RunWorkerAsync();
             * */
        }

        public static void Loop()
        {
            //Clear the render target
            timer.GetElapsedTime();
            Application.DoEvents();
            if (device.Disposed)
                return;

            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer,Color.DarkBlue,
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