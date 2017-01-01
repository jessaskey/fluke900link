namespace Fluke900Link.Dialogs
{
    partial class ProjectPropertiesDialog
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
            this.tabControlProject = new System.Windows.Forms.TabControl();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.tabPageGlobalParameters = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxResetOffset = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxResetDuration = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxResetVcc = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxResetPolarity = new System.Windows.Forms.ComboBox();
            this.radioButtonResetDisabled = new System.Windows.Forms.RadioButton();
            this.radioButtonResetEnabled = new System.Windows.Forms.RadioButton();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBoxUseSimulationData = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoIncludeDevices = new System.Windows.Forms.CheckBox();
            this.textBoxProjectPathFile = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.projectFilePreferencesControl1 = new Fluke900Link.Controls.ProjectFilePreferencesControl();
            this.tabControlProject.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.tabPageGlobalParameters.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlProject
            // 
            this.tabControlProject.Controls.Add(this.tabPageProject);
            this.tabControlProject.Controls.Add(this.tabPageGlobalParameters);
            this.tabControlProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlProject.Location = new System.Drawing.Point(0, 0);
            this.tabControlProject.Name = "tabControlProject";
            this.tabControlProject.SelectedIndex = 0;
            this.tabControlProject.Size = new System.Drawing.Size(613, 308);
            this.tabControlProject.TabIndex = 0;
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.projectFilePreferencesControl1);
            this.tabPageProject.Controls.Add(this.buttonCancel);
            this.tabPageProject.Controls.Add(this.buttonOK);
            this.tabPageProject.Controls.Add(this.groupBox3);
            this.tabPageProject.Controls.Add(this.textBoxProjectPathFile);
            this.tabPageProject.Controls.Add(this.label12);
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProject.Size = new System.Drawing.Size(605, 282);
            this.tabPageProject.TabIndex = 0;
            this.tabPageProject.Text = "Project";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // tabPageGlobalParameters
            // 
            this.tabPageGlobalParameters.Controls.Add(this.groupBox2);
            this.tabPageGlobalParameters.Controls.Add(this.groupBox1);
            this.tabPageGlobalParameters.Controls.Add(this.textBox3);
            this.tabPageGlobalParameters.Controls.Add(this.label3);
            this.tabPageGlobalParameters.Controls.Add(this.textBox2);
            this.tabPageGlobalParameters.Controls.Add(this.label2);
            this.tabPageGlobalParameters.Controls.Add(this.textBox1);
            this.tabPageGlobalParameters.Controls.Add(this.label1);
            this.tabPageGlobalParameters.Location = new System.Drawing.Point(4, 22);
            this.tabPageGlobalParameters.Name = "tabPageGlobalParameters";
            this.tabPageGlobalParameters.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGlobalParameters.Size = new System.Drawing.Size(605, 282);
            this.tabPageGlobalParameters.TabIndex = 1;
            this.tabPageGlobalParameters.Text = "Global Parameters";
            this.tabPageGlobalParameters.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox4);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.textBox5);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Location = new System.Drawing.Point(46, 251);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(487, 107);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "External Trigger:";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(343, 74);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(90, 20);
            this.textBox4.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(250, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Delay:";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(343, 44);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(90, 20);
            this.textBox5.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(250, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Duration:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 78);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Polarity:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(83, 75);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "Ext Gate:";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(83, 48);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 10;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(166, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Disabled";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(83, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(64, 17);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.Text = "Enabled";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxResetOffset);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxResetDuration);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxResetVcc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxResetPolarity);
            this.groupBox1.Controls.Add(this.radioButtonResetDisabled);
            this.groupBox1.Controls.Add(this.radioButtonResetEnabled);
            this.groupBox1.Location = new System.Drawing.Point(46, 114);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(487, 131);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reset:";
            // 
            // textBoxResetOffset
            // 
            this.textBoxResetOffset.Location = new System.Drawing.Point(343, 81);
            this.textBoxResetOffset.Name = "textBoxResetOffset";
            this.textBoxResetOffset.Size = new System.Drawing.Size(90, 20);
            this.textBoxResetOffset.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(250, 84);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Offset:";
            // 
            // textBoxResetDuration
            // 
            this.textBoxResetDuration.Location = new System.Drawing.Point(343, 51);
            this.textBoxResetDuration.Name = "textBoxResetDuration";
            this.textBoxResetDuration.Size = new System.Drawing.Size(90, 20);
            this.textBoxResetDuration.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Duration:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Vcc:";
            // 
            // comboBoxResetVcc
            // 
            this.comboBoxResetVcc.FormattingEnabled = true;
            this.comboBoxResetVcc.Location = new System.Drawing.Point(83, 78);
            this.comboBoxResetVcc.Name = "comboBoxResetVcc";
            this.comboBoxResetVcc.Size = new System.Drawing.Size(121, 21);
            this.comboBoxResetVcc.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Polarity:";
            // 
            // comboBoxResetPolarity
            // 
            this.comboBoxResetPolarity.FormattingEnabled = true;
            this.comboBoxResetPolarity.Location = new System.Drawing.Point(83, 51);
            this.comboBoxResetPolarity.Name = "comboBoxResetPolarity";
            this.comboBoxResetPolarity.Size = new System.Drawing.Size(121, 21);
            this.comboBoxResetPolarity.TabIndex = 2;
            // 
            // radioButtonResetDisabled
            // 
            this.radioButtonResetDisabled.AutoSize = true;
            this.radioButtonResetDisabled.Checked = true;
            this.radioButtonResetDisabled.Location = new System.Drawing.Point(166, 19);
            this.radioButtonResetDisabled.Name = "radioButtonResetDisabled";
            this.radioButtonResetDisabled.Size = new System.Drawing.Size(66, 17);
            this.radioButtonResetDisabled.TabIndex = 1;
            this.radioButtonResetDisabled.TabStop = true;
            this.radioButtonResetDisabled.Text = "Disabled";
            this.radioButtonResetDisabled.UseVisualStyleBackColor = true;
            // 
            // radioButtonResetEnabled
            // 
            this.radioButtonResetEnabled.AutoSize = true;
            this.radioButtonResetEnabled.Location = new System.Drawing.Point(83, 19);
            this.radioButtonResetEnabled.Name = "radioButtonResetEnabled";
            this.radioButtonResetEnabled.Size = new System.Drawing.Size(64, 17);
            this.radioButtonResetEnabled.TabIndex = 0;
            this.radioButtonResetEnabled.Text = "Enabled";
            this.radioButtonResetEnabled.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(389, 44);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(90, 20);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Test Time:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(136, 76);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 20);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Threshold:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(136, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(90, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fault Mask:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(542, 241);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(56, 27);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(472, 241);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(56, 27);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxUseSimulationData);
            this.groupBox3.Controls.Add(this.checkBoxAutoIncludeDevices);
            this.groupBox3.Location = new System.Drawing.Point(17, 54);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(581, 81);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Library Options:";
            // 
            // checkBoxUseSimulationData
            // 
            this.checkBoxUseSimulationData.AutoSize = true;
            this.checkBoxUseSimulationData.Location = new System.Drawing.Point(16, 47);
            this.checkBoxUseSimulationData.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxUseSimulationData.Name = "checkBoxUseSimulationData";
            this.checkBoxUseSimulationData.Size = new System.Drawing.Size(271, 17);
            this.checkBoxUseSimulationData.TabIndex = 1;
            this.checkBoxUseSimulationData.Text = "Always include Simulation/Shadow data if available.";
            this.checkBoxUseSimulationData.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoIncludeDevices
            // 
            this.checkBoxAutoIncludeDevices.AutoSize = true;
            this.checkBoxAutoIncludeDevices.Location = new System.Drawing.Point(16, 25);
            this.checkBoxAutoIncludeDevices.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAutoIncludeDevices.Name = "checkBoxAutoIncludeDevices";
            this.checkBoxAutoIncludeDevices.Size = new System.Drawing.Size(279, 17);
            this.checkBoxAutoIncludeDevices.TabIndex = 0;
            this.checkBoxAutoIncludeDevices.Text = "Automatically build device libraries for known devices.";
            this.checkBoxAutoIncludeDevices.UseVisualStyleBackColor = true;
            // 
            // textBoxProjectPathFile
            // 
            this.textBoxProjectPathFile.Location = new System.Drawing.Point(99, 15);
            this.textBoxProjectPathFile.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProjectPathFile.Name = "textBoxProjectPathFile";
            this.textBoxProjectPathFile.ReadOnly = true;
            this.textBoxProjectPathFile.Size = new System.Drawing.Size(499, 20);
            this.textBoxProjectPathFile.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 18);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Project File:";
            // 
            // projectFilePreferencesControl1
            // 
            this.projectFilePreferencesControl1.Location = new System.Drawing.Point(8, 140);
            this.projectFilePreferencesControl1.Name = "projectFilePreferencesControl1";
            this.projectFilePreferencesControl1.ProjectFileCopyBehavior = Fluke900Link.FileLocationCopyBehavior.System;
            this.projectFilePreferencesControl1.Size = new System.Drawing.Size(590, 84);
            this.projectFilePreferencesControl1.TabIndex = 10;
            // 
            // ProjectPropertiesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 308);
            this.Controls.Add(this.tabControlProject);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectPropertiesDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project Properties";
            this.tabControlProject.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.tabPageProject.PerformLayout();
            this.tabPageGlobalParameters.ResumeLayout(false);
            this.tabPageGlobalParameters.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlProject;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.TabPage tabPageGlobalParameters;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxResetOffset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxResetDuration;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxResetVcc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxResetPolarity;
        private System.Windows.Forms.RadioButton radioButtonResetDisabled;
        private System.Windows.Forms.RadioButton radioButtonResetEnabled;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private Controls.ProjectFilePreferencesControl projectFilePreferencesControl1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxUseSimulationData;
        private System.Windows.Forms.CheckBox checkBoxAutoIncludeDevices;
        private System.Windows.Forms.TextBox textBoxProjectPathFile;
        private System.Windows.Forms.Label label12;
    }
}