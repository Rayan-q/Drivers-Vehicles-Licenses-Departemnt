using ApplicationBusinessLayer;
using ApplicationTypesBusinessLayer;
using DriverBusinessLayer;
using DVLDManagement.Manage_LDLApplications;
using LicenseClassesBusinessLayer;
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

namespace DVLDManagement.Manage_Licenses
{
    public partial class frmRenewLocalLicense : Form
    {
        clsPerson _Person;
        clsUser _User;
        clsLLicense _LocalLicense;
        clsApplication _Application;
        clsLLicense _RLLicense = new clsLLicense();

        int _IssueReason = 2;
        int _ApplicationType = 2;
        int _TotalFees;
        public frmRenewLocalLicense(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }


        private void frmRenewLocalLicense_Load(object sender, EventArgs e)
        {
            lbApplicationDate.Text = DateTime.Today.ToString("yyyy/MMM/dd");
            lbIssueDate.Text = DateTime.Today.ToString("yyyy/MMM/dd");
            lbCreatedByUser.Text = _User.UserName;
            lbAppFees.Text = Convert.ToInt32(clsApplicationType.Find(2).ApplicationTypeFees).ToString();
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
                if(_LocalLicense.ExpirationDate >  DateTime.Today)
                {
                    MessageBox.Show("This License is Yet To Expire");
                    btnRenew.Enabled = false;
                }

                crtLicenseInfo1._LoadData(_LocalLicense, _Person);

                lbOLicenseID.Text = _LocalLicense.LicenseID.ToString();

                int ApplicationFees = Convert.ToInt32(clsApplicationType.Find(2).ApplicationTypeFees);

                clsLicenseClass LicenseClass = clsLicenseClass.Find(_LocalLicense.LicenseClassID);

                int LicenseFees = Convert.ToInt32(LicenseClass.ClassFees);

                lbAppFees.Text = ApplicationFees.ToString();
                lbLicenseFees.Text = LicenseFees.ToString();

                _TotalFees = ApplicationFees + LicenseFees;
                lbTotal.Text = _TotalFees.ToString();

                // Get Expiration Date
                DateTime newExpirationDate = DateTime.Now;
                newExpirationDate = newExpirationDate.AddYears(LicenseClass.DefaultValidityLength);
                lbExpDate.Text = newExpirationDate.ToString("yyyy/MMM/dd");


            }
            else
            {
                MessageBox.Show("License ID is invalid");
            }
        }

        private void btnSearchForLicense_Click(object sender, EventArgs e)
        {
            _SearchForLicense();
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _SearchForLicense();
            }
        }

        private void _RenewApplication()
        {
            _Application = new clsApplication();

            _Application.ApplicantPersonID = _Person.PersonId;
            _Application.ApplicationDate = DateTime.Today;
            _Application.ApplicationTypeID = _ApplicationType;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Today;
            _Application.PaidFees = Convert.ToDecimal(_TotalFees);
            _Application.CreatedByUserID = _User.UserID;

            _Application.Save();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            _RenewApplication();

            _RLLicense.ApplicationID = _Application.ApplicationID;
            _RLLicense.DriverID = _LocalLicense.DriverID;
            _RLLicense.LicenseClassID = _LocalLicense.LicenseClassID;

            DateTime IssueDate = DateTime.Now;
            _RLLicense.IssueDate = IssueDate;

            int DefValLength = clsLicenseClass.Find(_RLLicense.LicenseClassID).DefaultValidityLength;
            _RLLicense.ExpirationDate = IssueDate.AddYears(DefValLength);

            _RLLicense.Notes = rtxNotes.Text;
            _RLLicense.PaidFees = _TotalFees;
            _RLLicense.isActive = true;
            _RLLicense.IssueReason = _IssueReason;
            _RLLicense.CreatedByUserID = _User.UserID;

            if(_RLLicense.Save())
            {
                // Deactivate Expired License and Load as inactive
                _LocalLicense.DeactivateLicense();
                crtLicenseInfo1._LoadData(_LocalLicense, _Person);

                // Complete Application status and save
                _Application.ApplicationStatus = 3;
                _Application.Save();

                // Load full info
                lbRLApplicationID.Text = _Application.ApplicationID.ToString();
                lbRLicenseID.Text = _RLLicense.LicenseID.ToString();

                lbShowLicense.Enabled = true;
                MessageBox.Show("License Renewed Successfully :-)");
                groupBox1.Enabled = false;
            }
            else
            {
                MessageBox.Show("License Was NOT Renewed :-(");
            }

        }

        private void lbLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Person == null)
            {
                MessageBox.Show("Enter A License ID First!");
                return;
            }
            frmLicenseHistory ShowLicenseHistory = new frmLicenseHistory(_Person.PersonId);
            ShowLicenseHistory.ShowDialog();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo RenewedLicenseInfo = new frmLicenseInfo(_RLLicense, _Person);
            RenewedLicenseInfo.Show();
        }
    }
}
