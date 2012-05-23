using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Text;
using Resources = AvengersUtd.Odyssey.Properties.Resources;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Timer = AvengersUtd.Odyssey.Utils.Timer;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Graphics.Resources;
using System.IO;

namespace AvengersUtd.Odyssey
{
    public static class Game
    {
        public const string EngineTag = "Odyssey";
        public const string NetworkTag = "Network";
        public const string RenderingTag = "Rendering";
        public const string UITag = "UI";
        private static object locker;
        private static Timer timer;
        private static bool inputEnabled;
        static internal ManualResetEventSlim RenderEvent { get; private set; }

        
        public static Renderer CurrentRenderer { get; private set;}
        public static DebugLogger Logger { get; private set; }
        public static DeviceContext11 Context { get; set; }
        public static double FrameTime { get; private set; }
        public static bool IsInputEnabled { get; internal set; }

        public static bool IsRunning { get; private set; }

        static Game()
        {
            locker = new Object();
            timer = new Timer();
            timer.Reset();
            Logger = new DebugLogger();
            RenderEvent = new ManualResetEventSlim(false);
            IsRunning = true;
        }



        public static void Init()
        {
            ResourceManager.PerformIntegrityCheck();
            ResourceManager.LoadDefaultStyles();

            LogEvent.Engine.Log(Resources.INFO_OE_Starting);

            DeviceSettings deviceSettings = new DeviceSettings
                                          {
                                              AdapterOrdinal = 0,
                                              CreationFlags = DeviceCreationFlags.Debug,
                                              ScreenWidth = 1920,
                                              ScreenHeight = 1080,
                                              SampleDescription = new SampleDescription(1,0),
                                              Format = Format.R8G8B8A8_UNorm,
                                              IsStereo = true
                                          };

            RenderForm form = new RenderForm
                                  {
                                      ClientSize = new Size(deviceSettings.ScreenWidth, deviceSettings.ScreenHeight),
                                      Text = "Odyssey11 Demo" 
                                  };
            form.Activated += delegate { IsInputEnabled = true; };
            
            Context = new DeviceContext11(form.Handle, deviceSettings);
            OdysseyUI.SetupHooks(form);
            Global.FormOwner = form;
            HookEvents();

            LogEvent.Engine.Log(Resources.INFO_OE_Started);
            Logger.Init();
        }

        public static void HookEvents()
        {
            Context.DeviceDisposing += OdysseyUI.OnDispose;
        }

        public static void Close(int exitCode=0)
        {
            IsRunning = false;
            ResourceManager.Dispose();
            EffectManager.Dispose();

            if (CurrentRenderer != null)
                CurrentRenderer.Dispose();

            if (Context != null)
                Context.Dispose();

            Environment.Exit(exitCode);
        }

        public static void Loop()
        {
            try
            {
                CurrentRenderer.Camera.Update();
                CurrentRenderer.Scene.Update();

                RenderEvent.Reset();
                CurrentRenderer.Begin();
                CurrentRenderer.Render();
                CurrentRenderer.Present();

                RenderEvent.Set();
                FrameTime = timer.GetElapsedTime();
                CurrentRenderer.ProcessInput();
            }
            catch (Exception ex)
            {
                CriticalEvent.Unhandled.LogError(new TraceData(ex.TargetSite.DeclaringType,  ex.TargetSite), ex);
                Game.Close(CriticalEvent.Unhandled.Id);
            }
        }

        public static void ChangeRenderer(Renderer renderer)
        {
            CurrentRenderer = renderer;
            renderer.Init();
        }


    }
}
