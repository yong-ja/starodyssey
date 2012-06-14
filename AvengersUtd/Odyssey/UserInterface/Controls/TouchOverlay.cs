using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Input;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class TouchOverlay : SimpleShapeControl
    {
        public event EventHandler<TouchEventArgs> TouchDown;
        public event EventHandler<TouchEventArgs> TouchUp;

        const string ControlTag = "TouchOverlay";
        private static int count;

        #region Constructors

        public TouchOverlay()
            : base(ControlTag + ++count, "Empty")
        {
        }

        #endregion

        protected virtual void OnTouchDown(TouchEventArgs e)
        {
            if (TouchDown != null)
                TouchDown(this, e);
        }

        protected virtual void OnTouchUp(TouchEventArgs e)
        {
            if (TouchUp != null)
                TouchUp(this, e);
        }

        internal void ProcessTouchDown(TouchEventArgs e)
        {
            OnTouchDown(e);
        }
    }
}
