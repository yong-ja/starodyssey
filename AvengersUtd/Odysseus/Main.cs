using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Settings;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Button = AvengersUtd.Odyssey.UserInterface.Controls.Button;

namespace AvengersUtd.Odysseus
{
    public partial class Main : Form
    {
        readonly Toolbox toolbox;

        internal PictureBox RenderPanel { get; set; }
        internal UIRenderer UIRenderer { get; set; }

        public Main()
        {
            InitializeComponent();
            
            InitDX11();

            renderPanel.ClientSize= Game.Context.Settings.ScreenSize;
            int diffW = 8+ renderPanel.Width + renderPanel.Location.X - Width;
            int diffH = 32 + renderPanel.Height + renderPanel.Location.Y - Height;

            Width += diffW;
            Height += diffH;

            renderPanel.Location = new Point((ClientSize.Width - renderPanel.Width)/2,
                menuStrip.Height + (ClientSize.Height- menuStrip.Height - renderPanel.Height)/2);

            toolbox = new Toolbox();
            UIRenderer.Toolbox = toolbox;
        }

        void InitDX11()
        {
            DeviceSettings settings = new DeviceSettings
                                          {
                                              AdapterOrdinal = 0,
                                              CreationFlags = DeviceCreationFlags.Debug,
                                              ScreenWidth = 1024,
                                              ScreenHeight = 768,
                                              SampleDescription = new SampleDescription(1,0)
                                          };

            DeviceContext11 deviceContext11 = new DeviceContext11(renderPanel.Handle, settings);


            Game.Context = deviceContext11;
            Game.HookEvents();

            UIRenderer = new UIRenderer(deviceContext11);
            Game.ChangeRenderer(UIRenderer);
            OdysseyUI.SetupHooks(renderPanel);
            
            Toolbox.MainForm = this;
        }



        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Game.Close();
            base.OnClosing(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            renderPanel.Location = new Point((ClientSize.Width - renderPanel.Width) / 2,
                menuStrip.Height + (ClientSize.Height - menuStrip.Height - renderPanel.Height) / 2);
        }


        private void RenderPanelMouseMove(object sender, MouseEventArgs e)
        {
            Hud hud = UIRenderer.Hud;
            ControlSelector controlSelector = UIRenderer.ControlSelector;
            if (toolbox.SelectedButton != null)
            {
                if (renderPanel.Cursor == Cursors.Arrow)
                    renderPanel.Cursor = Cursors.Cross;
            }
            else
            {
                if (controlSelector.TargetControl != null)
                    renderPanel.Cursor = controlSelector.GetCursorFor(e.Location);
                else
                {
                    BaseControl control = hud.Find(e.Location);
                    if (control == null)
                    {
                        renderPanel.Cursor = Cursors.Arrow;
                        return;
                    }

                    renderPanel.Cursor = System.Windows.Forms.Cursors.Hand;

                }
            }

        }
    }
}
