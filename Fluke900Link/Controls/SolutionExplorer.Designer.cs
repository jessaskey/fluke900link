namespace Fluke900Link.Controls
{
    partial class SolutionExplorer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionExplorer));
            this.toolStripSolution = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddSequence = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddLibrary = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddPCSequence = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAddExisting = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExpandAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonOpenExplorer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonCheckForErrors = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRunSequence = new System.Windows.Forms.ToolStripButton();
            this.treeViewSolution = new System.Windows.Forms.TreeView();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.excludeFromProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileLocationInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.toolStripSolution.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripSolution
            // 
            this.toolStripSolution.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripSolution.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddSequence,
            this.toolStripButtonAddLocation,
            this.toolStripButtonAddLibrary,
            this.toolStripButtonAddPCSequence,
            this.toolStripButtonAddExisting,
            this.toolStripSeparator2,
            this.toolStripButtonExpandAll,
            this.toolStripButtonCollapseAll,
            this.toolStripSeparator1,
            this.toolStripButtonOpenExplorer,
            this.toolStripButtonRefresh,
            this.toolStripSeparator3,
            this.toolStripButtonCheckForErrors,
            this.toolStripButtonRunSequence});
            this.toolStripSolution.Location = new System.Drawing.Point(0, 0);
            this.toolStripSolution.Name = "toolStripSolution";
            this.toolStripSolution.Size = new System.Drawing.Size(351, 27);
            this.toolStripSolution.TabIndex = 0;
            this.toolStripSolution.Text = "toolStrip1";
            // 
            // toolStripButtonAddSequence
            // 
            this.toolStripButtonAddSequence.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddSequence.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddSequence.Image")));
            this.toolStripButtonAddSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddSequence.Name = "toolStripButtonAddSequence";
            this.toolStripButtonAddSequence.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddSequence.Text = "Add a new Sequence";
            this.toolStripButtonAddSequence.Click += new System.EventHandler(this.toolStripButtonAddSequence_Click);
            // 
            // toolStripButtonAddLocation
            // 
            this.toolStripButtonAddLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddLocation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddLocation.Image")));
            this.toolStripButtonAddLocation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddLocation.Name = "toolStripButtonAddLocation";
            this.toolStripButtonAddLocation.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddLocation.Text = "Add a new Location Set";
            this.toolStripButtonAddLocation.Click += new System.EventHandler(this.toolStripButtonAddLocation_Click);
            // 
            // toolStripButtonAddLibrary
            // 
            this.toolStripButtonAddLibrary.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddLibrary.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddLibrary.Image")));
            this.toolStripButtonAddLibrary.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddLibrary.Name = "toolStripButtonAddLibrary";
            this.toolStripButtonAddLibrary.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddLibrary.Text = "Add a new Library";
            this.toolStripButtonAddLibrary.Click += new System.EventHandler(this.toolStripButtonAddLibrary_Click);
            // 
            // toolStripButtonAddPCSequence
            // 
            this.toolStripButtonAddPCSequence.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddPCSequence.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddPCSequence.Image")));
            this.toolStripButtonAddPCSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddPCSequence.Name = "toolStripButtonAddPCSequence";
            this.toolStripButtonAddPCSequence.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddPCSequence.Text = "Add PC Test Sequence";
            this.toolStripButtonAddPCSequence.Click += new System.EventHandler(this.toolStripButtonAddPCSequence_Click);
            // 
            // toolStripButtonAddExisting
            // 
            this.toolStripButtonAddExisting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAddExisting.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddExisting.Image")));
            this.toolStripButtonAddExisting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddExisting.Name = "toolStripButtonAddExisting";
            this.toolStripButtonAddExisting.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonAddExisting.Text = "Add Existing File";
            this.toolStripButtonAddExisting.Click += new System.EventHandler(this.toolStripButtonAddExisting_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonExpandAll
            // 
            this.toolStripButtonExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExpandAll.Image")));
            this.toolStripButtonExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExpandAll.Name = "toolStripButtonExpandAll";
            this.toolStripButtonExpandAll.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonExpandAll.Text = "Expand All Nodes";
            this.toolStripButtonExpandAll.Click += new System.EventHandler(this.toolStripButtonExpandAll_Click);
            // 
            // toolStripButtonCollapseAll
            // 
            this.toolStripButtonCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCollapseAll.Image")));
            this.toolStripButtonCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCollapseAll.Name = "toolStripButtonCollapseAll";
            this.toolStripButtonCollapseAll.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonCollapseAll.Text = "Collapse All Nodes";
            this.toolStripButtonCollapseAll.Click += new System.EventHandler(this.toolStripButtonCollapseAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonOpenExplorer
            // 
            this.toolStripButtonOpenExplorer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonOpenExplorer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenExplorer.Image")));
            this.toolStripButtonOpenExplorer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenExplorer.Name = "toolStripButtonOpenExplorer";
            this.toolStripButtonOpenExplorer.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonOpenExplorer.Text = "Open";
            this.toolStripButtonOpenExplorer.ToolTipText = "Open Location in Windows Explorer";
            this.toolStripButtonOpenExplorer.Click += new System.EventHandler(this.toolStripButtonOpenExplorer_Click);
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonRefresh.Text = "Refresh Project";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonCheckForErrors
            // 
            this.toolStripButtonCheckForErrors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCheckForErrors.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCheckForErrors.Image")));
            this.toolStripButtonCheckForErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCheckForErrors.Name = "toolStripButtonCheckForErrors";
            this.toolStripButtonCheckForErrors.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonCheckForErrors.Text = "Check Project for Errors";
            this.toolStripButtonCheckForErrors.Click += new System.EventHandler(this.toolStripButtonCheckForErrors_Click);
            // 
            // toolStripButtonRunSequence
            // 
            this.toolStripButtonRunSequence.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRunSequence.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRunSequence.Image")));
            this.toolStripButtonRunSequence.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRunSequence.Name = "toolStripButtonRunSequence";
            this.toolStripButtonRunSequence.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonRunSequence.Text = "Check+Copy+Compile to Fluke";
            this.toolStripButtonRunSequence.Click += new System.EventHandler(this.toolStripButtonRunSequence_Click);
            // 
            // treeViewSolution
            // 
            this.treeViewSolution.ContextMenuStrip = this.contextMenuStripTree;
            this.treeViewSolution.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewSolution.ImageIndex = 0;
            this.treeViewSolution.ImageList = this.imageListTree;
            this.treeViewSolution.Location = new System.Drawing.Point(0, 27);
            this.treeViewSolution.Name = "treeViewSolution";
            this.treeViewSolution.SelectedImageIndex = 0;
            this.treeViewSolution.ShowNodeToolTips = true;
            this.treeViewSolution.Size = new System.Drawing.Size(351, 290);
            this.treeViewSolution.TabIndex = 1;
            this.treeViewSolution.DoubleClick += new System.EventHandler(this.treeViewSolution_DoubleClick);
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.excludeFromProjectToolStripMenuItem,
            this.openFileLocationInWindowsExplorerToolStripMenuItem});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(220, 70);
            this.contextMenuStripTree.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripTree_Opening);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // excludeFromProjectToolStripMenuItem
            // 
            this.excludeFromProjectToolStripMenuItem.Name = "excludeFromProjectToolStripMenuItem";
            this.excludeFromProjectToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.excludeFromProjectToolStripMenuItem.Text = "Exclude from Project";
            this.excludeFromProjectToolStripMenuItem.Click += new System.EventHandler(this.excludeFromProjectToolStripMenuItem_Click);
            // 
            // openFileLocationInWindowsExplorerToolStripMenuItem
            // 
            this.openFileLocationInWindowsExplorerToolStripMenuItem.Name = "openFileLocationInWindowsExplorerToolStripMenuItem";
            this.openFileLocationInWindowsExplorerToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.openFileLocationInWindowsExplorerToolStripMenuItem.Text = "Open Folder in File Explorer";
            this.openFileLocationInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationInWindowsExplorerToolStripMenuItem_Click);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "ThumbnailView.png");
            this.imageListTree.Images.SetKeyName(1, "File_6276 - lib.png");
            this.imageListTree.Images.SetKeyName(2, "File_6276 - loc.png");
            this.imageListTree.Images.SetKeyName(3, "File_6276 - seq.png");
            this.imageListTree.Images.SetKeyName(4, "MemoryWindow_6537.png");
            this.imageListTree.Images.SetKeyName(5, "script-binary-icon.png");
            // 
            // SolutionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 317);
            this.Controls.Add(this.treeViewSolution);
            this.Controls.Add(this.toolStripSolution);
            this.Name = "SolutionExplorer";
            this.toolStripSolution.ResumeLayout(false);
            this.toolStripSolution.PerformLayout();
            this.contextMenuStripTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripSolution;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.TreeView treeViewSolution;
        private System.Windows.Forms.ToolStripButton toolStripButtonExpandAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonCollapseAll;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddSequence;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddLocation;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddLibrary;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem excludeFromProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenExplorer;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddExisting;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonCheckForErrors;
        private System.Windows.Forms.ToolStripButton toolStripButtonRunSequence;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddPCSequence;
    }
}
