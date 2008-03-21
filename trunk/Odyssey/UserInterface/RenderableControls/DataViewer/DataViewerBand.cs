#region Disclaimer

/* 
 * DataViewerBand
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
using AvengersUtd.Odyssey.UserInterface.Properties;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    [Flags]
    public enum TableElementStates
    {
        None = 0,
        Selected = 2,
        Visible = 1
    }

    public abstract class DataViewerBand : DataViewerElement
    {
        DataViewerCellStyle defaultCellStyle;
        Type defaultHeaderCellType;
        DataViewerHeaderCell headerCell;

        int index;
        bool isRow;
        TableElementStates states;

        #region Properties

        public bool IsRow
        {
            get { return isRow; }
            internal set { isRow = value; }
        }

        public Type DefaultHeaderCellType
        {
            get
            {
                if (defaultHeaderCellType != null)
                    return defaultHeaderCellType;

                if (isRow)
                    return typeof (DataViewerRowHeaderCell);
                else
                    return typeof (DataViewerColumnHeaderCell);
            }
            set
            {
                if (value != null)
                {
                    if (!typeof (DataViewerHeaderCell).IsAssignableFrom(value))
                        throw new ArgumentException(
                            "Wrong type assigned to DefaultHeaderCell. Custom headers must inherit from DataViewerHeaderCell.");
                    defaultHeaderCellType = value;
                }
            }
        }

        public DataViewerCellStyle DefaultCellStyle
        {
            get
            {
                if (defaultCellStyle == null)
                    return new DataViewerCellStyle();
                else
                    return defaultCellStyle;
            }
            set { defaultCellStyle = value; }
        }

        internal bool HasHeaderCell
        {
            get { return headerCell != null; }
        }

        public DataViewerHeaderCell HeaderCell
        {
            get
            {
                if (HasHeaderCell)
                    return headerCell;
                else
                {
                    HeaderCell = (DataViewerHeaderCell) Activator.CreateInstance(DefaultHeaderCellType);
                    return headerCell;
                }
            }
            set
            {
                if (value != null)
                {
                    if (HasHeaderCell)
                    {
                        headerCell.DataViewer = null;
                        headerCell.OwningRow = null;
                        headerCell.OwningColumn = null;
                    }

                    if (DataViewer != null)
                    {
                        value.DataViewer = DataViewer;

                        if (isRow)
                            value.OwningRow = (DataViewerRow) this;
                        else
                            value.OwningColumn = (DataViewerColumn) this;
                    }

                    headerCell = value;
                }
                else
                    throw new InvalidOperationException(Exceptions.DataViewerColumn_DefaultHeaderCell_Null);
            }
        }

        public abstract DataViewerCellStyle InheritedStyle { get; }

        public bool HasDefaultCellStyle
        {
            get
            {
                if (defaultCellStyle != null)
                    return true;
                else return false;
            }
        }

        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }

        public bool Visible
        {
            get { return CheckTableElementStates(TableElementStates.Visible); }
            set { SetTableElementStates(TableElementStates.Visible, value); }
        }

        public bool Selected
        {
            get { return CheckTableElementStates(TableElementStates.Selected); }
            set { SetTableElementStates(TableElementStates.Selected, value); }
        }

        #endregion

        protected DataViewerBand()
        {
            index = -1;
        }

        protected bool CheckTableElementStates(TableElementStates flagsToCheck)
        {
            return ((states & flagsToCheck) != TableElementStates.None);
        }

        protected void SetTableElementStates(TableElementStates flagToSet, bool toEnable)
        {
            if (toEnable)
                states |= flagToSet;
            else
                states ^= flagToSet;
        }
    }
}