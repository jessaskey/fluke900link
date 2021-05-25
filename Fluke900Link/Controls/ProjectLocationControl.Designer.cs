namespace Fluke900Link.Controls
{
    partial class ProjectLocationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectLocationControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxCheckSum = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxClipCheck = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelSimulation = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxRDDrive = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxICSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonBrowseLibrary = new System.Windows.Forms.Button();
            this.textBoxICName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxPinActivity = new System.Windows.Forms.GroupBox();
            this.groupBoxFloatCheck = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxCheckSum);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBoxClipCheck);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.labelSimulation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxRDDrive);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxICSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.buttonBrowseLibrary);
            this.groupBox1.Controls.Add(this.textBoxICName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Definition";
            // 
            // textBoxCheckSum
            // 
            this.textBoxCheckSum.Location = new System.Drawing.Point(421, 55);
            this.textBoxCheckSum.Name = "textBoxCheckSum";
            this.textBoxCheckSum.Size = new System.Drawing.Size(77, 20);
            this.textBoxCheckSum.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(360, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "CheckSum:";
            // 
            // comboBoxClipCheck
            // 
            this.comboBoxClipCheck.FormattingEnabled = true;
            this.comboBoxClipCheck.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.comboBoxClipCheck.Location = new System.Drawing.Point(421, 28);
            this.comboBoxClipCheck.Name = "comboBoxClipCheck";
            this.comboBoxClipCheck.Size = new System.Drawing.Size(77, 21);
            this.comboBoxClipCheck.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(361, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Clip Check:";
            // 
            // labelSimulation
            // 
            this.labelSimulation.AutoSize = true;
            this.labelSimulation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSimulation.ForeColor = System.Drawing.Color.Black;
            this.labelSimulation.Location = new System.Drawing.Point(259, 58);
            this.labelSimulation.Name = "labelSimulation";
            this.labelSimulation.Size = new System.Drawing.Size(30, 13);
            this.labelSimulation.TabIndex = 8;
            this.labelSimulation.Text = "N/A";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Simulation:";
            // 
            // comboBoxRDDrive
            // 
            this.comboBoxRDDrive.FormattingEnabled = true;
            this.comboBoxRDDrive.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxRDDrive.Location = new System.Drawing.Point(255, 28);
            this.comboBoxRDDrive.Name = "comboBoxRDDrive";
            this.comboBoxRDDrive.Size = new System.Drawing.Size(77, 21);
            this.comboBoxRDDrive.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(195, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "RD Drive:";
            // 
            // comboBoxICSize
            // 
            this.comboBoxICSize.FormattingEnabled = true;
            this.comboBoxICSize.Items.AddRange(new object[] {
            "8",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "28"});
            this.comboBoxICSize.Location = new System.Drawing.Point(72, 55);
            this.comboBoxICSize.Name = "comboBoxICSize";
            this.comboBoxICSize.Size = new System.Drawing.Size(100, 21);
            this.comboBoxICSize.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IC Size:";
            // 
            // buttonBrowseLibrary
            // 
            this.buttonBrowseLibrary.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseLibrary.Image")));
            this.buttonBrowseLibrary.Location = new System.Drawing.Point(150, 29);
            this.buttonBrowseLibrary.Name = "buttonBrowseLibrary";
            this.buttonBrowseLibrary.Size = new System.Drawing.Size(22, 20);
            this.buttonBrowseLibrary.TabIndex = 2;
            this.buttonBrowseLibrary.UseVisualStyleBackColor = true;
            // 
            // textBoxICName
            // 
            this.textBoxICName.Location = new System.Drawing.Point(72, 29);
            this.textBoxICName.Name = "textBoxICName";
            this.textBoxICName.Size = new System.Drawing.Size(81, 20);
            this.textBoxICName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IC Name:";
            // 
            // groupBoxPinActivity
            // 
            this.groupBoxPinActivity.Location = new System.Drawing.Point(3, 117);
            this.groupBoxPinActivity.Name = "groupBoxPinActivity";
            this.groupBoxPinActivity.Size = new System.Drawing.Size(249, 264);
            this.groupBoxPinActivity.TabIndex = 1;
            this.groupBoxPinActivity.TabStop = false;
            this.groupBoxPinActivity.Text = "Pin Activity";
            // 
            // groupBoxFloatCheck
            // 
            this.groupBoxFloatCheck.Location = new System.Drawing.Point(265, 117);
            this.groupBoxFloatCheck.Name = "groupBoxFloatCheck";
            this.groupBoxFloatCheck.Size = new System.Drawing.Size(260, 264);
            this.groupBoxFloatCheck.TabIndex = 2;
            this.groupBoxFloatCheck.TabStop = false;
            this.groupBoxFloatCheck.Text = "Float Check";
            // 
            // ProjectLocationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxFloatCheck);
            this.Controls.Add(this.groupBoxPinActivity);
            this.Controls.Add(this.groupBox1);
            this.Name = "ProjectLocationControl";
            this.Size = new System.Drawing.Size(1108, 485);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxCheckSum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxClipCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSimulation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxRDDrive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxICSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonBrowseLibrary;
        private System.Windows.Forms.TextBox textBoxICName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxPinActivity;
        private System.Windows.Forms.GroupBox groupBoxFloatCheck;
    }
}
