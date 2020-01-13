namespace ControlPanel
{
    partial class ControlPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.keystrokesCheckbox = new System.Windows.Forms.CheckBox();
            this.machineCheckbox = new System.Windows.Forms.CheckBox();
            this.userCheckbox = new System.Windows.Forms.CheckBox();
            this.remDrivesCheckbox = new System.Windows.Forms.CheckBox();
            this.fsCheckbox = new System.Windows.Forms.CheckBox();
            this.printerCheckbox = new System.Windows.Forms.CheckBox();
            this.cryptoCheckbox = new System.Windows.Forms.CheckBox();
            this.antivirCheckbox = new System.Windows.Forms.CheckBox();
            this.screenshoterCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.intervalDomainUpDown = new System.Windows.Forms.DomainUpDown();
            this.salabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.adButton = new System.Windows.Forms.Button();
            this.clipboardCheckbox = new System.Windows.Forms.CheckBox();
            this.powerCheckebox = new System.Windows.Forms.CheckBox();
            this.dalabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.adlabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.daButton = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.controlPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorizareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkInstallationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(246, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Events to log:";
            // 
            // keystrokesCheckbox
            // 
            this.keystrokesCheckbox.AutoSize = true;
            this.keystrokesCheckbox.Location = new System.Drawing.Point(324, 112);
            this.keystrokesCheckbox.Name = "keystrokesCheckbox";
            this.keystrokesCheckbox.Size = new System.Drawing.Size(78, 17);
            this.keystrokesCheckbox.TabIndex = 1;
            this.keystrokesCheckbox.Text = "Keystrokes";
            this.keystrokesCheckbox.UseVisualStyleBackColor = true;
            // 
            // machineCheckbox
            // 
            this.machineCheckbox.AutoSize = true;
            this.machineCheckbox.Location = new System.Drawing.Point(324, 43);
            this.machineCheckbox.Name = "machineCheckbox";
            this.machineCheckbox.Size = new System.Drawing.Size(99, 17);
            this.machineCheckbox.TabIndex = 2;
            this.machineCheckbox.Text = "Machine on/off";
            this.machineCheckbox.UseVisualStyleBackColor = true;
            // 
            // userCheckbox
            // 
            this.userCheckbox.AutoSize = true;
            this.userCheckbox.Location = new System.Drawing.Point(324, 89);
            this.userCheckbox.Name = "userCheckbox";
            this.userCheckbox.Size = new System.Drawing.Size(116, 17);
            this.userCheckbox.TabIndex = 3;
            this.userCheckbox.Text = "User Logon/Logoff";
            this.userCheckbox.UseVisualStyleBackColor = true;
            // 
            // remDrivesCheckbox
            // 
            this.remDrivesCheckbox.AutoSize = true;
            this.remDrivesCheckbox.Location = new System.Drawing.Point(324, 135);
            this.remDrivesCheckbox.Name = "remDrivesCheckbox";
            this.remDrivesCheckbox.Size = new System.Drawing.Size(143, 17);
            this.remDrivesCheckbox.TabIndex = 4;
            this.remDrivesCheckbox.Text = "Removable drives usage";
            this.remDrivesCheckbox.UseVisualStyleBackColor = true;
            // 
            // fsCheckbox
            // 
            this.fsCheckbox.AutoSize = true;
            this.fsCheckbox.Location = new System.Drawing.Point(324, 158);
            this.fsCheckbox.Name = "fsCheckbox";
            this.fsCheckbox.Size = new System.Drawing.Size(124, 17);
            this.fsCheckbox.TabIndex = 5;
            this.fsCheckbox.Text = "File system operation";
            this.fsCheckbox.UseVisualStyleBackColor = true;
            // 
            // printerCheckbox
            // 
            this.printerCheckbox.AutoSize = true;
            this.printerCheckbox.Location = new System.Drawing.Point(324, 203);
            this.printerCheckbox.Name = "printerCheckbox";
            this.printerCheckbox.Size = new System.Drawing.Size(92, 17);
            this.printerCheckbox.TabIndex = 6;
            this.printerCheckbox.Text = "Printer activity";
            this.printerCheckbox.UseVisualStyleBackColor = true;
            // 
            // cryptoCheckbox
            // 
            this.cryptoCheckbox.AutoSize = true;
            this.cryptoCheckbox.Enabled = false;
            this.cryptoCheckbox.Location = new System.Drawing.Point(324, 226);
            this.cryptoCheckbox.Name = "cryptoCheckbox";
            this.cryptoCheckbox.Size = new System.Drawing.Size(92, 17);
            this.cryptoCheckbox.TabIndex = 7;
            this.cryptoCheckbox.Text = "Crypto activity";
            this.cryptoCheckbox.UseVisualStyleBackColor = true;
            // 
            // antivirCheckbox
            // 
            this.antivirCheckbox.AutoSize = true;
            this.antivirCheckbox.Enabled = false;
            this.antivirCheckbox.Location = new System.Drawing.Point(324, 249);
            this.antivirCheckbox.Name = "antivirCheckbox";
            this.antivirCheckbox.Size = new System.Drawing.Size(147, 17);
            this.antivirCheckbox.TabIndex = 8;
            this.antivirCheckbox.Text = "Antivirus definitions status";
            this.antivirCheckbox.UseVisualStyleBackColor = true;
            // 
            // screenshoterCheckBox
            // 
            this.screenshoterCheckBox.AutoSize = true;
            this.screenshoterCheckBox.Location = new System.Drawing.Point(249, 288);
            this.screenshoterCheckBox.Name = "screenshoterCheckBox";
            this.screenshoterCheckBox.Size = new System.Drawing.Size(139, 17);
            this.screenshoterCheckBox.TabIndex = 9;
            this.screenshoterCheckBox.Text = "Screenshoter enabled - ";
            this.screenshoterCheckBox.UseVisualStyleBackColor = true;
            this.screenshoterCheckBox.CheckedChanged += new System.EventHandler(this.screenshoterCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(425, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "(m) interval.";
            // 
            // intervalDomainUpDown
            // 
            this.intervalDomainUpDown.Enabled = false;
            this.intervalDomainUpDown.Items.Add("60");
            this.intervalDomainUpDown.Items.Add("55");
            this.intervalDomainUpDown.Items.Add("50");
            this.intervalDomainUpDown.Items.Add("45");
            this.intervalDomainUpDown.Items.Add("40");
            this.intervalDomainUpDown.Items.Add("35");
            this.intervalDomainUpDown.Items.Add("30");
            this.intervalDomainUpDown.Items.Add("25");
            this.intervalDomainUpDown.Items.Add("20");
            this.intervalDomainUpDown.Items.Add("15");
            this.intervalDomainUpDown.Items.Add("10");
            this.intervalDomainUpDown.Items.Add("5");
            this.intervalDomainUpDown.Items.Add("1");
            this.intervalDomainUpDown.Location = new System.Drawing.Point(383, 287);
            this.intervalDomainUpDown.Name = "intervalDomainUpDown";
            this.intervalDomainUpDown.ReadOnly = true;
            this.intervalDomainUpDown.Size = new System.Drawing.Size(40, 20);
            this.intervalDomainUpDown.TabIndex = 11;
            this.intervalDomainUpDown.Text = "domainUpDown1";
            // 
            // salabel
            // 
            this.salabel.AutoSize = true;
            this.salabel.Location = new System.Drawing.Point(10, 16);
            this.salabel.Name = "salabel";
            this.salabel.Size = new System.Drawing.Size(83, 13);
            this.salabel.TabIndex = 12;
            this.salabel.Text = "Status: Running";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(407, 321);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // adButton
            // 
            this.adButton.Location = new System.Drawing.Point(6, 32);
            this.adButton.Name = "adButton";
            this.adButton.Size = new System.Drawing.Size(93, 44);
            this.adButton.TabIndex = 15;
            this.adButton.Text = "Unlock";
            this.adButton.UseVisualStyleBackColor = true;
            this.adButton.Click += new System.EventHandler(this.unlockButton_Click);
            // 
            // clipboardCheckbox
            // 
            this.clipboardCheckbox.AutoSize = true;
            this.clipboardCheckbox.Location = new System.Drawing.Point(324, 180);
            this.clipboardCheckbox.Name = "clipboardCheckbox";
            this.clipboardCheckbox.Size = new System.Drawing.Size(102, 17);
            this.clipboardCheckbox.TabIndex = 16;
            this.clipboardCheckbox.Text = "Clipboard usage";
            this.clipboardCheckbox.UseVisualStyleBackColor = true;
            // 
            // powerCheckebox
            // 
            this.powerCheckebox.AutoSize = true;
            this.powerCheckebox.Location = new System.Drawing.Point(324, 66);
            this.powerCheckebox.Name = "powerCheckebox";
            this.powerCheckebox.Size = new System.Drawing.Size(83, 17);
            this.powerCheckebox.TabIndex = 17;
            this.powerCheckebox.Text = "Power suply";
            this.powerCheckebox.UseVisualStyleBackColor = true;
            // 
            // dalabel
            // 
            this.dalabel.AutoSize = true;
            this.dalabel.Location = new System.Drawing.Point(10, 16);
            this.dalabel.Name = "dalabel";
            this.dalabel.Size = new System.Drawing.Size(83, 13);
            this.dalabel.TabIndex = 18;
            this.dalabel.Text = "Status: Running";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.adlabel);
            this.groupBox1.Controls.Add(this.adButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 87);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application directory";
            // 
            // adlabel
            // 
            this.adlabel.AutoSize = true;
            this.adlabel.Location = new System.Drawing.Point(10, 16);
            this.adlabel.Name = "adlabel";
            this.adlabel.Size = new System.Drawing.Size(79, 13);
            this.adlabel.TabIndex = 20;
            this.adlabel.Text = "Status: Locked";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.salabel);
            this.groupBox2.Location = new System.Drawing.Point(12, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 41);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Service agent";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.daButton);
            this.groupBox3.Controls.Add(this.dalabel);
            this.groupBox3.Location = new System.Drawing.Point(12, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 95);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Desktop agent";
            // 
            // daButton
            // 
            this.daButton.Location = new System.Drawing.Point(6, 38);
            this.daButton.Name = "daButton";
            this.daButton.Size = new System.Drawing.Size(93, 44);
            this.daButton.TabIndex = 21;
            this.daButton.Text = "Stop";
            this.daButton.UseVisualStyleBackColor = true;
            this.daButton.Click += new System.EventHandler(this.daButton_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlPanelToolStripMenuItem,
            this.monitorizareToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(494, 24);
            this.menuStrip1.TabIndex = 23;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // controlPanelToolStripMenuItem
            // 
            this.controlPanelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSettingsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.controlPanelToolStripMenuItem.Name = "controlPanelToolStripMenuItem";
            this.controlPanelToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.controlPanelToolStripMenuItem.Text = "Control Panel";
            // 
            // saveSettingsToolStripMenuItem
            // 
            this.saveSettingsToolStripMenuItem.Name = "saveSettingsToolStripMenuItem";
            this.saveSettingsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveSettingsToolStripMenuItem.Text = "&Save settings";
            this.saveSettingsToolStripMenuItem.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // monitorizareToolStripMenuItem
            // 
            this.monitorizareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installToolStripMenuItem,
            this.uninstallToolStripMenuItem,
            this.checkInstallationToolStripMenuItem});
            this.monitorizareToolStripMenuItem.Name = "monitorizareToolStripMenuItem";
            this.monitorizareToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.monitorizareToolStripMenuItem.Text = "Agents";
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.installToolStripMenuItem.Text = "&Install";
            this.installToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.uninstallToolStripMenuItem.Text = "&Uninstall";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // checkInstallationToolStripMenuItem
            // 
            this.checkInstallationToolStripMenuItem.Name = "checkInstallationToolStripMenuItem";
            this.checkInstallationToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.checkInstallationToolStripMenuItem.Text = "&Check installation";
            this.checkInstallationToolStripMenuItem.Click += new System.EventHandler(this.checkInstallationToolStripMenuItem_Click);
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 369);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.powerCheckebox);
            this.Controls.Add(this.clipboardCheckbox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.intervalDomainUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.screenshoterCheckBox);
            this.Controls.Add(this.antivirCheckbox);
            this.Controls.Add(this.cryptoCheckbox);
            this.Controls.Add(this.printerCheckbox);
            this.Controls.Add(this.fsCheckbox);
            this.Controls.Add(this.remDrivesCheckbox);
            this.Controls.Add(this.userCheckbox);
            this.Controls.Add(this.machineCheckbox);
            this.Controls.Add(this.keystrokesCheckbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ControlPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Control Center";
            this.Load += new System.EventHandler(this.ControlPanel_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox keystrokesCheckbox;
        private System.Windows.Forms.CheckBox machineCheckbox;
        private System.Windows.Forms.CheckBox userCheckbox;
        private System.Windows.Forms.CheckBox remDrivesCheckbox;
        private System.Windows.Forms.CheckBox fsCheckbox;
        private System.Windows.Forms.CheckBox printerCheckbox;
        private System.Windows.Forms.CheckBox cryptoCheckbox;
        private System.Windows.Forms.CheckBox antivirCheckbox;
        private System.Windows.Forms.CheckBox screenshoterCheckBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DomainUpDown intervalDomainUpDown;
        private System.Windows.Forms.Label salabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button adButton;
        private System.Windows.Forms.CheckBox clipboardCheckbox;
        private System.Windows.Forms.CheckBox powerCheckebox;
        private System.Windows.Forms.Label dalabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label adlabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button daButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem controlPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitorizareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkInstallationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;

    }
}