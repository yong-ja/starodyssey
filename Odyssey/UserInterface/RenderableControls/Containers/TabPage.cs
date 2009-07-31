#region Disclaimer

/* 
 * TabPage
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

namespace AvengersUtd.Odyssey.UserInterface
{
    internal class TabPage : Panel
    {
        const string controlTag = "TabPage";
        static int count;

        Button tabButton;

        public TabPage()
        {
            count++;
            Id = controlTag + count;
        }

        public string Text
        {
            get { return tabButton.Text; }
            set { tabButton.Text = value; }
        }

        protected internal void AttachToTabButton(Button button)
        {
            tabButton = button;
        }
    }
}