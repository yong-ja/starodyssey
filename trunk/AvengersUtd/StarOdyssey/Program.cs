using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using SlimDX.Windows;
using AvengersUtd.StarOdyssey.Scenes;

namespace AvengersUtd.StarOdyssey
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Game.Init();

            Game.ChangeRenderer(new TestRenderer(Game.Context));
            MessagePump.Run(Global.Form, Game.Loop);

            Game.Close();

        }
    }
}
