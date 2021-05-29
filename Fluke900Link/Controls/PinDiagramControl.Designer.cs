namespace Fluke900Link.Controls
{
    partial class PinDiagramControl
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.labelFloatCheck1 = new System.Windows.Forms.Label();
            this.labelFloatCheck2 = new System.Windows.Forms.Label();
            this.labelActivity1 = new System.Windows.Forms.Label();
            this.labelActivity2 = new System.Windows.Forms.Label();
            this.labelTriggerWord1a = new System.Windows.Forms.Label();
            this.labelTriggerWord2a = new System.Windows.Forms.Label();
            this.labelTriggerWord2b = new System.Windows.Forms.Label();
            this.labelTriggerWord1b = new System.Windows.Forms.Label();
            this.labelGate1 = new System.Windows.Forms.Label();
            this.labelGate2 = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.labelGate2);
            this.panelMain.Controls.Add(this.labelGate1);
            this.panelMain.Controls.Add(this.labelTriggerWord2b);
            this.panelMain.Controls.Add(this.labelTriggerWord1b);
            this.panelMain.Controls.Add(this.labelTriggerWord2a);
            this.panelMain.Controls.Add(this.labelTriggerWord1a);
            this.panelMain.Controls.Add(this.labelActivity2);
            this.panelMain.Controls.Add(this.labelActivity1);
            this.panelMain.Controls.Add(this.labelFloatCheck2);
            this.panelMain.Controls.Add(this.labelFloatCheck1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(948, 360);
            this.panelMain.TabIndex = 0;
            this.panelMain.Click += new System.EventHandler(this.panelMain_Click);
            this.panelMain.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            this.panelMain.Resize += new System.EventHandler(this.panelMain_Resize);
            // 
            // labelFloatCheck1
            // 
            this.labelFloatCheck1.AutoSize = true;
            this.labelFloatCheck1.Location = new System.Drawing.Point(313, 76);
            this.labelFloatCheck1.Name = "labelFloatCheck1";
            this.labelFloatCheck1.Size = new System.Drawing.Size(30, 13);
            this.labelFloatCheck1.TabIndex = 0;
            this.labelFloatCheck1.Text = "Float";
            // 
            // labelFloatCheck2
            // 
            this.labelFloatCheck2.AutoSize = true;
            this.labelFloatCheck2.Location = new System.Drawing.Point(470, 76);
            this.labelFloatCheck2.Name = "labelFloatCheck2";
            this.labelFloatCheck2.Size = new System.Drawing.Size(30, 13);
            this.labelFloatCheck2.TabIndex = 1;
            this.labelFloatCheck2.Text = "Float";
            // 
            // labelActivity1
            // 
            this.labelActivity1.AutoSize = true;
            this.labelActivity1.Location = new System.Drawing.Point(266, 76);
            this.labelActivity1.Name = "labelActivity1";
            this.labelActivity1.Size = new System.Drawing.Size(41, 13);
            this.labelActivity1.TabIndex = 2;
            this.labelActivity1.Text = "Activity";
            // 
            // labelActivity2
            // 
            this.labelActivity2.AutoSize = true;
            this.labelActivity2.Location = new System.Drawing.Point(506, 76);
            this.labelActivity2.Name = "labelActivity2";
            this.labelActivity2.Size = new System.Drawing.Size(41, 13);
            this.labelActivity2.TabIndex = 3;
            this.labelActivity2.Text = "Activity";
            // 
            // labelTriggerWord1a
            // 
            this.labelTriggerWord1a.AutoSize = true;
            this.labelTriggerWord1a.Location = new System.Drawing.Point(149, 76);
            this.labelTriggerWord1a.Name = "labelTriggerWord1a";
            this.labelTriggerWord1a.Size = new System.Drawing.Size(42, 13);
            this.labelTriggerWord1a.TabIndex = 4;
            this.labelTriggerWord1a.Text = "Word 1";
            // 
            // labelTriggerWord2a
            // 
            this.labelTriggerWord2a.AutoSize = true;
            this.labelTriggerWord2a.Location = new System.Drawing.Point(197, 76);
            this.labelTriggerWord2a.Name = "labelTriggerWord2a";
            this.labelTriggerWord2a.Size = new System.Drawing.Size(42, 13);
            this.labelTriggerWord2a.TabIndex = 5;
            this.labelTriggerWord2a.Text = "Word 2";
            // 
            // labelTriggerWord2b
            // 
            this.labelTriggerWord2b.AutoSize = true;
            this.labelTriggerWord2b.Location = new System.Drawing.Point(611, 76);
            this.labelTriggerWord2b.Name = "labelTriggerWord2b";
            this.labelTriggerWord2b.Size = new System.Drawing.Size(42, 13);
            this.labelTriggerWord2b.TabIndex = 7;
            this.labelTriggerWord2b.Text = "Word 2";
            // 
            // labelTriggerWord1b
            // 
            this.labelTriggerWord1b.AutoSize = true;
            this.labelTriggerWord1b.Location = new System.Drawing.Point(563, 76);
            this.labelTriggerWord1b.Name = "labelTriggerWord1b";
            this.labelTriggerWord1b.Size = new System.Drawing.Size(42, 13);
            this.labelTriggerWord1b.TabIndex = 6;
            this.labelTriggerWord1b.Text = "Word 1";
            // 
            // labelGate1
            // 
            this.labelGate1.AutoSize = true;
            this.labelGate1.Location = new System.Drawing.Point(87, 76);
            this.labelGate1.Name = "labelGate1";
            this.labelGate1.Size = new System.Drawing.Size(30, 13);
            this.labelGate1.TabIndex = 8;
            this.labelGate1.Text = "Gate";
            // 
            // labelGate2
            // 
            this.labelGate2.AutoSize = true;
            this.labelGate2.Location = new System.Drawing.Point(673, 76);
            this.labelGate2.Name = "labelGate2";
            this.labelGate2.Size = new System.Drawing.Size(30, 13);
            this.labelGate2.TabIndex = 9;
            this.labelGate2.Text = "Gate";
            // 
            // PinDiagramControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "PinDiagramControl";
            this.Size = new System.Drawing.Size(948, 360);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelFloatCheck2;
        private System.Windows.Forms.Label labelFloatCheck1;
        private System.Windows.Forms.Label labelActivity2;
        private System.Windows.Forms.Label labelActivity1;
        private System.Windows.Forms.Label labelTriggerWord2b;
        private System.Windows.Forms.Label labelTriggerWord1b;
        private System.Windows.Forms.Label labelTriggerWord2a;
        private System.Windows.Forms.Label labelTriggerWord1a;
        private System.Windows.Forms.Label labelGate2;
        private System.Windows.Forms.Label labelGate1;
    }
}
