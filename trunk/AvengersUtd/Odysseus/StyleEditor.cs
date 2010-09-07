using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Text;

namespace AvengersUtd.Odysseus
{
    public partial class StyleEditor : Form
    {
        public StyleEditor()
        {
            InitializeComponent();

            TreeNode controlDescriptionNode = new TreeNode("Control Styles");
            TreeNode textDescriptionNode = new TreeNode("Text Styles");

            foreach (ControlDescription cDesc in StyleManager.ControlDescriptions)
                controlDescriptionNode.Nodes.Add(new TreeNode(cDesc.Name));

            foreach (TextDescription tDesc in StyleManager.TextDescriptions)
                textDescriptionNode.Nodes.Add(new TreeNode(tDesc.Name));

            treeView.Nodes.Add(controlDescriptionNode);
            treeView.Nodes.Add(textDescriptionNode);

            treeView.ExpandAll();

        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool isControlDescription = e.Node.Parent == treeView.Nodes[0];
            if (isControlDescription)
                propertyGrid1.SelectedObject = new OControlDescription(StyleManager.GetControlDescription(e.Node.Name));

        }
    }
}
