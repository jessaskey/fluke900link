namespace Fluke900Link.Controls
{
    partial class LibraryBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryBrowser));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLibraryNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowAllFolders = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenExplorer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.treeViewLibraries = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLibraryNew,
            this.toolStripSeparator1,
            this.toolStripButtonShowAllFolders,
            this.toolStripButtonOpenExplorer,
            this.toolStripButtonRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(223, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonLibraryNew
            // 
            this.toolStripButtonLibraryNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLibraryNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLibraryNew.Image")));
            this.toolStripButtonLibraryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLibraryNew.Name = "toolStripButtonLibraryNew";
            this.toolStripButtonLibraryNew.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonLibraryNew.Text = "Create New Library";
            this.toolStripButtonLibraryNew.Click += new System.EventHandler(this.toolStripButtonLibraryNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonShowAllFolders
            // 
            this.toolStripButtonShowAllFolders.Checked = true;
            this.toolStripButtonShowAllFolders.CheckOnClick = true;
            this.toolStripButtonShowAllFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonShowAllFolders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowAllFolders.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowAllFolders.Image")));
            this.toolStripButtonShowAllFolders.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowAllFolders.Name = "toolStripButtonShowAllFolders";
            this.toolStripButtonShowAllFolders.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowAllFolders.Text = "Show All Folders";
            this.toolStripButtonShowAllFolders.CheckStateChanged += new System.EventHandler(this.toolStripButtonShowAllFolders_CheckStateChanged);
            // 
            // toolStripButtonOpenExplorer
            // 
            this.toolStripButtonOpenExplorer.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonOpenExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenExplorer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenExplorer.Image")));
            this.toolStripButtonOpenExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenExplorer.Name = "toolStripButtonOpenExplorer";
            this.toolStripButtonOpenExplorer.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonOpenExplorer.Text = "Open Windows Explorer";
            this.toolStripButtonOpenExplorer.Click += new System.EventHandler(this.toolStripButtonOpenExplorer_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonRefresh.Text = "Refresh";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // treeViewLibraries
            // 
            this.treeViewLibraries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLibraries.Location = new System.Drawing.Point(0, 25);
            this.treeViewLibraries.Name = "treeViewLibraries";
            this.treeViewLibraries.Size = new System.Drawing.Size(223, 259);
            this.treeViewLibraries.TabIndex = 1;
            this.treeViewLibraries.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLibraries_AfterCollapse);
            this.treeViewLibraries.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLibraries_AfterExpand);
            this.treeViewLibraries.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewLibraries_NodeMouseDoubleClick);
            // 
            // LibraryBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeViewLibraries);
            this.Controls.Add(this.toolStrip1);
            this.Name = "LibraryBrowser";
            this.Size = new System.Drawing.Size(223, 284);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonLibraryNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowAllFolders;
        private System.Windows.Forms.TreeView treeViewLibraries;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenExplorer;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
    }
}
