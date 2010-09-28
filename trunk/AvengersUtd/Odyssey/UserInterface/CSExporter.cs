using System;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;
using AvengersUtd.Odyssey.UserInterface.Xml;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class CSExporter
    {
        public static string Export(Hud hud)
        {
            if (hud.Controls.IsEmpty)
                throw Error.ArgumentInvalid("controls", typeof (CSExporter), "Export");

            StringBuilder sb = new StringBuilder();

            XmlHud xmlHud = new XmlHud(hud);
            xmlHud.WriteCSharpCode(sb);

            sb.AppendLine("\nHud.BeginDesign();");

            foreach (XmlBaseControl xmlControl in from baseControl in hud.Controls.Where(ctl => ctl.IsVisible)
                                                  let xmlControlType = UIParser.GetWrapperTypeForControl(baseControl.GetType())
                                                  select (XmlBaseControl) Activator.CreateInstance(xmlControlType, baseControl))
            {
                xmlControl.WriteCSharpCode(sb);
            }

            xmlHud.WriteContainerCSCode();
            

            return sb.ToString();

        }

        


    }
}
