using System.Windows.Forms;

namespace AvengersUTD.Odyssey.Engine
{
    /// <summary>
    /// Descrizione di riepilogo per Global.
    /// </summary>
    public class Global
    {
        public const string MeshPath = "Meshes//";
        public const string FXPath = "Effects//";
        public const string XmlPath = "Data//";
        public const string TexturePath = "Textures//";
        public const string GUIPath = "GUI//";

        public static Form owner;

        public static Form FormOwner
        {
            get { return owner; }
            set { owner = value; }
        }

        public Global()
        {
        }
    }
}