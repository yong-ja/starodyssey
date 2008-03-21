using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Engine;
using AvengersUTD.Odyssey.Engine;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using SlimDX.Direct3D9;
using AvengersUtd.Odyssey.UserInterface;


namespace AvengersUtd.StarOdyssey.Scenes
{
    public class ImageRenderer : Renderer
    {
        Texture t;
        Hud hud;

        void TestUI()
        {
            hud = GUIDesigner.MainMenuTest;
        }

        public ImageRenderer() : base()
        {
            t = Texture.FromFile(Game.Device, Global.TexturePath + "mainmenu.jpg");
            TestUI();
        }

        public override void Init()
        {
            Game.Input.IsInputEnabled = true;
        }

        public override void Render()
        {
            //qCam.Update();

            //EntityManager.RenderOverlayQuad(t, Settings.Width, Settings.Height);
            //device.SetTexture(0, null);
            hud.Render();
            DebugManager.Instance.DisplayStats();
        }

        public override void ProcessInput()
        {
            return;
        }

        public override void Dispose()
        {
            if (!t.Disposed)
                t.Dispose();

        }

    }
}