namespace Fluke900Link.Controls
{
    partial class TerminalSendControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalSendControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSend = new System.Windows.Forms.ToolStripButton();
            this.scintillaEditor = new ScintillaNET.Scintilla();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRedo,
            this.toolStripButtonSend});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(452, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRedo
            // 
            this.toolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRedo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRedo.Image")));
            this.toolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRedo.Name = "toolStripButtonRedo";
            this.toolStripButtonRedo.Size = new System.Drawing.Size(32, 22);
            this.toolStripButtonRedo.Text = "ETX";
            this.toolStripButtonRedo.ToolTipText = "End Transaction";
            this.toolStripButtonRedo.Click += new System.EventHandler(this.toolStripButtonRedo_Click);
            // 
            // toolStripButtonSend
            // 
            this.toolStripButtonSend.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripButtonSend.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSend.Image")));
            this.toolStripButtonSend.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSend.Name = "toolStripButtonSend";
            this.toolStripButtonSend.Size = new System.Drawing.Size(39, 22);
            this.toolStripButtonSend.Text = "Send";
            this.toolStripButtonSend.Click += new System.EventHandler(this.toolStripButtonSend_Click);
            // 
            // scintillaEditor
            // 
            this.scintillaEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaEditor.Location = new System.Drawing.Point(0, 25);
            this.scintillaEditor.Name = "scintillaEditor";
            this.scintillaEditor.Size = new System.Drawing.Size(452, 200);
            this.scintillaEditor.TabIndex = 1;
            // 
            // TerminalSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scintillaEditor);
            this.Controls.Add(this.toolStrip1);
            this.Name = "TerminalSend";
            this.Size = new System.Drawing.Size(452, 225);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRedo;
        private System.Windows.Forms.ToolStripButton toolStripButtonSend;
        private ScintillaNET.Scintilla scintillaEditor;
    }
}
