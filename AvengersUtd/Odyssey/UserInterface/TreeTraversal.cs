#region Disclaimer

/* 
 * TreeTraversal
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in commercial
 * projects, without the express and written consent of
 * the Author.
 *
 */

#endregion

using System.Collections.Generic;
using System.Linq;
using AvengersUtd.Odyssey.UserInterface.Controls;

namespace AvengersUtd.Odyssey.UserInterface
{
    public static class TreeTraversal
    {
        public static IEnumerable<BaseControl> PreOrderControlVisit(IContainer container)
        {
            foreach (BaseControl ctl in container.PrivateControlCollection.AllControls)
            {
                yield return ctl;
                IContainer containerControl = ctl as IContainer;
                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty)
                    continue;

                foreach (BaseControl ctlChild in PreOrderControlVisit(containerControl))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<BaseControl> PreOrderVisibleControlsVisit(IContainer container)
        {
            foreach (BaseControl ctl in
                container.PrivateControlCollection.Where(ctl => ctl.IsVisible))
            {
                yield return ctl;
                IContainer containerControl = ctl as IContainer;
                
                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty) continue;

                foreach (BaseControl ctlChild in
                    PreOrderVisibleControlsVisit(containerControl).Where(ctlChild => ctlChild.IsVisible))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<BaseControl> PreOrderHiddenControlsVisit(IContainer container)
        {
            foreach (BaseControl ctl in container.PrivateControlCollection)
            {
                if (!ctl.IsVisible) yield return ctl;

                IContainer containerControl = ctl as IContainer;
                if (containerControl == null || containerControl.PrivateControlCollection.IsEmpty) continue;

                foreach (BaseControl ctlChild in PreOrderHiddenControlsVisit(containerControl))
                    yield return ctlChild;
            }
        }

        public static IEnumerable<BaseControl> PostOrderControlInteractionVisit(IContainer container)
        {
            foreach (BaseControl ctl in container.PrivateControlCollection.InteractionEnabledControls)
            {
                IContainer containerControl = ctl as IContainer;
                if (containerControl != null && !containerControl.PrivateControlCollection.IsEmpty)
                    foreach (BaseControl ctlChild in PostOrderControlInteractionVisit(containerControl))
                        yield return ctlChild;
                yield return ctl;
            }
        }
    }
}