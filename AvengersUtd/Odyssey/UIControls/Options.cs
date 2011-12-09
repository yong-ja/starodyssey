using System;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class Options : Form
    {
        private readonly Size[] resolutions = new[] {new Size(1024, 768), new Size(1280, 800)};

        public Size Resolution { get; private set; }

        public Options()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            foreach (Size resolution in resolutions)
            {
                float ar = (resolution.Width/(float) resolution.Height);
                string arString = (ar == 1.6f) ? "16:10" : (ar == 4/3f) ? "4:3" : string.Empty;
                cbResolution.Items.Add(string.Format("{0}x{1} ({2})", resolution.Width, resolution.Height, arString));
            }

            Size currentSize = new Size(Game.Context.Settings.ScreenWidth, Game.Context.Settings.ScreenHeight);
            int index = Array.IndexOf(resolutions, currentSize);
            Resolution = resolutions[index];
            cbResolution.SelectedIndex = index;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Resolution = resolutions[cbResolution.SelectedIndex];
        }
    }
}
