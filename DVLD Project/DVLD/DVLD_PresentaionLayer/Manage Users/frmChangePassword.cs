using DVLDManagement.Global_Classes;
using PersonBusinessLayer;
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

namespace DVLDManagement.Users_Management
{
    public partial class frmChangePassword : Form
    {
        clsUser _User;
        int _PersonID;
        public frmChangePassword(int PersonID, clsUser User)
        {
            InitializeComponent();
            _User = User;
            _PersonID = PersonID;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            clsPerson Person = clsPerson.Find(_PersonID);
            crtShowDetails1._LoadData(Person);
            crtLoginInfo1._LoadData(_User);
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {

            // Validation Required Here
            if(!isFormValid())
            {
                return;
            }

            // Encrypt new password
            string HashedNewPassword = clsGlobal.ComputeHash(txtNewPassword.Text);
            // Assign encrypted password to User object for saving
            _User.Password = HashedNewPassword;

            if(_User.Save())
            {
                MessageBox.Show("Password Updated Successfully");
            }
            else
            {
                MessageBox.Show("There Was An Error Updating Password");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool isFormValid()
        {
            bool isValid = true;

            string HashedCurrentPassword = clsGlobal.ComputeHash(txtCurrentPassword.Text);
            // Check current Password
            if (HashedCurrentPassword != _User.Password || string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show("Current Password Is Wrong!");
                isValid = false;
            }

            string HashedNewPassword = clsGlobal.ComputeHash(txtNewPassword.Text);
            // Check new password
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text) || HashedNewPassword == _User.Password)
            {
                MessageBox.Show("Choose A New Password!");
                isValid = false;
            }

            // Check New Password Is NOT Previously Used
            if(clsUser.isPasswordOld(_User.UserID, HashedNewPassword))
            {
                MessageBox.Show("This Password Was Previously Used. Choose A New One");
                isValid = false;
            }

            // Check New Password Match
            if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                MessageBox.Show("Password Must Match");
                isValid = false;
            }

                return isValid;
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            string HashedCurrentPassword = clsGlobal.ComputeHash(txtCurrentPassword.Text);

            if (HashedCurrentPassword != _User.Password || string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong!");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtConfirmNewPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            string HashedNewPassword = clsGlobal.ComputeHash(txtNewPassword.Text);

            if(string.IsNullOrWhiteSpace(txtNewPassword.Text) || HashedNewPassword == _User.Password)
            {
                errorProvider1.SetError(txtNewPassword, "Choose a New Password");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, "");
            }

            // Throw an error if the new password was previously used
            if (clsUser.isPasswordOld(_User.UserID, HashedNewPassword))
            {
                errorProvider1.SetError(txtNewPassword, "This Password Was Previously Used. Choose a New Password");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

        private void txtConfirmNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                errorProvider1.SetError(txtConfirmNewPassword, "Password Must Match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmNewPassword, "");
            }
        }
    }
}
