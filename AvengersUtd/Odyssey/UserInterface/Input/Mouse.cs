
using System.Drawing;
using System.Windows.Forms;

namespace AvengersUtd.Odyssey.UserInterface.Input
{
    public class Mouse
    {
        private MouseButtons activeButtons;

        private Point PreviousCursorLocation { get; set; }
        private Point CursorLocation { get; set; }

        public Mouse()
        {
            ClickButton = SystemInformation.MouseButtonsSwapped ? MouseButtons.Right : MouseButtons.Left;
            
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            activeButtons |= e.Button;
            PreviousCursorLocation = CursorLocation;
            CursorLocation = e.Location;
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            activeButtons ^= e.Button;
            PreviousCursorLocation = CursorLocation;
            CursorLocation = e.Location;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            PreviousCursorLocation = CursorLocation;
            CursorLocation = e.Location;
        }

        /// <summary>
        /// Returns the mouse button used to function as the left mouse button. On some systems 
        /// </summary>
        /// <value>The button used to function as the left mouse button.</value>
        public static MouseButtons ClickButton { get; private set; }
    }
}
