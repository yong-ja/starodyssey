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
        public OdysseusForm()
        {
            InitializeComponent();
            UIRenderer uiRenderer = new UIRenderer();
            uiRenderer.Init();
            Game.CurrentScene = uiRenderer;

        }

        void FillControlToolstrip()
        {
            
            Type[] baseTypes = {
                                   typeof (BaseButton)
                               };

           
                Assembly odysseyUI = Assembly.GetAssembly(typeof(BaseControl));
            Type[] controlTypes = odysseyUI.GetTypes();
                Comparison<Type> alphabeticComparison = (Type x, Type y) => string.Compare(x.Name, y.Name);
                Array.Sort(controlTypes, alphabeticComparison);

                foreach (Type type in controlTypes)
                {
                    if (!type.IsPublic || type.BaseType == null)
                        continue;

                    AddInToolStrip(type);
                }
            
        }

        void AddInToolStrip(Type controlType)
        {


            ToolStrip destinationToolStrip = null;


            if (controlType.BaseType.Name == Resources.ControlCategory_Button)
            {
                destinationToolStrip = toolStrip1;
                
            }

            if (destinationToolStrip == null)
                return;

            ToolStripButton item = new ToolStripButton
            {
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Text = controlType.Name
            };

            destinationToolStrip.Items.Add(item);
        }

        private void OdysseusForm_Load(object sender, EventArgs e)
        {
            FillControlToolstrip();
            OdysseyUI.SetupHooks(renderPanel);
        }
    }
}
