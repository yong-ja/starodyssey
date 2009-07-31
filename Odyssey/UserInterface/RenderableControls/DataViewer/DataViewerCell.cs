#region Disclaimer

/* 
 * DataViewerCell
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
using System.Drawing;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Properties;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

#endregion

namespace AvengersUtd.Odyssey.UserInterface
{
    public abstract class DataViewerCell : DataViewerElement, ICloneable
    {
        DataViewerCellStyle cellStyle;

        object cellValue;
        Type cellValueType;
        BaseControl controlInternal;
        DataViewerColumn owningColumn;
        DataViewerRow owningRow;

        #region Properties

        public int ColumnIndex
        {
            get
            {
                if (owningColumn == null)
                    return -1;
                else
                    return owningColumn.Index;
            }
        }

        public int RowIndex
        {
            get
            {
                if (owningRow == null)
                    return -1;
                else
                    return owningRow.Index;
            }
        }

        public DataViewerColumn OwningColumn
        {
            get { return owningColumn; }
            internal set { owningColumn = value; }
        }

        public DataViewerRow OwningRow
        {
            get { return owningRow; }
            internal set { owningRow = value; }
        }

        public bool HasStyle
        {
            get
            {
                if (cellStyle != null)
                    return true;
                else
                    return false;
            }
        }

        public DataViewerCellStyle InheritedStyle
        {
            get { return GetInheritedStyle(); }
        }

        public virtual DataViewerCellStyle Style
        {
            get
            {
                if (cellStyle == null)
                    cellStyle = new DataViewerCellStyle();
                return cellStyle;
            }
            set { cellStyle = value; }
        }

        public object Value
        {
            get { return cellValue; }
            set
            {
                if (cellValue != value)
                {
                    cellValue = value;
                    UpdateValue();
                    DataViewer.OnCellValueChanged(new CellEventArgs(RowIndex, ColumnIndex));
                }
            }
        }


        public Type ValueType
        {
            get
            {
                if (cellValueType == null)
                    return owningColumn.ValueType;
                else
                    return cellValueType;
            }
            set { cellValueType = value; }
        }


        internal BaseControl ControlInternal
        {
            get { return controlInternal; }
            set { controlInternal = value; }
        }

        #endregion

        public DataViewerCell()
        {
        }

        #region ICloneable Members

        public virtual object Clone()
        {
            DataViewerCell clonedCell = (DataViewerCell) Activator.CreateInstance(GetType());
            if (HasStyle)
                clonedCell.Style = cellStyle;
            return clonedCell;
        }

        #endregion

        protected internal virtual void UpdateValue()
        {
            DynamicPropertyAccessor propertyAccessor = owningColumn.PropertyAccessor;
            if (propertyAccessor == null)
                throw new InvalidOperationException(
                    string.Format(Exceptions.Culture,
                                  "Column {0} is not bound to a property. Assign the property name to this column's DataPropertyName.",
                                  ColumnIndex));

            cellValue = propertyAccessor.Get(DataViewer.GetDataSourceObject(RowIndex));

            Type type;
            if ((type = cellValue.GetType()) != owningColumn.ValueType)
            {
                cellValueType = type;
            }
        }


        protected DataViewerCellStyle GetInheritedStyle()
        {
            DataViewerCellStyle finalStyle = DataViewer.DefaultCellStyle;

            if (ColumnIndex > -1 && OwningColumn.HasDefaultCellStyle)
                finalStyle.MergeStyle(OwningColumn.DefaultCellStyle);

            finalStyle.MergeStyle(DataViewer.RowsDefaultCellStyle);

            if (RowIndex > -1 && OwningRow.HasDefaultCellStyle)
                finalStyle.MergeStyle(OwningRow.DefaultCellStyle);

            if (HasStyle)
                finalStyle.MergeStyle(Style);

            return finalStyle;
        }

        internal static Vector2 ComputeCellPosition(DataViewer dataViewer, DataViewerCell cell)
        {
            int rowIndex = cell.RowIndex;
            int columnIndex = cell.ColumnIndex;

            // This method is used to compute each cell position, handling all special cases.

            // TopLeftHeaderCell
            if (rowIndex == -1 && columnIndex == -1)
            {
                // Simply use padding values.
                //return new Vector2(dataViewer.Padding.Left, dataViewer.Padding.Top);
                return new Vector2();
            }
                // RowHeaderCell
            else if (columnIndex == -1 && rowIndex >= 0)
            {
                float offset;
                if (rowIndex == 0)
                {
                    // This is the first row header cell.
                    offset = dataViewer.ColumnHeadersHeight;
                }
                else
                {
                    // To compute all other row headers, use the previos row values.
                    DataViewerRow previousRow = dataViewer.Rows[rowIndex - 1];
                    offset = previousRow.HeaderCell.ControlInternal.Position.Y + previousRow.Height;
                }

                //return new Vector2(dataViewer.Padding.Left,
                return new Vector2(0,
                                   dataViewer.CellSpacing + offset);
            }
                // ColumnHeaderCell
            else if (rowIndex == -1 && columnIndex >= 0)
            {
                float offset;
                if (columnIndex == 0)
                {
                    // This is the first column header cell
                    offset = dataViewer.RowHeadersWidth;
                }
                else
                {
                    // To compute all other column headers, use the previous column values.
                    DataViewerColumn previousColumn = dataViewer.Columns[columnIndex - 1];
                    offset = previousColumn.HeaderCell.ControlInternal.Position.X + previousColumn.Width;
                }
                return new Vector2(dataViewer.CellSpacing + offset,
                                   //dataViewer.Padding.Top);
                                   0);
            }

            // All other cells
            int previousColumnIndex = columnIndex - 1;
            DataViewerRow owningRow = cell.OwningRow;

            float x;
            float y;

            if (previousColumnIndex < 0)
                x = owningRow.HeaderCell.ControlInternal.Position.X + dataViewer.RowHeadersWidth +
                    dataViewer.CellSpacing;
            else
                x = owningRow.Cells[previousColumnIndex].ControlInternal.Position.X +
                    owningRow.Cells[previousColumnIndex].OwningColumn.Width + dataViewer.CellSpacing;

            y = owningRow.HeaderCell.ControlInternal.Position.Y;

            return new Vector2(x, y);
        }

        internal static Size ComputeCellSize(DataViewer dataViewer, DataViewerCell cell)
        {
            int rowIndex = cell.RowIndex;
            int columnIndex = cell.ColumnIndex;

            if (rowIndex == -1 & columnIndex == -1)
                return new Size(dataViewer.RowHeadersWidth, dataViewer.ColumnHeadersHeight);
            if (rowIndex == -1 & columnIndex >= 0)
                return new Size(cell.OwningColumn.Width, dataViewer.ColumnHeadersHeight);
            else if (columnIndex == -1 && rowIndex >= 0)
                return new Size(dataViewer.RowHeadersWidth, cell.OwningRow.Height);
            else
                return new Size(cell.OwningColumn.Width, cell.OwningRow.Height);
        }

        protected internal virtual void CreateRenderableCell()
        {
            controlInternal.Id = GenerateCellId();
            controlInternal.ControlStyle = InheritedStyle.ToControlStyle();
            controlInternal.Position = ComputeCellPosition(DataViewer, this);

            controlInternal.Size = ComputeCellSize(DataViewer, this);


            // Add the cell to the Table's ControlCollection.

            DataViewer.Controls.Add(controlInternal);
        }

        protected string GenerateCellId()
        {
            string id = DataViewer.Id;
            bool isRowHeader = false;
            bool isColumnHeader = false;

            if (owningRow == null)
                isColumnHeader = true;
            else if (owningColumn == null)
                isRowHeader = true;

            StringBuilder sb = new StringBuilder(2);
            sb.Append(id);
            if (isRowHeader)
                sb.AppendFormat("_RowHeader:{0:D2}", RowIndex);
            else if (isColumnHeader)
                sb.AppendFormat("_ColumnHeader:{0:D2}", ColumnIndex);
            else
                sb.AppendFormat("_Cell:R{0:D2}C{1:D2}", RowIndex, ColumnIndex);

            return sb.ToString();
        }
    }
}