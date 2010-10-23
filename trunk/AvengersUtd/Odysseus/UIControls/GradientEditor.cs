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
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.Graphics.ImageProcessing;
using AvengersUtd.Odyssey.Graphics.Rendering;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Xml;
using AvengersUtd.Odyssey.Utils.Xml;

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


        }

        #region GradientBuilder events
        private void UpdateGradient()
        {
            widgetTextureRenderer.ActiveShader.Gradient = gradientBuilder.GradientStops;
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
            InputBox inputBox = new InputBox { Text = "Create new gradient", DialogTitle = "New gradient name:" };
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
            IEnumerable<ColorShader> shaders = from c in widgetTextureRenderer.Control.Description.BorderShaders
                                               select c;

            ListViewItem item = new ListViewItem(shaders.First().Name);
            item.Font = new Font(item.Font, FontStyle.Italic);
            item.Tag = widgetTextureRenderer.Control.Description.BorderShaders[0];
            listView1.Items.Add(item);
            
            //WidgetTextureRenderer.UpdateGradient(gradientBuilder.GradientStops);
            gradientBuilder.SetMarkers(widgetTextureRenderer.ActiveShader.Gradient);
            widgetTextureRenderer.Render();

            pictureBox1.Image = ImageHelper.BitmapFromTexture(widgetTextureRenderer.OutputTexture);
            pictureBox1.Invalidate();
        }

        private void GradientEditorFormClosed(object sender, FormClosedEventArgs e)
        {
            OdysseyUI.CurrentHud = mainHud;
            widgetTextureRenderer.FreeResources();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTestInfo =listView1.HitTest(e.X, e.Y);
                if (hitTestInfo.Item != null)
                {
                    //show the context menu strip
                    contextMenu.Show(this, e.X, e.Y);
                }
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ColorShader[] shaders = (from ListViewItem item in listView1.Items
                           select item.Tag).Cast<ColorShader>().ToArray();
            XmlColorShader[] xmlShaders = new XmlColorShader[shaders.Length];
            for (int i = 0; i < xmlShaders.Length; i++)
            {
                xmlShaders[i] = new XmlColorShader(shaders[i]);
            }
            Data.Serialize(xmlShaders, "UIshaders.xml");
            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }

        private void fill1MenuItem_Click(object sender, EventArgs e)
        {
            widgetTextureRenderer.4
        }
    }
}
