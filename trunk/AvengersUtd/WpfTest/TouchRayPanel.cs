using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey;

namespace WpfTest
{
    public class TouchRayPanel : RayPickingPanel
    {
        TexturedIcon crosshairLeft;
        TexturedIcon crosshairRight;

        public TouchRayPanel()
        {
            crosshairLeft = new TexturedIcon
            {
                Position = new Vector2(512f, 512f),
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshair.png")

            };


            crosshairRight = new TexturedIcon
            {
                Position = new Vector2(512f, 512f),
                Size = new System.Drawing.Size(64, 64),
                Texture = Texture2D.FromFile(Game.Context.Device, "Resources/Textures/crosshairBlue.png")

            };

            Add(crosshairLeft);
            Add(crosshairRight);
        }

        protected override void OnTouchUp(AvengersUtd.Odyssey.UserInterface.Input.TouchEventArgs e)
        {
            base.OnTouchUp(e);
            crosshairLeft.Position = e.Location - new Vector2(crosshairLeft.Width / 2, crosshairLeft.Height / 2);
        }
    }
}
