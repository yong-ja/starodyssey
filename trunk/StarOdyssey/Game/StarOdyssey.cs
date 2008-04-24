using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.Engine;
using AvengersUTD.Odyssey.Engine;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.StarOdyssey.Scenes;

namespace AvengersUtd.StarOdyssey
{
    public class StarOdyssey : StarOdysseyEngine
    {
        public void InitGame()
        {
            Text = "Odyssey UI v0.3.41 SlimDX Demo";
            //ImageRenderer mainMenu = new ImageRenderer();
            GridRenderer mainMenu = new GridRenderer();
            mainMenu.Init();
            Game.CurrentScene = mainMenu;
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
