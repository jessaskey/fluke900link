namespace Fluke900Link.Controls
{
    partial class ProjectFilePreferencesControl
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonFileCopyCartridgeThenSystem = new System.Windows.Forms.RadioButton();
            this.radioButtonFileCopySplit = new System.Windows.Forms.RadioButton();
            this.radioButtonFileCopySystem = new System.Windows.Forms.RadioButton();
            this.radioButtonFileCopyCartridge = new System.Windows.Forms.RadioButton();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonFileCopyCartridgeThenSystem);
            this.groupBox3.Controls.Add(this.radioButtonFileCopySplit);
            this.groupBox3.Controls.Add(this.radioButtonFileCopySystem);
            this.groupBox3.Controls.Add(this.radioButtonFileCopyCartridge);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(648, 84);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "File Copy Behavior";
            // 
            // radioButtonFileCopyCartridgeThenSystem
            // 
            this.radioButtonFileCopyCartridgeThenSystem.AutoSize = true;
            this.radioButtonFileCopyCartridgeThenSystem.Location = new System.Drawing.Point(278, 36);
            this.radioButtonFileCopyCartridgeThenSystem.Name = "radioButtonFileCopyCartridgeThenSystem";
            this.radioButtonFileCopyCartridgeThenSystem.Size = new System.Drawing.Size(188, 17);
            this.radioButtonFileCopyCartridgeThenSystem.TabIndex = 3;
            this.radioButtonFileCopyCartridgeThenSystem.TabStop = true;
            this.radioButtonFileCopyCartridgeThenSystem.Text = "SEQ/LOC on SYST, LIB on CART";
            this.radioButtonFileCopyCartridgeThenSystem.UseVisualStyleBackColor = true;
            this.radioButtonFileCopyCartridgeThenSystem.CheckedChanged += new System.EventHandler(this.radioButton_CheckChanged);
            // 
            // radioButtonFileCopySplit
            // 
            this.radioButtonFileCopySplit.AutoSize = true;
            this.radioButtonFileCopySplit.Location = new System.Drawing.Point(490, 36);
            this.radioButtonFileCopySplit.Name = "radioButtonFileCopySplit";
            this.radioButtonFileCopySplit.Size = new System.Drawing.Size(87, 17);
            this.radioButtonFileCopySplit.TabIndex = 2;
            this.radioButtonFileCopySplit.TabStop = true;
            this.radioButtonFileCopySplit.Text = "SMARTCopy";
            this.radioButtonFileCopySplit.UseVisualStyleBackColor = true;
            this.radioButtonFileCopySplit.CheckedChanged += new System.EventHandler(this.radioButton_CheckChanged);
            // 
            // radioButtonFileCopySystem
            // 
            this.radioButtonFileCopySystem.AutoSize = true;
            this.radioButtonFileCopySystem.Location = new System.Drawing.Point(22, 36);
            this.radioButtonFileCopySystem.Name = "radioButtonFileCopySystem";
            this.radioButtonFileCopySystem.Size = new System.Drawing.Size(75, 17);
            this.radioButtonFileCopySystem.TabIndex = 1;
            this.radioButtonFileCopySystem.TabStop = true;
            this.radioButtonFileCopySystem.Text = "To System";
            this.radioButtonFileCopySystem.UseVisualStyleBackColor = true;
            this.radioButtonFileCopySystem.CheckedChanged += new System.EventHandler(this.radioButton_CheckChanged);
            // 
            // radioButtonFileCopyCartridge
            // 
            this.radioButtonFileCopyCartridge.AutoSize = true;
            this.radioButtonFileCopyCartridge.Location = new System.Drawing.Point(141, 36);
            this.radioButtonFileCopyCartridge.Name = "radioButtonFileCopyCartridge";
            this.radioButtonFileCopyCartridge.Size = new System.Drawing.Size(83, 17);
            this.radioButtonFileCopyCartridge.TabIndex = 0;
            this.radioButtonFileCopyCartridge.TabStop = true;
            this.radioButtonFileCopyCartridge.Text = "To Cartridge";
            this.radioButtonFileCopyCartridge.UseVisualStyleBackColor = true;
            this.radioButtonFileCopyCartridge.CheckedChanged += new System.EventHandler(this.radioButton_CheckChanged);
            // 
            // ProjectFilePreferencesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Name = "ProjectFilePreferencesControl";
            this.Size = new System.Drawing.Size(648, 84);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonFileCopyCartridgeThenSystem;
        private System.Windows.Forms.RadioButton radioButtonFileCopySplit;
        private System.Windows.Forms.RadioButton radioButtonFileCopySystem;
        private System.Windows.Forms.RadioButton radioButtonFileCopyCartridge;
    }
}
