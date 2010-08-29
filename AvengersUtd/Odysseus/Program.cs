using System;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using SlimDX.Windows;

namespace AvengersUtd.Odysseus
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
            Main mainForm = new Main();
            MessagePump.Run(mainForm, Game.Loop);

            Game.Close();
            //Application.Run(new Main());
        }
    }
}
