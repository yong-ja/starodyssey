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
using System.Linq;
using System.Windows.Forms;
using AvengersUtd.Odyssey.Geometry;
using AvengersUtd.Odyssey.UserInterface.Drawing;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Text;
using SlimDX;

namespace AvengersUtd.Odyssey.UserInterface.Controls
{
    /// <summary>
    /// This is the DropDownList control, also known as "ComboBox". 
    /// It allows the user to choose from a list of strings a particular choice or
    /// option. When the DropDownList is not clicked it displays the currently 
    /// selected choice. Othewise, when the user clicks on it, it displays the full
    /// list of choices.
    /// </summary>
    public class DropDownList : BaseControl, IContainer
    {
        const string ControlTag = "DropDownList";
        public const int DefaultDropDownButtonHeight = 30;
        public const int DefaultDropDownButtonWidth = 20;
        public const int DefaultListTextLiteralOffsetX = 10;
        public const int DefaultListTextLiteralOffsetY = 3;
        const string DropDownButtonTag = "DropDownButton";
        const string DropDownPanelTag = "DropDownPanel";

        static int count;

        #region Private members
        // Bool variable used to detect if the control is in its droppeddown status
        bool droppedDown;

        Panel listPanel;
        // Currently selected TextLiteral.
        TextLiteral selectedTextLiteral;

        ControlCollection controls;

        // Currently Selected Index

        // Sizes for the various sub-parts of the control
        ShapeDescription boxDescriptor;
        Size boxSize;
        DecoratorButton dropDownButton;
        int highlightedTextLiteralIndex;
        Size itemSize;

        // PositionV3 vectors
        Vector2 textLiteralListPosition;
        Size listSize;

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
        protected ControlCollection PrivateControlCollection
        {
            get { return controls; }
        }

        public int SelectedIndex { get; private set; }

        public string SelectedItem
        {
            get { return ((TextLiteral) Controls[SelectedIndex]).Content; }
        }

        public string[] Items { get; set; }

        #endregion

        #region Constructors

        public DropDownList()
            : base(ControlTag + ++count, ControlTag)
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
            itemSize = new Size(Size.Width, TextDescription.Size + Description.Padding.Vertical);
            Shapes = new ShapeCollection(1);
            Items = new[]{Id};
        }

        void CreateDropDownButton()
        {
            dropDownButton = new DecoratorButton
                                 {
                                     Id = ControlTag + '_' + DropDownButtonTag,
                                     DecorationType = DecorationType.DownsideTriangle,
                                     IsSubComponent = true,
                                     CanRaiseEvents = false,
                                     IsFocusable = false
                                 };
            // DropDownButton

            PrivateControlCollection.Add(dropDownButton);
        }

        void CreateDropDownPanel()
        {
            listPanel = new Panel
                            {
                                Id = ControlTag + "_Panel",
                                ControlDescriptionClass = DropDownPanelTag,
                                IsVisible = false,
                                IsSubComponent = true,
                                Position = new Vector2(0, boxSize.Height)
                            };

            listPanel.MouseMove += (sender, e) =>
                                       {
                                           highlightedTextLiteralIndex = (int)
                                                                         ((e.Location.Y - listPanel.AbsolutePosition.Y +
                                                                           Description.BorderSize)/
                                                                          (itemSize.Height));

                                           if (highlightedTextLiteralIndex >= listPanel.Controls.Count)
                                               return;

                                           // DebugManager.LogToScreen(highlightedTextLiteralIndex.ToString());

                                           for (int i = 0; i < listPanel.Controls.Count; i++)
                                           {
                                               TextLiteral textLiteral = (TextLiteral) listPanel.Controls[i];
                                               textLiteral.IsHighlighted = i == highlightedTextLiteralIndex;
                                           }
                                       };

            listPanel.MouseDown += (sender, e) => Select(highlightedTextLiteralIndex);

            listPanel.MouseUp += (sender, e) =>
                                     {
                                         dropDownButton.IsHighlighted = false;
                                         Select(highlightedTextLiteralIndex);
                                     };

            // Assign needed depth info
            PrivateControlCollection.Add(listPanel);


            // Create a copy of the currently selected TextLiteral
            selectedTextLiteral = new TextLiteral(true)
                                      {
                                          Id = ControlTag + "_SelectedTextLiteral",
                                          Position = TopLeftPosition,
                                          TextDescriptionClass = ControlTag,
                                          IsSubComponent = true
                                      };

            PrivateControlCollection.Add(selectedTextLiteral);
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
                OnDropDownClosed(e);

            dropDownButton.IsSelected = false;
            dropDownButton.IsHighlighted = false;
            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!droppedDown)
            {
                OnDropDown(EventArgs.Empty);
                dropDownButton.IsSelected = true;
            }
            else
            {
                dropDownButton.IsSelected = false;
                OnDropDownClosed(EventArgs.Empty);
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            dropDownButton.UpdateAppearance(UpdateAction.Move);
        }

