#region Disclaimer

/* 
 * DataViewerHeaderCell
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

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    public abstract class DataViewerHeaderCell : DataViewerCell
    {
        Button headerButton;

        protected DataViewerHeaderCell()
        {
            headerButton = new Button();

            ControlInternal = headerButton;
        }

        protected Button HeaderButton
        {
            get { return headerButton; }
        }

        public string Caption
        {
            get { return HeaderButton.Text; }
            internal set { HeaderButton.Text = value; }
        }

        protected internal override void CreateRenderableCell()
        {
            BaseControl cell = ControlInternal;

            cell.Id = GenerateCellId();
            cell.Position = ComputeCellPosition(DataViewer, this);
            cell.TextStyleClass = "HeaderCell";

            cell.Size = ComputeCellSize(DataViewer, this);

            // Add the cell to the Table's ControlCollection.
            DataViewer.Controls.Add(cell);
        }
    }
}