using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Utils.Logging;
using System.Windows;
using System.Windows.Input;
using AvengersUtd.Odyssey.UserInterface;

namespace WpfTest
{
    public class TouchRayPanel : RayPickingPanel
    {
        const float MinDistance = 100.0f;
        TexturedIcon crosshairLeft;
        TexturedIcon crosshairRight;
        Vector2 lastLeft;
        Vector2 lastRight;
        Window window;
        Dictionary<TouchDevice, TexturedIcon> crosshairs;

        public TouchRayPanel()
        {
            crosshairs = new Dictionary<TouchDevice, TexturedIcon>();
            //crosshairLeft = new TexturedIcon
            //{
            //    CanRaiseEvents = false,
            //    Position = new Vector2(512f, 512f),
            //    Size = new System.Drawing.Size(64, 64),
            //    Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")

            //};


            //crosshairRight = new TexturedIcon
            //{
            //    CanRaiseEvents = false,
            //    Position = new Vector2(512f, 512f),
            //    Size = new System.Drawing.Size(64, 64),
            //    Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshairBlue.png")

            //};

            //Add(crosshairLeft);
            //Add(crosshairRight);
            window = Global.Window;
        }

        protected override void OnTouchDown(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchDown(e);
            window.CaptureTouch(e.TouchDevice);

            TexturedIcon crosshair = new TexturedIcon
            {
                CanRaiseEvents = false,
                Position = e.Location,
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")
            };
            crosshairs.Add(e.TouchDevice, crosshair);
            OdysseyUI.CurrentHud.BeginDesign();
            Add(crosshair);
            OdysseyUI.CurrentHud.EndDesign();

        }

        protected override void OnTouchUp(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            window.ReleaseTouchCapture(e.TouchDevice);
            OdysseyUI.CurrentHud.BeginDesign();
            Remove(crosshairs[e.TouchDevice]);
            OdysseyUI.CurrentHud.EndDesign();
            
            crosshairs.Remove(e.TouchDevice);

        }

        protected override void OnTouchMove(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchMove(e);

            TexturedIcon crosshair = crosshairs[e.TouchDevice];
            if (crosshair == null) return;

            crosshair.Position = e.Location - new Vector2(crosshair.Width / 2, crosshair.Height / 2);

        }

        bool IsLeft(Vector2 location)
        {
            Vector2 delta = location - lastLeft;
            if (delta.LengthSquared() < MinDistance)
                return true;
            else return false;
        }

        bool IsRight(Vector2 location)
        {
            return (location - lastRight).LengthSquared() < MinDistance;
        }
    }
}
