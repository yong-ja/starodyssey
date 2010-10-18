using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.ImageProcessing;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;
using Button = AvengersUtd.Odyssey.UserInterface.Controls.Button;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class GradientEditor : Form
    {
        private TextureRenderer textureRenderer;
        private readonly SortedDictionary<string, Gradient> gradientDictionary;
        private Hud mainHud;

        public GradientEditor()
        {
            InitializeComponent();
            gradientDictionary = new SortedDictionary<string, Gradient>();
            cbControls.Items.Add("Button");
            cbControls.SelectedIndex = 0;
            tbSize.Text = "96; 32";
            mainHud = OdysseyUI.CurrentHud;

            textureRenderer = new TextureRenderer(320, 320, Game.Context);

            gradientBuilder.SelectedMarkerOffsetChanged += GradientBuilderSelectedMarkerOffsetChanged;
            gradientBuilder.SelectedMarkerColorChanged += new MarkerEventHandler(gradientBuilder_SelectedMarkerColorChanged);

        }

        void UpdateGradient()
        {
            textureRenderer.Gradient = gradientBuilder.GradientStops;
            textureRenderer.Render();
            pictureBox1.Image = ImageHelper.BitmapFromTexture(textureRenderer.OutputTexture);
            pictureBox1.Invalidate();
        }

        void gradientBuilder_SelectedMarkerColorChanged(object sender, MarkerEventArgs e)
        {
            UpdateGradient();
        }


        void GradientBuilderSelectedMarkerOffsetChanged(object sender, MarkerEventArgs e)
        {
            UpdateGradient();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox() {Text = "Create new gradient", DialogTitle = "New gradient name:"};
            inputBox.ShowDialog();
            
            if (inputBox.DialogResult != System.Windows.Forms.DialogResult.OK) return;

            Gradient gradient = new Gradient(inputBox.InputValue, gradientBuilder.CurrentMarkers);
            listBox.Items.Add(gradient.Name);
            gradientDictionary.Add(inputBox.InputValue, gradient);
        }

        public void SelectGradient(Gradient gradient)
        {
            gradientBuilder.SetMarkers(gradient.Markers);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGradient(gradientDictionary[(string)listBox.SelectedItem]);
        }

        private void GradientEditor_Load(object sender, EventArgs e)
        {
            textureRenderer.Init();
            //textureRenderer.UpdateGradient(gradientBuilder.GradientStops);
            gradientBuilder.SetMarkers(textureRenderer.Gradient);
            textureRenderer.Render();
            
            pictureBox1.Image = ImageHelper.BitmapFromTexture(textureRenderer.OutputTexture);
            pictureBox1.Invalidate();
        }

        private void GradientEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            OdysseyUI.CurrentHud = mainHud;
            textureRenderer.FreeResources();
        }

    }
}
