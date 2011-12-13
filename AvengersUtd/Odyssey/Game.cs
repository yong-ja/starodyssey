using System;
using System.Drawing;
using System.Threading;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Log;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Text;
using Resources = AvengersUtd.Odyssey.Properties.Resources;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Timer = AvengersUtd.Odyssey.Utils.Timer;

namespace AvengersUtd.Odyssey
{
    public static class Game
    {
        public const string EngineTag = "Odyssey";
        private static object locker;
        private static Timer timer;
        static internal ManualResetEventSlim RenderEvent { get; private set; }

        
        public static Renderer CurrentRenderer { get; private set;}
        public static DebugLogger Logger { get; private set; }
        public static DeviceContext11 Context { get; set; }
        public static double FrameTime { get; private set; }

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
            LogEvent.EngineEvent.Log(Resources.INFO_OE_Starting);

            DeviceSettings deviceSettings = new DeviceSettings
                                          {
                                              AdapterOrdinal = 0,
                                              CreationFlags = DeviceCreationFlags.Debug,
                                              ScreenWidth = 1920,
                                              ScreenHeight = 1080,
                                              SampleDescription = new SampleDescription(1,0),
                                              Format = Format.R8G8B8A8_UNorm
                                          };

            RenderForm form = new RenderForm
                                  {
                                      ClientSize = new Size(deviceSettings.ScreenWidth, deviceSettings.ScreenHeight),
                                      Text = "Odyssey11 Demo" 
                                  };

            Context = new DeviceContext11(form.Handle, deviceSettings);
            OdysseyUI.SetupHooks(form);
            Global.FormOwner = form;
            HookEvents();

            LogEvent.EngineEvent.Log(Resources.INFO_OE_Started);

        }

        public static void HookEvents()
        {
            Context.DeviceDisposing += Graphics.Resources.EffectManager.OnDispose;
            Context.DeviceDisposing += Graphics.Resources.ResourceManager.OnDispose;
            Context.DeviceDisposing += OdysseyUI.OnDispose;
        }

        public static void Close()
        {
            IsRunning = false;
            Context.Dispose();
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
                
            }
        }

        public static void ChangeRenderer(Renderer renderer)
        {
            CurrentRenderer = renderer;
            renderer.Init();
        }


    }
}
