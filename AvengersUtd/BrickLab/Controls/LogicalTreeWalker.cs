using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace AvengersUtd.BrickLab.Controls
{
    internal static class LogicalTreeWalker
    {
        /// <summary>
        /// This method is necessary in case the element is not
        /// part of a logical tree.  It finds the closest ancestor
        /// element which is in a logical tree.
        /// </summary>
        /// <param name="initial">The element on which the user clicked.</param>
        private static DependencyObject FindClosestLogicalAncestor(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                DependencyObject logicalParent = LogicalTreeHelper.GetParent(current);
                if (logicalParent != null)
                {
                    result = current;
                    break;
                }
                current = VisualTreeHelper.GetParent(current);
            }

            return result;
        }

        /// <summary>
        /// Walks up the logical tree starting at 'initial' and returns
        /// the first element of the type T enountered.
        /// </summary>
        /// <param name="initial">It is assumed that this element is in a logical tree.</param>
        public static T FindParentOfType<T>(DependencyObject initial) where T : class
        {
            DependencyObject current = FindClosestLogicalAncestor(initial);
            DependencyObject result = initial;

            while (current != null && !(current is T))
            {
                result = current;
                current = LogicalTreeHelper.GetParent(current);
            }

            return (result as T);
        }
    }
}
