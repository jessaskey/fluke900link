namespace Fluke900Link.Controls
{
    partial class DeveloperOutput
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
            this.tabControlDeveloper = new System.Windows.Forms.TabControl();
            this.tabPageErrors = new System.Windows.Forms.TabPage();
            this.objectListViewIssues = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabPageOutput = new System.Windows.Forms.TabPage();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.tabControlDeveloper.SuspendLayout();
            this.tabPageErrors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewIssues)).BeginInit();
            this.tabPageOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlDeveloper
            // 
            this.tabControlDeveloper.Controls.Add(this.tabPageErrors);
            this.tabControlDeveloper.Controls.Add(this.tabPageOutput);
            this.tabControlDeveloper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDeveloper.Location = new System.Drawing.Point(0, 0);
            this.tabControlDeveloper.Margin = new System.Windows.Forms.Padding(2);
            this.tabControlDeveloper.Name = "tabControlDeveloper";
            this.tabControlDeveloper.SelectedIndex = 0;
            this.tabControlDeveloper.Size = new System.Drawing.Size(882, 267);
            this.tabControlDeveloper.TabIndex = 0;
            // 
            // tabPageErrors
            // 
            this.tabPageErrors.Controls.Add(this.objectListViewIssues);
            this.tabPageErrors.Location = new System.Drawing.Point(4, 22);
            this.tabPageErrors.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageErrors.Name = "tabPageErrors";
            this.tabPageErrors.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageErrors.Size = new System.Drawing.Size(874, 241);
            this.tabPageErrors.TabIndex = 0;
            this.tabPageErrors.Text = "Errors/Warnings";
            this.tabPageErrors.UseVisualStyleBackColor = true;
            // 
            // objectListViewIssues
            // 
            this.objectListViewIssues.AllColumns.Add(this.olvColumn1);
            this.objectListViewIssues.AllColumns.Add(this.olvColumn2);
            this.objectListViewIssues.AllColumns.Add(this.olvColumn3);
            this.objectListViewIssues.CellEditUseWholeCell = false;
            this.objectListViewIssues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3});
            this.objectListViewIssues.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewIssues.EmptyListMsg = "There are no issues detected at this time.";
            this.objectListViewIssues.FullRowSelect = true;
            this.objectListViewIssues.Location = new System.Drawing.Point(2, 2);
            this.objectListViewIssues.MultiSelect = false;
            this.objectListViewIssues.Name = "objectListViewIssues";
            this.objectListViewIssues.ShowGroups = false;
            this.objectListViewIssues.Size = new System.Drawing.Size(870, 237);
            this.objectListViewIssues.TabIndex = 0;
            this.objectListViewIssues.UseCompatibleStateImageBehavior = false;
            this.objectListViewIssues.View = System.Windows.Forms.View.Details;
            this.objectListViewIssues.ItemActivate += new System.EventHandler(this.objectListViewIssues_ItemActivate);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "ErrorType";
            this.olvColumn1.ImageAspectName = "Image";
            this.olvColumn1.Text = "Issue";
            this.olvColumn1.Width = 75;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Message";
            this.olvColumn2.FillsFreeSpace = true;
            this.olvColumn2.Text = "Message";
            this.olvColumn2.Width = 595;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "FileLocation";
            this.olvColumn3.Text = "Location";
            this.olvColumn3.Width = 300;
            // 
            // tabPageOutput
            // 
            this.tabPageOutput.Controls.Add(this.textBoxOutput);
            this.tabPageOutput.Location = new System.Drawing.Point(4, 22);
            this.tabPageOutput.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageOutput.Name = "tabPageOutput";
            this.tabPageOutput.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageOutput.Size = new System.Drawing.Size(874, 241);
            this.tabPageOutput.TabIndex = 1;
            this.tabPageOutput.Text = "Output";
            this.tabPageOutput.UseVisualStyleBackColor = true;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Font = new System.Drawing.Font("Courier New", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutput.Location = new System.Drawing.Point(2, 2);
            this.textBoxOutput.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(870, 237);
            this.textBoxOutput.TabIndex = 0;
            // 
            // DeveloperConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlDeveloper);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DeveloperConsole";
            this.Size = new System.Drawing.Size(882, 267);
            this.tabControlDeveloper.ResumeLayout(false);
            this.tabPageErrors.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewIssues)).EndInit();
            this.tabPageOutput.ResumeLayout(false);
            this.tabPageOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlDeveloper;
        private System.Windows.Forms.TabPage tabPageErrors;
        private System.Windows.Forms.TabPage tabPageOutput;
        private System.Windows.Forms.TextBox textBoxOutput;
        private BrightIdeasSoftware.ObjectListView objectListViewIssues;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
    }
}
