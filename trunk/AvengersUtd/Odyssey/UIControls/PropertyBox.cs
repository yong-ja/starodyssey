using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Xml;

namespace AvengersUtd.Odysseus.UIControls
{
    public partial class PropertyBox : Form
    {
        public XmlBaseControl SelectedControl
        {
            get { return (XmlBaseControl)propertyGrid1.SelectedObject; }
            set { propertyGrid1.SelectedObject = value; }
        }

        public PropertyBox()
        {
            InitializeComponent();
        }
    }
}
