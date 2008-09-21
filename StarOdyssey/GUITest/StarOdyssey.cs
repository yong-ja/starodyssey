using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey;
using GUITest;

namespace AvengersUtd.StarOdyssey
{
    public class StarOdyssey : StarOdysseyEngine
    {
        public void InitGame()
        {
            Text = "Odyssey Engine Atmospheric Scattering demo";
            GuiRenderer mainMenu = new GuiRenderer();
            Game.CurrentScene = mainMenu;
            mainMenu.Init();
            
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            OdysseyUI.ReleaseResources();
            Game.CurrentScene.Dispose();
            Game.Device.Dispose();
        }
    }
}
