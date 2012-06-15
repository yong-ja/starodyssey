#region Disclaimer

/* 
 * IControl
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

using System;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Xml;
using SlimDX;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = AvengersUtd.Odyssey.UserInterface.Input.MouseEventArgs;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public interface IControl
    {
        string Id { get; set; }
        BaseControl Parent { get; }

        bool CanRaiseEvents { get; set; }
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        bool IsFocused { get; }
        bool IsHighlighted { get; set; }

        Size Size { get; set; }
        Vector2 Position { get; set; }
        Vector2 AbsolutePosition { get; }
        ControlDescription Description { get; set; }

        bool IntersectTest(Vector2 cursorLocation);
        void Focus();
        void UpdateAppearance(UpdateAction updateAction);
        void UpdateShape();
        void CreateShape();

        event EventHandler<MouseEventArgs> MouseDown;
        event EventHandler<MouseEventArgs> MouseClick;
        event EventHandler<MouseEventArgs> MouseUp;
        event EventHandler<MouseEventArgs> MouseMove;
        event EventHandler<MouseEventArgs> MouseWheel;

        event EventHandler<KeyEventArgs> KeyDown;
        event KeyPressEventHandler KeyPress;
        event EventHandler<KeyEventArgs> KeyUp;

        event EventHandler MouseEnter;
        event EventHandler MouseLeave;
        event EventHandler MouseCaptureChanged;
        event EventHandler GotFocus;
        event EventHandler LostFocus;
        event EventHandler VisibleChanged;
        event EventHandler SizeChanged;
        event EventHandler PositionChanged;
        event EventHandler HighlightedChanged;
        event EventHandler Move;
    }
    
    public interface IXmlControl
       
    {
        XmlBaseControl ToXmlControl();
    }
}