#region #Disclaimer

// /* 
//  * Timer
//  *
//  * Created on 21 August 2007
//  * Last update on 29 July 2010
//  * 
//  * Author: Adalberto L. Simeone (Taranto, Italy)
//  * E-Mail: avengerdragon@gmail.com
//  * Website: http://www.avengersutd.com
//  *
//  * Part of the Odyssey Engine.
//  *
//  * This source code is Intellectual property of the Author
//  * and is released under the Creative Commons Attribution 
//  * NonCommercial License, available at:
//  * http://creativecommons.org/licenses/by-nc/3.0/ 
//  * You can alter and use this source code as you wish, 
//  * provided that you do not use the results in commercial
//  * projects, without the express and written consent of
//  * the Author.
//  *
//  */

#endregion

#region Using directives

using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;

#endregion

namespace AvengersUtd.Odyssey.UserInterface
{
    public class HitManager
    {
        private readonly BaseControl ownerControl;

        private Rectangle bottom;
        private Rectangle cornerNE;
        private Rectangle cornerNW;
        private Rectangle cornerSE;
        private Rectangle cornerSW;
        private Rectangle innerArea;
        private Rectangle left;
        private Rectangle right;
        private Rectangle top;
        private Rectangle totalArea;

        public HitManager(BaseControl ownerControl, Size sensibleArea)
        {
            this.ownerControl = ownerControl;
            SensibleArea = sensibleArea;
        }

        public Size SensibleArea { get; set; }

        public void ComputeExtents()
        {
            top = new Rectangle
                      {
                          X = (int) ownerControl.AbsolutePosition.X + SensibleArea.Width,
                          Y = (int) ownerControl.AbsolutePosition.Y - SensibleArea.Height,
                          Width = ownerControl.Size.Width - 2*SensibleArea.Width,
                          Height = SensibleArea.Height*2
                      };
            bottom = new Rectangle
                         {
                             X = (int) ownerControl.AbsolutePosition.X + SensibleArea.Width,
                             Y = (int) ownerControl.AbsolutePosition.Y + ownerControl.Size.Height - SensibleArea.Height,
                             Width = ownerControl.Size.Width - 2*SensibleArea.Width,
                             Height = SensibleArea.Height*2
                         };
            left = new Rectangle
                       {
                           X = (int) ownerControl.AbsolutePosition.X - SensibleArea.Width,
                           Y = (int) ownerControl.AbsolutePosition.Y + SensibleArea.Height,
                           Width = SensibleArea.Width*2,
                           Height = ownerControl.Size.Height - 2*SensibleArea.Height
                       };
            right = new Rectangle
                        {
                            X = (int) ownerControl.AbsolutePosition.X + ownerControl.Size.Width - SensibleArea.Width,
                            Y = (int) ownerControl.AbsolutePosition.Y + SensibleArea.Height,
                            Width = SensibleArea.Width*2,
                            Height = ownerControl.Size.Height - 2*SensibleArea.Height
                        };
            cornerNW = new Rectangle
                           {
                               X = (int) ownerControl.AbsolutePosition.X - SensibleArea.Width,
                               Y = (int) ownerControl.AbsolutePosition.Y - SensibleArea.Height,
                               Width = SensibleArea.Width*2,
                               Height = SensibleArea.Height*2
                           };
            cornerNE = new Rectangle
                           {
                               X = right.X,
                               Y = (int) ownerControl.AbsolutePosition.Y - SensibleArea.Height,
                               Width = SensibleArea.Width*2,
                               Height = SensibleArea.Height*2
                           };
            cornerSE = new Rectangle
                           {
                               X = right.X,
                               Y = bottom.Y,
                               Width = SensibleArea.Width*2,
                               Height = SensibleArea.Height*2
                           };
            cornerSW = new Rectangle
                           {
                               X = (int) ownerControl.AbsolutePosition.X - SensibleArea.Width,
                               Y = bottom.Y,
                               Width = SensibleArea.Width*2,
                               Height = SensibleArea.Height*2
                           };
            innerArea = new Rectangle
                            {
                                X = bottom.X,
                                Y = left.Y,
                                Width = bottom.Width,
                                Height = left.Height
                            };

            totalArea = new Rectangle
                            {
                                X = cornerNW.X,
                                Y = cornerNW.Y,
                                Width = ownerControl.Size.Width + 2*SensibleArea.Width,
                                Height = ownerControl.Size.Height + 2*SensibleArea.Height
                            };
        }

        public IntersectionLocation FindIntersection(Point location)
        {
            if (innerArea.Contains(location))
                return IntersectionLocation.Inner;
            else if (cornerNW.Contains(location))
                return IntersectionLocation.CornerNW;
            else if (top.Contains(location))
                return IntersectionLocation.Top;
            else if (cornerNE.Contains(location))
                return IntersectionLocation.CornerNE;
            else if (right.Contains(location))
                return IntersectionLocation.Right;
            else if (cornerSE.Contains(location))
                return IntersectionLocation.CornerSE;
            else if (bottom.Contains(location))
                return IntersectionLocation.Bottom;
            else if (cornerSW.Contains(location))
                return IntersectionLocation.CornerSW;
            else if (left.Contains(location))
                return IntersectionLocation.Left;
            else
                return IntersectionLocation.None;
        }

        public bool Intersect(Vector2 location)
        {
            return Geometry.Intersection.RectangleTest(
                new Vector2(totalArea.X, totalArea.Y), totalArea.Size, location);
        }
    }
}