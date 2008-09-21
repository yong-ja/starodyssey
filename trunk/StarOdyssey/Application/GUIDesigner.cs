using System;
using System.Drawing;
using AvengersUtd.MultiversalRuleSystem.Properties;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.UserInterface.Xml;
using SlimDX;

namespace AvengersUtd.StarOdyssey
{
    public static class GUIDesigner
    {
        public static Hud MainMenuTest
        {
            get
            {
                // These lines are needed to load the default theme.
                StyleManager.LoadControlStyles("Odyssey ControlStyles.ocs");
                StyleManager.LoadTextStyles("Odyssey TextStyles.ots");


                // Create a new hud object. It is important to
                // set the hud to be as big as the current resolution
                // that you are using. Unfortunately it is not yet
                // aware of resolution switches. But it will be included
                // in future releases.
                Hud hud = new Hud();
                hud.Id = "MainMenu";
                hud.Size = new Size(1280, 960);
                OdysseyUI.CurrentHud = hud;

                // This call is important as it signals the UI that
                // we are starting to design our user interface.
                hud.BeginDesign();

                //Panel panel = new Panel();
                //panel.Id = "UI Test Panel";
                //panel.PositionV3 = new Vector2(25, 150);
                //panel.Size = new Size(500,400);

                // Create two labels. The Vector2 parameter
                // refers to its positionV3 in the parent control.
                // Since the labels are going to be
                // added to the previous panel, the
                // absolute positionV3 of the first
                // one will then be: (X: 25 + 20, Y: 250 + 20).

                Label lTest = new Label();
                lTest.Id = "LabelTestPanel";
                lTest.Text = "The blue window is a Window Control and this is a Label";
                lTest.Position = new Vector2(10, 10);

                Label lTrackBar = new Label();
                lTrackBar.Id = "LabelTrackBar";
                lTrackBar.Text = "This TrackBar control goes from 0 to 10 with a 'TickFrequency' value of 2";
                lTrackBar.Position = new Vector2(10, 40);

                // This is a trackbar control. You have to set
                // the trackbar minim value, tick Frequency
                // and maximum values with the SetValues method.
                // (but you can also access the individual properties)
                // You can also attach a delegate method for
                // its "ValueChanged" event.
                TrackBar trackBar = new TrackBar();
                trackBar.Id = "TrackBar";
                trackBar.Position = new Vector2(20, 100);
                trackBar.SetValues(0, 2, 10);
                trackBar.ValueChanged +=
                delegate(object sender, EventArgs e) { lTrackBar.Text = "The trackbar value is now: " + trackBar.Value; };

                // This is a textbox. When you click on it, you can start typing.
                // It supports selection, mouse placement of the caret and
                // password fields.
                TextBox textBox = new TextBox();
                textBox.Id = "TextBox";
                textBox.Position = new Vector2(10, 150);
                textBox.Text = "This is a TextBox";

                // This is the DropDownList control also known as
                // a combobox.
                DropDownList dropDownList = new DropDownList();
                dropDownList.Position = new Vector2(240, 100);
                dropDownList.AddItems("This");
                dropDownList.AddItems("is the");
                dropDownList.AddItems("DropDownList");
                dropDownList.AddItems("Control");

                // This is a groupbox control: a simple panel
                // with a flat border and a caption.
                GroupBox groupBox = new GroupBox();
                groupBox.Id = "GroupBox";
                groupBox.Caption = "This is a GroupBox";
                groupBox.Position = new Vector2(10, 220);

                // This is an OptionGroup control: a collection of radio buttons.
                // You can also attach
                // a delegate too its "SelectedIndexChanged" event
                OptionGroup optionGroup = new OptionGroup();
                optionGroup.Id = "OptionGroup";
                optionGroup.Position = new Vector2(10, 10);
                optionGroup.OptionButtonSize = new Size(200, 30);
                optionGroup.AddItems("This is", "the OptionGroup", "control");

                // This is the CheckBox Control. You can access its
                // selected value through its .IsChecked property.
                CheckBox checkBox = new CheckBox();
                checkBox.Id = "CheckBox";
                checkBox.Caption = "CheckBox";
                checkBox.Position = new Vector2(450, 100);

                // This is a button. We have attached a delegate
                // to its "MouseClick" event: it will be fired
                // when the user clicks on the button.
                Button exampleButton = new Button();
                exampleButton.Id = "ExampleButton";
                exampleButton.Position = new Vector2(10, 360);
                exampleButton.Text = "This is a Button";
                exampleButton.MouseClick += delegate
                {
#if (!OdysseyUIDemo)
                    UIParser.SerializeHud(hud, "ui.xml");
#else
exampleButton.Text = "Yep you clicked me";
#endif
                };

                // This is another button: it demonstrates the use of
                // modal windows.
                Button dialogTestButton = new Button();
                dialogTestButton.Id = "DialogTestButton";
                dialogTestButton.Position = new Vector2(400, 360);
                dialogTestButton.Text = "Show me a Dialog";
                dialogTestButton.MouseClick +=
                delegate
                {
                    DialogBox.Show(
                    "Test",
                    "Do you like this User Interface?\n\nBy the way, this dialog is modal!",
                    DialogBoxButtons.YesNo,
                    delegate(object ctl, DialogResultEventArgs args)
                    {
                        if (args.DialogResult == DialogResult.No)
                            DialogBox.Show("Really?", "Sigh.... :(",
                            DialogBoxButtons.Ok, null);
                        else
                            DialogBox.Show("Thanks",
                            "I'm glad that you liked it!\n\nIf you have any feedback drop me a line at avengerdragon at gmail.com or visit my development blog at: [hover=\"Aquamarine\"]http://www.avengersutd.com/blog[/]",
                            DialogBoxButtons.Ok, null);
                    });
                };

                Label tabTestLabel1 = new Label();
                tabTestLabel1.Id = "TabTestLabel1";
                tabTestLabel1.Text = "Test TabPage #2";
                tabTestLabel1.Position = new Vector2(10, 10);


                // This is the RichTextArea control. It is a panel that
                // automatically formats input depending on BBCode like
                // string. We simply set the MarkupText property to
                // the string to be formatted and the style to use as default
                // (if the default one is Arial 20pt for example, the
                // bold command will apply the bold effect on the Arial 20pt font)
                // Accepted commands are b,i,s for bold, italic and shadowd
                // respectively and c or color for the standard color and
                // h or hover for the color to use when the mouse pointer
                // is over the label. Nested markup is not supported at
                // the moment.
                RichTextArea richTextArea = new RichTextArea();
                richTextArea.Id = "RichTextArea";
                richTextArea.Size = new Size(340, 140);
                richTextArea.MarkupText =
                "[s]RichTextArea Demonstration[/]\n\nThe above text was [b]shadowed[/]. And this [i,color=\"Orange\"]should[/] demonstrate that the word wrapping algorithm is working [b,color=\"FF00FF00\"]fine![/]";

                // This is the TabPanel control. A Panel that has some
                // buttons on the top that allow the user to access different
                // pages in it. Each page in this example has a label control.
                // You can switch page by clicking the top buttons.
                TabPanel tabPanel = new TabPanel();
                tabPanel.Id = "TabPanel";
                tabPanel.Position = new Vector2(240, 180);
                tabPanel.Size = new Size(350, 160);
                tabPanel.AddTab("Page #1");
                tabPanel.AddTab("Page #2");
                tabPanel.AddControlInTab(richTextArea, 0);
                tabPanel.AddControlInTab(tabTestLabel1, 1);

                Label dataViewerLabel = new Label();
                dataViewerLabel.Id = "DataViewerLabel";
                dataViewerLabel.Text =
                "This is the DataViewer object: it is bound to an object implementing the IList<T> interface and it displays the run-time content of user-selected properties. In this case, it is displaying the name of the style classes used for each control in the first window, as written in the xml theme.";

                // These are to windows that we will add to the Hud.
                Window win1 = new Window();
                win1.Id = "Window";
                win1.Title = "Test Window #1";
                win1.Position = new Vector2(100, 100);


                Window win2 = new Window();
                win2.Id = "Window2";
                win2.Title = "Test Window #2";
                win2.Position = new Vector2(500, 100);
                win2.Size = new Size(750, 550);

                //#if(!OdysseyUIDemo)
                // PictureBox pictureBox = new PictureBox();
                // pictureBox.PositionV3 = new Vector2(10, 10);
                // pictureBox.Texture = Microsoft.DirectX.Direct3D.TextureLoader.FromFile(OdysseyUI.Device,
                // @"Textures\Logos\tf.png");
                //#endif
                // Finally we add each control to its parent container.
                groupBox.Add(optionGroup);

                win1.Controls.Add(lTest);
                win1.Controls.Add(lTrackBar);
                win1.Controls.Add(textBox);
                win1.Controls.Add(trackBar);
                win1.Controls.Add(dropDownList);
                win1.Controls.Add(groupBox);
                win1.Controls.Add(checkBox);
                win1.Controls.Add(exampleButton);
                win1.Controls.Add(dialogTestButton);
                win1.Controls.Add(tabPanel);

                // This is the DataViewer control. As explained by
                // the above label, it displays the run-time value
                // of the bound properties.
                // I'm creating this one now because
                // we want it to bound it to the first window's
                // ControlCollection object. Now it is no more empty.
                DataViewer dataViewer = new DataViewer();
                dataViewer.Id = "DataViewer";

                dataViewer.Position = new Vector2(10, 80);
                dataViewer.CellSpacing = 4;
                dataViewer.ColumnCount = 3;

                // It does not yet support automatic row population.
                dataViewer.RowCount = win1.Controls.Count;
                dataViewer.DataSource = win1.Controls;
                dataViewer.Columns[0].Name = dataViewer.Columns[0].DataPropertyName = "Id";
                dataViewer.Columns[1].Name = dataViewer.Columns[1].DataPropertyName = "ControlStyleClass";
                dataViewer.Columns[2].Name = dataViewer.Columns[2].DataPropertyName = "TextStyleClass";
                dataViewer.Columns[0].Width = dataViewer.Columns[1].Width = dataViewer.Columns[2].Width = 200;

                // It does not yet support automatic content refreshing.
                dataViewer.RefreshValues();

                //win2.Controls.Add(pictureBox);
                win2.Controls.Add(dataViewerLabel);
                win2.Controls.Add(dataViewer);

                hud.Add(win1);
                //hud.Add(win2);

                // Signal the hud object that we have finished
                // creating the User Interface.

                hud.EndDesign();

                return hud;
            }
        }

