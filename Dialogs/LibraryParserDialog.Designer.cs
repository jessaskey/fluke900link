namespace Fluke900Link.Dialogs
{
    partial class LibraryParserDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryParserDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.buttonSaveBinary = new System.Windows.Forms.Button();
            this.buttonSaveFileTextLib = new System.Windows.Forms.Button();
            this.buttonPrintUsedEnums = new System.Windows.Forms.Button();
            this.buttonSaveSimPointers = new System.Windows.Forms.Button();
            this.buttonSaveBinaryFileResults = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewBinaryFileLoadResults = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonLoadBinaryFile = new System.Windows.Forms.Button();
            this.buttonBrowseLibraryBinaryFile = new System.Windows.Forms.Button();
            this.textBoxLibraryFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.linkLabelUpdateBinarySize = new System.Windows.Forms.LinkLabel();
            this.labelBinarySize = new System.Windows.Forms.Label();
            this.buttonCreateASCIILibrary = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listViewDevices = new System.Windows.Forms.ListView();
            this.buttonCreateBinaryLibrary = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonConvertASCIIToBinary = new System.Windows.Forms.Button();
            this.buttonOpenList = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(752, 410);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.buttonSaveBinary);
            this.tabPage1.Controls.Add(this.buttonSaveFileTextLib);
            this.tabPage1.Controls.Add(this.buttonPrintUsedEnums);
            this.tabPage1.Controls.Add(this.buttonSaveSimPointers);
            this.tabPage1.Controls.Add(this.buttonSaveBinaryFileResults);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.listViewBinaryFileLoadResults);
            this.tabPage1.Controls.Add(this.buttonLoadBinaryFile);
            this.tabPage1.Controls.Add(this.buttonBrowseLibraryBinaryFile);
            this.tabPage1.Controls.Add(this.textBoxLibraryFile);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(744, 384);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Reader";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // buttonSaveBinary
            // 
            this.buttonSaveBinary.Location = new System.Drawing.Point(605, 349);
            this.buttonSaveBinary.Name = "buttonSaveBinary";
            this.buttonSaveBinary.Size = new System.Drawing.Size(118, 23);
            this.buttonSaveBinary.TabIndex = 14;
            this.buttonSaveBinary.Text = "Save Binary";
            this.buttonSaveBinary.UseVisualStyleBackColor = true;
            this.buttonSaveBinary.Click += new System.EventHandler(this.buttonSaveBinary_Click);
            // 
            // buttonSaveFileTextLib
            // 
            this.buttonSaveFileTextLib.Location = new System.Drawing.Point(358, 349);
            this.buttonSaveFileTextLib.Name = "buttonSaveFileTextLib";
            this.buttonSaveFileTextLib.Size = new System.Drawing.Size(118, 23);
            this.buttonSaveFileTextLib.TabIndex = 13;
            this.buttonSaveFileTextLib.Text = "Save Text LIB";
            this.buttonSaveFileTextLib.UseVisualStyleBackColor = true;
            this.buttonSaveFileTextLib.Click += new System.EventHandler(this.buttonSaveFileTextLib_Click);
            // 
            // buttonPrintUsedEnums
            // 
            this.buttonPrintUsedEnums.Location = new System.Drawing.Point(11, 349);
            this.buttonPrintUsedEnums.Name = "buttonPrintUsedEnums";
            this.buttonPrintUsedEnums.Size = new System.Drawing.Size(116, 23);
            this.buttonPrintUsedEnums.TabIndex = 11;
            this.buttonPrintUsedEnums.Text = "Print Used Enums";
            this.buttonPrintUsedEnums.UseVisualStyleBackColor = true;
            this.buttonPrintUsedEnums.Click += new System.EventHandler(this.buttonPrintUsedEnums_Click);
            // 
            // buttonSaveSimPointers
            // 
            this.buttonSaveSimPointers.Location = new System.Drawing.Point(236, 349);
            this.buttonSaveSimPointers.Name = "buttonSaveSimPointers";
            this.buttonSaveSimPointers.Size = new System.Drawing.Size(116, 23);
            this.buttonSaveSimPointers.TabIndex = 10;
            this.buttonSaveSimPointers.Text = "Save Sim Pointers";
            this.buttonSaveSimPointers.UseVisualStyleBackColor = true;
            this.buttonSaveSimPointers.Click += new System.EventHandler(this.buttonSaveSimPointers_Click);
            // 
            // buttonSaveBinaryFileResults
            // 
            this.buttonSaveBinaryFileResults.Location = new System.Drawing.Point(482, 349);
            this.buttonSaveBinaryFileResults.Name = "buttonSaveBinaryFileResults";
            this.buttonSaveBinaryFileResults.Size = new System.Drawing.Size(118, 23);
            this.buttonSaveBinaryFileResults.TabIndex = 9;
            this.buttonSaveBinaryFileResults.Text = "Save XML";
            this.buttonSaveBinaryFileResults.UseVisualStyleBackColor = true;
            this.buttonSaveBinaryFileResults.Click += new System.EventHandler(this.buttonSaveBinaryFileResults_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Load Results:";
            // 
            // listViewBinaryFileLoadResults
            // 
            this.listViewBinaryFileLoadResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listViewBinaryFileLoadResults.FullRowSelect = true;
            this.listViewBinaryFileLoadResults.Location = new System.Drawing.Point(11, 89);
            this.listViewBinaryFileLoadResults.Name = "listViewBinaryFileLoadResults";
            this.listViewBinaryFileLoadResults.Size = new System.Drawing.Size(713, 247);
            this.listViewBinaryFileLoadResults.TabIndex = 7;
            this.listViewBinaryFileLoadResults.UseCompatibleStateImageBehavior = false;
            this.listViewBinaryFileLoadResults.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Message";
            this.columnHeader1.Width = 500;
            // 
            // buttonLoadBinaryFile
            // 
            this.buttonLoadBinaryFile.Location = new System.Drawing.Point(618, 56);
            this.buttonLoadBinaryFile.Name = "buttonLoadBinaryFile";
            this.buttonLoadBinaryFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadBinaryFile.TabIndex = 6;
            this.buttonLoadBinaryFile.Text = "Load File";
            this.buttonLoadBinaryFile.UseVisualStyleBackColor = true;
            this.buttonLoadBinaryFile.Click += new System.EventHandler(this.buttonLoadBinaryFile_Click);
            // 
            // buttonBrowseLibraryBinaryFile
            // 
            this.buttonBrowseLibraryBinaryFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseLibraryBinaryFile.Image")));
            this.buttonBrowseLibraryBinaryFile.Location = new System.Drawing.Point(697, 25);
            this.buttonBrowseLibraryBinaryFile.Name = "buttonBrowseLibraryBinaryFile";
            this.buttonBrowseLibraryBinaryFile.Size = new System.Drawing.Size(27, 20);
            this.buttonBrowseLibraryBinaryFile.TabIndex = 5;
            this.buttonBrowseLibraryBinaryFile.UseVisualStyleBackColor = true;
            this.buttonBrowseLibraryBinaryFile.Click += new System.EventHandler(this.buttonBrowseLibraryBinaryFile_Click);
            // 
            // textBoxLibraryFile
            // 
            this.textBoxLibraryFile.Location = new System.Drawing.Point(115, 25);
            this.textBoxLibraryFile.Name = "textBoxLibraryFile";
            this.textBoxLibraryFile.Size = new System.Drawing.Size(578, 20);
            this.textBoxLibraryFile.TabIndex = 4;
            this.textBoxLibraryFile.Text = "Z:\\Files\\TestEquipment\\Fluke 900\\Fluke900av6firmware\\900V6U75";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Library Binary File:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonOpenList);
            this.tabPage2.Controls.Add(this.linkLabelUpdateBinarySize);
            this.tabPage2.Controls.Add(this.labelBinarySize);
            this.tabPage2.Controls.Add(this.buttonCreateASCIILibrary);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBoxFilter);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.listViewDevices);
            this.tabPage2.Controls.Add(this.buttonCreateBinaryLibrary);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(744, 384);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Writer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // linkLabelUpdateBinarySize
            // 
            this.linkLabelUpdateBinarySize.AutoSize = true;
            this.linkLabelUpdateBinarySize.Location = new System.Drawing.Point(216, 357);
            this.linkLabelUpdateBinarySize.Name = "linkLabelUpdateBinarySize";
            this.linkLabelUpdateBinarySize.Size = new System.Drawing.Size(40, 13);
            this.linkLabelUpdateBinarySize.TabIndex = 12;
            this.linkLabelUpdateBinarySize.TabStop = true;
            this.linkLabelUpdateBinarySize.Text = "update";
            this.linkLabelUpdateBinarySize.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUpdateBinarySize_LinkClicked);
            // 
            // labelBinarySize
            // 
            this.labelBinarySize.AutoSize = true;
            this.labelBinarySize.Location = new System.Drawing.Point(17, 358);
            this.labelBinarySize.Name = "labelBinarySize";
            this.labelBinarySize.Size = new System.Drawing.Size(138, 13);
            this.labelBinarySize.TabIndex = 11;
            this.labelBinarySize.Text = "Selected Items Binary Size: ";
            // 
            // buttonCreateASCIILibrary
            // 
            this.buttonCreateASCIILibrary.Location = new System.Drawing.Point(466, 358);
            this.buttonCreateASCIILibrary.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateASCIILibrary.Name = "buttonCreateASCIILibrary";
            this.buttonCreateASCIILibrary.Size = new System.Drawing.Size(131, 23);
            this.buttonCreateASCIILibrary.TabIndex = 9;
            this.buttonCreateASCIILibrary.Text = "Create ASCII Library";
            this.buttonCreateASCIILibrary.UseVisualStyleBackColor = true;
            this.buttonCreateASCIILibrary.Click += new System.EventHandler(this.buttonCreateASCIILibrary_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(446, 19);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Filter:";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(483, 16);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(252, 20);
            this.textBoxFilter.TabIndex = 7;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Available Devices:";
            // 
            // listViewDevices
            // 
            this.listViewDevices.CheckBoxes = true;
            this.listViewDevices.Location = new System.Drawing.Point(20, 43);
            this.listViewDevices.Margin = new System.Windows.Forms.Padding(2);
            this.listViewDevices.Name = "listViewDevices";
            this.listViewDevices.Size = new System.Drawing.Size(715, 298);
            this.listViewDevices.TabIndex = 5;
            this.listViewDevices.UseCompatibleStateImageBehavior = false;
            this.listViewDevices.View = System.Windows.Forms.View.List;
            // 
            // buttonCreateBinaryLibrary
            // 
            this.buttonCreateBinaryLibrary.Location = new System.Drawing.Point(602, 358);
            this.buttonCreateBinaryLibrary.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateBinaryLibrary.Name = "buttonCreateBinaryLibrary";
            this.buttonCreateBinaryLibrary.Size = new System.Drawing.Size(131, 23);
            this.buttonCreateBinaryLibrary.TabIndex = 4;
            this.buttonCreateBinaryLibrary.Text = "Create Binary Library";
            this.buttonCreateBinaryLibrary.UseVisualStyleBackColor = true;
            this.buttonCreateBinaryLibrary.Click += new System.EventHandler(this.buttonCreateBinaryLibrary_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.buttonConvertASCIIToBinary);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(744, 384);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tools";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(236, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(461, 34);
            this.label5.TabIndex = 12;
            this.label5.Text = "Browse to a Binary file and this will convert it into ASCII format which is suita" +
    "ble for RS-232 sending to the Fluke 900.";
            // 
            // buttonConvertASCIIToBinary
            // 
            this.buttonConvertASCIIToBinary.Location = new System.Drawing.Point(39, 47);
            this.buttonConvertASCIIToBinary.Margin = new System.Windows.Forms.Padding(2);
            this.buttonConvertASCIIToBinary.Name = "buttonConvertASCIIToBinary";
            this.buttonConvertASCIIToBinary.Size = new System.Drawing.Size(170, 19);
            this.buttonConvertASCIIToBinary.TabIndex = 11;
            this.buttonConvertASCIIToBinary.Text = "Convert ASCII to Binary";
            this.buttonConvertASCIIToBinary.UseVisualStyleBackColor = true;
            this.buttonConvertASCIIToBinary.Click += new System.EventHandler(this.buttonConvertASCIIToBinary_Click);
            // 
            // buttonOpenList
            // 
            this.buttonOpenList.Location = new System.Drawing.Point(131, 15);
            this.buttonOpenList.Name = "buttonOpenList";
            this.buttonOpenList.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenList.TabIndex = 13;
            this.buttonOpenList.Text = "Open List";
            this.buttonOpenList.UseVisualStyleBackColor = true;
            this.buttonOpenList.Click += new System.EventHandler(this.buttonOpenList_Click);
            // 
            // LibraryParserDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 410);
            this.Controls.Add(this.tabControl1);
            this.Name = "LibraryParserDialog";
            this.Text = "Fluke 900 Library Parser Tool";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button buttonBrowseLibraryBinaryFile;
        private System.Windows.Forms.TextBox textBoxLibraryFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewBinaryFileLoadResults;
        private System.Windows.Forms.Button buttonLoadBinaryFile;
        private System.Windows.Forms.Button buttonSaveBinaryFileResults;
        private System.Windows.Forms.Button buttonSaveSimPointers;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Button buttonPrintUsedEnums;
        private System.Windows.Forms.Button buttonSaveFileTextLib;
        private System.Windows.Forms.Button buttonSaveBinary;
        private System.Windows.Forms.Button buttonCreateBinaryLibrary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView listViewDevices;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonCreateASCIILibrary;
        private System.Windows.Forms.Label labelBinarySize;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonConvertASCIIToBinary;
        private System.Windows.Forms.LinkLabel linkLabelUpdateBinarySize;
        private System.Windows.Forms.Button buttonOpenList;
    }
}