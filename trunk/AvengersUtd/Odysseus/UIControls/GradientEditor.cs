#region #Disclaimer

// Author: Adalberto L. Simeone (Taranto, Italy)
// E-Mail: avengerdragon@gmail.com
// Website: http://www.avengersutd.com/blog
// 
// This source code is Intellectual property of the Author
// and is released under the Creative Commons Attribution 
// NonCommercial License, available at:
// http://creativecommons.org/licenses/by-nc/3.0/ 
// You can alter and use this source code as you wish, 
// provided that you do not use the results in commercial
// projects, without the express and written consent of
// the Author.

#endregion

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.ImageProcessing;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;

#endregion

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class GradientEditor : Form
    {
        private readonly SortedDictionary<string, Gradient> gradientDictionary;
        private readonly Hud mainHud;
        private readonly WidgetTextureRenderer widgetTextureRenderer;
        private bool shouldSave;

        public GradientEditor()
        {
            InitializeComponent();
            gradientDictionary = new SortedDictionary<string, Gradient>();
            cbControls.Items.Add("Button");
            cbControls.SelectedIndex = 0;
            tbSize.Text = "96; 32";
            mainHud = OdysseyUI.CurrentHud;

            widgetTextureRenderer = new WidgetTextureRenderer(320, 320, Game.Context);

            gradientBuilder.SelectedMarkerOffsetChanged += GradientBuilderSelectedMarkerOffsetChanged;
            gradientBuilder.SelectedMarkerColorChanged += GradientBuilderSelectedMarkerColorChanged;
            gradientBuilder.MarkersChanged += GradientBuilderMarkersChanged;
            shouldSave = true;
            ListViewItem item = new ListViewItem("(Untitled Gradient)");
                item.Font = new Font(item.Font, FontStyle.Italic);
            listView1.Items.Add(item);
        }

        #region GradientBuilder events
        private void UpdateGradient()
        {
            widgetTextureRenderer.Gradient = gradientBuilder.GradientStops;
            widgetTextureRenderer.Render();
            pictureBox1.Image = ImageHelper.BitmapFromTexture(widgetTextureRenderer.OutputTexture);
            pictureBox1.Invalidate();
        }

        private void GradientBuilderMarkersChanged(object sender, EventArgs e)
        {
            UpdateGradient();
        }

        private void GradientBuilderSelectedMarkerColorChanged(object sender, MarkerEventArgs e)
        {
            UpdateGradient();
        }


        private void GradientBuilderSelectedMarkerOffsetChanged(object sender, MarkerEventArgs e)
        {
            UpdateGradient();
        } 
        #endregion

        private void ButtonAddClick(object sender, EventArgs e)
        {
            InputBox inputBox = new InputBox {Text = "Create new gradient", DialogTitle = "New gradient name:"};
            inputBox.ShowDialog();

            if (inputBox.DialogResult != DialogResult.OK) return;

            Gradient gradient = new Gradient(inputBox.InputValue, gradientBuilder.CurrentMarkers);
            listView1.Items.Add(gradient.Name);
            gradientDictionary.Add(inputBox.InputValue, gradient);
        }

        public void SelectGradient(Gradient gradient)
        {
            gradientBuilder.SetMarkers(gradient.Markers);
        }

        private void ListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            SelectGradient(gradientDictionary[listView1.SelectedItems[0].Text]);
        }

        private void GradientEditorLoad(object sender, EventArgs e)
        {
            widgetTextureRenderer.Init();
            //WidgetTextureRenderer.UpdateGradient(gradientBuilder.GradientStops);
            gradientBuilder.SetMarkers(widgetTextureRenderer.Gradient);
            widgetTextureRenderer.Render();

            pictureBox1.Image = ImageHelper.BitmapFromTexture(widgetTextureRenderer.OutputTexture);
            pictureBox1.Invalidate();
        }

        private void GradientEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            OdysseyUI.CurrentHud = mainHud;
            widgetTextureRenderer.FreeResources();
        }
    }
}