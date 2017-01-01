namespace Fluke900Link.Controls
{
    partial class DirectoryEditorControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryEditorControl));
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelReadWriteLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelSummary = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCompile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonDeleteFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFormat = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonExplore = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStripLocalComputer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.decodeToBinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.treeViewMain = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStripLocalComputer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewFiles
            // 
            this.listViewFiles.AllowDrop = true;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderSize});
            this.listViewFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.Location = new System.Drawing.Point(0, 0);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(302, 260);
            this.listViewFiles.TabIndex = 2;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragDrop);
            this.listViewFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewFiles_DragEnter);
            this.listViewFiles.DoubleClick += new System.EventHandler(this.listViewFiles_DoubleClick);
            this.listViewFiles.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseClick);
            this.listViewFiles.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseDown);
            this.listViewFiles.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseMove);
            this.listViewFiles.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listViewFiles_MouseUp);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Filename";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.Width = 125;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelReadWriteLabel,
            this.toolStripLabelSummary,
            this.toolStripButtonRefresh,
            this.toolStripButtonCompile,
            this.toolStripSeparator1,
            this.toolStripButtonDeleteFile,
            this.toolStripButtonFormat,
            this.toolStripSeparator2,
            this.toolStripButtonExplore});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(592, 27);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelReadWriteLabel
            // 
            this.toolStripLabelReadWriteLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelReadWriteLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripLabelReadWriteLabel.Name = "toolStripLabelReadWriteLabel";
            this.toolStripLabelReadWriteLabel.Size = new System.Drawing.Size(78, 24);
            this.toolStripLabelReadWriteLabel.Text = "<readwrite>";
            this.toolStripLabelReadWriteLabel.Visible = false;
            // 
            // toolStripLabelSummary
            // 
            this.toolStripLabelSummary.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabelSummary.Name = "toolStripLabelSummary";
            this.toolStripLabelSummary.Size = new System.Drawing.Size(73, 24);
            this.toolStripLabelSummary.Text = "<summary>";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Enabled = false;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonRefresh.Text = "Refresh Listing";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
            // 
            // toolStripButtonCompile
            // 
            this.toolStripButtonCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCompile.Enabled = false;
            this.toolStripButtonCompile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCompile.Image")));
            this.toolStripButtonCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCompile.Name = "toolStripButtonCompile";
            this.toolStripButtonCompile.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripButtonCompile.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonCompile.Text = "Compile File";
            this.toolStripButtonCompile.Click += new System.EventHandler(this.toolStripButtonCompile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonDeleteFile
            // 
            this.toolStripButtonDeleteFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteFile.Enabled = false;
            this.toolStripButtonDeleteFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteFile.Image")));
            this.toolStripButtonDeleteFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteFile.Name = "toolStripButtonDeleteFile";
            this.toolStripButtonDeleteFile.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripButtonDeleteFile.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonDeleteFile.Text = "Delete File";
            this.toolStripButtonDeleteFile.Click += new System.EventHandler(this.toolStripButtonDeleteFile_Click);
            // 
            // toolStripButtonFormat
            // 
            this.toolStripButtonFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFormat.Enabled = false;
            this.toolStripButtonFormat.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFormat.Image")));
            this.toolStripButtonFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFormat.Name = "toolStripButtonFormat";
            this.toolStripButtonFormat.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripButtonFormat.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonFormat.Text = "Format Cartridge";
            this.toolStripButtonFormat.Click += new System.EventHandler(this.toolStripButtonFormat_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonExplore
            // 
            this.toolStripButtonExplore.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExplore.Enabled = false;
            this.toolStripButtonExplore.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExplore.Image")));
            this.toolStripButtonExplore.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExplore.Name = "toolStripButtonExplore";
            this.toolStripButtonExplore.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripButtonExplore.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonExplore.Text = "Open in Windows Explorer";
            this.toolStripButtonExplore.Click += new System.EventHandler(this.toolStripButtonExplore_Click);
            // 
            // contextMenuStripLocalComputer
            // 
            this.contextMenuStripLocalComputer.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripLocalComputer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.decodeToBinaryToolStripMenuItem});
            this.contextMenuStripLocalComputer.Name = "contextMenuStripLocalComputer";
            this.contextMenuStripLocalComputer.Size = new System.Drawing.Size(165, 26);
            // 
            // decodeToBinaryToolStripMenuItem
            // 
            this.decodeToBinaryToolStripMenuItem.Name = "decodeToBinaryToolStripMenuItem";
            this.decodeToBinaryToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.decodeToBinaryToolStripMenuItem.Text = "Decode to Binary";
            this.decodeToBinaryToolStripMenuItem.Click += new System.EventHandler(this.decodeToBinaryToolStripMenuItem_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 27);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.treeViewMain);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.listViewFiles);
            this.splitContainerMain.Size = new System.Drawing.Size(592, 260);
            this.splitContainerMain.SplitterDistance = 286;
            this.splitContainerMain.TabIndex = 5;
            // 
            // treeViewMain
            // 
            this.treeViewMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMain.HideSelection = false;
            this.treeViewMain.HotTracking = true;
            this.treeViewMain.Location = new System.Drawing.Point(0, 0);
            this.treeViewMain.Name = "treeViewMain";
            this.treeViewMain.Size = new System.Drawing.Size(286, 260);
            this.treeViewMain.TabIndex = 0;
            this.treeViewMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewMain_AfterSelect);
            // 
            // DirectoryEditorControl
            // 
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.toolStrip1);
            this.MinimumSize = new System.Drawing.Size(300, 100);
            this.Name = "DirectoryEditorControl";
            this.Size = new System.Drawing.Size(592, 287);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStripLocalComputer.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSummary;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonFormat;
        private System.Windows.Forms.ToolStripButton toolStripButtonExplore;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLocalComputer;
        private System.Windows.Forms.ToolStripMenuItem decodeToBinaryToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TreeView treeViewMain;
        private System.Windows.Forms.ToolStripLabel toolStripLabelReadWriteLabel;
        private System.Windows.Forms.ToolStripButton toolStripButtonCompile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


    }
}
