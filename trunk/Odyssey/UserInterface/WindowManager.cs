#region Disclaimer

/* 
 * WindowManager
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

using System.Collections.Generic;
using AvengersUtd.Odyssey.UserInterface;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class WindowManager
    {
        List<Window> windowList;

        public WindowManager()
        {
            windowList = new List<Window>();
        }

        public Window this[int index]
        {
            get { return windowList[index]; }
        }

        public int Count
        {
            get { return windowList.Count; }
        }

        public Window Foremost
        {
            get
            {
                if (windowList.Count == 0)
                    return null;
                else
                    return windowList[windowList.Count - 1];
            }
        }

        public int RegisterWindow(Window window)
        {
            windowList.Add(window);
            return windowList.Count;
        }

        public void BringToFront(int windowId)
        {
            if (windowId == 0)
            {
                if (Count > 0 && Foremost.IsActivated)
                    Foremost.IsActivated = false;
                return;
            }

            BringToFront(windowList[windowId - 1]);
        }

        public void BringToFront(Window window)
        {
            if (window.Depth.WindowLayer == windowList.Count)
            {
                if (!Foremost.IsActivated)
                    Foremost.IsActivated = true;
                return;
            }


            OdysseyUI.CurrentHud.BeginDesign();
            int windowOrder = windowList.Count;
            windowList.Remove(window);
            for (int i = 0; i < windowList.Count; i++)
            {
                Window currentWindow = windowList[i];
                currentWindow.Depth = new Depth(i + 1, currentWindow.Depth.ComponentLayer, currentWindow.Depth.ZOrder);
                currentWindow.IsActivated = false;
            }


            window.Depth = new Depth(windowOrder, window.Depth.ComponentLayer, window.Depth.ZOrder);
            window.IsActivated = true;
            //window.Focus();
            windowList.Add(window);
            //UI.CurrentHud.InternalControls.Sort();
            OdysseyUI.CurrentHud.EndDesign();
        }

        public void Remove(Window window)
        {
            if (windowList.Contains(window))
            {
                windowList.Remove(window);
                for (int i = 0; i < windowList.Count; i++)
                {
                    Window currentWindow = windowList[i];
                    currentWindow.Depth =
                        new Depth(i + 1, currentWindow.Depth.ComponentLayer, currentWindow.Depth.ZOrder);
                }
            }
        }
    }
}