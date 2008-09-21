#region Disclaimer

/* 
 * DataViewer
 * Custom EventArgs for the DataViewer class
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
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class RowsAddedEventArgs : EventArgs
    {
        // Private fields 
        int rowCount;
        int rowIndex;

        #region Public properties

        public int RowCount
        {
            get { return rowCount; }
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }

        #endregion [rgn]

        #region Constructors

        public RowsAddedEventArgs(int rowCount, int rowIndex)
        {
            this.rowCount = rowCount;
            this.rowIndex = rowIndex;
        }

        #endregion [rgn]
    }

    public class ColumnEventArgs : EventArgs
    {
        DataViewerColumn column;

        public ColumnEventArgs(DataViewerColumn column)
        {
            this.column = column;
        }

        public DataViewerColumn Column
        {
            get { return column; }
        }
    }

    public class CellFormattingEventArgs : EventArgs
    {
        DataViewerCellStyle cellStyle;
        int columnIndex;
        int rowIndex;

        public CellFormattingEventArgs(int rowIndex, int columnIndex, DataViewerCellStyle cellStyle)
        {
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            this.cellStyle = cellStyle;
        }

        public int ColumnIndex
        {
            get { return columnIndex; }
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }

        public DataViewerCellStyle CellStyle
        {
            get { return cellStyle; }
        }
    }

    public class CellEventArgs : EventArgs
    {
        int columnIndex;
        int rowIndex;

        public CellEventArgs(int rowIndex, int columnIndex)
        {
            this.rowIndex = rowIndex;
            this.columnIndex = columnIndex;
        }

        public int RowIndex
        {
            get { return rowIndex; }
        }

        public int ColumnIndex
        {
            get { return columnIndex; }
        }
    }
}