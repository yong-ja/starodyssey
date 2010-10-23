namespace AvengersUtd.Odysseus.UIControls
{
    partial class GradientEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gradientBuilder = new AvengersUtd.Odysseus.UIControls.GradientBuilder();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmdOpen = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cbControls = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSize = new System.Windows.Forms.TextBox();
            this.listButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btAdd = new System.Windows.Forms.Button();
            this.btRename = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sendToBorderLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.border1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.border2MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.applyToFillLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fill1MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.listButtons.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 198F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gradientBuilder, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.listButtons, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonPanel, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 328F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(676, 513);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(154, 4);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(512, 512);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 320);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // gradientBuilder
            // 
            this.gradientBuilder.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.gradientBuilder, 2);
            this.gradientBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientBuilder.Location = new System.Drawing.Point(152, 330);
            this.gradientBuilder.Margin = new System.Windows.Forms.Padding(2);
            this.gradientBuilder.Name = "gradientBuilder";
            this.gradientBuilder.Size = new System.Drawing.Size(522, 129);
            this.gradientBuilder.TabIndex = 16;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel1.Controls.Add(this.listView1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.tableLayoutPanel1.SetRowSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(144, 455);
            this.flowLayoutPanel1.TabIndex = 17;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.cmdOpen);
            this.flowLayoutPanel3.Controls.Add(this.cmdSave);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(146, 46);
            this.flowLayoutPanel3.TabIndex = 11;
            // 
            // cmdOpen
            // 
            this.cmdOpen.Image = global::AvengersUtd.Odysseus.Properties.Resources.open;
            this.cmdOpen.Location = new System.Drawing.Point(2, 2);
            this.cmdOpen.Margin = new System.Windows.Forms.Padding(2);
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(40, 40);
            this.cmdOpen.TabIndex = 8;
            this.cmdOpen.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Image = global::AvengersUtd.Odysseus.Properties.Resources.save;
            this.cmdSave.Location = new System.Drawing.Point(46, 2);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(2);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(40, 40);
            this.cmdSave.TabIndex = 9;
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.Location = new System.Drawing.Point(3, 53);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(140, 402);
            this.listView1.TabIndex = 12;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseClick);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.cbControls);
            this.flowLayoutPanel2.Controls.Add(this.label3);
            this.flowLayoutPanel2.Controls.Add(this.tbSize);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(480, 2);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(4, 5, 0, 0);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(194, 324);
            this.flowLayoutPanel2.TabIndex = 18;
            this.flowLayoutPanel2.WrapContents = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Preview Control:";
            // 
            // cbControls
            // 
            this.cbControls.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbControls.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbControls.FormattingEnabled = true;
            this.cbControls.Location = new System.Drawing.Point(6, 22);
            this.cbControls.Margin = new System.Windows.Forms.Padding(2);
            this.cbControls.Name = "cbControls";
            this.cbControls.Size = new System.Drawing.Size(91, 21);
            this.cbControls.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 5, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Control size:";
            // 
            // tbSize
            // 
            this.tbSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSize.Location = new System.Drawing.Point(6, 65);
            this.tbSize.Margin = new System.Windows.Forms.Padding(2);
            this.tbSize.Name = "tbSize";
            this.tbSize.Size = new System.Drawing.Size(91, 23);
            this.tbSize.TabIndex = 3;
            // 
            // listButtons
            // 
            this.listButtons.Controls.Add(this.btAdd);
            this.listButtons.Controls.Add(this.btRename);
            this.listButtons.Controls.Add(this.btDelete);
            this.listButtons.Location = new System.Drawing.Point(2, 463);
            this.listButtons.Margin = new System.Windows.Forms.Padding(2);
            this.listButtons.Name = "listButtons";
            this.listButtons.Size = new System.Drawing.Size(146, 46);
            this.listButtons.TabIndex = 10;
            // 
            // btAdd
            // 
            this.btAdd.Image = global::AvengersUtd.Odysseus.Properties.Resources.Add;
            this.btAdd.Location = new System.Drawing.Point(2, 2);
            this.btAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(40, 40);
            this.btAdd.TabIndex = 8;
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.ButtonAddClick);
            // 
            // btRename
            // 
            this.btRename.Image = global::AvengersUtd.Odysseus.Properties.Resources.Rename;
            this.btRename.Location = new System.Drawing.Point(46, 2);
            this.btRename.Margin = new System.Windows.Forms.Padding(2);
            this.btRename.Name = "btRename";
            this.btRename.Size = new System.Drawing.Size(40, 40);
            this.btRename.TabIndex = 9;
            this.btRename.UseVisualStyleBackColor = true;
            // 
            // btDelete
            // 
            this.btDelete.Image = global::AvengersUtd.Odysseus.Properties.Resources.Delete;
            this.btDelete.Location = new System.Drawing.Point(90, 2);
            this.btDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(40, 40);
            this.btDelete.TabIndex = 10;
            this.btDelete.UseVisualStyleBackColor = true;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.btCancel);
            this.buttonPanel.Controls.Add(this.btOk);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonPanel.Location = new System.Drawing.Point(480, 463);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(194, 48);
            this.buttonPanel.TabIndex = 15;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(120, 16);
            this.btCancel.Margin = new System.Windows.Forms.Padding(2, 16, 2, 2);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(72, 26);
            this.btCancel.TabIndex = 12;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(44, 16);
            this.btOk.Margin = new System.Windows.Forms.Padding(2, 16, 2, 2);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(72, 26);
            this.btOk.TabIndex = 13;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator1,
            this.sendToBorderLayerToolStripMenuItem,
            this.applyToFillLayerToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(189, 120);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.renameToolStripMenuItem.Text = "&Rename";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // sendToBorderLayerToolStripMenuItem
            // 
            this.sendToBorderLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.border1MenuItem,
            this.border2MenuItem});
            this.sendToBorderLayerToolStripMenuItem.Name = "sendToBorderLayerToolStripMenuItem";
            this.sendToBorderLayerToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.sendToBorderLayerToolStripMenuItem.Text = "Apply to &Border Layer";
            // 
            // border1MenuItem
            // 
            this.border1MenuItem.CheckOnClick = true;
            this.border1MenuItem.Name = "border1MenuItem";
            this.border1MenuItem.Size = new System.Drawing.Size(156, 22);
            this.border1MenuItem.Text = "Border Layer #1";
            // 
            // border2MenuItem
            // 
            this.border2MenuItem.Name = "border2MenuItem";
            this.border2MenuItem.Size = new System.Drawing.Size(156, 22);
            this.border2MenuItem.Text = "Border Layer #2";
            // 
            // applyToFillLayerToolStripMenuItem
            // 
            this.applyToFillLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fill1MenuItem});
            this.applyToFillLayerToolStripMenuItem.Name = "applyToFillLayerToolStripMenuItem";
            this.applyToFillLayerToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.applyToFillLayerToolStripMenuItem.Text = "Apply to &Fill Layer";
            // 
            // fill1MenuItem
            // 
            this.fill1MenuItem.CheckOnClick = true;
            this.fill1MenuItem.Name = "fill1MenuItem";
            this.fill1MenuItem.Size = new System.Drawing.Size(152, 22);
            this.fill1MenuItem.Text = "Fill Layer #1";
            this.fill1MenuItem.Click += new System.EventHandler(this.fill1MenuItem_Click);
            // 
            // GradientEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 513);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GradientEditor";
            this.Text = "GradientEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GradientEditorFormClosed);
            this.Load += new System.EventHandler(this.GradientEditorLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.listButtons.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel listButtons;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRename;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private UIControls.GradientBuilder gradientBuilder;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbControls;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSize;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button cmdOpen;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem sendToBorderLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem border1MenuItem;
        private System.Windows.Forms.ToolStripMenuItem border2MenuItem;
        private System.Windows.Forms.ToolStripMenuItem applyToFillLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fill1MenuItem;
        private System.Windows.Forms.ListView listView1;
    }
}