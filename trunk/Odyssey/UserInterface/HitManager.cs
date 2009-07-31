﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class HitManager
    {
        Size sensibleArea;
        
        BaseControl ownerControl;
        
        Rectangle top;
        Rectangle bottom;
        Rectangle left;
        Rectangle right;
        Rectangle cornerNW;
        Rectangle cornerNE;
        Rectangle cornerSE;
        Rectangle cornerSW;
        Rectangle innerArea;
        Rectangle totalArea;

        public Size SensibleArea
        {
            get
            {
                return sensibleArea;
            }
            set
            {
                sensibleArea = value;
            }
        }

        public HitManager(BaseControl ownerControl, Size sensibleArea)
        {
            this.ownerControl = ownerControl;
            this.sensibleArea = sensibleArea;
            ownerControl.ParentChanged += delegate
                                              {
                                                  ComputeExtents();
                                              };
            
        }

        public void ComputeExtents()
        {
            top = new Rectangle
                      {
                          X = (int)ownerControl.AbsolutePosition.X + sensibleArea.Width,
                          Y = (int)ownerControl.AbsolutePosition.Y - sensibleArea.Height,
                          Width = ownerControl.Size.Width - 2*sensibleArea.Width,
                          Height = sensibleArea.Height*2
                      };
            bottom = new Rectangle
                         {
                             X = (int) ownerControl.AbsolutePosition.X + sensibleArea.Width,
                             Y = (int) ownerControl.AbsolutePosition.Y + ownerControl.Size.Height - sensibleArea.Height,
                             Width = ownerControl.Size.Width - 2*sensibleArea.Width,
                             Height = sensibleArea.Height*2
                         };
            left = new Rectangle
                       {
                           X = (int) ownerControl.AbsolutePosition.X- sensibleArea.Width,
                           Y = (int) ownerControl.AbsolutePosition.Y + sensibleArea.Height,
                           Width = sensibleArea.Width*2,
                           Height = ownerControl.Size.Height - 2*sensibleArea.Height
                       };
            right = new Rectangle
                        {
                            X = (int) ownerControl.AbsolutePosition.X + ownerControl.Size.Width - sensibleArea.Width,
                            Y = (int) ownerControl.AbsolutePosition.Y + sensibleArea.Height,
                            Width = sensibleArea.Width*2,
                            Height = ownerControl.Size.Height - 2*sensibleArea.Height
                        };
            cornerNW = new Rectangle
                           {
                               X = (int)ownerControl.AbsolutePosition.X-sensibleArea.Width,
                               Y = (int)ownerControl.AbsolutePosition.Y- sensibleArea.Height,
                               Width = sensibleArea.Width * 2,
                               Height = sensibleArea.Height *2
                           };
            cornerNE = new Rectangle
            {
                X = right.X,
                Y = (int)ownerControl.AbsolutePosition.Y-sensibleArea.Height,
                Width = sensibleArea.Width*2,
                Height = sensibleArea.Height*2
            };
            cornerSE = new Rectangle
            {
                X = right.X,
                Y = bottom.Y,
                Width = sensibleArea.Width*2,
                Height = sensibleArea.Height*2
            };
            cornerSW = new Rectangle
            {
                X = (int)ownerControl.AbsolutePosition.X-sensibleArea.Width,
                Y = bottom.Y,
                Width = sensibleArea.Width*2,
                Height = sensibleArea.Height*2
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
                                Width = ownerControl.Size.Width + 2*sensibleArea.Width,
                                Height= ownerControl.Size.Height + 2*sensibleArea.Height
            
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

        public bool Intersect(Point location)
        {
            return Intersection.RectangleTest(
                new Vector2(totalArea.X, totalArea.Y), totalArea.Size, location);
        }
    }
}
