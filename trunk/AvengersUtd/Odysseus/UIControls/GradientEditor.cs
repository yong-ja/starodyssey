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
        private readonly SortedDictionary<string, LinearShader> shaderDictionary;
        private readonly Hud mainHud;
        private readonly WidgetTextureRenderer widgetTextureRenderer;
        private bool shouldSave;

        public GradientEditor()
        {
            InitializeComponent();
            shaderDictionary = new SortedDictionary<string, LinearShader>();
            cbControls.Items.Add("Button");
            cbControls.SelectedIndex = 0;
            tbSize.Text = "96; 32";
            mainHud = OdysseyUI.CurrentHud;

            cbGType.SelectedIndex = 2;

            widgetTextureRenderer = new WidgetTextureRenderer(320, 320, Game.Context);

            gradientBuilder.SelectedMarkerOffsetChanged += GradientBuilderSelectedMarkerOffsetChanged;
            gradientBuilder.SelectedMarkerColorChanged += GradientBuilderSelectedMarkerColorChanged;
            gradientBuilder.MarkersChanged += GradientBuilderMarkersChanged;
            shouldSave = true;


        }

        #region GradientBuilder events
        private void UpdateGradient()
        {
            widgetTextureRenderer.GradientStops = gradientBuilder.GradientStops;
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
            InputBox inputBox = new InputBox { Text = "Create new shader", DialogTitle = "New shader name:" };
            inputBox.ShowDialog();

            if (inputBox.DialogResult != DialogResult.OK) return;

            LinearShader shader = new LinearShader
                                      {
                                              Name = inputBox.InputValue,
                                              Gradient = gradientBuilder.GradientStops,
                                              GradientType = GradientType.LinearVerticalGradient,
                                              Method = LinearShader.LinearVerticalGradient
                                      };
            ListViewItem item = new ListViewItem
                                    {
                                            Name = shader.Name,
                                            Text = shader.Name
                                    };
            shaderList.Items.Add(item);
            shaderDictionary.Add(shader.Name, shader);
        }

        public void SelectGradient(LinearShader gradient)
        {
            gradientBuilder.SetMarkers(gradient.Gradient);
        }


        private void GradientEditorLoad(object sender, EventArgs e)
        {
            widgetTextureRenderer.Init();
            IEnumerable<LinearShader> shaders = from c in widgetTextureRenderer.Control.Description.BorderShaders
                                               select c;

            LinearShader shader = shaders.First();
            shaderDictionary.Add(shader.Name, shader);
            ListViewItem item = new ListViewItem
                                    {
                                            Name = shader.Name,
                                            Text = shader.Name,
                                    };
            item.Font = new Font(item.Font, FontStyle.Italic);

            shaderList.Items.Add(item);
            
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

        private void ShaderListMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo hitTestInfo =shaderList.HitTest(e.X, e.Y);
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
            LinearShader[] shaders = (from ListViewItem item in shaderList.Items
                           select item.Tag).Cast<LinearShader>().ToArray();
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
            widgetTextureRenderer.Control.Description.Enabled[0] = shaderDictionary[shaderList.SelectedItems[0].Name];
        }

        private void ShaderListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (shaderList.SelectedItems.Count == 0)
                return;

            LinearShader newShader = shaderDictionary[shaderList.SelectedItems[0].Name];
            gradientBuilder.SetMarkers(newShader.Gradient);

            switch (newShader.GradientType)
            {
                case GradientType.Uniform:
                    cbGType.SelectedIndex = 3;
                    break;
                case GradientType.LinearVerticalGradient:
                    cbGType.SelectedIndex = 0;
                    break;
                case GradientType.LinearHorizontalGradient:
                    cbGType.SelectedIndex = 1;
                    break;
            }
        }
    }
}
