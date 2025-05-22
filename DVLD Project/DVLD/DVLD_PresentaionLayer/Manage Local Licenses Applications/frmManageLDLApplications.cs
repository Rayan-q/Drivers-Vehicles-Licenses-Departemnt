using ApplicationBusinessLayer;
using LDLApplicationBusinessLayer;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using UsersBusinessLayer;
using Appointments_And_Tests_Business_Layer;
using DVLDManagement.Manage_LDLApplications;
using PersonBusinessLayer;
using DriverBusinessLayer;
using LicenseClassesBusinessLayer;
using System.Data;

namespace DVLDManagement.Manage_Applications
{
    public partial class frmManageLDLApplications : Form
    {
        clsUser _User;
        int _LDLApplicationID;
        DataTable _dtAllLocalDrivingLicenseApplications;
        public frmManageLDLApplications(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void _RefreshApplicationsList()
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalLicenseApplication.GetAllLDLApplications();
            dgvLDLApplications.DataSource = _dtAllLocalDrivingLicenseApplications;

            lbNumOfRecrods.Text = dgvLDLApplications.Rows.Count.ToString();
        }
        private void frmManageApplications_Load(object sender, EventArgs e)
        {
            _RefreshApplicationsList();
            _DisableFeatures();
        }

        private void _DisableFeatures()
        {
            cmsItemShowLicense.Enabled = false;
            smVisionTest.Enabled = false;
            smWrittenTest.Enabled = false;
            smStreetTest.Enabled = false;
            cmsItemShowLicense.Enabled = false;
            tsmIssueDrivingLicense.Enabled = false;
        }

        // Features Validation
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (_CanIssueLicnese())
            {
                tsmIssueDrivingLicense.Enabled = true;
            }
            else
            {
                tsmIssueDrivingLicense.Enabled = false;
            }
            if (_isApplicationComplete())
            {
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                canceltoolStripMenuItem.Enabled = false;
                scheduleToolStripMenuItem.Enabled = false;
                tsmIssueDrivingLicense.Enabled = false;
                cmsItemShowLicense.Enabled = true;
            }
            else
            {
                editApplicationToolStripMenuItem.Enabled = true;
                deleteApplicationToolStripMenuItem.Enabled = true;
                canceltoolStripMenuItem.Enabled = true;
                scheduleToolStripMenuItem.Enabled = true;
                if (_CanIssueLicnese())
                {
                    tsmIssueDrivingLicense.Enabled = true;
                }
                cmsItemShowLicense.Enabled = false;
            }
        }

        private bool _CanIssueLicnese()
        {
            bool isValid = true;

            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            if (!clsTest.isTestPassed(_LDLApplicationID, 1))
            {
                smVisionTest.Enabled = true;
                smWrittenTest.Enabled = false;
                smStreetTest.Enabled = false;

                isValid = false;
                return isValid;
            }
            else if (!clsTest.isTestPassed(_LDLApplicationID, 2))
            {
                smVisionTest.Enabled = false;
                smWrittenTest.Enabled = true;
                smStreetTest.Enabled = false;
                isValid = false;
                return isValid;
            }
            else if (!clsTest.isTestPassed(_LDLApplicationID, 3))
            {
                smVisionTest.Enabled = false;
                smWrittenTest.Enabled = false;
                smStreetTest.Enabled = true;
                isValid = false;
                return isValid;
            }
            smVisionTest.Enabled = false;
            smWrittenTest.Enabled = false;
            smStreetTest.Enabled = false;
            return isValid;
        }

        private bool _isApplicationComplete()
        {
            bool isComplete = true;

            if ((string)dgvLDLApplications.CurrentRow.Cells[6].Value != "Completed")
            {
                isComplete = false;
            }
            return isComplete;
        }
        //

        private void btnAddLDLApplication_Click(object sender, EventArgs e)
        {
            frmAddNewLDLApplication frmLDLApplication = new frmAddNewLDLApplication(_User, -1);
            frmLDLApplication.ShowDialog();
            _RefreshApplicationsList();
        }

        
        // Menu Items
        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsApplication Application = clsApplication.Find((int)dgvLDLApplications.CurrentRow.Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to Cancel Application [" + Application.ApplicationID + "]", "Confirm Cancel", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsApplication.CancelApplication(Application.ApplicationID))
                {
                    MessageBox.Show("Application Cancelled Successfully.");
                    _RefreshApplicationsList();
                }

                else
                    MessageBox.Show("Application is not Cancelled.");
            }
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;

            clsApplication Application = clsApplication.Find(_LDLApplicationID);

            if (MessageBox.Show("Are you sure you want to Delete Application [" + Application.ApplicationID + "]", "Confirm Cancel", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsLocalLicenseApplication.DeleteApplication(Application.ApplicationID, _LDLApplicationID))
                {
                    MessageBox.Show("Application Deleted Successfully.");
                    _RefreshApplicationsList();
                }

                else
                    MessageBox.Show("Application is not Deleted.");
            }
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewLDLApplication frmLDLApplication = new frmAddNewLDLApplication(_User, (int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frmLDLApplication.ShowDialog();
            _RefreshApplicationsList();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            frmApplicationDetails ApplicationDetails = new frmApplicationDetails(lDLApplication);
            ApplicationDetails.Show();
        }
        //
        
        // Tests
        private void smVisionTest_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            frmTestAppointments appointmentsList = new frmTestAppointments(lDLApplication, 1, _User);
            appointmentsList.ShowDialog();
            _RefreshApplicationsList();
        }

        private void smWrittenTest_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            frmTestAppointments appointmentsList = new frmTestAppointments(lDLApplication, 2, _User);
            appointmentsList.ShowDialog();
            _RefreshApplicationsList();
        }

        private void smStreetTest_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            frmTestAppointments appointmentsList = new frmTestAppointments(lDLApplication, 3, _User);
            appointmentsList.ShowDialog();
            _RefreshApplicationsList();
        }
        //


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmIssueDrivingLicense_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            frmIssueDLFirstTime IssueLicense = new frmIssueDLFirstTime(lDLApplication, 1, _User);
            IssueLicense.ShowDialog();
            _RefreshApplicationsList();
        }

        private void personLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalLicenseApplication lDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);
            clsApplication Application = clsApplication.Find(lDLApplication.LDLApplicationID);

            int _PersonID = clsPerson.Find(Application.ApplicantPersonID).PersonId;

            frmLicenseHistory ShowLicenseHistory = new frmLicenseHistory(_PersonID);

            ShowLicenseHistory.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // LDL Application ID to get Application Object
            _LDLApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;

            // Get full Application Object
            clsApplication Application = clsApplication.Find(_LDLApplicationID);

            // Get Person Info through Application Object
            clsPerson _Person = clsPerson.Find(Application.ApplicantPersonID);

            // Get class id through Class Name
            string _ClassName = dgvLDLApplications.CurrentRow.Cells[1].Value.ToString();
            int _ClassID = clsLicenseClass.Find(_ClassName).LicenseClassID;

            // Get Licnese Info
            clsLLicense License = clsLLicense.GetLDLicenseInfo(Application.ApplicationID, _ClassID);


            frmLicenseInfo LicenseInfo = new frmLicenseInfo(License, _Person);
            LicenseInfo.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lbNumOfRecrods.Text = dgvLDLApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {

                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;


                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lbNumOfRecrods.Text = dgvLDLApplications.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                //in this case we deal with integer not string.
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lbNumOfRecrods.Text = dgvLDLApplications.Rows.Count.ToString();
        }
    }
}
