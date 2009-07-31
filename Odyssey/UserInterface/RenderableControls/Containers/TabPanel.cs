#region Disclaimer

/* 
 * TabPanel
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

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

#endregion

namespace AvengersUtd.Odyssey.UserInterface
{
    public class TabPanel : ContainerControl, ISpriteControl
    {
        public const string ControlTag = "TabPanel";
        const int DefaultTabBaseWidth = 80;
        const int DefaultTabButtonWidth = DefaultTabBaseWidth + DefaultTabTriangleWidth;
        const int DefaultTabHeight = 30;
        internal const int DefaultTabTriangleWidth = 20;
        const string tabButtonTag = "TabButton";
        static int count;
        Panel currentTab;

        int selectedIndex = -1;

        List<Button> tabButtons;
        List<Panel> tabPanels;

        int totalWidth;

        #region Properties

        protected override ControlCollection PublicControlCollection
        {
            get { return currentTab.Controls; }
        }

        #endregion

        #region Constructors

        public TabPanel()
        {
            ApplyControlStyle(ControlStyle.EmptyStyle);
            tabButtons = new List<Button>();
            tabPanels = new List<Panel>();
        }

        #endregion

        #region Exposed Events

        public event EventHandler SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }

        #endregion

        #region ISpriteControl Members

        public void Render()
        {
            foreach (Button bt in tabButtons)
                bt.Render();
        }

        #endregion

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            foreach (Button tabButton in tabButtons)
            {
                tabButton.TextStyle = TextStyle;
            }
        }


        public void AddTab(string tabLabel)
        {
            int tabBaseWidth = DefaultTabBaseWidth;
            int tabPreferredWidth = FontManager.MeasureString(tabLabel, TextStyle);

            if (tabPreferredWidth > DefaultTabBaseWidth)
                tabBaseWidth = tabPreferredWidth;

            Button tabButton = new Button();
            tabButton.Id = ControlTag + tabButtonTag + (tabButtons.Count + 1);
            tabButton.Position = new Vector2(totalWidth, -DefaultTabHeight);
            tabButton.Size = new Size(tabBaseWidth, DefaultTabHeight);
            tabButton.ControlStyleClass = tabButtonTag;
            tabButton.TextStyleClass = tabButtonTag;
            tabButton.IsSubComponent = true;
            tabButton.MouseClick += new MouseEventHandler(tabButton_MouseClick);

            TabPage tabPage = new TabPage();
            tabPage.AttachToTabButton(tabButton);
            tabPage.Size = Size;
            tabPage.Text = tabLabel;

            totalWidth += tabBaseWidth + DefaultTabTriangleWidth;

            currentTab = tabPage;

            foreach (Button button in tabButtons)
                button.IsSelected = false;
            foreach (Panel panel in tabPanels)
                panel.IsVisible = false;

            tabButtons.Add(tabButton);
            tabPanels.Add(tabPage);
            tabButton.IsSelected = true;


            PrivateControlCollection.Add(currentTab);
            PrivateControlCollection.Add(tabButton);
            selectedIndex = currentTab.Index;
        }

        void tabButton_MouseClick(object sender, MouseEventArgs e)
        {
            int newIndex = tabButtons.IndexOf(sender as Button);
            if (newIndex != selectedIndex)
                Select(newIndex);
        }

        public void Select(int tabPage)
        {
            if (tabPage != selectedIndex)
            {
                currentTab = tabPanels[tabPage];


                OdysseyUI.CurrentHud.BeginDesign();

                foreach (Button tabButton in tabButtons)
                    tabButton.IsSelected = false;
                foreach (Panel tabPanel in tabPanels)
                    tabPanel.IsVisible = false;

                tabButtons[tabPage].IsSelected = true;
                currentTab.IsVisible = true;

                selectedIndex = tabPage;
                OnSelectedIndexChanged(EventArgs.Empty);
                OdysseyUI.CurrentHud.EndDesign();
            }
        }

        public void AddControlInTab(BaseControl control, int tabPage)
        {
            tabPanels[tabPage].Add(control);
            selectedIndex = tabPage;
        }
    }
}