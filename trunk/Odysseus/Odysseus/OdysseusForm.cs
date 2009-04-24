using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey;
using System.Reflection;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odysseus.Properties;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odysseus
{
    public partial class OdysseusForm : Form
    {
        ToolStripToggleButton pointerButton;
        readonly UIRenderer uiRenderer;
        internal readonly int SidePanelOffset;

        public ToolStripButton SelectedButton
        {
            get { return toolStrip.Tag as ToolStripButton; }
        }


        public RenderPanel RenderPanel
        {
            get { return renderPanel; }
        }

        public OdysseusForm()
        {
            InitializeComponent();
            renderPanel.Create();
            UIRenderer.OdysseusForm = this;
            uiRenderer = new UIRenderer();
            uiRenderer.Init();
            Game.CurrentScene = uiRenderer;

            SidePanelOffset = renderPanel.Location.X;
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
                                                    uiRenderer.DesignMode = !pointerButton.Checked;
                                                };

            pointerButton.Checked = true;
            toolStrip.Items.Add(pointerButton);

            // Fill Toolstrip with Assembly controls
            Assembly odysseyUI = Assembly.GetAssembly(typeof(BaseControl));

            Dictionary<string, Type> baseTypes = new Dictionary<string, Type>();
            baseTypes.Add("Buttons", typeof (BaseButton));
            Type[] controlTypes = odysseyUI.GetTypes();

            foreach (KeyValuePair<string,Type> item in baseTypes)
            {
                ToolStripLabel label = new ToolStripLabel
                {
                    Text = item.Key,
                    Font = new Font(Font, FontStyle.Bold | FontStyle.Underline)
                };

                toolStrip.Items.Add(label);
                Type currentType = item.Value;
                Predicate<Type> baseTypePredicate = (t) => t.BaseType == currentType;
                Type[] matchingTypes = Array.FindAll(controlTypes, baseTypePredicate);

                Comparison<Type> alphabeticComparison = (x, y) => string.Compare(x.Name, y.Name);
                Array.Sort(matchingTypes, alphabeticComparison);
                foreach (Type type in matchingTypes)
                {
                    if (!type.IsPublic || type.BaseType == null)
                        continue;
                    AddInToolStrip(type);
                }
            }
        }


        void AddInToolStrip(Type controlType)
        {

            ToolStrip destinationToolStrip = null;

            if (controlType.BaseType.Name == Resources.ControlCategory_Button)
            {
                destinationToolStrip = toolStrip;
                
            }

            if (destinationToolStrip == null)
                return;

            ToolStripToggleButton item = new ToolStripToggleButton
            {
                CheckOnClick =true,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Tag = controlType,
                Text = controlType.Name
            };


            destinationToolStrip.Items.Add(item);
        }



        private void OdysseusForm_Load(object sender, EventArgs e)
        {
            FillControlToolstrip();

            OdysseyUI.SetupHooks(renderPanel);
        }

        private void renderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedButton != null)
            {
                if (renderPanel.Cursor == Cursors.Arrow)
                    renderPanel.Cursor = Cursors.Cross;
            }
            else
            {
                ControlSelector controlSelector;


                controlSelector = OdysseyUI.CurrentHud.CaptureControl as ControlSelector;
                if (controlSelector != null)
                    renderPanel.Cursor = controlSelector.GetCursorFor(e.Location);
                else
                {
                    controlSelector = OdysseyUI.CurrentHud.Find(e.Location) as ControlSelector;
                    if (controlSelector != null)
                        renderPanel.Cursor = controlSelector.GetCursorFor(e.Location);
                    else
                        renderPanel.Cursor = Cursors.Arrow;
                }

            }

        }

        public void DeselectToolStripButton()
        {
            pointerButton.Checked = true;
            SelectedButton.Checked = false;
            toolStrip.Tag = null;
            renderPanel.Cursor = Cursors.Arrow;
        }





    }
}
