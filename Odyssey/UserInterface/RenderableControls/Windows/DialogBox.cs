#region Disclaimer

/* 
 * DialogBox
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
    public enum DialogBoxButtons
    {
        Ok,
        OkCancel,
        YesNo
    }

    public enum DialogResult
    {
        None,
        Ok,
        Cancel,
        Yes,
        No
    }

    public class DialogResultEventArgs : EventArgs
    {
        DialogResult dialogResult;

        public DialogResult DialogResult
        {
            get { return dialogResult; }
        }


        public DialogResultEventArgs(DialogResult dialogResult)
        {
            this.dialogResult = dialogResult;
        }
    }


    public class DialogBox : Window
    {
        const int DefaultButtonPaddingX = 5;
        const int DefaultButtonPaddingY = 5;
        static Size DefaultButtonSize = new Size(90, 30);
        static Size DefaultDialogSize = new Size(500, 250);

        DialogResult closeBehavior;
        DialogResult dialogResult;

        RichTextArea rta;


        internal RichTextArea RichTextArea
        {
            get { return rta; }
        }

        static DialogBox()
        {
            EventDialogResultAvailable = new object();
        }

        DialogBox()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(GetType().Name));
            IsModal = true;

            rta = new RichTextArea();
            rta.Id = Id + "_RTA";

            PublicControlCollection.Add(rta);

            CaptionBar.CloseButton.MouseClick +=
                delegate(object sender, MouseEventArgs e)
                    {
                        dialogResult = closeBehavior;
                        Close();
                        OnDialogResultAvailable(new DialogResultEventArgs(dialogResult));
                    };
        }

        void ConfigureButtons(DialogBoxButtons buttons)
        {
            switch (buttons)
            {
                default:
                case DialogBoxButtons.Ok:
                    closeBehavior = DialogResult.Ok;
                    AddButtons(1, new string[] {"Ok"}, ReturnOk);

                    break;

                case DialogBoxButtons.OkCancel:
                    closeBehavior = DialogResult.Cancel;
                    AddButtons(2, new string[] {"Ok", "Cancel"}, ReturnOk, ReturnCancel);

                    break;

                case DialogBoxButtons.YesNo:
                    closeBehavior = DialogResult.No;
                    AddButtons(2, new string[] {"Yes", "No"}, ReturnYes, ReturnNo);

                    break;
            }
        }

        static readonly object EventDialogResultAvailable;

        public event EventHandler<DialogResultEventArgs> DialogResultAvailable
        {
            add { Events.AddHandler(EventDialogResultAvailable, value); }
            remove { Events.RemoveHandler(EventDialogResultAvailable, value); }
        }

        public static void Show(string title, string text, DialogBoxButtons buttons,
                                EventHandler<DialogResultEventArgs> eventHandler)
        {
            Hud hud = OdysseyUI.CurrentHud;
            hud.BeginDesign();
            Size hudSize = hud.Size;

            DialogBox dialog = new DialogBox();
            dialog.Title = title;
            dialog.RichTextArea.MarkupText = text;
            dialog.Position = new Vector2(hudSize.Width/2 - dialog.Size.Width/2,
                                          hudSize.Height/2 - dialog.Size.Height/2);
            ;
            dialog.ConfigureButtons(buttons);
            dialog.DialogResultAvailable += eventHandler;

            hud.Add(dialog);
            hud.WindowManager.BringToFront(dialog);
            hud.EndDesign();
        }


        void AddButtons(int number, string[] labels, params MouseEventHandler[] handlers)
        {
            if (labels.Length != number && handlers.Length != number)
                throw new ArgumentException("You need to supply an equal number of buttons, labels and event handlers");


            for (int i = 0; i < number; i++)
            {
                Vector2 buttonPosition = new Vector2(
                    Size.Width + DefaultButtonSize.Width*(i - number) +
                    Padding.Right*(i + 1 - number) - (Padding.Left + BorderSize),
                    Size.Height - Padding.Bottom - CaptionBar.DefaultCaptionBarHeight);

                Button button = new Button();
                button.Id = string.Format("{0}Button", labels[i]);
                button.Position = buttonPosition;
                button.Size = DefaultButtonSize;
                button.IsSubComponent = true;
                button.MouseClick += handlers[i];
                button.Text = labels[i];

                PrivateControlCollection.Add(button);
            }
        }

        protected override void UpdateSizeDependantParameters()
        {
            ClientSize = CaptionBar.Size = new Size(Size.Width - 2*BorderSize, CaptionBar.DefaultCaptionBarHeight);
            ClientSize = ContainerPanel.Size = new Size(
                                                   Size.Width - (BorderSize*2 + Padding.Horizontal),
                                                   Size.Height -
                                                   (BorderSize*2 + CaptionBar.Size.Height + DefaultButtonSize.Height +
                                                    Padding.Vertical));
            rta.Size = ContainerPanel.Size;
        }

        void ReturnOk(object sender, MouseEventArgs e)
        {
            dialogResult = DialogResult.Ok;
            Close();
            OnDialogResultAvailable(new DialogResultEventArgs(dialogResult));
        }

        void ReturnCancel(object sender, MouseEventArgs e)
        {
            dialogResult = DialogResult.Cancel;
            Close();
            OnDialogResultAvailable(new DialogResultEventArgs(dialogResult));
        }

        void ReturnYes(object sender, MouseEventArgs e)
        {
            dialogResult = DialogResult.Yes;
            Close();
            OnDialogResultAvailable(new DialogResultEventArgs(dialogResult));
        }

        void ReturnNo(object sender, MouseEventArgs e)
        {
            dialogResult = DialogResult.No;
            Close();
            OnDialogResultAvailable(new DialogResultEventArgs(dialogResult));
        }

        public virtual void OnDialogResultAvailable(DialogResultEventArgs e)
        {
            EventHandler<DialogResultEventArgs> handler =
                (EventHandler<DialogResultEventArgs>) Events[EventDialogResultAvailable];
            if (handler != null)
                handler(this, e);
        }
    }
}