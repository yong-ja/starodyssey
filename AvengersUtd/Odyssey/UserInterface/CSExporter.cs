using System;
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class CSExporter
    {
        public static string Export(ControlCollection controls)
        {
            if (controls.IsEmpty)
                throw Error.ArgumentInvalid("controls", typeof (CSExporter), "Export");
            
            StringBuilder sb = new StringBuilder();

            foreach (BaseControl ctl in controls)
            {
                Type ctlType = ctl.GetType();
                string typeName = ctlType.Name;
                sb.AppendFormat("{0} {1} = new {0}\n", typeName, ctl.Id);
                sb.Append("\t{\n");
                sb.AppendFormat("\t\tId = \"{0}\",\n", ctl.Id);
                sb.AppendFormat("\t\tControlDescriptionClass = \"{0}\",\n", ctl.ControlDescriptionClass);
                if (ctl.Position != Vector2.Zero)
                    sb.AppendFormat("\t\tPosition = new Vector2({0},{1},{2}),\n", ctl.Position.X, ctl.Position.Y,
                    ctl.Position);
                if (ctl.Size != Size.Empty)
                    sb.AppendFormat("\t\tSize = new Size({0},{1}),\n", ctl.Size.Width, ctl.Size.Height);

                switch (typeName)
                {
                    case "Button":
                        ConvertButton(sb, (Button)ctl);
                        break;

                    case "DropDownList":
                        ConvertDropDownList(sb, (DropDownList) ctl);
                        break;
                }

                sb.Append("\t};\n\n");
            }

            return sb.ToString();
        }

        private static void ConvertBaseControl(StringBuilder sb, BaseControl ctl)
        {
            Type ctlType = ctl.GetType();
            string typeName = ctlType.Name;
            sb.AppendFormat("{0} {1} = new {0}\n", typeName, ctl.Id);
            sb.Append("\t{\n");
            sb.AppendFormat("\t\tId = \"{0}\",\n", ctl.Id);
            sb.AppendFormat("\t\tControlDescriptionClass = \"{0}\",\n", ctl.ControlDescriptionClass);
            if (ctl.Position != Vector2.Zero)
                sb.AppendFormat("\t\tPosition = new Vector2({0},{1},{2}),\n", ctl.Position.X, ctl.Position.Y,
                ctl.Position);
            if (ctl.Size != Size.Empty)
                sb.AppendFormat("\t\tSize = new Size({0},{1}),\n", ctl.Size.Width, ctl.Size.Height);

            sb.Append("\t};\n\n");
        }

        private static void ConvertDropDownList(StringBuilder sb, DropDownList ctl)
        {
            sb.AppendFormat()
        }

        static void ConvertButton(StringBuilder sb, Button button)
        {
            sb.AppendFormat("\t\tContent = \"{0}\",", button.Content);
        }


    }
}
