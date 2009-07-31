using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls.Containers
{

    public class CollapsiblePanel : ContainerControl
    {
        const string ControlTag = "CollapsiblePanel";
        static int count;
        DecoratedButton button;

        public CollapsiblePanel() : base(ControlTag + count, ControlTag, ControlTag)
        {
            button = new DecoratedButton();
        }

        protected override void UpdatePositionDependantParameters()
        {
            base.UpdatePositionDependantParameters();
        }


    }
}
