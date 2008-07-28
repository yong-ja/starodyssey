using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.MultiversalRuleSystem.Space.GalaxyGeneration;

namespace WindowsFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GalaxyGenerator generator = new GalaxyGenerator();
            SortedDictionary<string,SolarSystem> galaxy = generator.GenerateStars();

            int system = galaxy.Values.Count;
            textBox1.Text = generator.Log;
        }
    }
}