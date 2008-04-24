#region Disclaimer

/* 
 * DataViewer
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
using System.Collections;
using System.Drawing;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Properties;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public class DataViewer : ContainerControl
    {
        // [rgn] Constants 

        const string controlTag = "DataViewer";

        const int DefaultColumnHeadersHeight = 30;
        const int DefaultRowHeadersWidth = 50;
        static int count;

        // [rgn] Private fields 
        int cellSpacing;
        int columnHeadersHeight;
        DataViewerColumnCollection columns;
        Type dataElementType;
        object dataSource;
        DataViewerCellStyle defaultCellStyle;
        IList list;
        int rowHeadersWidth;
        DataViewerRowCollection rows;
        DataViewerCellStyle rowsDefaultCellStyle;

        #region Public properties 

        public int CellSpacing
        {
            get { return cellSpacing; }
            set { cellSpacing = value; }
        }

        public int RowHeadersWidth
        {
            get { return rowHeadersWidth; }
            set { rowHeadersWidth = value; }
        }

        public int ColumnHeadersHeight
        {
            get { return columnHeadersHeight; }
            set { columnHeadersHeight = value; }
        }

        public DataViewerCellStyle DefaultCellStyle
        {
            get
            {
                if (defaultCellStyle == null)
                    defaultCellStyle = new DataViewerCellStyle();

                return defaultCellStyle;
            }
            set { defaultCellStyle = value; }
        }

        public DataViewerCellStyle RowsDefaultCellStyle
        {
            get
            {
                if (rowsDefaultCellStyle == null)
                    rowsDefaultCellStyle = new DataViewerCellStyle();

                return rowsDefaultCellStyle;
            }
            set { rowsDefaultCellStyle = value; }
        }

        public DataViewerRowCollection Rows
        {
            get { return rows; }
        }

        public DataViewerColumnCollection Columns
        {
            get { return columns; }
        }

        public int RowCount
        {
            get { return rows.Count; }
            set
            {
                int currentRowCount = rows.Count;
                if (value == 0)
                    rows.Clear();
                else if (value < currentRowCount)
                {
                    for (int i = value; i < currentRowCount; i++)
                    {
                        rows.RemoveAt(i);
                    }
                }
                else if (value > currentRowCount)
                {
                    for (int i = currentRowCount; i < value; i++)
                    {
                        rows.Add(new DataViewerRow());
                    }
                }
            }
        }

        public int ColumnCount
        {
            get { return columns.Count; }
            set
            {
                int currentColumnsCount = columns.Count;

                if (value == 0)
                    columns.Clear();
                else if (value < currentColumnsCount)
                    for (int i = value; i < currentColumnsCount; i++)
                    {
                        columns.RemoveAt(i);
                    }
                else if (value > currentColumnsCount)
                {
                    for (int i = currentColumnsCount; i < value; i++)
                    {
                        columns.Add(new DataViewerLabelColumn());
                    }
                }
            }
        }

        internal Type DataElementType
        {
            get
            {
                if (dataElementType == null)
                    dataElementType = StaticReflection.FindUnderlyingType(dataSource.GetType());
                return dataElementType;
            }
        }

        public object DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;
                    OnDataSourceChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Exposed Events

        static readonly object EventCellFormatting;
        static readonly object EventCellValueChanged;
        static readonly object EventColumnAdded;
        static readonly object EventColumnNameChanged;
        static readonly object EventColumnWidthChanged;
        static readonly object EventDataSourceChanged;
        static readonly object EventRowsAdded;

        public event EventHandler<CellFormattingEventArgs> CellFormatting
        {
            add { Events.AddHandler(EventCellFormatting, value); }
            remove { Events.RemoveHandler(EventCellFormatting, value); }
        }

        public event EventHandler<CellFormattingEventArgs> CellValueChanged
        {
            add { Events.AddHandler(EventCellValueChanged, value); }
            remove { Events.RemoveHandler(EventCellValueChanged, value); }
        }

        public event EventHandler<ColumnEventArgs> ColumnAdded
        {
            add { Events.AddHandler(EventColumnAdded, value); }
            remove { Events.RemoveHandler(EventColumnAdded, value); }
        }

        public event EventHandler<ColumnEventArgs> ColumnNameChanged
        {
            add { Events.AddHandler(EventColumnNameChanged, value); }
            remove { Events.RemoveHandler(EventColumnNameChanged, value); }
        }

        public event EventHandler<ColumnEventArgs> ColumnWidthChanged
        {
            add { Events.AddHandler(EventColumnWidthChanged, value); }
            remove { Events.RemoveHandler(EventColumnWidthChanged, value); }
        }

        public event EventHandler DataSourceChanged
        {
            add { Events.AddHandler(EventDataSourceChanged, value); }
            remove { Events.RemoveHandler(EventDataSourceChanged, value); }
        }

        public event EventHandler<RowsAddedEventArgs> RowsAdded
        {
            add { Events.AddHandler(EventRowsAdded, value); }
            remove { Events.RemoveHandler(EventRowsAdded, value); }
        }

        protected internal virtual void OnCellValueChanged(CellEventArgs e)
        {
            EventHandler<CellEventArgs> handler = (EventHandler<CellEventArgs>) Events[EventCellValueChanged];
            if (handler != null)
                handler(this, e);
        }

        protected internal virtual void OnCellFormatting(CellFormattingEventArgs e)
        {
            EventHandler<CellFormattingEventArgs> handler =
                (EventHandler<CellFormattingEventArgs>) Events[EventRowsAdded];
            if (handler != null)
                handler(this, e);
        }

        protected internal virtual void OnColumnNameChanged(ColumnEventArgs e)
        {
            DataViewerColumn dataViewerColumn = e.Column;


            EventHandler<ColumnEventArgs> handler = (EventHandler<ColumnEventArgs>) Events[EventColumnNameChanged];
            if (handler != null)
                handler(this, e);
        }

        protected internal virtual void OnColumnWidthChanged(ColumnEventArgs e)
        {
            DataViewerColumn dataViewerColumn = e.Column;

            if (dataViewerColumn.HasHeaderCell)
            {
                dataViewerColumn.HeaderCell.ControlInternal.Size =
                    DataViewerCell.ComputeCellSize(this, dataViewerColumn.HeaderCell);


                if (dataViewerColumn.Index + 1 <= ColumnCount)
                {
                    for (int c = dataViewerColumn.Index + 1; c < ColumnCount; c++)
                    {
                        DataViewerColumn column = columns[c];
                        column.HeaderCell.ControlInternal.Position =
                            DataViewerCell.ComputeCellPosition(this, column.HeaderCell);
                    }
                }
            }

            for (int r = 0; r < RowCount; r++)
            {
                DataViewerCell cell = rows[r].Cells[dataViewerColumn.Index];
                cell.ControlInternal.Size = DataViewerCell.ComputeCellSize(this, cell);
            }

            if (dataViewerColumn.Index + 1 <= ColumnCount)
                for (int r = 0; r < RowCount; r++)
                {
                    for (int c = dataViewerColumn.Index + 1; c < ColumnCount; c++)
                    {
                        DataViewerCell cell = rows[r].Cells[c];
                        cell.ControlInternal.Position = DataViewerCell.ComputeCellPosition(this, cell);
                    }
                }


            EventHandler<ColumnEventArgs> handler = (EventHandler<ColumnEventArgs>) Events[EventColumnWidthChanged];
            if (handler != null)
                handler(this, e);
        }

        protected internal virtual void OnColumnAdded(ColumnEventArgs e)
        {
            DataViewerColumn dataViewerColumn = e.Column;
            dataViewerColumn.HeaderCell.CreateRenderableCell();

            for (int i = 0; i < RowCount; i++)
            {
                DataViewerRow dataViewerRow = rows[i];
                DataViewerCell dataViewerCell = (DataViewerCell) Activator.CreateInstance(dataViewerColumn.CellType);
                dataViewerRow.Cells.Add(dataViewerCell);

                dataViewerCell.DataViewer = this;
                dataViewerCell.OwningColumn = dataViewerColumn;
            }

            EventHandler<ColumnEventArgs> handler = (EventHandler<ColumnEventArgs>) Events[EventColumnAdded];
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            if ((dataElementType = StaticReflection.FindUnderlyingType(dataSource.GetType())) == null)
            {
                throw new InvalidDataSourceException(Exceptions.DataViewer_InvalidDataSource);
            }
            list = (IList) dataSource;

            EventHandler handler = (EventHandler) Events[EventDataSourceChanged];
            if (handler != null)
                handler(this, e);
        }

        protected internal virtual void OnRowsAdded(RowsAddedEventArgs e)
        {
            for (int i = e.RowIndex; i < e.RowIndex + e.RowCount; i++)
            {
                CheckRow(rows[i]);
            }
            EventHandler<RowsAddedEventArgs> handler = (EventHandler<RowsAddedEventArgs>) Events[EventRowsAdded];
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Constructors 

        static DataViewer()
        {
            EventCellFormatting = new object();
            EventCellValueChanged = new object();
            EventDataSourceChanged = new object();
            EventRowsAdded = new object();
            EventColumnAdded = new object();
            EventColumnNameChanged = new object();
            EventColumnWidthChanged = new object();
        }

        public DataViewer()
        {
            ApplyStatusChanges = false;
            ApplyControlStyle(StyleManager.GetControlStyle(controlTag));
            rows = new DataViewerRowCollection(this);
            columns = new DataViewerColumnCollection(this);

            rowHeadersWidth = DefaultRowHeadersWidth;
            columnHeadersHeight = DefaultColumnHeadersHeight;

            cellSpacing = 1;
        }

        #endregion

        void CheckRow(DataViewerRow row)
        {
            row.HeaderCell.CreateRenderableCell();

            // Trim excess cells
            if (row.Cells.Count > ColumnCount)
            {
                for (int i = ColumnCount - 1; i < row.Cells.Count; i++)
                {
                    row.Cells.RemoveAt(i);
                }
            }
                // Add cells if necessary
            else if (row.Cells.Count < ColumnCount)
            {
                for (int i = row.Cells.Count; i < ColumnCount; i++)
                {
                    DataViewerCell newCell = (DataViewerCell) Activator.CreateInstance(columns[i].CellType);
                    row.Cells.Add(newCell);
                }
            }

            // Position each cell and get its value if bound to a DataSource
            for (int i = 0; i < row.Cells.Count; i++)
            {
                DataViewerCell dataViewerCell = row.Cells[i];
                if (dataViewerCell.DataViewer == null)
                {
                    dataViewerCell.DataViewer = this;
                    dataViewerCell.OwningColumn = columns[i];
                }
                dataViewerCell.CreateRenderableCell();
            }
        }

        void ComputeSize()
        {
            int width = BorderSize*2 + Padding.Horizontal + rowHeadersWidth;
            int height = BorderSize*2 + Padding.Vertical + columnHeadersHeight;

            foreach (DataViewerRow row in rows)
            {
                height += row.Height + cellSpacing;
            }

            foreach (DataViewerColumn column in columns)
            {
                width += column.Width + cellSpacing;
            }

            Size = new Size(width, height);
        }

        internal object GetDataSourceObject(int rowIndex)
        {
            return list[rowIndex];
        }

        public void RefreshValues()
        {
            ComputeSize();

            foreach (DataViewerRow row in rows)
            {
                row.HeaderCell.Caption = (row.Index + 1).ToString();

                foreach (DataViewerCell cell in row.Cells)
                {
                    cell.UpdateValue();
                }
            }
        }


        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        public override void CreateShape()
        {
            base.CreateShape();
            ShapeDescriptor = Shapes.DrawFullRectangle(AbsolutePosition, Size,
                                                       InnerAreaColor, BorderColor,
                                                       ControlStyle.Shading,
                                                       BorderSize,
                                                       BorderStyle,
                                                       DefaultCellStyle.Borders);
            ShapeDescriptor.Depth = Depth;
            ShapeDescriptors[0] = ShapeDescriptor;
        }

        public override void UpdateShape()
        {
            ShapeDescriptor.UpdateShape(Shapes.DrawFullRectangle(AbsolutePosition, Size,
                                                                 InnerAreaColor, BorderColor,
                                                                 ControlStyle.Shading,
                                                                 BorderSize,
                                                                 BorderStyle,
                                                                 DefaultCellStyle.Borders)
                );
        }

        #region Overriden inherited methods 

        #endregion [rgn]
    }
}