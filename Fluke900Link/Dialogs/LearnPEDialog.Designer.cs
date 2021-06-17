namespace Fluke900Link.Dialogs
{
    partial class LearnPEDialog
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
            this.components = new System.ComponentModel.Container();
            this.buttonUse = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownFaultMaskFrom = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFaultMaskStep = new System.Windows.Forms.NumericUpDown();
            this.labelFaultMaskTo = new System.Windows.Forms.Label();
            this.labelThresholdTo = new System.Windows.Forms.Label();
            this.numericUpDownThresholdStep = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownThresholdFrom = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panelTestResult = new System.Windows.Forms.Panel();
            this.timerTest = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFaultMaskFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFaultMaskStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonUse
            // 
            this.buttonUse.Location = new System.Drawing.Point(330, 300);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new System.Drawing.Size(108, 23);
            this.buttonUse.TabIndex = 0;
            this.buttonUse.Text = "Use Suggested";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new System.EventHandler(this.buttonUse_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(463, 300);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "FaultMask From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "FaultMask Step:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "FaultMask To:";
            // 
            // numericUpDownFaultMaskFrom
            // 
            this.numericUpDownFaultMaskFrom.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFaultMaskFrom.Location = new System.Drawing.Point(158, 31);
            this.numericUpDownFaultMaskFrom.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownFaultMaskFrom.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownFaultMaskFrom.Name = "numericUpDownFaultMaskFrom";
            this.numericUpDownFaultMaskFrom.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownFaultMaskFrom.TabIndex = 5;
            this.numericUpDownFaultMaskFrom.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numericUpDownFaultMaskStep
            // 
            this.numericUpDownFaultMaskStep.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFaultMaskStep.Location = new System.Drawing.Point(158, 56);
            this.numericUpDownFaultMaskStep.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownFaultMaskStep.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownFaultMaskStep.Name = "numericUpDownFaultMaskStep";
            this.numericUpDownFaultMaskStep.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownFaultMaskStep.TabIndex = 6;
            this.numericUpDownFaultMaskStep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelFaultMaskTo
            // 
            this.labelFaultMaskTo.AutoSize = true;
            this.labelFaultMaskTo.Location = new System.Drawing.Point(158, 86);
            this.labelFaultMaskTo.Name = "labelFaultMaskTo";
            this.labelFaultMaskTo.Size = new System.Drawing.Size(35, 13);
            this.labelFaultMaskTo.TabIndex = 7;
            this.labelFaultMaskTo.Text = "label4";
            // 
            // labelThresholdTo
            // 
            this.labelThresholdTo.AutoSize = true;
            this.labelThresholdTo.Location = new System.Drawing.Point(366, 86);
            this.labelThresholdTo.Name = "labelThresholdTo";
            this.labelThresholdTo.Size = new System.Drawing.Size(35, 13);
            this.labelThresholdTo.TabIndex = 13;
            this.labelThresholdTo.Text = "label4";
            // 
            // numericUpDownThresholdStep
            // 
            this.numericUpDownThresholdStep.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownThresholdStep.Location = new System.Drawing.Point(366, 56);
            this.numericUpDownThresholdStep.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.numericUpDownThresholdStep.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownThresholdStep.Name = "numericUpDownThresholdStep";
            this.numericUpDownThresholdStep.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownThresholdStep.TabIndex = 12;
            this.numericUpDownThresholdStep.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // numericUpDownThresholdFrom
            // 
            this.numericUpDownThresholdFrom.Location = new System.Drawing.Point(366, 31);
            this.numericUpDownThresholdFrom.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.numericUpDownThresholdFrom.Name = "numericUpDownThresholdFrom";
            this.numericUpDownThresholdFrom.Size = new System.Drawing.Size(72, 20);
            this.numericUpDownThresholdFrom.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(285, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Threshold To:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(276, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Threshold Step:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(275, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Threshold From:";
            // 
            // panelTestResult
            // 
            this.panelTestResult.Location = new System.Drawing.Point(57, 113);
            this.panelTestResult.Name = "panelTestResult";
            this.panelTestResult.Size = new System.Drawing.Size(439, 171);
            this.panelTestResult.TabIndex = 14;
            this.panelTestResult.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTestResult_Paint);
            // 
            // timerTest
            // 
            this.timerTest.Interval = 500;
            this.timerTest.Tick += new System.EventHandler(this.timerTest_Tick);
            // 
            // LearnPEDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 335);
            this.Controls.Add(this.panelTestResult);
            this.Controls.Add(this.labelThresholdTo);
            this.Controls.Add(this.numericUpDownThresholdStep);
            this.Controls.Add(this.numericUpDownThresholdFrom);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.labelFaultMaskTo);
            this.Controls.Add(this.numericUpDownFaultMaskStep);
            this.Controls.Add(this.numericUpDownFaultMaskFrom);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LearnPEDialog";
            this.ShowInTaskbar = false;
            this.Text = "Learn Performance Envelope";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFaultMaskFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFaultMaskStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownThresholdFrom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUse;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownFaultMaskFrom;
        private System.Windows.Forms.NumericUpDown numericUpDownFaultMaskStep;
        private System.Windows.Forms.Label labelFaultMaskTo;
        private System.Windows.Forms.Label labelThresholdTo;
        private System.Windows.Forms.NumericUpDown numericUpDownThresholdStep;
        private System.Windows.Forms.NumericUpDown numericUpDownThresholdFrom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelTestResult;
        private System.Windows.Forms.Timer timerTest;
    }
}