        #endregion

     
        public override void UpdateShape()
        {
            // Updates sub-parts shapes
            boxDescriptor.UpdateVertices(ShapeCreator.DrawFullRectangle(AbsoluteOrthoPosition, Size,
                Description.FillShader,InnerAreaColor,Description.BorderSize, Description.BorderStyle, BorderColor).Vertices);
        }

        public override void CreateShape()
        {
            boxDescriptor = ShapeCreator.DrawFullRectangle(AbsoluteOrthoPosition, Size,
                Description.FillShader,InnerAreaColor,Description.BorderSize, Description.BorderStyle, BorderColor);
            boxDescriptor.Depth = Depth;
            boxDescriptor.Tag = Id;
            Shapes[0] = boxDescriptor;

            listPanel.Depth = new Depth
            {
                WindowLayer = Depth.WindowLayer,
                ComponentLayer = Depth.ComponentLayer - 1,
                ZOrder = Depth.ZOrder
            };

            foreach (BaseControl control in listPanel.Controls)
            {
                control.Depth = Depth.AsChildOf(listPanel.Depth);
            }
            AddItems(Items);
            
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

        public void AddItems(params string[] dropDownItems)
        {
            foreach (TextLiteral textLiteral in dropDownItems.Select(item => new TextLiteral
                                                                         {
                                                                             Id = ControlTag + "_Item" + listPanel.Controls.Count,
                                                                             Content = item, 
                                                                             Position = new Vector2(0, listPanel.Controls.Count*itemSize.Height),
                                                                             TextDescription = TextDescription,
                                                                             IsSubComponent = true
                                                                         }))
            {
                listPanel.Add(textLiteral);
                listPanel.Size = new Size(itemSize.Width,
                    itemSize.Height*listPanel.Controls.Count + 2*Description.BorderSize +
                    listPanel.Description.Padding.Vertical);

                // let the panel handle highlights
                textLiteral.IsHighlighted = false;
            }

            if (listPanel.Controls[0] == null) return;

            selectedTextLiteral.Content = ((TextLiteral) listPanel.Controls[0]).Content;
            SelectedIndex = 0;
        }

        /// <summary>
        /// Selects the TextLiteral whose index is passed as parameter.
        /// </summary>
        /// <param name="index">The index of the TextLiteral to select.</param>
        public void Select(int index)
        {
            if (index != SelectedIndex && index < listPanel.Controls.Count)
            {
                SelectedIndex = index;
                selectedTextLiteral.Content = ((TextLiteral) listPanel.Controls[SelectedIndex]).Content;
                OnSelectedIndexChanged(EventArgs.Empty);
            }
            if (droppedDown)
                OnLostFocus(EventArgs.Empty);
        }

        void CreateSubComponents()
        {
            boxSize = new Size(Size.Width, Size.Height);
            itemSize = new Size(Size.Width, itemSize.Height);
            // Create child-controls
            CreateDropDownButton();
            CreateDropDownPanel();
            
            textLiteralListPosition = TopLeftPosition;
            UpdateSizeDependantParameters();
        }

        protected override void UpdateSizeDependantParameters()
        {
            // Define sub-parts sizes
            if (dropDownButton != null)
            {
                dropDownButton.Position = new Vector2(Size.Width - DefaultDropDownButtonWidth, 0);
                dropDownButton.Size = new Size(DefaultDropDownButtonWidth, Size.Height);
            }
            boxSize = new Size(Size.Width, Size.Height);
            itemSize = new Size(Size.Width, itemSize.Height);
        }

        protected override void UpdatePositionDependantParameters()
        {
            textLiteralListPosition = TopLeftPosition;
            dropDownButton.ComputeAbsolutePosition();
        }

        protected override void OnTextDescriptionChanged(EventArgs e)
        {
            base.OnTextDescriptionChanged(e);

            itemSize = new Size(itemSize.Width, TextDescription.Size + listPanel.Description.Padding.Vertical);
            foreach (TextLiteral l in listPanel.Controls)
            {
                l.Size = itemSize;
                l.TextDescription = TextDescription;
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