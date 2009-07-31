#region Disclaimer

/* 
 * Panel
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

using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class Panel : ContainerControl
    {
        const string ControlTag = "Panel";
        static int count;
        #region Constructors

        public Panel() : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
        }

        #endregion

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }
    }
}