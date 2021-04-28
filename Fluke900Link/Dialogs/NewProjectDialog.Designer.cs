namespace Fluke900Link.Dialogs
{
    partial class NewProjectDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxProjectName = new System.Windows.Forms.TextBox();
            this.textBoxCreateDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseProjectFolder = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.checkBoxCreateDirectory = new System.Windows.Forms.CheckBox();
            this.labelProjectDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Create Project in:";
            // 
            // textBoxProjectName
            // 
            this.textBoxProjectName.Location = new System.Drawing.Point(150, 96);
            this.textBoxProjectName.Name = "textBoxProjectName";
            this.textBoxProjectName.Size = new System.Drawing.Size(500, 20);
            this.textBoxProjectName.TabIndex = 2;
            // 
            // textBoxCreateDirectory
            // 
            this.textBoxCreateDirectory.Location = new System.Drawing.Point(150, 137);
            this.textBoxCreateDirectory.Name = "textBoxCreateDirectory";
            this.textBoxCreateDirectory.Size = new System.Drawing.Size(500, 20);
            this.textBoxCreateDirectory.TabIndex = 3;
            // 
            // buttonBrowseProjectFolder
            // 
            this.buttonBrowseProjectFolder.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseProjectFolder.Image")));
            this.buttonBrowseProjectFolder.Location = new System.Drawing.Point(647, 135);
            this.buttonBrowseProjectFolder.Name = "buttonBrowseProjectFolder";
            this.buttonBrowseProjectFolder.Size = new System.Drawing.Size(26, 23);
            this.buttonBrowseProjectFolder.TabIndex = 4;
            this.buttonBrowseProjectFolder.UseVisualStyleBackColor = true;
            this.buttonBrowseProjectFolder.Click += new System.EventHandler(this.buttonBrowseProjectFolder_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(598, 186);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(498, 186);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // checkBoxCreateDirectory
            // 
            this.checkBoxCreateDirectory.AutoSize = true;
            this.checkBoxCreateDirectory.Checked = true;
            this.checkBoxCreateDirectory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateDirectory.Location = new System.Drawing.Point(150, 186);
            this.checkBoxCreateDirectory.Name = "checkBoxCreateDirectory";
            this.checkBoxCreateDirectory.Size = new System.Drawing.Size(153, 17);
            this.checkBoxCreateDirectory.TabIndex = 7;
            this.checkBoxCreateDirectory.Text = "Create Directory for Project";
            this.checkBoxCreateDirectory.UseVisualStyleBackColor = true;
            // 
            // labelProjectDescription
            // 
            this.labelProjectDescription.Location = new System.Drawing.Point(25, 19);
            this.labelProjectDescription.Name = "labelProjectDescription";
            this.labelProjectDescription.Size = new System.Drawing.Size(643, 63);
            this.labelProjectDescription.TabIndex = 8;
            this.labelProjectDescription.Text = resources.GetString("labelProjectDescription.Text");
            // 
            // NewProjectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 232);
            this.ControlBox = false;
            this.Controls.Add(this.labelProjectDescription);
            this.Controls.Add(this.checkBoxCreateDirectory);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBrowseProjectFolder);
            this.Controls.Add(this.textBoxCreateDirectory);
            this.Controls.Add(this.textBoxProjectName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Project";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxProjectName;
        private System.Windows.Forms.TextBox textBoxCreateDirectory;
        private System.Windows.Forms.Button buttonBrowseProjectFolder;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.CheckBox checkBoxCreateDirectory;
        private System.Windows.Forms.Label labelProjectDescription;
    }
}