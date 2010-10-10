namespace AvengersUtd.Odysseus
{
    partial class GradientEditor2
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btAdd = new System.Windows.Forms.Button();
            this.btRename = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.gradientBuilder1 = new AvengersUtd.Odysseus.UIControls.GradientBuilder();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.listButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 520F));
            this.tableLayoutPanel1.Controls.Add(this.treeView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listButtons, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gradientBuilder1, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 518F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(723, 731);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(4, 4);
            this.treeView.Margin = new System.Windows.Forms.Padding(4);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(192, 510);
            this.treeView.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btCancel);
            this.flowLayoutPanel1.Controls.Add(this.btOk);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(206, 682);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(514, 46);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(415, 8);
            this.btCancel.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(96, 32);
            this.btCancel.TabIndex = 12;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Location = new System.Drawing.Point(313, 8);
            this.btOk.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(96, 32);
            this.btOk.TabIndex = 13;
            this.btOk.Text = "Ok";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox1, 2);
            this.pictureBox1.Location = new System.Drawing.Point(203, 3);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(512, 512);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(512, 512);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // listButtons
            // 
            this.listButtons.Controls.Add(this.btAdd);
            this.listButtons.Controls.Add(this.btRename);
            this.listButtons.Controls.Add(this.btDelete);
            this.listButtons.Location = new System.Drawing.Point(3, 521);
            this.listButtons.Name = "listButtons";
            this.listButtons.Size = new System.Drawing.Size(194, 44);
            this.listButtons.TabIndex = 10;
            // 
            // btAdd
            // 
            this.btAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAdd.Image = global::AvengersUtd.Odysseus.Properties.Resources.Add;
            this.btAdd.Location = new System.Drawing.Point(3, 3);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(40, 40);
            this.btAdd.TabIndex = 8;
            this.btAdd.UseVisualStyleBackColor = true;
            // 
            // btRename
            // 
            this.btRename.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btRename.Image = global::AvengersUtd.Odysseus.Properties.Resources.Rename;
            this.btRename.Location = new System.Drawing.Point(49, 3);
            this.btRename.Name = "btRename";
            this.btRename.Size = new System.Drawing.Size(40, 40);
            this.btRename.TabIndex = 9;
            this.btRename.UseVisualStyleBackColor = true;
            // 
            // btDelete
            // 
            this.btDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btDelete.Image = global::AvengersUtd.Odysseus.Properties.Resources.Delete;
            this.btDelete.Location = new System.Drawing.Point(95, 3);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(40, 40);
            this.btDelete.TabIndex = 10;
            this.btDelete.UseVisualStyleBackColor = true;
            // 
            // gradientBuilder1
            // 
            this.gradientBuilder1.Location = new System.Drawing.Point(206, 521);
            this.gradientBuilder1.Name = "gradientBuilder1";
            this.gradientBuilder1.Size = new System.Drawing.Size(514, 155);
            this.gradientBuilder1.TabIndex = 16;
            // 
            // GradientEditor2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 731);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GradientEditor2";
            this.Text = "GradientEditor2";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.listButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.FlowLayoutPanel listButtons;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btRename;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOk;
        private UIControls.GradientBuilder gradientBuilder1;
    }
}