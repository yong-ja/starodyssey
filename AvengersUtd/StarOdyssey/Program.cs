using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using SlimDX.Windows;
using AvengersUtd.StarOdyssey.Scenes;

namespace StarOdyssey
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
            RenderForm11 form = new RenderForm11();
            Game.ChangeRenderer(new TestRenderer());
            MessagePump.Run(form.Handle,Game.Loop);
            //form.Dispose();
        }
    }
}
