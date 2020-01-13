using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlPanel
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            this.ActiveControl = userTextbox;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            CheckCreds();
            
        }
        void CheckCreds()
        {
            if (userTextbox.Text == "admin" && passTextBox.Text == "admin")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
                MessageBox.Show("Login failed", "Monitorizare", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
        }

        private void passTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToByte(e.KeyChar) == 13)
                CheckCreds();
        }

        private void userTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToByte(e.KeyChar) == 13)
                this.ActiveControl = passTextBox;
        }
    }
}
