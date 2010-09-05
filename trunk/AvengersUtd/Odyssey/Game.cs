using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Timer = AvengersUtd.Odyssey.Utils.Timer;

namespace AvengersUtd.Odyssey
{
    public static class Game
    {
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
            DeviceSettings settings = new DeviceSettings
                                          {
                                              AdapterOrdinal = 0,
                                              CreationFlags = DeviceCreationFlags.Debug,
                                              ScreenWidth = 1024,
                                              ScreenHeight = 768,
                                              SampleDescription = new SampleDescription(4,4)
                                          };
            RenderForm form = new RenderForm
                                  {
                                      ClientSize = new Size(settings.ScreenWidth, settings.ScreenHeight),
                                      Text = "Odyssey11 Demo" 
                                  };

            Context = new DeviceContext11(form.Handle, settings);
            OdysseyUI.SetupHooks(form);
            Global.FormOwner = form;
            HookEvents();
            
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

        public static void ChangeRenderer(Renderer renderer)
        {
            CurrentRenderer = renderer;
            renderer.Init();
        }


    }
}
