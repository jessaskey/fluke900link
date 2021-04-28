namespace Fluke900Link.Controls
{
    partial class LocationsEditor
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
            this.toolStripLocations = new System.Windows.Forms.ToolStrip();
            this.objectListViewLocations = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripLocations
            // 
            this.toolStripLocations.Location = new System.Drawing.Point(0, 0);
            this.toolStripLocations.Name = "toolStripLocations";
            this.toolStripLocations.Size = new System.Drawing.Size(682, 25);
            this.toolStripLocations.TabIndex = 0;
            this.toolStripLocations.Text = "toolStrip1";
            // 
            // objectListViewLocations
            // 
            this.objectListViewLocations.AllColumns.Add(this.olvColumn1);
            this.objectListViewLocations.AllColumns.Add(this.olvColumn2);
            this.objectListViewLocations.CellEditUseWholeCell = false;
            this.objectListViewLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2});
            this.objectListViewLocations.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListViewLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectListViewLocations.Location = new System.Drawing.Point(0, 25);
            this.objectListViewLocations.Name = "objectListViewLocations";
            this.objectListViewLocations.Size = new System.Drawing.Size(682, 391);
            this.objectListViewLocations.TabIndex = 1;
            this.objectListViewLocations.UseCompatibleStateImageBehavior = false;
            this.objectListViewLocations.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.Text = "Location";
            this.olvColumn1.Width = 73;
            // 
            // olvColumn2
            // 
            this.olvColumn2.Text = "Device";
            this.olvColumn2.Width = 70;
            // 
            // LocationsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectListViewLocations);
            this.Controls.Add(this.toolStripLocations);
            this.Name = "LocationsEditor";
            this.Size = new System.Drawing.Size(682, 416);
            ((System.ComponentModel.ISupportInitialize)(this.objectListViewLocations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripLocations;
        private BrightIdeasSoftware.ObjectListView objectListViewLocations;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
    }
}
