using DVLDManagement.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsersBusinessLayer;

namespace DVLDManagement
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.Find(txtUserName.Text);
            string HashedPassword = clsGlobal.ComputeHash(txtPassword.Text);

            if(User != null && HashedPassword == User.Password)
            {
                // Store Login Info to remember
                if(cbRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                }
                // Store nothing
                else
                {
                    clsGlobal.RememberUsernameAndPassword("", "");
                    //clsGlobal.RememberUsernameAndPassword("", "");
                }

                if(!User.isActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.Hide();
                frmMainForm MainForm = new frmMainForm(User, this);
                MainForm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
        }

        private void Login_Screen_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";
            if(clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                cbRememberMe.Checked = true;
            }
            else
            {
                cbRememberMe.Checked = false;
            }
        }
    }
}
