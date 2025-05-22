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

namespace DVLDManagement.Manage_Detained_Licenses
{
    public partial class frmDetainLicense : Form
    {
        clsPerson _Person;
        clsUser _User;
        clsLLicense _License;
        clsDetain _DetainLicense = new clsDetain();

        public frmDetainLicense(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lbCreatedByUser.Text = _User.UserName;
            lbDetainDate.Text = DateTime.Today.ToString("yyy/MMM/dd");
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

            _License = clsLLicense.GetLDLicenseInfoByID(LicenseID);
            _Person = clsPerson.FindByLicense(LicenseID);

            if (_License != null)
            {
                if(_License.isActive == false)
                {
                    MessageBox.Show("This License Is Inactive!");
                    btnDetain.Enabled = false;
                }
                else if (clsLLicense.isDetained(_License.LicenseID))
                {
                    MessageBox.Show("This License Is Already Detained!");
                    btnDetain.Enabled = false;
                }
                else
                {
                    btnDetain.Enabled = true;
                }
                crtLicenseInfo1._LoadData(_License, _Person);
                lbShowLicense.Enabled = false;
                lbLicenseID.Text = _License.LicenseID.ToString();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(!_isLicenseValid())
            {
                return;
            }

            _DetainLicense.LicenseID = _License.LicenseID;
            _DetainLicense.DetainDate = DateTime.Today;
            _DetainLicense.FineFees = Convert.ToDecimal(txtFineFees.Text);
            _DetainLicense.DetainedByUserID = _User.UserID;
            _DetainLicense.isReleased = false;

            if(_DetainLicense.Detain(_License.LicenseID))
            {
                MessageBox.Show("License Detained Successfully!");
                lbDetainID.Text = _DetainLicense.DetainID.ToString();
                crtLicenseInfo1._LoadData(_License, _Person);
                lbShowLicense.Enabled = true;
            }
            else
            {
                MessageBox.Show("License Was NOT Detained!");
            }
        }

        private bool _isLicenseValid()
        {
            bool isValid = true;
            int FineFees = 0;

            if(!int.TryParse(txtFineFees.Text, out FineFees))
            {
                MessageBox.Show("Enter A Valid Fine Fee");
                isValid = false;
            }
            else if (clsLLicense.isDetained(_DetainLicense.LicenseID))
            {
                MessageBox.Show("License Is Already Detained");
                isValid = false;
            }
            return isValid;
        }

        private void lbShowLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo ShowLicense = new frmLicenseInfo(_License, _Person);
            ShowLicense.Show();
        }

        private void lbLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory ShowLicenseHistory = new frmLicenseHistory(_Person.PersonId);
            ShowLicenseHistory.ShowDialog();
        }
    }
}
