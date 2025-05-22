using ApplicationBusinessLayer;
using ApplicationTypesBusinessLayer;
using DriverBusinessLayer;
using DVLDManagement.Manage_LDLApplications;
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

namespace DVLDManagement.Manage_International_Licenses
{
    public partial class frmAddInternationalLicense : Form
    {
        clsApplication _Application = new clsApplication();
        clsLLicense _LocalLicense;
        clsILicense _IntLicense;
        clsPerson _Person;
        clsUser _User;
        public frmAddInternationalLicense(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void frmAddInternationalLicense_Load(object sender, EventArgs e)
        {
            DateTime IssueDate = DateTime.Now;
            lbApplicationDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            lbIssueDate.Text = IssueDate.ToString("yyyy-MM-dd");
            lbFees.Text = Convert.ToInt32(clsApplicationType.Find(6).ApplicationTypeFees).ToString();
            lbExpDate.Text = IssueDate.AddYears(1).ToString("yyyy-MM-dd");
            lbCreatedByUser.Text = _User.UserName;

        }

        private void searchForLicense_Click(object sender, EventArgs e)
        {
            _SearchForLicense();
        }

        private void _SearchForLicense()
        {
            int LicenseID = 0;

            if (int.TryParse(txtLicenseID.Text, out LicenseID))
            {
                LicenseID = Convert.ToInt32(txtLicenseID.Text);
            }
            else
            {
                MessageBox.Show("Please Enter A Number");
                return;
            }

             _LocalLicense = clsLLicense.GetLDLicenseInfoByID(LicenseID);
            _Person = clsPerson.FindByLicense(LicenseID);

            if (_LocalLicense != null)
            {
                if (_LocalLicense.LicenseClassID != 3)
                {
                    MessageBox.Show("License Class Must Be Class 3");
                }
                else
                {
                    if (clsILicense.isInterLicenseExist(_LocalLicense.LicenseID))
                    {
                        MessageBox.Show("This License is Already Used To Issue An International License");
                        btnIssue.Enabled = false;
                        lbShowLicense.Enabled = true;
                    }
                    crtLicenseInfo1._LoadData(_LocalLicense, _Person);
                    LDLicenseID.Text = _LocalLicense.LicenseClassID.ToString();
                }
            }
            else
            {
                MessageBox.Show("License ID is invalid");
            }
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _SearchForLicense();
            }
        }

        private void lbLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_Person == null)
            {
                MessageBox.Show("Enter A License ID First!");
                return;
            }
            frmLicenseHistory ShowLicenseHistory = new frmLicenseHistory(_Person.PersonId);
            ShowLicenseHistory.ShowDialog();
        }

        private void _SaveApplication()

        {
            _Application.ApplicantPersonID = _Person.PersonId;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 6;
            _Application.ApplicationStatus = 3;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = clsApplicationType.Find(6).ApplicationTypeFees;
            _Application.CreatedByUserID = _User.UserID;

            _Application.Save();

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseID  = 0;
            if(!_isLicenseIDValid(LicenseID))
            {
                return;
            }
            _SaveApplication();

            clsILicense InternationalLicense = new clsILicense();

            InternationalLicense.ApplicationID = _Application.ApplicationID;
            InternationalLicense.DriverID = clsDriver.Find(_Person.PersonId).DriverID;
            InternationalLicense.LocalLicenseID = _LocalLicense.LicenseID;
            DateTime issuDate = DateTime.Today;
            InternationalLicense.IssueDate = issuDate;
            InternationalLicense.ExpirationDate = issuDate.AddYears(10);
            InternationalLicense.isActive = true;
            InternationalLicense.CreatedByUserID = _User.UserID;

            if(InternationalLicense.Save())
            {
                MessageBox.Show("International License Issued Successfully!");
            }
            else
            {
                MessageBox.Show("International License Was NOT Issued!");
            }

            ILApplicationID.Text = _Application.ApplicationID.ToString();
            ILLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            btnIssue.Enabled = false;
            lbShowLicense.Enabled = true;
            _IntLicense = InternationalLicense;
        }

        private bool _isLicenseIDValid(int LicenseID)
        {
            bool isValid = true;

            if(!int.TryParse(txtLicenseID.Text, out LicenseID))
            {
                MessageBox.Show("Please Enter A Valid Number");
                isValid = false;
            }

            else if(_LocalLicense.isActive == false)
            {
                MessageBox.Show("Local License Must Be Active");
                isValid = false;
            }
            else if(_LocalLicense.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("Local License Is Expired. You Need To Renew It First.");
                isValid = false;
            }
            return isValid;
        }

        private void lbShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Get International License info by Driver ID
            _IntLicense = clsILicense.Find(_LocalLicense.DriverID);

            frmShowInternationalLicenseInfo IntLicenseInfo = new frmShowInternationalLicenseInfo(_Person, _IntLicense);
            IntLicenseInfo.Show();
        }
    }
    
}
