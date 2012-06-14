using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using AvengersUtd.Odyssey.Graphics.Rendering;
using SlimDX.Direct3D11;
using System.Windows.Media;
using System.Diagnostics.Contracts;
using AvengersUtd.Odyssey.UserInterface;
using System.Runtime.InteropServices;

namespace AvengersUtd.Odyssey
{
   
    public class WpfWindow : System.Windows.Window
    {
        Image slimDXimage;
        D3DImageSlimDX d3dImage;

        protected D3DImageSlimDX D3DImage { get { return d3dImage; } }

        public WpfWindow()
        {
            Loaded += Window_Loaded;
            Closing += Window_Closing;
            
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OdysseyUI.SetupHooksWpf(this);
            slimDXimage = new Image();

            slimDXimage.Width = Game.Context.Settings.ScreenWidth;
            slimDXimage.Height = Game.Context.Settings.ScreenHeight;
            slimDXimage.FocusVisualStyle = null;
            Content = slimDXimage;

            d3dImage = new D3DImageSlimDX();
            d3dImage.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;
            slimDXimage.Source = d3dImage;
            d3dImage.SetBackBufferSlimDX(Game.Context.GetBackBuffer());

        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            if (d3dImage != null)
            {
                d3dImage.Dispose();
                d3dImage = null;
            }
            Game.Close();
        }

        void OnIsFrontBufferAvailableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // This fires when the screensaver kicks in, the machine goes into sleep or hibernate
            // and any other catastrophic losses of the d3d device from WPF's point of view
            if (d3dImage.IsFrontBufferAvailable)
            {
                BeginRenderingScene();
            }
            else
            {
                StopRenderingScene();
            }
        }

        protected void BeginRenderingScene()
        {
            if (d3dImage.IsFrontBufferAvailable)
            {
                Texture2D Texture = Game.Context.GetBackBuffer();
                d3dImage.SetBackBufferSlimDX(Texture);
                CompositionTarget.Rendering += OnRendering;
            }
        }

        protected void OnRendering(object sender, EventArgs e)
        {
            Game.Loop();
            d3dImage.InvalidateD3DImage();
        }

        protected void StopRenderingScene()
        {
            CompositionTarget.Rendering -= OnRendering;
        }

    }
}
