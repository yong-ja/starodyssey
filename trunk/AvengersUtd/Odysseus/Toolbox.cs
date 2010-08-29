using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odysseus
{
    public partial class Toolbox : Form
    {
        private ToolStripToggleButton pointerButton;

        static internal Main MainForm { get; set; }
        public ToolStripButton SelectedButton { get { return toolStrip.Tag as ToolStripButton; } }
        

        public Toolbox()
        {
            InitializeComponent();
            FillControlToolstrip();
        }

        void FillControlToolstrip()
        {
            // Insert "Pointer" ToolStripButton
            pointerButton = new ToolStripToggleButton()
            {
                CheckOnClick = true,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Text = "Pointer111111111"
            };

            pointerButton.CheckedChanged += delegate
            {
                MainForm.UIRenderer.DesignMode = !pointerButton.Checked;
            };

            pointerButton.MouseDown += delegate
                                           {
                                               MainForm.UIRenderer.DesignMode = !pointerButton.Checked;
                                           };

            pointerButton.Checked = true;
            toolStrip.Items.Add(pointerButton);

            // Fill Toolstrip with Assembly controls
            Assembly odysseyUI = Assembly.GetAssembly(typeof(BaseControl));

            Dictionary<string, Type> baseTypes = new Dictionary<string, Type>
                                                     {{"Buttons", typeof (Odyssey.UserInterface.Controls.Button)}};
            Type[] controlTypes = odysseyUI.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseControl))).ToArray();

            foreach (KeyValuePair<string, Type> item in baseTypes)
            {
                ToolStripLabel label = new ToolStripLabel
                {
                    Text = item.Key,
                    Font = new Font(Font, FontStyle.Bold | FontStyle.Underline)
                };

                toolStrip.Items.Add(label);
                Type currentType = item.Value;
                Predicate<Type> baseTypePredicate = (t) => t.IsSubclassOf(currentType) || t == currentType;
                Type[] matchingTypes = Array.FindAll(controlTypes, baseTypePredicate);

                Comparison<Type> alphabeticComparison = (x, y) => string.Compare(x.Name, y.Name);
                Array.Sort(matchingTypes, alphabeticComparison);

                foreach (Type type in matchingTypes.Where(type => type.IsPublic && type.BaseType != null))
                {
                    AddInToolStrip(type);
                }
            }
        }

        void AddInToolStrip(Type controlType)
        {

            ToolStrip destinationToolStrip = null;

            if (controlType.Name == "Button")
            {
                destinationToolStrip = toolStrip;

            }

            if (destinationToolStrip == null)
                return;

            ToolStripToggleButton item = new ToolStripToggleButton
            {
                CheckOnClick = true,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Tag = controlType,
                Text = controlType.Name
            };


            destinationToolStrip.Items.Add(item);
        }

        public void DeselectToolStripButton()
        {
            pointerButton.Checked = true;
            SelectedButton.Checked = false;
            toolStrip.Tag = null;
            MainForm.Cursor = Cursors.Arrow;
        }

        
    }
}
