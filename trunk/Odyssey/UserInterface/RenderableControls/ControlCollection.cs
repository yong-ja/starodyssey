#region Disclaimer

/* 
 * ControlCollection
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class ControlCollection : Collection<BaseControl>, IEnumerable<BaseControl>
    {
        BaseControl owner;

        public ControlCollection(BaseControl owner)
        {
            this.owner = owner;
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public BaseControl Owner
        {
            get { return owner; }
        }

        public IEnumerable InteractionEnabledControls
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    BaseControl control = this[i];
                    if (control.IsVisible && control.IsEnabled)
                        yield return control;
                    else continue;
                }
            }
        }

        internal IEnumerable AllControls
        {
            get
            {
                for (int i = 0; i < Count; i++)
                {
                    yield return this[i];
                }
            }
        }

        #region IEnumerable<BaseControl> Members

        public IEnumerator<BaseControl> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                BaseControl control = this[i];
                if (!control.IsSubComponent)
                    yield return control;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        protected void ProcessForInsertion(BaseControl control)
        {
            if (control == null)
                throw new ArgumentNullException("control", "You cannot add null controls.");

            if (Contains(control))
                throw new ArgumentException("The collection already contains control: " + control.Id);
            else if (IndexOf(control.Id) != -1)
                throw new ArgumentException("The collection already contains a control by the same Id: " + control.Id);

            Depth depth = owner.Depth;

            int windowLayer = depth.WindowLayer;
            int componentLayer = depth.ComponentLayer;
            int zOrder = depth.ZOrder;

            Window window = control as Window;
            if (window != null)
                if (owner is Hud)
                    windowLayer = OdysseyUI.CurrentHud.WindowManager.RegisterWindow(window);
                else
                    throw new ArgumentException("Windows can only be added to the HUD.");


            control.Depth = new Depth(windowLayer, componentLayer, zOrder + 1);
            control.Parent = owner;
        }

        protected override void InsertItem(int index, BaseControl item)
        {
            ProcessForInsertion(item);
            base.InsertItem(index, item);
            owner.OnControlAdded(new ControlEventArgs(item));
        }


        /// <summary>
        /// Returns the index of the first level child control (not recursive) whose Id is 
        /// the one specified.
        /// </summary>
        /// <param name="id">The Id of the control to find.</param>
        /// <returns>The 0 based index if found, -1 if not.</returns>
        public int IndexOf(string id)
        {
            for (int i = 0; i < Count; i++)
            {
                BaseControl ctl = this[i];
                if (ctl.Id == id)
                    return i;
            }

            return -1;
        }

        protected void ProcessForDeletion(BaseControl control)
        {
            Hud hud = owner as Hud;
            Window window = control as Window;

            if (hud != null && window != null)
                hud.WindowManager.Remove(window);

            control.CanRaiseEvents = false;
            control.IsBeingRemoved = true;

            IContainer containerControl = control as IContainer;

            if (containerControl != null)
                foreach (BaseControl childControl in containerControl.PrivateControlCollection.AllControls)
                {
                    childControl.CanRaiseEvents = false;
                    childControl.IsBeingRemoved = true;
                }
        }

        protected override void RemoveItem(int index)
        {
            ProcessForDeletion(this[index]);
            base.RemoveItem(index);
        }

        /// <summary>
        /// Sort the control collection from the foremost to the ones in the
        /// background.
        /// </summary>
        public void Sort()
        {
            BaseControl[] controlArray = ToArray();
            Array.Sort(controlArray);

            Clear();

            for (int i = 0; i < controlArray.Length; i++)
            {
                base.InsertItem(i, controlArray[i]);
            }
        }

        public BaseControl[] ToArray()
        {
            BaseControl[] controlArray = new BaseControl[Count];
            for (int i = 0; i < Count; i++)
            {
                controlArray[i] = this[i];
            }
            return controlArray;
        }

        static int DepthSort(BaseControl ctl1, BaseControl ctl2)
        {
            return -(ctl1.Depth.CompareTo(ctl2.Depth));
        }
    }
}