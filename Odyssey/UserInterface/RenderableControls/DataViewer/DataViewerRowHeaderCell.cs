#region Disclaimer

/* 
 * DataViewerRowHeaderCell
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
    public class DataViewerRowHeaderCell : DataViewerHeaderCell
    {
        public DataViewerRowHeaderCell() : base()
        {
        }

        //public new DataViewerCellStyle Style
        //{
        //    get
        //    {
        //        if (!HasStyle)
        //        {
        //            DataViewerCellStyle columnHeaderStyle = new DataViewerCellStyle();
        //            columnHeaderStyle.TextStyle =
        //                new TextStyle(true, false, StyleManager.DefaultFontSizeScaled, Color.White);
        //            base.Style = columnHeaderStyle;
        //        }
        //        return base.Style;
        //    }
        //    set { base.Style = value; }
        //}
    }
}