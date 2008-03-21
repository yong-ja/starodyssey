using System;
using System.Collections.Generic;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Helpers;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class ProgressChangedEventArgs : EventArgs
    {
        int progressPercentage;

        public int ProgressPercentage
        {
            get { return progressPercentage; }
        }

        public ProgressChangedEventArgs(int progressPercentage)
        {
            this.progressPercentage = progressPercentage;
        }
    }

    public class ProgressBar : BaseControl,ISpriteControl
    {

        Label progressLabel;
        Size progressAreaSize;

        ShapeDescriptor containerDescriptor;
        ShapeDescriptor progressAreaDescriptor;
        bool showProgressLabel=true;
        int progressValue;
        int minimum = 0;
        int maximum = 100;
        int step = 10;

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the progress label is shown.
        /// </summary>
        /// <value><c>true</c> if the label should be shown; otherwise, <c>false</c>.</value>
        /// <remarks>This value is set to <c>true</c> by default.</remarks>
        public bool ShowProgressLabel
        {
            get { return showProgressLabel; }
            set { showProgressLabel = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value of the range of the control.
        /// </summary>
        /// <value>The minimum value of the range. The default is 0.</value>
        public int Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value of the range of the control. 
        /// </summary>
        /// <value>The maximum value of the range. The default is 100.</value>
        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }


        /// <summary>
        /// Gets or sets the amount by which a call to the <see cref="PerformStep"/> method 
        /// increases the current position of the progress bar.
        /// </summary>
        /// <value>The amount by which to increment the progress bar with each 
        /// call to the <c>PerformStep</c> method. The default is 10. </value>
        public int Step
        {
            get { return step; }
            set { step = value; }
        }


        /// <summary>
        /// Gets or sets the current position of the progress bar. 
        /// </summary>
        /// <value>The position within the range of the progress bar. The default is 0.</value>
        public int Value
        {
            get { return progressValue; }
            set
            {
                value = MathHelper.Clamp(value, minimum, maximum);

                if (progressValue != value)
                    progressValue = value;
                OnProgressChanged(new ProgressChangedEventArgs(progressValue));
            }
        }

        #endregion

        #region Exposed events
        
        static object EventProgressChanged = new object();

        /// <summary>
        /// Occurs when <see cref="Increment"/> or <see cref="PerformStep"/> are called.
        /// </summary>
        protected event EventHandler<ProgressChangedEventArgs> ProgressChanged
        {
            add { Events.AddHandler(EventProgressChanged, value); }
            remove { Events.RemoveHandler(EventProgressChanged, value); }
        }

        /// <summary>
        /// Raises the <see cref="ProgressChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="AvengersUtd.Odyssey.UserInterface.RenderableControls.ProgressChangedEventArgs"/> instance containing the event data.</param>
        protected void OnProgressChanged(ProgressChangedEventArgs e)
        {
            float percentage = progressValue/((float)(maximum-minimum));

            progressLabel.Text = string.Format("{0:P0}", percentage);
            progressAreaSize = new Size((int)(ClientSize.Width * percentage), ClientSize.Height);

            UpdateAppearance();
            EventHandler<ProgressChangedEventArgs> handler = (EventHandler<ProgressChangedEventArgs>)Events[EventProgressChanged];
            if (handler != null)
                handler(this, e);
        } 
        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> control.
        /// </summary>
        public ProgressBar()
        {
            string controlTag = GetType().Name;
            ApplyControlStyle(StyleManager.GetControlStyle(controlTag));

            CanRaiseEvents = false;
            progressLabel = new Label();
            progressLabel.Id = controlTag + "_Label";
            progressLabel.Parent = this;
            progressLabel.TextStyleClass = TextStyleClass;
            progressLabel.Text = string.Format("{0:P0}", 0f);
          
        }

        /// <summary>
        /// Increments the progress percentage by the specified value.
        /// </summary>
        /// <remarks>This will cause a visual update of the control.</remarks>
        /// <param name="value">The increment value.</param>
        public void Increment(int value)
        {
            progressValue = MathHelper.Clamp(progressValue + value, minimum, maximum);
            OnProgressChanged(new ProgressChangedEventArgs(progressValue));
        }

        /// <summary>
        /// Increments the progress percentage by the <see cref="Step"/> property value.
        /// <remarks>This will cause a visual update of the control.</remarks>
        /// </summary>
        public void PerformStep()
        {
            progressValue = MathHelper.Clamp(progressValue + step, minimum, Maximum);
            OnProgressChanged(new ProgressChangedEventArgs(progressValue));
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public override void CreateShape()
        {
            base.CreateShape();
            ShapeDescriptors = new ShapeDescriptorCollection(2);
            containerDescriptor = Shapes.DrawFullRectangle(
                AbsolutePosition, Size, InnerAreaColor, BorderColor, ControlStyle.Shading,
                BorderSize, BorderStyle);
            containerDescriptor.Depth = Depth;
            progressAreaDescriptor = Shapes.DrawRectangle(TopLeftPosition, Size.Empty, ControlStyle.ColorArray.Selected,
                ControlStyle.Shading);
            progressAreaDescriptor.Depth = Depth.AsChildOf(Depth);

            ShapeDescriptors[0] = containerDescriptor;
            ShapeDescriptors[1] = progressAreaDescriptor;
        }

        public override void UpdateShape()
        {
            if (containerDescriptor.IsDirty)
                containerDescriptor.UpdateShape(Shapes.DrawFullRectangle(
                AbsolutePosition, Size, InnerAreaColor, BorderColor, ControlStyle.Shading,
                BorderSize, BorderStyle));

            progressAreaDescriptor.UpdateShape(Shapes.DrawRectangle(AbsolutePosition + TopLeftPosition, progressAreaSize, ControlStyle.ColorArray.Selected,
                ControlStyle.Shading));
        }

        protected override void UpdatePositionDependantParameters()
        {
            progressLabel.ComputeAbsolutePosition();
        }

        #region ISpriteControl Members

        public void Render()
        {
            if (showProgressLabel)
                progressLabel.Render();
            else return;
        }

        #endregion
    }
}
