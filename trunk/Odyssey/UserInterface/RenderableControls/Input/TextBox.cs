#region Disclaimer

/* 
 * TextBox
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
using AvengersUtd.Odyssey.UserInterface.Style;
#if !(SlimDX)
    using Microsoft.DirectX;
#else
using SlimDX;
#endif

namespace AvengersUtd.Odyssey.UserInterface.RenderableControls
{
    [Flags]
    public enum KeyType
    {
        None = 0,
        Letter = 1,
        Digit = 2,
        LetterOrDigit = Letter | Digit,
        Punctuation = 4,
        Separator = 8,
        Control = 16
    }

    public class TextBox : BaseControl, ISpriteControl
    {
        const string ControlTag = "TextBox";
        const char DefaultPasswordChar = '*';
        static int count;

        bool acceptsReturn;
        bool isPassword;
        char passwordChar = DefaultPasswordChar;
        ShapeDescriptor boxDescriptor;
        Vector2 caretAbsolutePosition;
        Color caretColor = Color.Transparent;
        ShapeDescriptor caretDescriptor;
        int caretIndex;
        int passwordCharWidth;
        Vector2 caretPosition;
        bool drag;
        bool hideSelection = true;
        KeyType keyMask = KeyType.LetterOrDigit | KeyType.Punctuation | KeyType.Separator;
        int maximumLength = 32767;
        bool nonAcceptedKeyEntered;
        ShapeDescriptor selectionDescriptor;
        int selectionLength;
        int selectionStart;
        string text;
        string maskedText;
        Label textLabel;

        #region Properties

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                textLabel.Text = (isPassword) ? Helpers.Text.MakePasswordString(text, passwordChar) : text;
            }
        }

        public char PasswordChar
        {
            get { return passwordChar; }
            set { passwordChar = value; }
        }

        public int MaximumLength
        {
            get { return maximumLength; }
            set { maximumLength = value; }
        }

        public int SelectionStart
        {
            get
            {
                if (selectionLength > 0)
                    return selectionStart;
                else return selectionStart + selectionLength;
            }
            set
            {
                if (selectionLength == 0)
                    MoveCaretAtPosition(value);
                else
                {
                    Deselect();
                    Select(value, selectionLength);
                }
                if (IsFocused)
                    UpdateAppearance();
            }
        }

        public int SelectionLength
        {
            get { return Math.Abs(selectionLength); }
            set
            {
                if (selectionLength == 0)
                    Deselect();
                else
                {
                    Deselect();
                    Select(selectionStart, value);
                }
                if (IsFocused)
                    UpdateAppearance();
            }
        }

        public string SelectedText
        {
            get { return text.Substring(selectionStart, selectionLength); }
        }

        public bool AcceptsReturn
        {
            get { return acceptsReturn; }
            set { acceptsReturn = value; }
        }

        public bool IsPassword
        {
            get { return isPassword; }
            set
            {
                if (isPassword != value)
                {
                    isPassword = value;
                    Text = text;
                }
            }
        }

        public KeyType KeyMask
        {
            get { return keyMask; }
            set { keyMask = value; }
        }

        public bool HideSelection
        {
            get { return hideSelection; }
            set { hideSelection = value; }
        }

        int PasswordCharWidth
        {
            get
            {
                if (passwordCharWidth == 0)
                    passwordCharWidth = FontManager.GetCharacterLength(passwordChar, TextStyle);
                return passwordCharWidth;
            }
        }

        #endregion

        #region Constructors

        public TextBox() : base(ControlTag + count, ControlTag, ControlTag)
        {
            count++;


            caretPosition = TopLeftPosition;
            textLabel = new Label();
            textLabel.Id = ControlTag + "_Label";
            textLabel.Position = caretPosition;
            textLabel.Parent = this;
            textLabel.IsSubComponent = true;
            textLabel.TextStyleClass = TextStyleClass;

            ShapeDescriptors = new ShapeDescriptorCollection(3);
        }

        protected override void UpdatePositionDependantParameters()
        {
            textLabel.ComputeAbsolutePosition();
            caretAbsolutePosition = caretPosition + AbsolutePosition;
        }

        #endregion

        #region ISpriteControl Members

        public void Render()
        {
            textLabel.Render();
        }

        #endregion

        static KeyType GetKeyType(Char c)
        {
            KeyType keyType = KeyType.None;
            if (Char.IsLetter(c))
                keyType = KeyType.Letter;
            else if (Char.IsDigit(c))
                keyType = KeyType.Digit;
            else if (Char.IsPunctuation(c))
                keyType = KeyType.Punctuation;
            else if (Char.IsSeparator(c))
                keyType = KeyType.Separator;
            else
                keyType = KeyType.Control;

            return keyType;
        }

        /// <summary>
        /// Computes the absolute index of the caret if it had
        /// the specified index, but doesn't move it.
        /// </summary>
        /// <param name="index">The position to move the caret to.</param>
        /// <returns>The computed absolute position.</returns>
        Vector2 ComputeCaretPositionAtIndex(int index)
        {
            if (index == 0)
                return TopLeftPosition;

            string subString = text.Substring(0, index);
            int textWidth = (isPassword) ? PasswordCharWidth * subString.Length : FontManager.MeasureString(subString, textLabel.TextStyle);

            Vector2 newCaretPosition;
            newCaretPosition = new Vector2(textWidth + Padding.Left + BorderSize,
                                           caretPosition.Y);

            return newCaretPosition;
        }

        /// <summary>
        /// Moves the caret at the specified position and updates the
        /// caret's shapeDescriptor.
        /// </summary>
        /// <param name="index">The position to move the caret to.</param>
        void MoveCaretAtPosition(int index)
        {
            if (index < 0)
            {
                caretIndex = 0;
                return;
            }
            else if (index > text.Length)
            {
                caretIndex = text.Length;
                return;
            }

            if (selectionLength == 0)
                selectionStart = index;

            caretPosition = ComputeCaretPositionAtIndex(index);
            caretAbsolutePosition = caretPosition + AbsolutePosition;
            caretColor = Color.Black;
            caretIndex = index;

            caretDescriptor.IsDirty = true;
        }

        public void Deselect()
        {
            selectionStart = caretIndex;
            selectionLength = 0;
            selectionDescriptor.IsDirty = true;
        }

        Size GetSelectedSize(int startIndex, int length)
        {
            Vector2 startPosition, endPosition;

            if (length > 0)
            {
                startPosition = ComputeCaretPositionAtIndex(startIndex);
                endPosition = ComputeCaretPositionAtIndex(startIndex + length);
            }
            else
            {
                startPosition = ComputeCaretPositionAtIndex(startIndex + length);
                endPosition = ComputeCaretPositionAtIndex(startIndex);
            }
            return new Size((int) (endPosition.X - startPosition.X), ClientSize.Height);
        }

        void Select(int startIndex, int amount)
        {
            int leftToRightSelectionStart = startIndex + selectionLength;

            if (amount == 0 ||
                (leftToRightSelectionStart <= 0 && amount < 0) ||
                (leftToRightSelectionStart >= text.Length && amount > 0))
                return;

            selectionStart = startIndex;
            selectionLength += amount;
            caretDescriptor.IsDirty = true;
            selectionDescriptor.IsDirty = true;
            MoveCaretAtPosition(selectionStart + selectionLength);
        }

        int GetNearestIndextoClickLocatio(int clickLocationX)
        {
            clickLocationX -= (int) AbsolutePosition.X - Padding.Top;
            int[] charLenghts = FontManager.GetCharLengthArray(text, textLabel.TextStyle);


            int cursor = Padding.Top;
            if (clickLocationX <= ComputeCaretPositionAtIndex(text.Length).X)
                for (int i = 0; i < charLenghts.Length; i++)
                {
                    cursor += charLenghts[i];
                    if (cursor > clickLocationX)
                        return i;
                }

            return text.Length;
        }

        //void Select(int start, int length)


        public override bool IntersectTest(Point cursorLocation)
        {
            return Intersection.RectangleTest(AbsolutePosition, Size, cursorLocation);
        }


        public override void CreateShape()
        {
            base.CreateShape();
            boxDescriptor = ShapeDescriptor.ComputeShape(this, Shape.Rectangle);
            boxDescriptor.Depth = Depth;
            caretDescriptor = Shapes.DrawLine(0,
                                              caretColor,
                                              caretAbsolutePosition,
                                              new Vector2(caretAbsolutePosition.X, caretAbsolutePosition.Y +
                                                                                   ClientSize.Height));

            // We create one "dummy" descriptor. When text is selected, the selectionDescriptor will be
            // updated with real values. For now we just reserve our space in the vertexbuffer
            // by allocating a descriptor with the same number of vertices.
            selectionDescriptor = Shapes.DrawRectangle(AbsolutePosition,
                                                       System.Drawing.Size.Empty, Color.Empty,
                                                       new Shading(ShadingType.RectangleFlat));

            ShapeDescriptors[0] = boxDescriptor;
            ShapeDescriptors[1] = caretDescriptor;
            ShapeDescriptors[2] = selectionDescriptor;
            selectionDescriptor.Depth = caretDescriptor.Depth = Depth.AsChildOf(Depth);
        }

        public override void UpdateShape()
        {
            boxDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, Shape.Rectangle));

            if (caretDescriptor.IsDirty)
                caretDescriptor.UpdateShape(Shapes.DrawLine((IsFocused && selectionLength == 0) ? 2:0,
                                                            caretColor,
                                                            caretAbsolutePosition,
                                                            new Vector2(caretAbsolutePosition.X,
                                                                        caretAbsolutePosition.Y +
                                                                        ClientSize.Height))
                    );

            if (selectionDescriptor.IsDirty)
            {
                selectionDescriptor.UpdateShape(
                    Shapes.DrawRectangle(ComputeCaretPositionAtIndex(SelectionStart) + AbsolutePosition,
                                         GetSelectedSize(selectionStart, selectionLength),
                                         (IsFocused && selectionLength != 0)
                                             ? ControlStyle.ColorArray.Selected
                                             : Color.Green,
                                         new Shading(ShadingType.RectangleFlat)));
            }
        }

        #region Overriden inherited events

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (caretIndex >= 0)
                    {
                        if (e.Shift)
                        {
                            Select(selectionStart, -1);
                            UpdateAppearance();
                        }
                        else
                        {
                            if (selectionLength != 0)
                                Deselect();
                            MoveCaretAtPosition(caretIndex - 1);
                            UpdateAppearance();
                        }
                    }
                    break;

                case Keys.Right:
                    if (caretIndex <= text.Length)
                    {
                        if (e.Shift)
                        {
                            Select(selectionStart, 1);
                            UpdateAppearance();
                        }
                        else
                        {
                            if (selectionLength != 0)
                                Deselect();

                            MoveCaretAtPosition(caretIndex + 1);
                            UpdateAppearance();
                        }
                    }
                    break;

                case Keys.Back:
                    if (caretIndex >= 0)
                    {
                        if (selectionLength != 0)
                        {
                            Text = text.Remove(SelectionStart, SelectionLength);
                            if (selectionLength > 0)
                                MoveCaretAtPosition(selectionStart);
                            else
                                MoveCaretAtPosition(selectionStart + selectionLength);

                            Deselect();
                        }
                        else
                        {
                            if (caretIndex != 0)
                                Text = text.Remove(caretIndex - 1, 1);
                            MoveCaretAtPosition(caretIndex - 1);
                        }

                        UpdateAppearance();

                        nonAcceptedKeyEntered = true;
                    }
                    break;

                case Keys.Delete:
                    if (caretIndex <= text.Length)
                    {
                        if (selectionLength != 0)
                        {
                            Text = text.Remove(SelectionStart, SelectionLength);
                            if (selectionLength > 0)
                                MoveCaretAtPosition(selectionStart);
                            else
                                MoveCaretAtPosition(selectionStart + selectionLength);
                            Deselect();

                            UpdateAppearance();
                        }
                        else
                        {
                            if (caretIndex != text.Length)
                                Text = text.Remove(caretIndex, 1);
                        }


                        nonAcceptedKeyEntered = true;
                    }
                    break;

                case Keys.Home:
                    if (e.Shift)
                    {
                        Select(caretIndex, -caretIndex);
                        UpdateAppearance();
                    }
                    else if (caretIndex >= 0)
                    {
                        if (selectionLength != 0)
                            Deselect();

                        MoveCaretAtPosition(0);
                        UpdateAppearance();
                    }
                    break;

                case Keys.End:
                    if (e.Shift)
                    {
                        Select(caretIndex, text.Length - caretIndex);
                        UpdateAppearance();
                    }
                    else if (caretIndex <= text.Length)
                    {
                        if (selectionLength != 0)
                            Deselect();

                        MoveCaretAtPosition(text.Length);
                        UpdateAppearance();
                    }
                    break;

                case Keys.Escape:
                    nonAcceptedKeyEntered = true;
                    OnLostFocus(EventArgs.Empty);
                    break;

                case Keys.Return:
                    if (!acceptsReturn)
                    {
                        nonAcceptedKeyEntered = true;
                        OnLostFocus(EventArgs.Empty);
                    }
                    break;

                default:
                    nonAcceptedKeyEntered = false;
                    break;
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (nonAcceptedKeyEntered || text.Length + 1 > maximumLength)
                return;
            else
            {
                Char c = e.KeyChar;
                KeyType type = GetKeyType(c);
                if ((type & keyMask) == type)
                {
                    if (selectionLength > 0)
                    {
                        Text = text.Replace(text.Substring(selectionStart, selectionLength), c.ToString());

                        MoveCaretAtPosition(selectionStart + 1);
                        Deselect();
                        UpdateAppearance();
                    }
                    else if (selectionLength < 0)
                    {
                        int leftToRightStart = selectionStart + selectionLength;

                        Text = text.Replace(text.Substring(leftToRightStart, Math.Abs(selectionLength)), c.ToString());
                        MoveCaretAtPosition(leftToRightStart + 1);
                        Deselect();
                        UpdateAppearance();
                    }
                    else if (textLabel.MeasureStringIfIncreasedBy(c).Size.Width <= ClientSize.Width)
                    {
                        if (caretIndex == text.Length)
                            Text += c;
                        else
                            Text = text.Insert(caretIndex, c.ToString());

                        MoveCaretAtPosition(caretIndex + 1);
                        UpdateAppearance();
                    }
                }
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (text.Length > 0)
            {
                Select(0, text.Length);
                caretColor = Color.Black;
                caretIndex = text.Length;
            }
            else
            {
                MoveCaretAtPosition(text.Length);
            }
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            caretColor = Color.FromArgb(0, 255, 0 ,0);
            caretDescriptor.IsDirty = true;
            if (drag)
            {
                drag = false;
                OnMouseCaptureChanged(e);
            }

            if (hideSelection)
                Deselect();

            base.OnLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsFocused)
            {
                MoveCaretAtPosition(GetNearestIndextoClickLocatio(e.X));
                Deselect();
                drag = true;
                OdysseyUI.CurrentHud.CaptureControl = this;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (drag)
            {
                selectionLength = 0;
                int selectionAmount = GetNearestIndextoClickLocatio(e.X) - selectionStart;
                Select(selectionStart, selectionAmount);

                UpdateAppearance();
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (drag)
            {
                drag = false;
                OnMouseCaptureChanged(e);
            }
        }

        protected override void OnTextStyleChanged(EventArgs e)
        {
            base.OnTextStyleChanged(e);
            textLabel.TextStyle = TextStyle;
            passwordCharWidth = FontManager.GetCharacterLength(passwordChar, TextStyle);
        }

        #endregion
    }
}