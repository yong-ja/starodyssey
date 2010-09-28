#region Disclaimer

/* 
 * ContainerControl
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    public abstract class ContainerControl : SimpleShapeControl, IContainer
    {
        #region Properties

        //public override bool CanRaiseEvents
        //{
        //    get { return base.CanRaiseEvents; }
        //    set
        //    {
        //        if (CanRaiseEvents != value)
        //        {
        //            base.CanRaiseEvents = value;
        //            foreach (BaseControl ctl in controls.AllControls)
        //                ctl.CanRaiseEvents = false;
        //        }
        //    }
        //}

        protected virtual ControlCollection PublicControlCollection
        {
            get { return PrivateControlCollection; }
        }

        protected ControlCollection PrivateControlCollection { get; private set; }

        /// <summary>
        /// Returns the publicly available collection of child controls.
        /// </summary>
        public virtual ControlCollection Controls
        {
            get { return PublicControlCollection; }
        }

        #endregion

        protected ContainerControl(string id, string controlDescriptionClass)
            : base(id, controlDescriptionClass)
        {
            Initialize();
        
        }

        #region IContainer Members

        ControlCollection IContainer.PrivateControlCollection
        {
            get { return PrivateControlCollection; }
        }

        ControlCollection IContainer.PublicControlCollection
        {
            get { return PublicControlCollection; }
        }

        #endregion

        /// <summary>
        /// Determines whether the <b>ContainerControl</b> contains the specified keys.
        /// </summary>
        /// <param name="control">The control to locate in the control collection.</param>
        /// <returns><b>True</b> if it the collection contains that element ,<b>false</b> otherwise. </returns>
        /// <remarks>The control passed as parameter does not have to be a top level child,
        /// but this method will also return true if the specified <see cref="BaseControl"/> belongs to the tree formed
        /// by the ContainerControl's children.</remarks>
        public bool ContainsControl(BaseControl control)
        {
            if (control == null)
                throw Error.ArgumentNull("control", GetType(), "ContainsControl");

            return PublicControlCollection.Contains(control);
        }


        void Initialize()
        {
            PrivateControlCollection = new ControlCollection(this);
            IsFocusable = false;
            ApplyStatusChanges = false;
        }

        public virtual void AddRange(BaseControl[] controls)
        {
            foreach (BaseControl ctl in controls)
                Add(ctl);
        }

        public void Add(BaseControl control)
        {
            PublicControlCollection.Add(control);
        }

        public void Insert(int index, BaseControl control)
        {
            PublicControlCollection.Insert(index, control);
        }

        public void Remove(BaseControl control)
        {
            PrivateControlCollection.Remove(control);
        }

        public BaseControl Find(string id)
        {
            foreach (BaseControl ctl in TreeTraversal.PreOrderControlVisit(this))
            {
                if (ctl.Id == id)
                    return ctl;
                else
                    continue;
            }
            throw Error.ArgumentInvalid("id", GetType(), "Find", Properties.Resources.ERR_IdNotFound);
        }

        #region Visit Algorithms

        internal static void Debug(IEnumerable<BaseControl> iterator)
        {
            System.Diagnostics.Debug.WriteLine("---------");
            foreach (BaseControl ctl in iterator)
            {
                System.Diagnostics.Debug.WriteLine(ctl.Id);
            }
            System.Diagnostics.Debug.WriteLine("---------");
        }

        #endregion

        public BaseControl Find(Point cursorLocation)
        {
            return TreeTraversal.PostOrderControlInteractionVisit(this)
                .Reverse()
                .FirstOrDefault(control => control.IntersectTest(cursorLocation));
        }


        bool IContainer.ContainsSprites
        {
            get
            {
                foreach (ISpriteObject spriteObject in
                    PublicControlCollection.Select(baseControl => baseControl as ISpriteObject))
                {
                    if (spriteObject != null)
                        return true;
                    else continue;
                }
                return false;
            }
        }
    }
}