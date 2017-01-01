namespace Fluke900Link
{
    partial class ShowExceptionDialog
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
            this.listViewException = new System.Windows.Forms.ListView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewException
            // 
            this.listViewException.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewException.Dock = System.Windows.Forms.DockStyle.Top;
            this.listViewException.FullRowSelect = true;
            this.listViewException.Location = new System.Drawing.Point(0, 0);
            this.listViewException.Name = "listViewException";
            this.listViewException.Size = new System.Drawing.Size(674, 186);
            this.listViewException.TabIndex = 0;
            this.listViewException.UseCompatibleStateImageBehavior = false;
            this.listViewException.View = System.Windows.Forms.View.Details;
            this.listViewException.SelectedIndexChanged += new System.EventHandler(this.listViewException_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 186);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(674, 201);
            this.propertyGrid1.TabIndex = 1;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Date+Time";
            this.columnHeader1.Width = 152;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Message";
            this.columnHeader2.Width = 517;
            // 
            // ShowExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 387);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.listViewException);
            this.Name = "ShowExceptionDialog";
            this.Text = "Exception Log";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewException;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;

    }
}