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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UsersBusinessLayer;

namespace DVLDManagement.Manage_Detained_Licenses
{
    public partial class frmReleaseLicense : Form
    {
        clsPerson _Person;
        clsUser _User;

        clsLLicense _DetainedLicense;
        clsDetain _DetainApp;
        clsApplication _Application;

        int _ApplicationType = 5;
        decimal _ApplicationFees;
        decimal _TotalFees;
        int _LicenseID;
        public frmReleaseLicense(clsUser User, int LicenseID = 0)
        {
            InitializeComponent();
            _User = User;
            _LicenseID = LicenseID;

            if(_LicenseID != 0)
            {
                _SearchForLicense(LicenseID);
            }
        }

        private void _LoadLicenseInfo(int LicenseID)
        {
            _DetainedLicense = clsLLicense.GetLDLicenseInfoByID(LicenseID);
            _Person = clsPerson.FindByLicense(LicenseID);


            btnRelease.Enabled = true;
            _DetainApp = clsDetain.Find(_DetainedLicense.LicenseID);

            lbLicenseID.Text = _DetainedLicense.LicenseID.ToString();
            lbDetainID.Text = _DetainApp.DetainID.ToString();
            lbDetainDate.Text = _DetainApp.DetainDate.ToString("yyyy/MMM/dd");

            _ApplicationFees = clsApplicationType.Find(_ApplicationType).ApplicationTypeFees;
            lbApplicatioinFees.Text = Convert.ToInt32(_ApplicationFees).ToString();

            _TotalFees = _ApplicationFees + _DetainApp.FineFees;
            lbTotalFees.Text = Convert.ToInt32(_TotalFees).ToString();

            lbFineFees.Text = Convert.ToInt32(_DetainApp.FineFees).ToString();

            lbCreatedByUser.Text = _User.UserName;

            crtLicenseInfo1._LoadData(_DetainedLicense, _Person);
            lbApplicationID.Text = "???";
            lbShowLicense.Enabled = false;
        }

        private void _SearchForLicense(int licenseId = 0)
        {
            int LicenseID = 0;


            if(licenseId != 0)
            {
                txtLicenseID.Text = licenseId.ToString();
                groupBox1.Enabled = false;

                _LoadLicenseInfo(licenseId);

                return;
            }


            if (int.TryParse(txtLicenseID.Text, out LicenseID))
            {
                LicenseID = Convert.ToInt32(txtLicenseID.Text);
            }
            else
            {
                MessageBox.Show("Please Enter A Number");
                return;
            }


            _DetainedLicense = clsLLicense.GetLDLicenseInfoByID(LicenseID);
            _Person = clsPerson.FindByLicense(LicenseID);

            if (_DetainedLicense != null)
            {
                if (!clsLLicense.isDetained(_DetainedLicense.LicenseID))
                {
                    MessageBox.Show("This License Is NOT Detained");
                    btnRelease.Enabled = false;
                }
                else
                {
                    btnRelease.Enabled = true;
                    _DetainApp = clsDetain.Find(_DetainedLicense.LicenseID);

                    lbLicenseID.Text = _DetainedLicense.LicenseID.ToString();
                    lbDetainID.Text = _DetainApp.DetainID.ToString();
                    lbDetainDate.Text = _DetainApp.DetainDate.ToString("yyyy/MMM/dd");

                    _ApplicationFees = clsApplicationType.Find(_ApplicationType).ApplicationTypeFees;
                    lbApplicatioinFees.Text = Convert.ToInt32(_ApplicationFees).ToString();

                    _TotalFees = _ApplicationFees + _DetainApp.FineFees;
                    lbTotalFees.Text = Convert.ToInt32(_TotalFees).ToString();

                    lbFineFees.Text = Convert.ToInt32(_DetainApp.FineFees).ToString();

                    lbCreatedByUser.Text = _User.UserName;
                }
                crtLicenseInfo1._LoadData(_DetainedLicense, _Person);
                lbApplicationID.Text = "???";
                lbShowLicense.Enabled = false;


            }
            else
            {
                MessageBox.Show("License ID is invalid");
            }
        }

        private void _ReleaseApplication()
        {
            _Application = new clsApplication();

            
            _Application.ApplicantPersonID = _Person.PersonId;
            _Application.ApplicationDate = DateTime.Today;
            _Application.ApplicationTypeID = _ApplicationType;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Today;

            // Get Total Fees
            _Application.PaidFees = Convert.ToDecimal(_TotalFees);

            _Application.CreatedByUserID = _User.UserID;

            _Application.Save();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if(!_isReleased())
            {
                return;
            }
            _ReleaseApplication();

            _DetainApp.ReleaseDate = DateTime.Today;
            _DetainApp.ReleasedByUserID = _User.UserID;
            _DetainApp.isReleased = true;
            _DetainApp.ReleaseApplicationID = _Application.ApplicationID;

            if(_DetainApp.Release())
            {
                MessageBox.Show("License Was Released Successfully");
                _Application.ApplicationStatus = 3;
                _Application.LastStatusDate = DateTime.Now;
                _Application.Save();

                lbApplicationID.Text = _Application.ApplicationID.ToString();

                crtLicenseInfo1._LoadData(_DetainedLicense, _Person);
                lbShowLicense.Enabled = true;

                if(_LicenseID != 0)
                {
                    btnRelease.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("License Was NOT Released");
            }
        }

        private bool _isReleased()
        {
            bool isValid = true;
            
            if (!clsLLicense.isDetained(_DetainedLicense.LicenseID))
            {
                MessageBox.Show("License Is Released Detained");
                isValid = false;
            }
            return isValid;
        }

        private void lbShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo ShowLicense = new frmLicenseInfo(_DetainedLicense, _Person);
            ShowLicense.ShowDialog();
        }

        private void lbLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory ShowHistory = new frmLicenseHistory(_Person.PersonId);
            ShowHistory.Show();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
