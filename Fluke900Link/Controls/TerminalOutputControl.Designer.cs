namespace Fluke900Link.Controls
{
    partial class TerminalOutputControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalOutputControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonClearWindow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.scintillaMain = new ScintillaNET.Scintilla();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonClearWindow,
            this.toolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(539, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonClearWindow
            // 
            this.toolStripButtonClearWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonClearWindow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClearWindow.Image")));
            this.toolStripButtonClearWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClearWindow.Name = "toolStripButtonClearWindow";
            this.toolStripButtonClearWindow.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonClearWindow.Text = "ClearWindow";
            this.toolStripButtonClearWindow.Click += new System.EventHandler(this.toolStripButtonClearWindow_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonSave.Text = "Save Text";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // scintillaMain
            // 
            this.scintillaMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaMain.Lexer = ScintillaNET.Lexer.Null;
            this.scintillaMain.Location = new System.Drawing.Point(0, 25);
            this.scintillaMain.Name = "scintillaMain";
            this.scintillaMain.ReadOnly = true;
            this.scintillaMain.ScrollWidth = 350;
            this.scintillaMain.Size = new System.Drawing.Size(539, 462);
            this.scintillaMain.TabIndex = 1;
            // 
            // TerminalOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 487);
            this.Controls.Add(this.scintillaMain);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TerminalOutputControl";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonClearWindow;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.BindingSource bindingSource1;
        private ScintillaNET.Scintilla scintillaMain;
    }
}
