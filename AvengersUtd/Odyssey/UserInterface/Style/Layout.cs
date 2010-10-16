using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Controls;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Style
{
    public static class Layout
    {
        public static float CenterControlHorizontal(BaseControl control, BaseControl container)
        {
            return (container.ClientSize.Width - control.Size.Width)/2f;
        }

        public static float CenterControlVertical(BaseControl control, BaseControl container)
        {
            return (container.ClientSize.Height - control.Size.Height)/2f;
        }

        public static Vector2 CenterControl(BaseControl control, BaseControl container)
        {
            return new Vector2(CenterControlHorizontal(control,container),
                CenterControlVertical(control,container));
        }

        public static Vector3 OrthographicTransform(Vector2 screenPosition, float zOrder, Size screenSize)
        {
            return new Vector3
                       {
                           X =(float)Math.Floor(((screenSize.Width/ 2f) * -1f) + screenPosition.X),
                           Y =(float)Math.Floor((screenSize.Height / 2f) - screenPosition.Y),
                           Z = zOrder,
                       };
        }
    }
}
