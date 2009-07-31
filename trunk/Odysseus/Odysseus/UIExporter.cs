using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Helpers;

namespace AvengersUtd.Odysseus
{
    public static class UIExporter
    {

        public static void ToCS(Hud hud)
        {
            StringBuilder sb = new StringBuilder();
            foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(hud))
            {
                sb.AppendLine(string.Format("{0} {1} = new {0}", ctl.GetType().Name, ctl.Id));
                sb.AppendLine("\t{");
                sb.AppendLine(string.Format("\t\tId = \"{0}\",", ctl.Id));
                sb.AppendLine(string.Format("\t\tPosition = new Vector2({0}, {1}),", ctl.Position.X, ctl.Position.Y));
                sb.AppendLine(string.Format("\t\tSize = new Size({0}, {1}),", ctl.Size.Width, ctl.Size.Height));
                sb.AppendLine(string.Format("\t\tControlStyleClass = \"{0}\",", ctl.ControlStyleClass));
                sb.AppendLine(string.Format("\t\tTextStyleClass = \"{0}\",", ctl.TextStyleClass));
                sb.AppendLine("\t}\n\n");

                ContainerControl ctlContainer = ctl as ContainerControl;
                if (ctlContainer != null)
                {
                    foreach (BaseControl ctlChild in ctlContainer.Controls)
                    {
                        sb.AppendLine(string.Format("{0}.Add({1});", ctlContainer.Id, ctlChild.Id));
                    }
                }

            }

            return;
        }

        
    }
}
