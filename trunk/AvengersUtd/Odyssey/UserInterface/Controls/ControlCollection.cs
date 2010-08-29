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
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public class ControlCollection : Collection<BaseControl>, IEnumerable<BaseControl>
    {
        public ControlCollection(BaseControl owner)
        {
            this.Owner = owner;
        }

        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public BaseControl Owner { get; private set; }

        public IEnumerable<BaseControl> InteractionEnabledControls
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

        internal IEnumerable<BaseControl> AllControls
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

        IEnumerator<BaseControl> IEnumerable<BaseControl>.GetEnumerator()
        {
            foreach (BaseControl control in this)
                yield return control;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        protected void ProcessForInsertion(BaseControl control)
        {
            if (control == null)
                throw Error.ArgumentNull("control", GetType(), "ProcessForInsertion",Properties.Resources.ERR_AddNullControl);

            if (Contains(control))
                throw Error.ArgumentInvalid("control", GetType(), "ProcessForInsertion", Properties.Resources.ERR_ControlInCollection, control.Id);
            else if (IndexOf(control.Id) != -1)
                throw Error.ArgumentInvalid("control", GetType(), "ProcessForInsertion", Properties.Resources.ERR_IdInCollection, control.Id);

            //Window window = control as Window;
            //if (window != null)
            //    if (Owner is Hud)
            //        windowLayer = OdysseyUI.CurrentHud.WindowManager.RegisterWindow(window);
            //    else
            //        throw new ArgumentException("Windows can only be added to the HUD.");
            control.Parent = Owner;
            control.Depth = Depth.AsChildOf(Owner.Depth);
        }

        protected override void InsertItem(int index, BaseControl item)
        {
            ProcessForInsertion(item);
            base.InsertItem(index, item);
            Owner.OnControlAdded(new ControlEventArgs(item));
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
            Hud hud = Owner as Hud;
            //Window window = control as Window;

            //if (hud != null && window != null)
            //    hud.WindowManager.Remove(window);

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
    }
}