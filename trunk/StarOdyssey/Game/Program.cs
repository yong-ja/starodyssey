using System;
using System.Collections.Generic;
using System.Windows.Forms;

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
            SlimDX.Direct3D9.Direct3D.Initialize();
            StarOdyssey so = new StarOdyssey();
            so.InitGame();
            Application.Run(so);
        }
    }
}