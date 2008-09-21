#region Disclaimer

/* 
 * GroupBox
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
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class GroupBox : ContainerControl
    {
        const string ControlTag = "GroupBox";

        const int DefaultGroupBoxOffset = 5;
        static int count;

        Label label;

        #region Properties

        public string Caption
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        #endregion

        public GroupBox()
            : base(ControlTag + ++count, ControlTag, ControlTag)
        {
            ApplyStatusChanges = false;
            CanRaiseEvents = false;
            CreateSubComponents();
        }


        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            label.TextStyle = TextStyle;
        }


        void CreateSubComponents()
        {
            label = new Label();
            label.Id = ControlTag + "_Label";
            label.Position = new Vector2(0 + DefaultGroupBoxOffset, -TextStyle.Size);
            label.TextStyleClass = ControlTag;

            label.IgnoreBounds = true;
            label.IsSubComponent = true;
            PrivateControlCollection.Add(label);
        }
    }
}