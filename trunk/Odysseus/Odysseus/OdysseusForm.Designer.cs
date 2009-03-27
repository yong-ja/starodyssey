namespace AvengersUtd.Odysseus
{
    partial class OdysseusForm
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
            this.menu = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.renderPanel = new AvengersUtd.Odyssey.RenderPanel();
            this.menu.SuspendLayout();
            this.toolStripPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(948, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // fileMenu
            // 
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(37, 20);
            this.fileMenu.Text = "&File";
            // 
            // toolStripPanel
            // 
            this.toolStripPanel.Controls.Add(this.toolStrip);
            this.toolStripPanel.Location = new System.Drawing.Point(0, 24);
            this.toolStripPanel.Name = "toolStripPanel";
            this.toolStripPanel.Size = new System.Drawing.Size(142, 600);
            this.toolStripPanel.TabIndex = 3;
            // 
            // toolStrip
            // 
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(26, 111);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // renderPanel
            // 
            this.renderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.renderPanel.Location = new System.Drawing.Point(148, 24);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(800, 600);
            this.renderPanel.TabIndex = 2;
            this.renderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(renderPanel_MouseMove);
            // 
            // OdysseusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 624);
            this.Controls.Add(this.renderPanel);
            this.Controls.Add(this.toolStripPanel);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "OdysseusForm";
            this.Text = "Odysseus";
            this.Load += new System.EventHandler(this.OdysseusForm_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.toolStripPanel.ResumeLayout(false);
            this.toolStripPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.FlowLayoutPanel toolStripPanel;
        private AvengersUtd.Odyssey.RenderPanel renderPanel;
        private System.Windows.Forms.ToolStrip toolStrip;
    }
}

