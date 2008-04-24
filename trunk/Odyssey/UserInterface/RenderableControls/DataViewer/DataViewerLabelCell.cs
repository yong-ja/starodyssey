#region Disclaimer

/* 
 * DataViewerLabelCell
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
    public class DataViewerLabelCell : DataViewerCell
    {
        Label label;

        public DataViewerLabelCell()
        {
            label = new Label();

            Cell<Label> cell;
            cell = new Cell<Label>();
            //hostedCell.IsSubComponent = label.IsSubComponent = true;
            cell.HostedControl = label;

            ControlInternal = cell;
        }


        protected internal override void UpdateValue()
        {
            base.UpdateValue();

            if (ValueType == typeof (string))
                label.Text = (string) Value;
            else
                label.Text = Value.ToString();
        }


        public override object Clone()
        {
            DataViewerLabelCell clonedDataViewerCell = (DataViewerLabelCell) base.Clone();

            if (!string.IsNullOrEmpty(label.Text))
                clonedDataViewerCell.Value = label.Text;

            return clonedDataViewerCell;
        }
    }
}