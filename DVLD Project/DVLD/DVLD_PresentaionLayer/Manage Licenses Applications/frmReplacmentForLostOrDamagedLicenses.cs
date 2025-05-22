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

namespace DVLDManagement.Manage_Licenses_Applications
{
    public partial class frmLostDamReplace : Form
    {
        clsPerson _Person;
        clsUser _User;
        clsLLicense _LocalLicense;
        clsApplication _Application;
        int _ApplicationType;
        int _ApplicationFees;
        clsLLicense _RLLicense = new clsLLicense();

        int _IssueReason = 4;
        public frmLostDamReplace(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void frmLostDamReplace_Load(object sender, EventArgs e)
        {
            lbApplicationDate.Text = DateTime.Today.ToString("yyyy/MMM/dd");
            lbCreatedByUser.Text = _User.UserName;
            lbAppFees.Text = Convert.ToInt32(clsApplicationType.Find(3).ApplicationTypeFees).ToString();
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
                if (_LocalLicense.isActive == false)
                {
                    MessageBox.Show("This License Must Be Active");
                    btnIssue.Enabled = false;
                }
                else
                {
                    btnIssue.Enabled = true;
                }

                crtLicenseInfo1._LoadData(_LocalLicense, _Person);
                lbOLicenseID.Text = _LocalLicense.LicenseID.ToString();
                _ApplicationType = 3;
                lbShowLicense.Enabled = false;

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

        private void rdLost_CheckedChanged(object sender, EventArgs e)
        {
            if(rdLost.Checked)
            {
                _IssueReason = 4;
                rdDamaged.Checked = false;
                _ApplicationType = 3;
                _ApplicationFees = Convert.ToInt32(clsApplicationType.Find(3).ApplicationTypeFees);
                lbAppFees.Text = _ApplicationFees.ToString();
            }
        }

        private void rdDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if(rdDamaged.Checked)
            {
                _IssueReason = 3;
                rdLost.Checked = false;
                _ApplicationType = 4;
                _ApplicationFees = Convert.ToInt32(clsApplicationType.Find(4).ApplicationTypeFees);
                lbAppFees.Text = _ApplicationFees.ToString();
            }
        }

        private void _ReplacementApplication()
        {
            _Application = new clsApplication();

            _Application.ApplicantPersonID = _Person.PersonId;
            _Application.ApplicationDate = DateTime.Today;
            _Application.ApplicationTypeID = _ApplicationType;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Today;
            _Application.PaidFees = Convert.ToDecimal(_ApplicationFees);
            _Application.CreatedByUserID = _User.UserID;

            _Application.Save();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if (_LocalLicense.isActive == false)
            {
                MessageBox.Show("License Must Be Active");
                return;
            }
                _ReplacementApplication();

            _RLLicense.ApplicationID = _Application.ApplicationID;
            _RLLicense.DriverID = _LocalLicense.DriverID;
            _RLLicense.LicenseClassID = _LocalLicense.LicenseClassID;
            _RLLicense.IssueDate = _LocalLicense.IssueDate;
            _RLLicense.ExpirationDate = _LocalLicense.ExpirationDate;
            _RLLicense.Notes = _LocalLicense.Notes;

            _RLLicense.PaidFees = _ApplicationFees;
            _RLLicense.isActive = true;
            _RLLicense.IssueReason = _IssueReason;
            _RLLicense.CreatedByUserID = _User.UserID;

            if (_RLLicense.Save())
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
                MessageBox.Show("License Replacement Issued Successfully :-)");
            }
            else
            {
                MessageBox.Show("License Replacement Was NOT Issued :-(");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo ShowLicenseDetails = new frmLicenseInfo(_RLLicense, _Person);
            ShowLicenseDetails.Show();
        }
    }
}
