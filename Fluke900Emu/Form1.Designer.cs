namespace Fluke900Emu
{
    partial class Form1
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
            this.textBoxCommands = new System.Windows.Forms.TextBox();
            this.buttonConnectDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxCommands
            // 
            this.textBoxCommands.Location = new System.Drawing.Point(12, 57);
            this.textBoxCommands.Multiline = true;
            this.textBoxCommands.Name = "textBoxCommands";
            this.textBoxCommands.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxCommands.Size = new System.Drawing.Size(436, 390);
            this.textBoxCommands.TabIndex = 3;
            // 
            // buttonConnectDisconnect
            // 
            this.buttonConnectDisconnect.Location = new System.Drawing.Point(373, 12);
            this.buttonConnectDisconnect.Name = "buttonConnectDisconnect";
            this.buttonConnectDisconnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnectDisconnect.TabIndex = 2;
            this.buttonConnectDisconnect.Text = "Connect";
            this.buttonConnectDisconnect.UseVisualStyleBackColor = true;
            this.buttonConnectDisconnect.Click += new System.EventHandler(this.buttonConnectDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 469);
            this.Controls.Add(this.textBoxCommands);
            this.Controls.Add(this.buttonConnectDisconnect);
            this.Name = "Form1";
            this.Text = "Fluke900Emu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCommands;
        private System.Windows.Forms.Button buttonConnectDisconnect;
    }
}

