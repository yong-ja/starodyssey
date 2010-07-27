using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey11;

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
        }
    }
}
