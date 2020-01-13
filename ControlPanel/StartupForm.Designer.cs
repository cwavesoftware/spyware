namespace ControlPanel
{
    partial class StartupForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupForm));
            this.extractorButton = new System.Windows.Forms.Button();
            this.extractorLocationDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.CPButton = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // extractorButton
            // 
            this.extractorButton.DialogResult = System.Windows.Forms.DialogResult.No;
            this.extractorButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.extractorButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.extractorButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aqua;
            this.extractorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extractorButton.Image = ((System.Drawing.Image)(resources.GetObject("extractorButton.Image")));
            this.extractorButton.Location = new System.Drawing.Point(173, 0);
            this.extractorButton.Name = "extractorButton";
            this.extractorButton.Size = new System.Drawing.Size(169, 160);
            this.extractorButton.TabIndex = 8;
            this.extractorButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.extractorButton.UseVisualStyleBackColor = true;
            this.extractorButton.Click += new System.EventHandler(this.extractorButton_Click);
            this.extractorButton.MouseHover += new System.EventHandler(this.extractorButton_MouseHover);
            // 
            // CPButton
            // 
            this.CPButton.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.CPButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.CPButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Teal;
            this.CPButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aqua;
            this.CPButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CPButton.Image = ((System.Drawing.Image)(resources.GetObject("CPButton.Image")));
            this.CPButton.Location = new System.Drawing.Point(0, 0);
            this.CPButton.Name = "CPButton";
            this.CPButton.Size = new System.Drawing.Size(167, 160);
            this.CPButton.TabIndex = 9;
            this.CPButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.CPButton.UseVisualStyleBackColor = true;
            this.CPButton.MouseHover += new System.EventHandler(this.CPButton_MouseHover);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipTitle = "Monitoring";
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 160);
            this.ControlBox = false;
            this.Controls.Add(this.CPButton);
            this.Controls.Add(this.extractorButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "StartupForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitoring";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button extractorButton;
        private System.Windows.Forms.FolderBrowserDialog extractorLocationDialog;
        private System.Windows.Forms.Button CPButton;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;
    }
}

