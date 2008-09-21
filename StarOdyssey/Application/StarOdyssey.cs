using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.StarOdyssey.Scenes;
using AvengersUtd.Odyssey;

namespace AvengersUtd.StarOdyssey
{
    public class StarOdyssey : StarOdysseyEngine
    {
        public void InitGame()
        {
            Text = "Odyssey Engine";
            //ImageRenderer mainMenu = new ImageRenderer();
            GridRenderer mainMenu = new GridRenderer();
            //PlanetRenderer mainMenu = new PlanetRenderer();
            //SolarRenderer mainMenu = new SolarRenderer();
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
