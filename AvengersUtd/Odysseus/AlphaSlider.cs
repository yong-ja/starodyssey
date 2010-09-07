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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace AvengersUtd.Odysseus
{
    /// <summary>
    ///   A vertical slider control that shows a range for a color property (a.k.a. Hue, Saturation, Brightness,
    ///   Red, Green, Blue) and sends an event when the slider is changed.
    /// </summary>
    public class AlphaSlider : UserControl
    {
        #region Class Variables

        //	Slider properties

        //	These variables keep track of how to fill in the content inside the box;

        private readonly Container components;
        private bool m_bDragging;
        private int m_iMarker_Start_Y;

        #endregion

        #region Constructors / Destructors

        public AlphaSlider()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }


        /// <summary>
        ///   Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        ///   Required method for Designer support - do not modify 
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // ctrl1DColorBar
            // 
            this.Name = "ctrl1DColorBar";
            this.Size = new System.Drawing.Size(40, 264);
            this.Resize += this.ctrl1DColorBar_Resize;
            this.Load += this.ctrl1DColorBar_Load;
            this.MouseUp += this.ctrl1DColorBar_MouseUp;
            this.Paint += this.ctrl1DColorBar_Paint;
            this.MouseMove += this.ctrl1DColorBar_MouseMove;
            this.MouseDown += this.ctrl1DColorBar_MouseDown;
        }

        #endregion

        #region Control Events

        private void ctrl1DColorBar_Load(object sender, EventArgs e)
        {
            Redraw_Control();
        }


        private void ctrl1DColorBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) //	Only respond to left mouse button events
                return;

            m_bDragging = true; //	Begin dragging which notifies MouseMove function that it needs to update the marker

            int y;
            y = e.Y;
            y -= nTickHeight; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - nTickHeight*2) y = Height - nTickHeight*2;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bDragging) //	Only respond when the mouse is dragging the marker.
                return;

            int y;
            y = e.Y;
            y -= nTickHeight; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - nTickHeight*2) y = Height - nTickHeight*2;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) //	Only respond to left mouse button events
                return;

            m_bDragging = false;

            int y;
            y = e.Y;
            y -= nTickHeight; //	Calculate slider position
            if (y < 0) y = 0;
            if (y > Height - nTickHeight*2) y = Height - nTickHeight*2;

            if (y == m_iMarker_Start_Y) //	If the slider hasn't moved, no need to redraw it.
                return; //	or send a scroll notification

            DrawSlider(y, false); //	Redraw the slider

            if (Scroll != null) //	Notify anyone who cares that the controls slider(color) has changed
                Scroll(this, e);
        }


        private void ctrl1DColorBar_Paint(object sender, PaintEventArgs e)
        {
            Redraw_Control();
        }


        private void ctrl1DColorBar_Resize(object sender, EventArgs e)
        {
            Redraw_Control();
        }

        #endregion

        #region Events

        public event EventHandler Scroll;

        #endregion

        #region Public Methods

        private int nMaximum = 255;
        private int nMinimum;
        private int nValue;

        public int Minimum
        {
            get { return nMinimum; }
            set { nMinimum = value; }
        }

        public int Maximum
        {
            get { return nMaximum; }
            set { nMaximum = value; }
        }

        public int Value
        {
            get { return nValue; }
            set
            {
                nValue = value;
                //	Redraw the control based on the new eDrawStyle
                Reset_Slider(true);
                Redraw_Control();
            }
        }

        #endregion

        #region Private Methods

        private IntPtr _hParent;
        private int nTickHeight = 4;
        private int nTickMargin = 2;
        private int nTickWidth = 8;

        /// <summary>
        ///   Redraws the background over the slider area on both sides of the control
        /// </summary>
        private void ClearSlider()
        {
            Graphics g = CreateGraphics();
            Brush brush = SystemBrushes.Control;
            g.FillRectangle(brush, Width - nTickWidth, 0, nTickWidth, Height); //	clear right hand slider
        }


        public void SetParent(IntPtr hParent)
        {
            _hParent = hParent;
        }

        /// <summary>
        ///   Draws the slider arrows on both sides of the control.
        /// </summary>
        /// <param name = "position">position value of the slider, lowest being at the bottom.  The range
        ///   is between 0 and the controls height-9.  The values will be adjusted if too large/small</param>
        /// <param name = "Unconditional">If Unconditional is true, the slider is drawn, otherwise some logic 
        ///   is performed to determine is drawing is really neccessary.</param>
        private void DrawSlider(int position, bool Unconditional)
        {
            if (Unconditional)
                position = ((position*Height - (nTickHeight*2))/(nMaximum - nMinimum));
            //else
            nValue = ((position*(nMaximum - nMinimum))/(Height - (nTickHeight*2)));

            Trace.WriteLine(nValue);

            if (position < nTickHeight)
                position = nTickHeight;
            if (position >= Height - nTickHeight*2)
                position = Height - nTickHeight - 1;

            if (m_iMarker_Start_Y == position && !Unconditional) //	If the marker position hasn't changed
                return; //	since the last time it was drawn and we don't HAVE to redraw
            //	then exit procedure

            //Trace.WriteLine(m_iMarker_Start_Y);
            m_iMarker_Start_Y = position; //	Update the controls marker position

            ClearSlider(); //	Remove old slider

            Graphics g = CreateGraphics();

            Pen pencil = new Pen(Color.FromArgb(0, 0, 0)); //	Same gray color Photoshop uses
            Brush brush = Brushes.Black;

            Point[] arrow = new Point[4]; //	 GGGg
            arrow[0] = new Point(Width - nTickWidth, position); //	       G
            arrow[1] = new Point(Width - 0, position + nTickHeight*2); //	       G
            arrow[2] = new Point(Width - 0, position - nTickHeight*2); //	G      G
            arrow[3] = new Point(Width - nTickWidth, position); //	       G

            g.FillPolygon(brush, arrow); //	Fill right arrow with white
            g.DrawPolygon(pencil, arrow); //	Draw right arrow border with gray

            //IntPtr hWndParent = ExtensibleDialogs.NativeMethods.GetParent( this.Handle );
            UInt32 val = (UInt32) nValue;
            NativeMethods.SendMessage(_hParent, 0x5050, val, null);
        }


        /// <summary>
        ///   Draws the border around the control, in this case the border around the content area between
        ///   the slider arrows.
        /// </summary>
        private void DrawBorder()
        {
            Graphics g = CreateGraphics();

            Pen pencil;

            //	To make the control look like Adobe Photoshop's the border around the control will be a gray line
            //	on the top and left side, a white line on the bottom and right side, and a black rectangle (line) 
            //	inside the gray/white rectangle

            pencil = new Pen(Color.FromArgb(172, 168, 153)); //	The same gray color used by Photoshop
            g.DrawLine(pencil, Width - nTickWidth - nTickMargin, nTickHeight, 0, nTickHeight); //	Draw top line
            g.DrawLine(pencil, 0, nTickHeight, 0, Height - nTickHeight); //	Draw left hand line

            pencil = new Pen(Color.White);
            g.DrawLine(pencil, Width - nTickWidth - nTickMargin, nTickHeight, Width - nTickWidth, Height - nTickHeight);
                //	Draw right hand line
            g.DrawLine(pencil, Width - nTickWidth, Height - nTickHeight, 0, Height - nTickHeight); //	Draw bottome line
        }

        private void Draw_Alpha_Scale()
        {
            Graphics g = CreateGraphics();

            Rectangle rc = ClientRectangle;

            LinearGradientBrush brush = new LinearGradientBrush(rc, // Rect of gradient
                Color.FromArgb(255, 255, 255, 255), // Second colour
                Color.FromArgb(255, 0, 0, 0), // First colour
                LinearGradientMode.Vertical);
            g.FillRectangle(brush, 0, nTickHeight, Width - nTickWidth - nTickMargin, Height - nTickHeight*2);
        }

        /// <summary>
        ///   Calls all the functions neccessary to redraw the entire control.
        /// </summary>
        private void Redraw_Control()
        {
            DrawSlider(nValue, true);
            DrawBorder();
            Draw_Alpha_Scale();
        }


        /// <summary>
        ///   Resets the vertical position of the slider to match the controls color.  Gives the option of redrawing the slider.
        /// </summary>
        /// <param name = "Redraw">Set to true if you want the function to redraw the slider after determining the best position</param>
        private void Reset_Slider(bool Redraw)
        {
            //	The position of the marker (slider) changes based on the current drawstyle:
            m_iMarker_Start_Y = (Height - 8) - Round((Height - 8));

            if (Redraw)
                DrawSlider(nValue, true);
        }


        /// <summary>
        ///   Kindof self explanitory, I really need to look up the .NET function that does this.
        /// </summary>
        /// <param name = "val">double value to be rounded to an integer</param>
        /// <returns></returns>
        private int Round(double val)
        {
            int ret_val = (int) val;

            int temp = (int) (val*100);

            if ((temp%100) >= 50)
                ret_val += 1;

            return ret_val;
        }

        #endregion
    }
}