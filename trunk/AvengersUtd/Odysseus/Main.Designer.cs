namespace AvengersUtd.Odysseus
{
    partial class Main
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
            this.renderPanel = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsCFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.snapToGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.styleEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportOUIDialog = new System.Windows.Forms.SaveFileDialog();
            this.exportCSDialog = new System.Windows.Forms.SaveFileDialog();
            this.gradientEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.renderPanel)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.renderPanel.Location = new System.Drawing.Point(0, 33);
            this.renderPanel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(542, 412);
            this.renderPanel.TabIndex = 0;
            this.renderPanel.TabStop = false;
            this.renderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RenderPanelMouseMove);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.windowMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(641, 28);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menu";
            // 
            // fileMenu
            // 
            this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem,
            this.exportAsCFileToolStripMenuItem});
            this.fileMenu.Name = "fileMenu";
            this.fileMenu.Size = new System.Drawing.Size(44, 24);
            this.fileMenu.Text = "&File";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.exportToolStripMenuItem.Text = "&Export as OUI";
            // 
            // exportAsCFileToolStripMenuItem
            // 
            this.exportAsCFileToolStripMenuItem.Name = "exportAsCFileToolStripMenuItem";
            this.exportAsCFileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.C)));
            this.exportAsCFileToolStripMenuItem.Size = new System.Drawing.Size(233, 24);
            this.exportAsCFileToolStripMenuItem.Text = "Export as &C# file";
            this.exportAsCFileToolStripMenuItem.Click += new System.EventHandler(this.exportAsCFileToolStripMenuItem_Click);
            // 
            // editMenu
            // 
            this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snapToGridToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.editMenu.Name = "editMenu";
            this.editMenu.Size = new System.Drawing.Size(47, 24);
            this.editMenu.Text = "&Edit";
            // 
            // snapToGridToolStripMenuItem
            // 
            this.snapToGridToolStripMenuItem.CheckOnClick = true;
            this.snapToGridToolStripMenuItem.Name = "snapToGridToolStripMenuItem";
            this.snapToGridToolStripMenuItem.Size = new System.Drawing.Size(161, 24);
            this.snapToGridToolStripMenuItem.Text = "&Snap to Grid";
            this.snapToGridToolStripMenuItem.Click += new System.EventHandler(this.snapToGridToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(161, 24);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // windowMenu
            // 
            this.windowMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.toolboxToolStripMenuItem,
            this.styleEditorToolStripMenuItem,
            this.gradientEditorToolStripMenuItem});
            this.windowMenu.Name = "windowMenu";
            this.windowMenu.Size = new System.Drawing.Size(76, 24);
            this.windowMenu.Text = "&Window";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolboxToolStripMenuItem
            // 
            this.toolboxToolStripMenuItem.Name = "toolboxToolStripMenuItem";
            this.toolboxToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.toolboxToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.toolboxToolStripMenuItem.Text = "&Toolbox";
            this.toolboxToolStripMenuItem.Click += new System.EventHandler(this.toolboxToolStripMenuItem_Click);
            // 
            // styleEditorToolStripMenuItem
            // 
            this.styleEditorToolStripMenuItem.Name = "styleEditorToolStripMenuItem";
            this.styleEditorToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.styleEditorToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.styleEditorToolStripMenuItem.Text = "&Style editor";
            this.styleEditorToolStripMenuItem.Click += new System.EventHandler(this.styleEditorToolStripMenuItem_Click);
            // 
            // exportOUIDialog
            // 
            this.exportOUIDialog.DefaultExt = "oui";
            this.exportOUIDialog.FileName = "Interface1.oui";
            this.exportOUIDialog.Filter = "Odyssey UI|*.oui";
            // 
            // exportCSDialog
            // 
            this.exportCSDialog.DefaultExt = "cs";
            this.exportCSDialog.FileName = "Interface1.cs";
            this.exportCSDialog.Filter = "C# Source Code|*.cs";
            // 
            // gradientEditorToolStripMenuItem
            // 
            this.gradientEditorToolStripMenuItem.Name = "gradientEditorToolStripMenuItem";
            this.gradientEditorToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.gradientEditorToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.gradientEditorToolStripMenuItem.Text = "&Gradient editor";
            this.gradientEditorToolStripMenuItem.Click += new System.EventHandler(this.gradientEditorToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 475);
            this.Controls.Add(this.renderPanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Main";
            this.Text = "Odysseus";
            ((System.ComponentModel.ISupportInitialize)(this.renderPanel)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox renderPanel;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenu;
        private System.Windows.Forms.ToolStripMenuItem editMenu;
        private System.Windows.Forms.ToolStripMenuItem snapToGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowMenu;
        private System.Windows.Forms.ToolStripMenuItem toolboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem styleEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAsCFileToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog exportOUIDialog;
        private System.Windows.Forms.SaveFileDialog exportCSDialog;
        private System.Windows.Forms.ToolStripMenuItem gradientEditorToolStripMenuItem;
    }
}

