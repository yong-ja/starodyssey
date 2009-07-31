#region Disclaimer

/* 
 * OptionGroup
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
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface
{
    public class OptionGroup : ContainerControl
    {
        const string ControlTag = "OptionGroup";
        const int DefaultOptionPadding = 5;
        static int count;

        Size optionButtonSize;
        int selectedIndex;

        #region Exposed Events

        static readonly object EventSelectedIndexChanged = new object();

        public event EventHandler SelectedIndexChanged
        {
            add { Events.AddHandler(EventSelectedIndexChanged, value); }
            remove { Events.RemoveHandler(EventSelectedIndexChanged, value); }
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) Events[EventSelectedIndexChanged];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Properties

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        public Size OptionButtonSize
        {
            get
            {
                if (optionButtonSize.IsEmpty)
                    optionButtonSize = new Size(200, 30);
                return optionButtonSize;
            }
            set
            {
                optionButtonSize = value;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        #endregion

        public OptionGroup() :
            base(ControlTag + count, ControlTag, OptionButton.ControlTag)
        {
            count++;
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            Size = new Size(optionButtonSize.Width, optionButtonSize.Height*Controls.Count);
        }


        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            foreach (OptionButton optionButton in Controls)
            {
                optionButton.TextStyle = TextStyle;
            }
        }

        public void AddItems(params string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                string s = items[i];

                OptionButton ob = new OptionButton();
                ob.Id = ControlTag + "_OB" + i.ToString("00");
                ob.Caption = s;
                ob.Position = new Vector2(0, i*(optionButtonSize.Height/2));
                ob.Size = optionButtonSize;
                ob.Index = Controls.Count;

                Add(ob);
            }
            if (Controls[0] != null)
            {
                ((OptionButton) Controls[0]).IsChecked = true;
                selectedIndex = 0;
            }
        }


        public void Select(int optionNumber)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                OptionButton ob = (OptionButton) Controls[i];
                if (i != optionNumber)
                    ob.IsChecked = false;
                else
                    ob.IsChecked = true;

                ob.UpdateAppearance();
            }

            selectedIndex = optionNumber;
            OnSelectedIndexChanged(EventArgs.Empty);
        }
    }
}