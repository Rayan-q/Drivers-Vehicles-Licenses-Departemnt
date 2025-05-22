using DVLDManagement.Global_Classes;
using DVLDManagement.People_Management;
using PersonBusinessLayer;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using UsersBusinessLayer;
using static DVLDManagement.crtPersonCard;

namespace DVLDManagement.Users_Management
{
    enum enMode { AddNew =  0, Update = 1 }
    public partial class frmAddEditUser : Form
    {
        clsUser _User;
        clsPerson _Person;
        int UserID = 0;
        enMode _Mode;
        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
            
            if(UserID == -1) 
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
        }

        private void _searchForPerson()
        {
            if(txtSearch.Text == "")
            {
                MessageBox.Show("Enter a valid input");
                return;
            }

            //int PersonId = Convert.ToInt32(txtSearch.Text);
            _Person = clsPerson.Find(txtSearch.Text);

            if (_Person != null)
            {
                crtShowDetails1._LoadData(_Person);
            }
            else
            {
                MessageBox.Show("Person was not found");
            }
        }

        private void btnSearchPerson_Click_1(object sender, EventArgs e)
        {
            _searchForPerson();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                _searchForPerson();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Person == null)
            {
                MessageBox.Show("Select A Person before proceeding");
                return;
            }
            if (clsUser.isPersonExist(_Person.PersonId) && _Mode == enMode.AddNew)
            {
                MessageBox.Show("This Person Already Has An Account");
                return;
            }
            tbcUserinfo.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if(!isFormValid())
            {
                MessageBox.Show("ERROR: Check Every thing is valid");
                return;
            }

            _User.PersonID = _Person.PersonId;
            _User.UserName = txtUserName.Text;
            string HashedPassword = clsGlobal.ComputeHash(txtPassword.Text);
            _User.Password = HashedPassword;
            _User.isActive = cbIsActive.Checked;

            if(_User.Save())
            {
                MessageBox.Show("User Saved Successfully");
            }
            else
            {
                MessageBox.Show("User was NOT Saved");
            }

            lbCurrentAction.Text = "Edit User";
            lbUserID.Text = _User.UserID.ToString();

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Must Be Match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
            if(_Mode == enMode.AddNew)
            {
                _User = new clsUser();
                lbCurrentAction.Text = "Add New User";
                return;
            }

            _User = clsUser.Find(UserID);

            if(_User == null)
            {
                MessageBox.Show("User Was Not Found This Form Will Close");
                this.Close();
            }

            _Person = clsPerson.Find(_User.PersonID);
            gbsearchBox.Enabled = false;
            lbUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            cbIsActive.Checked = _User.isActive;

            crtShowDetails1._LoadData(_Person);

        }

        private bool isFormValid()
        {
            bool isValid = true;

            // Check Username is required
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                MessageBox.Show("Enter A UserName");
                isValid = false;
            }

            // Check Password is required

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Choose A Password");
                isValid = false;
            }

            // Check Password match
            if (txtConfirmPassword.Text != txtPassword.Text)
            {
                MessageBox.Show("Password Must Match");
                isValid = false;
            }


            // Check Person Is Selected
            if (_Person == null && _Mode == enMode.AddNew)
            {
                MessageBox.Show("Select A Person before proceeding");
                isValid = false;

            }
            else if (clsUser.isPersonExist(_Person.PersonId) && _Mode == enMode.AddNew)
            {
                MessageBox.Show("This Person Already Has An Account");
                isValid = false;
            }

            return isValid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbcUserinfo.SelectedIndex = 0;
        }

        private void tpPersonalInfo_Click(object sender, EventArgs e)
        {

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addPerson = new frmAddEditPerson(-1);

            addPerson.SendPersoinID += RecievePersonID;

            addPerson.ShowDialog();
        }

        private void RecievePersonID(object obj, int PersonID)
        {
            txtSearch.Text = PersonID.ToString();
            _searchForPerson();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName,"Enter A UserName");
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Choose A Password");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }
    }
}
