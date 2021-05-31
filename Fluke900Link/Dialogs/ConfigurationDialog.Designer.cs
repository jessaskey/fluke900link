using Fluke900;

namespace Fluke900Link
{
    partial class ConfigurationDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationDialog));
            this.tabControlConfiguration = new System.Windows.Forms.TabControl();
            this.tabPageCommunication = new System.Windows.Forms.TabPage();
            this.labelInstrutionsStraightThrough = new System.Windows.Forms.Label();
            this.pictureBoxCableNullModem = new System.Windows.Forms.PictureBox();
            this.labelInstructionsNullModem = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxSerialCableType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxStopBits = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDataBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxParity = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxCableStraightThrough = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDownTabsToSpaces = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxAutoCopyDocuments = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoCopyTemplates = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoCopyExamples = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxRememberToolWindows = new System.Windows.Forms.CheckBox();
            this.checkBoxRememberDocuments = new System.Windows.Forms.CheckBox();
            this.buttonBroseLibloadDirectory = new System.Windows.Forms.Button();
            this.textBoxLibloadDirectory = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBoxSyncDateTime = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoConnect = new System.Windows.Forms.CheckBox();
            this.buttonBrowseFilesDirectory = new System.Windows.Forms.Button();
            this.textBoxFilesDirectory = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPageProject = new System.Windows.Forms.TabPage();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.projectFilePreferencesControl = new Fluke900Link.Controls.ProjectFilePreferencesControl();
            this.tabControlConfiguration.SuspendLayout();
            this.tabPageCommunication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCableNullModem)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCableStraightThrough)).BeginInit();
            this.tabPageGeneral.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabsToSpaces)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlConfiguration
            // 
            this.tabControlConfiguration.Controls.Add(this.tabPageCommunication);
            this.tabControlConfiguration.Controls.Add(this.tabPageGeneral);
            this.tabControlConfiguration.Controls.Add(this.tabPageProject);
            this.tabControlConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlConfiguration.Location = new System.Drawing.Point(0, 0);
            this.tabControlConfiguration.Name = "tabControlConfiguration";
            this.tabControlConfiguration.SelectedIndex = 0;
            this.tabControlConfiguration.Size = new System.Drawing.Size(669, 411);
            this.tabControlConfiguration.TabIndex = 0;
            // 
            // tabPageCommunication
            // 
            this.tabPageCommunication.Controls.Add(this.labelInstrutionsStraightThrough);
            this.tabPageCommunication.Controls.Add(this.pictureBoxCableNullModem);
            this.tabPageCommunication.Controls.Add(this.labelInstructionsNullModem);
            this.tabPageCommunication.Controls.Add(this.groupBox1);
            this.tabPageCommunication.Controls.Add(this.pictureBoxCableStraightThrough);
            this.tabPageCommunication.Controls.Add(this.label1);
            this.tabPageCommunication.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommunication.Name = "tabPageCommunication";
            this.tabPageCommunication.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommunication.Size = new System.Drawing.Size(661, 385);
            this.tabPageCommunication.TabIndex = 0;
            this.tabPageCommunication.Text = "Communication";
            this.tabPageCommunication.UseVisualStyleBackColor = true;
            // 
            // labelInstrutionsStraightThrough
            // 
            this.labelInstrutionsStraightThrough.Location = new System.Drawing.Point(267, 235);
            this.labelInstrutionsStraightThrough.Name = "labelInstrutionsStraightThrough";
            this.labelInstrutionsStraightThrough.Size = new System.Drawing.Size(365, 130);
            this.labelInstrutionsStraightThrough.TabIndex = 6;
            this.labelInstrutionsStraightThrough.Text = resources.GetString("labelInstrutionsStraightThrough.Text");
            // 
            // pictureBoxCableNullModem
            // 
            this.pictureBoxCableNullModem.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCableNullModem.Image")));
            this.pictureBoxCableNullModem.Location = new System.Drawing.Point(241, 31);
            this.pictureBoxCableNullModem.Name = "pictureBoxCableNullModem";
            this.pictureBoxCableNullModem.Size = new System.Drawing.Size(414, 186);
            this.pictureBoxCableNullModem.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCableNullModem.TabIndex = 5;
            this.pictureBoxCableNullModem.TabStop = false;
            // 
            // labelInstructionsNullModem
            // 
            this.labelInstructionsNullModem.AutoSize = true;
            this.labelInstructionsNullModem.Location = new System.Drawing.Point(267, 235);
            this.labelInstructionsNullModem.Name = "labelInstructionsNullModem";
            this.labelInstructionsNullModem.Size = new System.Drawing.Size(365, 130);
            this.labelInstructionsNullModem.TabIndex = 4;
            this.labelInstructionsNullModem.Text = resources.GetString("labelInstructionsNullModem.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxSerialCableType);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.comboBoxStopBits);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.comboBoxDataBits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBoxParity);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.comboBoxBaudRate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxPorts);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(8, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 257);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Port Settings:";
            // 
            // comboBoxSerialCableType
            // 
            this.comboBoxSerialCableType.FormattingEnabled = true;
            this.comboBoxSerialCableType.Items.AddRange(new object[] {
            "StraightThrough",
            "NullModem"});
            this.comboBoxSerialCableType.Location = new System.Drawing.Point(80, 26);
            this.comboBoxSerialCableType.Name = "comboBoxSerialCableType";
            this.comboBoxSerialCableType.Size = new System.Drawing.Size(130, 21);
            this.comboBoxSerialCableType.TabIndex = 13;
            this.comboBoxSerialCableType.SelectedIndexChanged += new System.EventHandler(this.comboBoxSerialCableType_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(64, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Cable Type:";
            // 
            // comboBoxStopBits
            // 
            this.comboBoxStopBits.FormattingEnabled = true;
            this.comboBoxStopBits.Location = new System.Drawing.Point(80, 201);
            this.comboBoxStopBits.Name = "comboBoxStopBits";
            this.comboBoxStopBits.Size = new System.Drawing.Size(130, 21);
            this.comboBoxStopBits.TabIndex = 11;
            this.comboBoxStopBits.SelectedIndexChanged += new System.EventHandler(this.comboBoxStopBits_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Stop Bits:";
            // 
            // comboBoxDataBits
            // 
            this.comboBoxDataBits.FormattingEnabled = true;
            this.comboBoxDataBits.Location = new System.Drawing.Point(80, 165);
            this.comboBoxDataBits.Name = "comboBoxDataBits";
            this.comboBoxDataBits.Size = new System.Drawing.Size(130, 21);
            this.comboBoxDataBits.TabIndex = 9;
            this.comboBoxDataBits.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataBits_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 168);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Data Bits:";
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.FormattingEnabled = true;
            this.comboBoxParity.Location = new System.Drawing.Point(80, 129);
            this.comboBoxParity.Name = "comboBoxParity";
            this.comboBoxParity.Size = new System.Drawing.Size(130, 21);
            this.comboBoxParity.TabIndex = 7;
            this.comboBoxParity.SelectedIndexChanged += new System.EventHandler(this.comboBoxParity_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Parity:";
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Location = new System.Drawing.Point(80, 94);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(130, 21);
            this.comboBoxBaudRate.TabIndex = 5;
            this.comboBoxBaudRate.SelectedIndexChanged += new System.EventHandler(this.comboBoxBaudRate_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Baud Rate:";
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(80, 61);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(130, 21);
            this.comboBoxPorts.TabIndex = 3;
            this.comboBoxPorts.SelectedIndexChanged += new System.EventHandler(this.comboBoxPorts_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // pictureBoxCableStraightThrough
            // 
            this.pictureBoxCableStraightThrough.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCableStraightThrough.Image")));
            this.pictureBoxCableStraightThrough.Location = new System.Drawing.Point(241, 31);
            this.pictureBoxCableStraightThrough.Name = "pictureBoxCableStraightThrough";
            this.pictureBoxCableStraightThrough.Size = new System.Drawing.Size(414, 186);
            this.pictureBoxCableStraightThrough.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxCableStraightThrough.TabIndex = 1;
            this.pictureBoxCableStraightThrough.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(364, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cable Details and Instructions:";
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.groupBox5);
            this.tabPageGeneral.Controls.Add(this.groupBox4);
            this.tabPageGeneral.Controls.Add(this.groupBox2);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(661, 385);
            this.tabPageGeneral.TabIndex = 1;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.numericUpDownTabsToSpaces);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(11, 254);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(647, 80);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "File Editor Options";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(382, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(220, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "(Fluke 900 does not support Tab characters.)";
            // 
            // numericUpDownTabsToSpaces
            // 
            this.numericUpDownTabsToSpaces.Location = new System.Drawing.Point(310, 24);
            this.numericUpDownTabsToSpaces.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownTabsToSpaces.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTabsToSpaces.Name = "numericUpDownTabsToSpaces";
            this.numericUpDownTabsToSpaces.Size = new System.Drawing.Size(55, 20);
            this.numericUpDownTabsToSpaces.TabIndex = 1;
            this.numericUpDownTabsToSpaces.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(284, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Tab Characters will be converted to this number of spaces:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxAutoCopyDocuments);
            this.groupBox4.Controls.Add(this.checkBoxAutoCopyTemplates);
            this.groupBox4.Controls.Add(this.checkBoxAutoCopyExamples);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(11, 173);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(642, 75);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Templates/Documents/Examples";
            // 
            // checkBoxAutoCopyDocuments
            // 
            this.checkBoxAutoCopyDocuments.AutoSize = true;
            this.checkBoxAutoCopyDocuments.Location = new System.Drawing.Point(150, 52);
            this.checkBoxAutoCopyDocuments.Name = "checkBoxAutoCopyDocuments";
            this.checkBoxAutoCopyDocuments.Size = new System.Drawing.Size(130, 17);
            this.checkBoxAutoCopyDocuments.TabIndex = 0;
            this.checkBoxAutoCopyDocuments.Text = "Fluke 900 Documents";
            this.checkBoxAutoCopyDocuments.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoCopyTemplates
            // 
            this.checkBoxAutoCopyTemplates.AutoSize = true;
            this.checkBoxAutoCopyTemplates.Location = new System.Drawing.Point(331, 52);
            this.checkBoxAutoCopyTemplates.Name = "checkBoxAutoCopyTemplates";
            this.checkBoxAutoCopyTemplates.Size = new System.Drawing.Size(94, 17);
            this.checkBoxAutoCopyTemplates.TabIndex = 1;
            this.checkBoxAutoCopyTemplates.Text = "Template Files";
            this.checkBoxAutoCopyTemplates.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoCopyExamples
            // 
            this.checkBoxAutoCopyExamples.AutoSize = true;
            this.checkBoxAutoCopyExamples.Location = new System.Drawing.Point(474, 52);
            this.checkBoxAutoCopyExamples.Name = "checkBoxAutoCopyExamples";
            this.checkBoxAutoCopyExamples.Size = new System.Drawing.Size(71, 17);
            this.checkBoxAutoCopyExamples.TabIndex = 2;
            this.checkBoxAutoCopyExamples.Text = "Examples";
            this.checkBoxAutoCopyExamples.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(20, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(612, 34);
            this.label11.TabIndex = 3;
            this.label11.Text = "If these are checked, then the associated directories and files will automaticall" +
    "y be copied to the Fluke900 Working directory on application startup.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxRememberToolWindows);
            this.groupBox2.Controls.Add(this.checkBoxRememberDocuments);
            this.groupBox2.Controls.Add(this.buttonBroseLibloadDirectory);
            this.groupBox2.Controls.Add(this.textBoxLibloadDirectory);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.checkBoxSyncDateTime);
            this.groupBox2.Controls.Add(this.checkBoxAutoConnect);
            this.groupBox2.Controls.Add(this.buttonBrowseFilesDirectory);
            this.groupBox2.Controls.Add(this.textBoxFilesDirectory);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(11, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(642, 161);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Startup";
            // 
            // checkBoxRememberToolWindows
            // 
            this.checkBoxRememberToolWindows.AutoSize = true;
            this.checkBoxRememberToolWindows.Location = new System.Drawing.Point(331, 121);
            this.checkBoxRememberToolWindows.Name = "checkBoxRememberToolWindows";
            this.checkBoxRememberToolWindows.Size = new System.Drawing.Size(177, 17);
            this.checkBoxRememberToolWindows.TabIndex = 10;
            this.checkBoxRememberToolWindows.Text = "Remember Open Tool Windows";
            this.checkBoxRememberToolWindows.UseVisualStyleBackColor = true;
            // 
            // checkBoxRememberDocuments
            // 
            this.checkBoxRememberDocuments.AutoSize = true;
            this.checkBoxRememberDocuments.Location = new System.Drawing.Point(331, 98);
            this.checkBoxRememberDocuments.Name = "checkBoxRememberDocuments";
            this.checkBoxRememberDocuments.Size = new System.Drawing.Size(163, 17);
            this.checkBoxRememberDocuments.TabIndex = 9;
            this.checkBoxRememberDocuments.Text = "Remember Open Documents";
            this.checkBoxRememberDocuments.UseVisualStyleBackColor = true;
            // 
            // buttonBroseLibloadDirectory
            // 
            this.buttonBroseLibloadDirectory.Image = ((System.Drawing.Image)(resources.GetObject("buttonBroseLibloadDirectory.Image")));
            this.buttonBroseLibloadDirectory.Location = new System.Drawing.Point(604, 51);
            this.buttonBroseLibloadDirectory.Name = "buttonBroseLibloadDirectory";
            this.buttonBroseLibloadDirectory.Size = new System.Drawing.Size(28, 23);
            this.buttonBroseLibloadDirectory.TabIndex = 8;
            this.buttonBroseLibloadDirectory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonBroseLibloadDirectory.UseVisualStyleBackColor = true;
            this.buttonBroseLibloadDirectory.Click += new System.EventHandler(this.buttonBroseLibloadDirectory_Click);
            // 
            // textBoxLibloadDirectory
            // 
            this.textBoxLibloadDirectory.Location = new System.Drawing.Point(128, 53);
            this.textBoxLibloadDirectory.Name = "textBoxLibloadDirectory";
            this.textBoxLibloadDirectory.Size = new System.Drawing.Size(470, 20);
            this.textBoxLibloadDirectory.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Libload Files Directory:";
            // 
            // checkBoxSyncDateTime
            // 
            this.checkBoxSyncDateTime.AutoSize = true;
            this.checkBoxSyncDateTime.Location = new System.Drawing.Point(20, 121);
            this.checkBoxSyncDateTime.Name = "checkBoxSyncDateTime";
            this.checkBoxSyncDateTime.Size = new System.Drawing.Size(260, 17);
            this.checkBoxSyncDateTime.TabIndex = 5;
            this.checkBoxSyncDateTime.Text = "Automatically Sync Date and Time Upon Connect";
            this.checkBoxSyncDateTime.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoConnect
            // 
            this.checkBoxAutoConnect.AutoSize = true;
            this.checkBoxAutoConnect.Location = new System.Drawing.Point(20, 98);
            this.checkBoxAutoConnect.Name = "checkBoxAutoConnect";
            this.checkBoxAutoConnect.Size = new System.Drawing.Size(177, 17);
            this.checkBoxAutoConnect.TabIndex = 4;
            this.checkBoxAutoConnect.Text = "Auto Connect on Program Open";
            this.checkBoxAutoConnect.UseVisualStyleBackColor = true;
            // 
            // buttonBrowseFilesDirectory
            // 
            this.buttonBrowseFilesDirectory.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrowseFilesDirectory.Image")));
            this.buttonBrowseFilesDirectory.Location = new System.Drawing.Point(604, 24);
            this.buttonBrowseFilesDirectory.Name = "buttonBrowseFilesDirectory";
            this.buttonBrowseFilesDirectory.Size = new System.Drawing.Size(28, 23);
            this.buttonBrowseFilesDirectory.TabIndex = 3;
            this.buttonBrowseFilesDirectory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonBrowseFilesDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseFilesDirectory.Click += new System.EventHandler(this.buttonBrowseFilesDirectory_Click);
            // 
            // textBoxFilesDirectory
            // 
            this.textBoxFilesDirectory.Location = new System.Drawing.Point(128, 26);
            this.textBoxFilesDirectory.Name = "textBoxFilesDirectory";
            this.textBoxFilesDirectory.Size = new System.Drawing.Size(470, 20);
            this.textBoxFilesDirectory.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Local Files Directory:";
            // 
            // tabPageProject
            // 
            this.tabPageProject.Controls.Add(this.projectFilePreferencesControl);
            this.tabPageProject.Location = new System.Drawing.Point(4, 22);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Size = new System.Drawing.Size(661, 385);
            this.tabPageProject.TabIndex = 2;
            this.tabPageProject.Text = "Project Defaults";
            this.tabPageProject.UseVisualStyleBackColor = true;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.label7);
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.buttonOK);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 411);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(669, 44);
            this.panelBottom.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(283, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "* Port Configuration Changes will apply on next re-connect.";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(485, 9);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(582, 9);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // projectFilePreferencesControl
            // 
            this.projectFilePreferencesControl.Location = new System.Drawing.Point(8, 12);
            this.projectFilePreferencesControl.Name = "projectFilePreferencesControl";
            this.projectFilePreferencesControl.ProjectFileCopyBehavior = FileLocationCopyBehavior.System;
            this.projectFilePreferencesControl.Size = new System.Drawing.Size(648, 84);
            this.projectFilePreferencesControl.TabIndex = 0;
            // 
            // ConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 455);
            this.ControlBox = false;
            this.Controls.Add(this.tabControlConfiguration);
            this.Controls.Add(this.panelBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Configuration";
            this.tabControlConfiguration.ResumeLayout(false);
            this.tabPageCommunication.ResumeLayout(false);
            this.tabPageCommunication.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCableNullModem)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCableStraightThrough)).EndInit();
            this.tabPageGeneral.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTabsToSpaces)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPageProject.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlConfiguration;
        private System.Windows.Forms.TabPage tabPageCommunication;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxParity;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxDataBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxStopBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxAutoConnect;
        private System.Windows.Forms.Button buttonBrowseFilesDirectory;
        private System.Windows.Forms.TextBox textBoxFilesDirectory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxSyncDateTime;
        private System.Windows.Forms.Button buttonBroseLibloadDirectory;
        private System.Windows.Forms.TextBox textBoxLibloadDirectory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelInstructionsNullModem;
        private System.Windows.Forms.CheckBox checkBoxRememberToolWindows;
        private System.Windows.Forms.CheckBox checkBoxRememberDocuments;
        private System.Windows.Forms.TabPage tabPageProject;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox checkBoxAutoCopyDocuments;
        private System.Windows.Forms.CheckBox checkBoxAutoCopyTemplates;
        private System.Windows.Forms.CheckBox checkBoxAutoCopyExamples;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.PictureBox pictureBoxCableNullModem;
        private System.Windows.Forms.ComboBox comboBoxSerialCableType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBoxCableStraightThrough;
        private System.Windows.Forms.Label labelInstrutionsStraightThrough;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDownTabsToSpaces;
        private System.Windows.Forms.Label label10;
        private Controls.ProjectFilePreferencesControl projectFilePreferencesControl;
    }
}