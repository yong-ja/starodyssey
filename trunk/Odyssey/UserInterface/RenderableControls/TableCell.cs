using System;
using System.Collections.Generic;
using System.Text;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odyssey.UserInterface
{
    public class TableCell : SimpleShapeControl, ISpriteControl
    {
        public const string ControlTag = "TableCell";
        static int count;
        readonly Label textLabel;

        int columnIndex;
        TableRow ownerRow;

        public TableRow OwnerRow
        {
            get { return ownerRow; }
            internal set { ownerRow = value; }
        }

        public string Text
        {
            get { return textLabel.Text; }
            set { textLabel.Text = value; }
        }

        internal int ColumnIndex
        {
            get { return columnIndex; }
            set { columnIndex = value; }
        }

        public TableCell()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            count++;
            textLabel = new Label
                            {
                                Id = (ControlTag + "_Label"),
                                IsSubComponent = true,
                                Parent = this,
                                TextStyleClass = TextStyleClass,
                                Position = TopLeftPosition
                            };
            IsFocusable = false;
        }

        #region ISpriteControl Members

        public void Render()
        {
            textLabel.Render();
        }

        #endregion

        protected override void UpdatePositionDependantParameters()
        {
            textLabel.ComputeAbsolutePosition();
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            textLabel.TextStyleClass = TextStyleClass;

        }

       
    }
}
