#region Disclaimer

/* 
 * DropDownList
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
using System.Drawing;
using System.Windows.Forms;
using AvengersUtd.Odyssey.UserInterface.Helpers;
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    /// <summary>
    /// This is the DropDownList control, also known as "ComboBox". 
    /// It allows the user to choose from a list of strings a particular choice or
    /// option. When the DropDownList is not clicked it displays the currently 
    /// selected choice. Othewise, when the user clicks on it, it displays the full
    /// list of choices.
    /// </summary>
    public class DropDownList : BaseControl, IContainer, ISpriteControl
    {
        const string ControlTag = "DropDownList";
        public const int DefaultDropDownButtonHeight = 30;
        public const int DefaultDropDownButtonWidth = 20;
        public const int DefaultListLabelOffsetX = 10;
        public const int DefaultListLabelOffsetY = 3;
        const string dropDownButtonTag = "DropDownButton";
        const string dropDownPanelTag = "DropDownPanel";

        static int count;

        // Bool variable used to detect if the control is in its droppeddown status
        bool droppedDown;
        Panel listPanel;

        // Currently Selected Index
        int selectedIndex;


        // Currently selected label.
        Label selectedLabel;

        ControlCollection controls;

        #region Private members

        // Sizes for the various sub-parts of the control
        ShapeDescriptor boxDescriptor;
        Size boxSize;
        DecoratedButton dropDownButton;
        int highlightedLabelIndex;
        Size itemSize;

        // Position vectors
        Vector2 labelListPosition;
        Size listSize;

        // ShapeDescriptor object

        #endregion

        #region Properties

        public ControlCollection Controls
        {
            get { return PublicControlCollection; }
        }

        protected ControlCollection PublicControlCollection
        {
            get { return listPanel.Controls; }
        }

        protected ControlCollection PrivateControlCollection
        {
            get { return controls; }
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
        }

        public string SelectedItem
        {
            get { return ((Label) Controls[selectedIndex]).Text; }
        }

        #endregion

        #region Constructors

        public DropDownList()
            : base(ControlTag + count, ControlTag, dropDownPanelTag)
        {
            Initialize();

            CreateSubComponents();
        }

        void Initialize()
        {
            count++;
            controls = new ControlCollection(this);
            ApplyStatusChanges = true;
            IsFocusable = true;
            itemSize = new Size(Size.Width, TextStyle.Size + Padding.Vertical);
            ShapeDescriptors = new ShapeDescriptorCollection(1);
        }

        void CreateDropDownButton()
        {
            dropDownButton = new DecoratedButton();
            dropDownButton.Id = ControlTag + '_' + dropDownButtonTag;

            dropDownButton.ControlStyleClass = dropDownButtonTag;
            dropDownButton.DecorationType = DecorationType.DownsideTriangle;
            dropDownButton.IsSubComponent = true;
            dropDownButton.CanRaiseEvents = dropDownButton.IsFocusable = false;
            PrivateControlCollection.Add(dropDownButton);
        }

        void CreateDropDownPanel()
        {
            listPanel = new Panel();

            listPanel.Id = ControlTag + "_Panel";
            listPanel.ControlStyleClass = dropDownPanelTag;
            listPanel.IsVisible = false;
            listPanel.IsSubComponent = true;

            listPanel.MouseMove += delegate(object sender, MouseEventArgs e)
                                       {
                                           highlightedLabelIndex =
                                               (int)
                                               ((e.Location.Y - listPanel.AbsolutePosition.Y +
                                                 ControlStyle.BorderSize)/
                                                (itemSize.Height));

                                           if (highlightedLabelIndex >= listPanel.Controls.Count)
                                               return;

                                           DebugManager.LogToScreen(highlightedLabelIndex.ToString());

                                           for (int i = 0; i < listPanel.Controls.Count; i++)
                                           {
                                               Label label = (Label) listPanel.Controls[i];
                                               if (i != highlightedLabelIndex)
                                                   label.SetHighlight(false);
                                               else
                                                   label.SetHighlight(true);
                                           }
                                       };

            listPanel.MouseDown += delegate(object sender, MouseEventArgs e) { Select(highlightedLabelIndex); };

            listPanel.MouseUp += delegate(object sender, MouseEventArgs e)
                                     {
                                         dropDownButton.IsHighlighted = false;
                                         Select(highlightedLabelIndex);
                                     };

            // Assign needed depth info
            PrivateControlCollection.Add(listPanel);
            listPanel.Depth =
                new Depth(listPanel.Depth.WindowLayer, listPanel.Depth.ComponentLayer + 1, listPanel.Depth.ZOrder);

            // Create a copy of the currently selected label
            selectedLabel = new Label();
            selectedLabel.Id = ControlTag + "_SelectedLabel";
            selectedLabel.Position = TopLeftPosition;
            selectedLabel.TextStyleClass = ControlTag;
            selectedLabel.IsSubComponent = true;

            PrivateControlCollection.Add(selectedLabel);
        }

        #endregion

        #region Exposed events

        public event EventHandler SelectedIndexChanged;
        public event EventHandler DropDown;
        public event EventHandler DropDownClosed;

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            Expand();

            if (DropDown != null)
                DropDown(this, e);
        }

        protected virtual void OnDropDownClosed(EventArgs e)
        {
            Collapse();

            if (DropDownClosed != null)
                DropDownClosed(this, e);
        }

        #endregion

        #region Overriden inherited events

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (OdysseyUI.CurrentHud.ClickedControl == null)
            {
                dropDownButton.IsHighlighted = true;
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (OdysseyUI.CurrentHud.ClickedControl == null)
            {
                dropDownButton.IsHighlighted = false;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (droppedDown)
            {
                OdysseyUI.CurrentHud.BeginDesign();
                OnDropDownClosed(e);
                OdysseyUI.CurrentHud.EndDesign();
            }
            dropDownButton.IsSelected = false;
            dropDownButton.IsHighlighted = false;
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!droppedDown)
            {
                OdysseyUI.CurrentHud.BeginDesign();
                dropDownButton.IsSelected = true;
                OnDropDown(EventArgs.Empty);
                OdysseyUI.CurrentHud.EndDesign();
            }
            else
            {
                OdysseyUI.CurrentHud.BeginDesign();
                OnDropDownClosed(EventArgs.Empty);
                OdysseyUI.CurrentHud.EndDesign();
            }
        }

        #endregion

        #region ISpriteControl Members

        public void Render()
        {
            selectedLabel.Render();
        }

        #endregion

        public override void UpdateShape()
        {
            // Updates sub-parts shapes
            boxDescriptor.UpdateShape(Shapes.DrawFullRectangle(AbsolutePosition, Size,
                                                               InnerAreaColor, BorderColor,
                                                               ControlStyle.Shading,
                                                               BorderSize,
                                                               BorderStyle,
                                                               Border.All ^ Border.Right));
        }

        public override void CreateShape()
        {
            base.CreateShape();
            boxDescriptor = Shapes.DrawFullRectangle(AbsolutePosition, Size,
                                                     InnerAreaColor, BorderColor,
                                                     ControlStyle.Shading,
                                                     BorderSize,
                                                     BorderStyle,
                                                     Border.All ^ Border.Right);
            ShapeDescriptors[0] = boxDescriptor;
            boxDescriptor.Depth = Depth;
        }

        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }

        void Expand()
        {
            droppedDown = true;
            listPanel.IsVisible = true;
        }

        void Collapse()
        {
            droppedDown = false;
            IsHighlighted = false;
            listPanel.IsVisible = false;
        }

        public void AddItems(params string[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                string item = items[i];

                Label label = new Label();
                label.Id = ControlTag + "_Item" + listPanel.Controls.Count;
                label.Text = item;
                label.Position =
                    //Vector2.Add(listPanel.TopLeftPosition, new Vector2(0, listPanel.Controls.Count*itemSize.Height));
                    new Vector2(0, listPanel.Controls.Count*itemSize.Height);
                label.TextStyle = TextStyle;

                listPanel.Add(label);
                listPanel.Size =
                    new Size(itemSize.Width,
                             itemSize.Height*listPanel.Controls.Count + 2*BorderSize + listPanel.Padding.Vertical);

                // let the panel handle highlights
                label.ApplyHighlight = false;
            }

            if (listPanel.Controls[0] != null)
            {
                selectedLabel.Text = ((Label) listPanel.Controls[0]).Text;
                selectedIndex = 0;
            }
        }

        /// <summary>
        /// Selects the label whose index is passed as parameter.
        /// </summary>
        /// <param name="index">The index of the label to select.</param>
        public void Select(int index)
        {
            if (index != selectedIndex && index < listPanel.Controls.Count)
            {
                selectedIndex = index;
                selectedLabel.Text = ((Label) listPanel.Controls[selectedIndex]).Text;
                OnSelectedIndexChanged(EventArgs.Empty);
            }
            if (droppedDown)
                OnLostFocus(EventArgs.Empty);
        }

        void CreateSubComponents()
        {
            // Create child-controls
            CreateDropDownButton();
            CreateDropDownPanel();
        }

        protected override void UpdateSizeDependantParameters()
        {
            base.UpdateSizeDependantParameters();
            // Define sub-parts sizes
            dropDownButton.Size = new Size(DefaultDropDownButtonWidth, Size.Height);
            boxSize = new Size(Size.Width - dropDownButton.Size.Width + BorderSize, Size.Height);
            itemSize = new Size(Size.Width, itemSize.Height);

            // DropDownButton
            dropDownButton.Position = new Vector2(boxSize.Width - BorderSize, 0);
            listPanel.Position = new Vector2(0, boxSize.Height);
        }

        protected override void UpdatePositionDependantParameters()
        {
            labelListPosition = TopLeftPosition;
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);

            itemSize = new Size(itemSize.Width, TextStyle.Size + listPanel.Padding.Vertical);
            foreach (Label l in listPanel.Controls)
            {
                l.Size = itemSize;
                l.TextStyle = TextStyle;
            }
        }

        #region IContainer Members

        ControlCollection IContainer.Controls
        {
            get { return Controls; }
        }

        ControlCollection IContainer.PrivateControlCollection
        {
            get { return PrivateControlCollection; }
        }

        ControlCollection IContainer.PublicControlCollection
        {
            get { return PublicControlCollection; }
        }

        #endregion
    }
}