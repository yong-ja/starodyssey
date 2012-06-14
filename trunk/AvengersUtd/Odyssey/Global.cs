using System.Windows.Forms;
using SlimDX.Windows;
using System.Windows;

namespace AvengersUtd.Odyssey
{
    /// <summary>
    /// Descrizione di riepilogo per Global.
    /// </summary>
    public static class Global
    {
        public const string Resources = "Resources/";
        public const string MeshPath = Resources + "Meshes/";
        public const string FXPath = Resources + "Effects/";
        public const string XmlPath = Resources + "Data/";
        public const string TexturePath = Resources + "Textures/";
        public const string GUIPath = Resources + "GUI/";

        public static Form Form { get; internal set; }
        public static Window Window { get; internal set; }
    }
}