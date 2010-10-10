namespace AvengersUtd.Odysseus.UIControls
{
    partial class GradientBuilder
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gradientContainer1 = new AvengersUtd.Odysseus.UIControls.GradientContainer();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.ctlOffset = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbHexColor = new System.Windows.Forms.TextBox();
            this.btColorWheel = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.gradientContainer1, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.groupBox, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 92F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(516, 162);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // gradientContainer1
            // 
            this.gradientContainer1.AutoSize = true;
            this.gradientContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientContainer1.Location = new System.Drawing.Point(0, 0);
            this.gradientContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.gradientContainer1.MinimumSize = new System.Drawing.Size(64, 32);
            this.gradientContainer1.Name = "gradientContainer1";
            this.gradientContainer1.Padding = new System.Windows.Forms.Padding(4, 0, 4, 8);
            this.gradientContainer1.Size = new System.Drawing.Size(516, 92);
            this.gradientContainer1.TabIndex = 5;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.ctlOffset);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.tbHexColor);
            this.groupBox.Controls.Add(this.btColorWheel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(3, 98);
            this.groupBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(510, 61);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Edit Markers";
            // 
            // ctlOffset
            // 
            this.ctlOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctlOffset.Location = new System.Drawing.Point(377, 22);
            this.ctlOffset.Name = "ctlOffset";
            this.ctlOffset.Size = new System.Drawing.Size(128, 27);
            this.ctlOffset.TabIndex = 5;
            this.ctlOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(316, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Offset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(52, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Color Hex";
            // 
            // tbHexColor
            // 
            this.tbHexColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHexColor.Location = new System.Drawing.Point(142, 22);
            this.tbHexColor.Name = "tbHexColor";
            this.tbHexColor.Size = new System.Drawing.Size(128, 27);
            this.tbHexColor.TabIndex = 2;
            // 
            // btColorWheel
            // 
            this.btColorWheel.Image = global::AvengersUtd.Odysseus.Properties.Resources.Color;
            this.btColorWheel.Location = new System.Drawing.Point(6, 17);
            this.btColorWheel.Name = "btColorWheel";
            this.btColorWheel.Size = new System.Drawing.Size(40, 40);
            this.btColorWheel.TabIndex = 1;
            this.btColorWheel.UseVisualStyleBackColor = true;
            // 
            // GradientBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "GradientBuilder";
            this.Size = new System.Drawing.Size(516, 162);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.NumericUpDown ctlOffset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbHexColor;
        private System.Windows.Forms.Button btColorWheel;
        private GradientContainer gradientContainer1;

    }
}
