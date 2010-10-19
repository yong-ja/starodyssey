﻿namespace AvengersUtd.Odysseus.UIControls
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.gradientContainer = new AvengersUtd.Odysseus.UIControls.GradientContainer();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.cmdDel = new System.Windows.Forms.Button();
            this.ctlOffset = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbHexColor = new System.Windows.Forms.TextBox();
            this.cmdColorWheel = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.gradientContainer, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.groupBox, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(387, 132);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // gradientContainer
            // 
            this.gradientContainer.AutoSize = true;
            this.gradientContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientContainer.Location = new System.Drawing.Point(0, 0);
            this.gradientContainer.Margin = new System.Windows.Forms.Padding(0);
            this.gradientContainer.MinimumSize = new System.Drawing.Size(32, 16);
            this.gradientContainer.Name = "gradientContainer";
            this.gradientContainer.Padding = new System.Windows.Forms.Padding(3, 0, 3, 6);
            this.gradientContainer.SelectedMarker = null;
            this.gradientContainer.Size = new System.Drawing.Size(387, 69);
            this.gradientContainer.TabIndex = 5;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.cmdDel);
            this.groupBox.Controls.Add(this.ctlOffset);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.tbHexColor);
            this.groupBox.Controls.Add(this.cmdColorWheel);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(2, 74);
            this.groupBox.Margin = new System.Windows.Forms.Padding(2, 5, 2, 2);
            this.groupBox.Name = "groupBox";
            this.groupBox.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox.Size = new System.Drawing.Size(383, 56);
            this.groupBox.TabIndex = 3;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Edit Markers";
            // 
            // cmdDel
            // 
            this.cmdDel.Image = global::AvengersUtd.Odysseus.Properties.Resources.Delete;
            this.cmdDel.Location = new System.Drawing.Point(339, 12);
            this.cmdDel.Margin = new System.Windows.Forms.Padding(2);
            this.cmdDel.Name = "cmdDel";
            this.cmdDel.Size = new System.Drawing.Size(40, 40);
            this.cmdDel.TabIndex = 6;
            this.cmdDel.UseVisualStyleBackColor = true;
            this.cmdDel.Click += new System.EventHandler(this.cmdDel_Click);
            // 
            // ctlOffset
            // 
            this.ctlOffset.DecimalPlaces = 2;
            this.ctlOffset.Enabled = false;
            this.ctlOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctlOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.ctlOffset.Location = new System.Drawing.Point(266, 20);
            this.ctlOffset.Margin = new System.Windows.Forms.Padding(2);
            this.ctlOffset.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ctlOffset.Name = "ctlOffset";
            this.ctlOffset.Size = new System.Drawing.Size(63, 23);
            this.ctlOffset.TabIndex = 5;
            this.ctlOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ctlOffset.ValueChanged += new System.EventHandler(this.ctlOffset_ValueChanged);
            this.ctlOffset.Validating += new System.ComponentModel.CancelEventHandler(this.ctlOffset_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(220, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Offset";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Color Hex";
            // 
            // tbHexColor
            // 
            this.tbHexColor.BackColor = System.Drawing.SystemColors.Window;
            this.tbHexColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHexColor.Location = new System.Drawing.Point(117, 20);
            this.tbHexColor.Margin = new System.Windows.Forms.Padding(2);
            this.tbHexColor.Name = "tbHexColor";
            this.tbHexColor.Size = new System.Drawing.Size(87, 23);
            this.tbHexColor.TabIndex = 2;
            this.tbHexColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbHexColor.Enter += new System.EventHandler(this.tbHexColor_Enter);
            this.tbHexColor.Validating += new System.ComponentModel.CancelEventHandler(this.tbHexColor_Validating);
            // 
            // cmdColorWheel
            // 
            this.cmdColorWheel.Image = global::AvengersUtd.Odysseus.Properties.Resources.ColorWheel;
            this.cmdColorWheel.Location = new System.Drawing.Point(3, 12);
            this.cmdColorWheel.Margin = new System.Windows.Forms.Padding(2);
            this.cmdColorWheel.Name = "cmdColorWheel";
            this.cmdColorWheel.Size = new System.Drawing.Size(40, 40);
            this.cmdColorWheel.TabIndex = 1;
            this.cmdColorWheel.UseVisualStyleBackColor = true;
            this.cmdColorWheel.Click += new System.EventHandler(this.BtColorWheelClick);
            // 
            // GradientBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GradientBuilder";
            this.Size = new System.Drawing.Size(387, 132);
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
        private System.Windows.Forms.Button cmdColorWheel;
        private GradientContainer gradientContainer;
        private System.Windows.Forms.Button cmdDel;
        private System.Windows.Forms.ToolTip toolTip;

    }
}
