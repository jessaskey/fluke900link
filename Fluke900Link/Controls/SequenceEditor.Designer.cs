namespace Fluke900Link.Controls
{
    partial class SequenceEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SequenceEditor));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageSequence = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewLocations = new System.Windows.Forms.TreeView();
            this.imageListTree = new System.Windows.Forms.ImageList(this.components);
            this.toolStripSequence = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNewLocation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDeleteLocation = new System.Windows.Forms.ToolStripButton();
            this.tabControlDevice = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonPinActivity = new System.Windows.Forms.Button();
            this.textBoxPinActivity = new System.Windows.Forms.TextBox();
            this.textBoxLocationName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxFloatCheck = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxClipCheck = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxChecksum = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxReferenceDeviceTest = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSimulation = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxReferenceDeviceDrive = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxGndPins = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxVccPins = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxICSize = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxICName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.panelDeviceSummary = new System.Windows.Forms.Panel();
            this.tabPageGlobals = new System.Windows.Forms.TabPage();
            this.tabControlMain.SuspendLayout();
            this.tabPageSequence.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripSequence.SuspendLayout();
            this.tabControlDevice.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(743, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageSequence);
            this.tabControlMain.Controls.Add(this.tabPageGlobals);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 25);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(743, 483);
            this.tabControlMain.TabIndex = 1;
            // 
            // tabPageSequence
            // 
            this.tabPageSequence.Controls.Add(this.splitContainer1);
            this.tabPageSequence.Location = new System.Drawing.Point(4, 22);
            this.tabPageSequence.Name = "tabPageSequence";
            this.tabPageSequence.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSequence.Size = new System.Drawing.Size(735, 457);
            this.tabPageSequence.TabIndex = 0;
            this.tabPageSequence.Text = "Sequence";
            this.tabPageSequence.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewLocations);
            this.splitContainer1.Panel1.Controls.Add(this.toolStripSequence);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControlDevice);
            this.splitContainer1.Panel2.Controls.Add(this.panelDeviceSummary);
            this.splitContainer1.Size = new System.Drawing.Size(729, 451);
            this.splitContainer1.SplitterDistance = 243;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeViewLocations
            // 
            this.treeViewLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLocations.ImageIndex = 0;
            this.treeViewLocations.ImageList = this.imageListTree;
            this.treeViewLocations.Location = new System.Drawing.Point(0, 25);
            this.treeViewLocations.Name = "treeViewLocations";
            this.treeViewLocations.SelectedImageIndex = 0;
            this.treeViewLocations.Size = new System.Drawing.Size(243, 426);
            this.treeViewLocations.TabIndex = 1;
            this.treeViewLocations.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewLocations_ItemDrag);
            this.treeViewLocations.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLocations_AfterSelect);
            this.treeViewLocations.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeViewLocations_DragDrop);
            this.treeViewLocations.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeViewLocations_DragEnter);
            // 
            // imageListTree
            // 
            this.imageListTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTree.ImageStream")));
            this.imageListTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTree.Images.SetKeyName(0, "ShowDiagramPane_280.png");
            this.imageListTree.Images.SetKeyName(1, "MemoryWindow_6537.png");
            // 
            // toolStripSequence
            // 
            this.toolStripSequence.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNewLocation,
            this.toolStripButtonDeleteLocation});
            this.toolStripSequence.Location = new System.Drawing.Point(0, 0);
            this.toolStripSequence.Name = "toolStripSequence";
            this.toolStripSequence.Size = new System.Drawing.Size(243, 25);
            this.toolStripSequence.TabIndex = 0;
            this.toolStripSequence.Text = "toolStrip1";
            // 
            // toolStripButtonNewLocation
            // 
            this.toolStripButtonNewLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNewLocation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNewLocation.Image")));
            this.toolStripButtonNewLocation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNewLocation.Name = "toolStripButtonNewLocation";
            this.toolStripButtonNewLocation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonNewLocation.Text = "Add Location";
            // 
            // toolStripButtonDeleteLocation
            // 
            this.toolStripButtonDeleteLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDeleteLocation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDeleteLocation.Image")));
            this.toolStripButtonDeleteLocation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDeleteLocation.Name = "toolStripButtonDeleteLocation";
            this.toolStripButtonDeleteLocation.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonDeleteLocation.Text = "Delete Location";
            // 
            // tabControlDevice
            // 
            this.tabControlDevice.Controls.Add(this.tabPage1);
            this.tabControlDevice.Controls.Add(this.tabPage2);
            this.tabControlDevice.Controls.Add(this.tabPage3);
            this.tabControlDevice.Controls.Add(this.tabPage4);
            this.tabControlDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDevice.Location = new System.Drawing.Point(0, 25);
            this.tabControlDevice.Name = "tabControlDevice";
            this.tabControlDevice.SelectedIndex = 0;
            this.tabControlDevice.Size = new System.Drawing.Size(482, 426);
            this.tabControlDevice.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonPinActivity);
            this.tabPage1.Controls.Add(this.textBoxPinActivity);
            this.tabPage1.Controls.Add(this.textBoxLocationName);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Controls.Add(this.comboBoxFloatCheck);
            this.tabPage1.Controls.Add(this.label11);
            this.tabPage1.Controls.Add(this.comboBoxClipCheck);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.textBoxChecksum);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.comboBoxReferenceDeviceTest);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.comboBoxSimulation);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.comboBoxReferenceDeviceDrive);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.textBoxGndPins);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.textBoxVccPins);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.comboBoxICSize);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxICName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(474, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Definition";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonPinActivity
            // 
            this.buttonPinActivity.Location = new System.Drawing.Point(396, 192);
            this.buttonPinActivity.Name = "buttonPinActivity";
            this.buttonPinActivity.Size = new System.Drawing.Size(25, 20);
            this.buttonPinActivity.TabIndex = 25;
            this.buttonPinActivity.Text = "...";
            this.buttonPinActivity.UseVisualStyleBackColor = true;
            this.buttonPinActivity.Click += new System.EventHandler(this.buttonPinActivity_Click);
            // 
            // textBoxPinActivity
            // 
            this.textBoxPinActivity.Location = new System.Drawing.Point(159, 192);
            this.textBoxPinActivity.Name = "textBoxPinActivity";
            this.textBoxPinActivity.ReadOnly = true;
            this.textBoxPinActivity.Size = new System.Drawing.Size(231, 20);
            this.textBoxPinActivity.TabIndex = 24;
            // 
            // textBoxLocationName
            // 
            this.textBoxLocationName.Location = new System.Drawing.Point(110, 18);
            this.textBoxLocationName.Name = "textBoxLocationName";
            this.textBoxLocationName.Size = new System.Drawing.Size(142, 20);
            this.textBoxLocationName.TabIndex = 23;
            this.textBoxLocationName.Leave += new System.EventHandler(this.textBoxLocationName_Leave);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Location Name:";
            // 
            // comboBoxFloatCheck
            // 
            this.comboBoxFloatCheck.FormattingEnabled = true;
            this.comboBoxFloatCheck.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.comboBoxFloatCheck.Location = new System.Drawing.Point(159, 343);
            this.comboBoxFloatCheck.Name = "comboBoxFloatCheck";
            this.comboBoxFloatCheck.Size = new System.Drawing.Size(116, 21);
            this.comboBoxFloatCheck.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(28, 346);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Float Check:";
            // 
            // comboBoxClipCheck
            // 
            this.comboBoxClipCheck.FormattingEnabled = true;
            this.comboBoxClipCheck.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.comboBoxClipCheck.Location = new System.Drawing.Point(159, 316);
            this.comboBoxClipCheck.Name = "comboBoxClipCheck";
            this.comboBoxClipCheck.Size = new System.Drawing.Size(116, 21);
            this.comboBoxClipCheck.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 319);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Clip Check:";
            // 
            // textBoxChecksum
            // 
            this.textBoxChecksum.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxChecksum.Enabled = false;
            this.textBoxChecksum.Location = new System.Drawing.Point(159, 290);
            this.textBoxChecksum.Name = "textBoxChecksum";
            this.textBoxChecksum.Size = new System.Drawing.Size(116, 20);
            this.textBoxChecksum.TabIndex = 17;
            this.textBoxChecksum.Text = "n/a";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(28, 293);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Checksum:";
            // 
            // comboBoxReferenceDeviceTest
            // 
            this.comboBoxReferenceDeviceTest.FormattingEnabled = true;
            this.comboBoxReferenceDeviceTest.Items.AddRange(new object[] {
            "Off",
            "On"});
            this.comboBoxReferenceDeviceTest.Location = new System.Drawing.Point(159, 263);
            this.comboBoxReferenceDeviceTest.Name = "comboBoxReferenceDeviceTest";
            this.comboBoxReferenceDeviceTest.Size = new System.Drawing.Size(116, 21);
            this.comboBoxReferenceDeviceTest.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 266);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Reference Device Test:";
            // 
            // comboBoxSimulation
            // 
            this.comboBoxSimulation.FormattingEnabled = true;
            this.comboBoxSimulation.Items.AddRange(new object[] {
            "Yes-Auto",
            "No",
            "Not Installed"});
            this.comboBoxSimulation.Location = new System.Drawing.Point(159, 219);
            this.comboBoxSimulation.Name = "comboBoxSimulation";
            this.comboBoxSimulation.Size = new System.Drawing.Size(116, 21);
            this.comboBoxSimulation.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 222);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Simulation:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Pin Activity:";
            // 
            // comboBoxReferenceDeviceDrive
            // 
            this.comboBoxReferenceDeviceDrive.FormattingEnabled = true;
            this.comboBoxReferenceDeviceDrive.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxReferenceDeviceDrive.Location = new System.Drawing.Point(159, 165);
            this.comboBoxReferenceDeviceDrive.Name = "comboBoxReferenceDeviceDrive";
            this.comboBoxReferenceDeviceDrive.Size = new System.Drawing.Size(116, 21);
            this.comboBoxReferenceDeviceDrive.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Reference Device Drive:";
            // 
            // textBoxGndPins
            // 
            this.textBoxGndPins.Location = new System.Drawing.Point(110, 123);
            this.textBoxGndPins.Name = "textBoxGndPins";
            this.textBoxGndPins.Size = new System.Drawing.Size(142, 20);
            this.textBoxGndPins.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Gnd Pins:";
            // 
            // textBoxVccPins
            // 
            this.textBoxVccPins.Location = new System.Drawing.Point(110, 97);
            this.textBoxVccPins.Name = "textBoxVccPins";
            this.textBoxVccPins.Size = new System.Drawing.Size(142, 20);
            this.textBoxVccPins.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vcc Pins:";
            // 
            // comboBoxICSize
            // 
            this.comboBoxICSize.FormattingEnabled = true;
            this.comboBoxICSize.Items.AddRange(new object[] {
            "4",
            "6",
            "8",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28"});
            this.comboBoxICSize.Location = new System.Drawing.Point(110, 70);
            this.comboBoxICSize.Name = "comboBoxICSize";
            this.comboBoxICSize.Size = new System.Drawing.Size(89, 21);
            this.comboBoxICSize.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IC Size:";
            // 
            // textBoxICName
            // 
            this.textBoxICName.Location = new System.Drawing.Point(110, 44);
            this.textBoxICName.Name = "textBoxICName";
            this.textBoxICName.Size = new System.Drawing.Size(142, 20);
            this.textBoxICName.TabIndex = 1;
            this.textBoxICName.Leave += new System.EventHandler(this.textBoxICName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Device Name:";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(474, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Initialization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(474, 400);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Performance Envelope";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(474, 400);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Results";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // panelDeviceSummary
            // 
            this.panelDeviceSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDeviceSummary.Location = new System.Drawing.Point(0, 0);
            this.panelDeviceSummary.Name = "panelDeviceSummary";
            this.panelDeviceSummary.Size = new System.Drawing.Size(482, 25);
            this.panelDeviceSummary.TabIndex = 0;
            // 
            // tabPageGlobals
            // 
            this.tabPageGlobals.Location = new System.Drawing.Point(4, 22);
            this.tabPageGlobals.Name = "tabPageGlobals";
            this.tabPageGlobals.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGlobals.Size = new System.Drawing.Size(735, 457);
            this.tabPageGlobals.TabIndex = 1;
            this.tabPageGlobals.Text = "Globals";
            this.tabPageGlobals.UseVisualStyleBackColor = true;
            // 
            // SequenceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.toolStripMain);
            this.Name = "SequenceEditor";
            this.Size = new System.Drawing.Size(743, 508);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageSequence.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripSequence.ResumeLayout(false);
            this.toolStripSequence.PerformLayout();
            this.tabControlDevice.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageSequence;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip toolStripSequence;
        private System.Windows.Forms.TabPage tabPageGlobals;
        private System.Windows.Forms.TabControl tabControlDevice;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Panel panelDeviceSummary;
        private System.Windows.Forms.TreeView treeViewLocations;
        private System.Windows.Forms.ImageList imageListTree;
        private System.Windows.Forms.ToolStripButton toolStripButtonNewLocation;
        private System.Windows.Forms.ToolStripButton toolStripButtonDeleteLocation;
        private System.Windows.Forms.TextBox textBoxICName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxICSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxGndPins;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxVccPins;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFloatCheck;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxClipCheck;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxChecksum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxReferenceDeviceTest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSimulation;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxReferenceDeviceDrive;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLocationName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button buttonPinActivity;
        private System.Windows.Forms.TextBox textBoxPinActivity;
    }
}
