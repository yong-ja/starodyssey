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
        public static string Export(ControlCollection controls)
        {
            if (controls.IsEmpty)
                throw Error.ArgumentInvalid("controls", typeof (CSExporter), "Export");

            StringBuilder sb = new StringBuilder();

            foreach (XmlBaseControl xmlControl in from baseControl in controls.Where(ctl => ctl.IsVisible)
                                                  let xmlControlType = UIParser.GetWrapperTypeForControl(baseControl.GetType())
                                                  select (XmlBaseControl) Activator.CreateInstance(xmlControlType, baseControl))
            {
                xmlControl.ToCSharpCode(sb);
            }

            return sb.ToString();
        }

        


    }
}
