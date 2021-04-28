namespace Fluke900Link.Dialogs
{
    partial class NewLibraryDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewLibraryDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLibraryName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCreateFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxCopyFrom = new System.Windows.Forms.TextBox();
            this.buttonBrowseCreateFolder = new System.Windows.Forms.Button();
            this.buttonBrowseCloneFile = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.comboBoxSourceType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Library Name:";
            // 
            // textBoxLibraryName
            // 
            this.textBoxLibraryName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxLibraryName.Location = new System.Drawing.Point(100, 23);
            this.textBoxLibraryName.MaxLength = 15;
            this.textBoxLibraryName.Name = "textBoxLibraryName";
            this.textBoxLibraryName.Size = new System.Drawing.Size(159, 20);
            this.textBoxLibraryName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = ".LIB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Create In:";
            // 
            // textBoxCreateFolder
            // 
            this.textBoxCreateFolder.Location = new System.Drawing.Point(100, 55);
            this.textBoxCreateFolder.Name = "textBoxCreateFolder";
            this.textBoxCreateFolder.Size = new System.Drawing.Size(402, 20);
            this.textBoxCreateFolder.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(97, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Copy From:";
            // 
            // textBoxCopyFrom
            // 
            this.textBoxCopyFrom.Enabled = false;
            this.textBoxCopyFrom.Location = new System.Drawing.Point(163, 129);
            this.textBoxCopyFrom.Name = "textBoxCopyFrom";
            this.textBoxCopyFrom.Size = new System.Drawing.Size(339, 20);
            this.textBoxCopyFrom.TabIndex = 6;
            // 
            // buttonBrowseCreateFolder
            // 
            this.buttonBrowseCreateFolder.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseCreateFolder.Image")));
            this.buttonBrowseCreateFolder.Location = new System.Drawing.Point(504, 55);
            this.buttonBrowseCreateFolder.Name = "buttonBrowseCreateFolder";
            this.buttonBrowseCreateFolder.Size = new System.Drawing.Size(27, 20);
            this.buttonBrowseCreateFolder.TabIndex = 7;
            this.buttonBrowseCreateFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseCreateFolder.Click += new System.EventHandler(this.buttonBrowseCreateFolder_Click);
            // 
            // buttonBrowseCloneFile
            // 
            this.buttonBrowseCloneFile.Enabled = false;
            this.buttonBrowseCloneFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseCloneFile.Image")));
            this.buttonBrowseCloneFile.Location = new System.Drawing.Point(504, 128);
            this.buttonBrowseCloneFile.Name = "buttonBrowseCloneFile";
            this.buttonBrowseCloneFile.Size = new System.Drawing.Size(27, 20);
            this.buttonBrowseCloneFile.TabIndex = 8;
            this.buttonBrowseCloneFile.UseVisualStyleBackColor = true;
            this.buttonBrowseCloneFile.Click += new System.EventHandler(this.buttonBrowseCloneFile_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(456, 175);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = " Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(364, 175);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 10;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // comboBoxSourceType
            // 
            this.comboBoxSourceType.FormattingEnabled = true;
            this.comboBoxSourceType.Items.AddRange(new object[] {
            "All Reference Libraries",
            "Another Library",
            "None - Empty Library will be created"});
            this.comboBoxSourceType.Location = new System.Drawing.Point(100, 91);
            this.comboBoxSourceType.Name = "comboBoxSourceType";
            this.comboBoxSourceType.Size = new System.Drawing.Size(431, 21);
            this.comboBoxSourceType.TabIndex = 12;
            this.comboBoxSourceType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSourceType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Source Library:";
            // 
            // NewLibraryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 221);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxSourceType);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBrowseCloneFile);
            this.Controls.Add(this.buttonBrowseCreateFolder);
            this.Controls.Add(this.textBoxCopyFrom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxCreateFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxLibraryName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewLibraryDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Create New Library";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxLibraryName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCreateFolder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxCopyFrom;
        private System.Windows.Forms.Button buttonBrowseCreateFolder;
        private System.Windows.Forms.Button buttonBrowseCloneFile;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBoxSourceType;
        private System.Windows.Forms.Label label5;
    }
}