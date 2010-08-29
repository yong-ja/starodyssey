using System.Windows.Forms;
using SlimDX.Windows;

namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// Descrizione di riepilogo per Global.
    /// </summary>
    public static class Global
    {
        public const string MeshPath = "Meshes/";
        public const string FXPath = "Effects/";
        public const string XmlPath = "Data/";
        public const string TexturePath = "Textures/";
        public const string GUIPath = "GUI/";

        private static RenderForm owner;

        public static RenderForm FormOwner
        {
            get { return owner; }
            set { owner = value; }
        }

    }
}