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
using Button = AvengersUtd.Odyssey.UserInterface.Controls.Button;
using ContainerControl = AvengersUtd.Odyssey.UserInterface.Controls.ContainerControl;
using Panel = AvengersUtd.Odyssey.UserInterface.Controls.Panel;

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
                Text = "Pointer"
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

            AddToolStripLabel("Common");
            AddInToolStrip(typeof (Button));
            AddInToolStrip(typeof(DropDownList));
            AddToolStripLabel("Containers");
            AddInToolStrip(typeof(Panel));
            //Assembly odysseyUI = Assembly.GetAssembly(typeof(BaseControl));

            //Dictionary<string, Type> baseTypes = new Dictionary<string, Type>
            //                                         {
            //                                             {"Common", typeof (BaseControl)},
            //                                             {"Containers", typeof (ContainerControl)}
            //                                         };
            //Type[] controlTypes = odysseyUI.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseControl))).ToArray();

            //foreach (KeyValuePair<string, Type> item in baseTypes)
            //{
            //    ToolStripLabel label = new ToolStripLabel
            //    {
            //        Text = item.Key,
            //        Font = new Font(Font, FontStyle.Bold | FontStyle.Underline)
            //    };

            //    toolStrip.Items.Add(label);
            //    Type currentType = item.Value;
            //    Predicate<Type> baseTypePredicate = (t) =>
            //        (t.IsSubclassOf(currentType) && !t.IsSubclassOf(typeof(Container)));
            //    Type[] matchingTypes = Array.FindAll(controlTypes, baseTypePredicate);

            //    Comparison<Type> alphabeticComparison = (x, y) => string.Compare(x.Name, y.Name);
            //    Array.Sort(matchingTypes, alphabeticComparison);

            //    foreach (Type type in matchingTypes.Where(type => type.IsPublic && type.BaseType != null))
            //    {
            //        AddInToolStrip(type);
            //    }
            //}
        }

        void AddToolStripLabel(string labelText)
        {
            ToolStripLabel label = new ToolStripLabel
            {
                Text = labelText,
                Font = new Font(Font, FontStyle.Bold | FontStyle.Underline)
            };

            toolStrip.Items.Add(label);
        }

        void AddInToolStrip(Type controlType)
        {

            //ToolStrip destinationToolStrip = null;

            //if (controlType.Name == "Button")
            //{
            //    destinationToolStrip = toolStrip;

            //}

            if (toolStrip == null)
                return;

            ToolStripToggleButton item = new ToolStripToggleButton
            {
                CheckOnClick = true,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Tag = controlType,
                Text = controlType.Name
            };


            toolStrip.Items.Add(item);
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
