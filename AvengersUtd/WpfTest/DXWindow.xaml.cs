using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using AvengersUtd.Odyssey;
using System.ComponentModel;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.Graphics.Rendering;

namespace WpfTest
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class DXWindow : SurfaceWindow
    {
        Renderer scene;
        Image slimDXimage;
        D3DImageSlimDX d3dImage;

        static DXWindow()
        {
        }

        public DXWindow()
        {
            OdysseyUI.SetupHooksWpf(this);
            Game.InitWPF();

            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            Loaded += Window_Loaded;
            Closing += Window_Closing;
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {

            slimDXimage = new Image();

            slimDXimage.Width = Game.Context.Settings.ScreenWidth;
            slimDXimage.Height = Game.Context.Settings.ScreenHeight;
            slimDXimage.FocusVisualStyle = null;
            Content = slimDXimage;

            d3dImage = new D3DImageSlimDX(Game.Context.Settings);
            d3dImage.IsFrontBufferAvailableChanged += OnIsFrontBufferAvailableChanged;
            slimDXimage.Source = d3dImage;
            d3dImage.SetBackBufferSlimDX(Game.Context.GetBackBuffer());

            scene = new BoxRenderer(Game.Context);
            Game.ChangeRenderer(scene);
            BeginRenderingScene();

            WindowStyle = System.Windows.WindowStyle.None;

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