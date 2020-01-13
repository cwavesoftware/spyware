namespace ControlPanel
{
    partial class Check
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
            this.salabel = new System.Windows.Forms.Label();
            this.dalabel = new System.Windows.Forms.Label();
            this.adlabel = new System.Windows.Forms.Label();
            this.cflabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // salabel
            // 
            this.salabel.AutoSize = true;
            this.salabel.Location = new System.Drawing.Point(34, 28);
            this.salabel.Name = "salabel";
            this.salabel.Size = new System.Drawing.Size(35, 13);
            this.salabel.TabIndex = 0;
            this.salabel.Text = "label1";
            // 
            // dalabel
            // 
            this.dalabel.AutoSize = true;
            this.dalabel.Location = new System.Drawing.Point(34, 61);
            this.dalabel.Name = "dalabel";
            this.dalabel.Size = new System.Drawing.Size(35, 13);
            this.dalabel.TabIndex = 1;
            this.dalabel.Text = "label2";
            // 
            // adlabel
            // 
            this.adlabel.AutoSize = true;
            this.adlabel.Location = new System.Drawing.Point(34, 93);
            this.adlabel.Name = "adlabel";
            this.adlabel.Size = new System.Drawing.Size(35, 13);
            this.adlabel.TabIndex = 2;
            this.adlabel.Text = "label3";
            // 
            // cflabel
            // 
            this.cflabel.AutoSize = true;
            this.cflabel.Location = new System.Drawing.Point(34, 123);
            this.cflabel.Name = "cflabel";
            this.cflabel.Size = new System.Drawing.Size(35, 13);
            this.cflabel.TabIndex = 3;
            this.cflabel.Text = "label3";
            // 
            // Check
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 171);
            this.Controls.Add(this.cflabel);
            this.Controls.Add(this.adlabel);
            this.Controls.Add(this.dalabel);
            this.Controls.Add(this.salabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Check";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checking installation...";
            this.Load += new System.EventHandler(this.Check_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Check_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label salabel;
        private System.Windows.Forms.Label dalabel;
        private System.Windows.Forms.Label adlabel;
        private System.Windows.Forms.Label cflabel;
    }
}