        public static Hud SolarMenuScreen
        {
            get
            {
                Hud hud = new Hud();
                OdysseyUI.CurrentHud = hud;
                hud.Id = "MainMenu";
                hud.Size = new Size(1280, 960);
                hud.BeginDesign();

                DataViewer dataViewer = new DataViewer();
                dataViewer.Id = "Temp";
                dataViewer.Position = new Vector2(10, 300);
                dataViewer.ColumnCount = 3;
                dataViewer.Columns[0].Name = dataViewer.Columns[0].DataPropertyName = Stringtable.SS_Climate;
                dataViewer.Columns[1].Name = dataViewer.Columns[1].DataPropertyName = "Size";
                dataViewer.Columns[2].Name = dataViewer.Columns[2].DataPropertyName = "AtmosphericDensity";
                dataViewer.Columns[0].Width = dataViewer.Columns[1].Width = dataViewer.Columns[2].Width = 200;

                hud.Add(dataViewer);
                //Table table = new Table();
                //TableRow row = new TableRow();
                //TableCell cell = new TableCell();
                //cell.Text = "Prova";
                //TableCell cell2 = new TableCell();
                //cell2.Text = "Prova";
                //row.Cells.Add(cell);
                //row.Cells.Add(cell2);
                //row.Width = 400;
                //row.Height = 34;
                
                //table.Rows.Add(row);
                
                //table.PositionV3 = new Vector2(25,25);

                //hud.Add(table);

                //hud.EndDesign();

                return hud;

            }
        }
    }
}
