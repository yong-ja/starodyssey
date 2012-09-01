using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AvengersUtd.BrickLab.Controls
{
    internal static class VisualTreeWalker
    {
        /// <summary>
        /// This method is necessary in case the user clicks on a ContentElement,
        /// which is not part of the visual tree.  It will walk up the logical
        /// tree, if necessary, to find the first ancestor in the visual tree.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        static DependencyObject FindClosestVisualAncestor(DependencyObject initial)
        {
            if (initial is Visual || initial is Visual3D)
                return initial;

            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    result = current;
                    break;
                }
                else
                {
                    // If we're in Logical Land then we must walk up the logical tree
                    // until we find a Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }
            }

            return result;
        }

        internal static T FindParentOfType<T>(DependencyObject initial)
            where T : class
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null && !(current is T))
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    // If we're in Logical Land then we must walk up the logical tree
                    // until we find a Visual/Visual3D to get us back to Visual Land.
                    current = LogicalTreeHelper.GetParent(current);
                }
            }

            return result as T;
        }


    }
}
