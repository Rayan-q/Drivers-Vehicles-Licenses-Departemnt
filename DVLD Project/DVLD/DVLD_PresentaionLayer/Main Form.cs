using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PersonBusinessLayer;
using CountriesBusinessLayer;
using DVLDManagement.Users_Management;
using DVLDManagement.Manage_Application_Types;
using DVLDManagement.Manage_Test_Types;
using DVLDManagement.Manage_Applications;
using DVLDManagement.Manage_Drivers;
using DVLDManagement.Manage_International_Licenses;
using DVLDManagement.Manage_Licenses;
using DVLDManagement.Manage_Licenses_Applications;
using DVLDManagement.Manage_Detained_Licenses;
using UsersBusinessLayer;

namespace DVLDManagement
{
    public partial class frmMainForm : Form
    {
        clsUser _User;
        frmLogin _Login;
        public frmMainForm(clsUser User, frmLogin login)
        {
            InitializeComponent();
            _User = User;
            _Login = login;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPPLManagement frm = new frmPPLManagement();
            frm.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            frmManageUsers frmUsersManagement = new frmManageUsers();
            frmUsersManagement.ShowDialog();
        }

        private void currentAccountInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserDetails frmUserInfoDetails = new frmUserDetails(_User.PersonID, _User);
            frmUserInfoDetails.ShowDialog();
        }

        private void changePassordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frmUpdatePassword = new frmChangePassword(_User.PersonID, _User);
            frmUpdatePassword.ShowDialog();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Login.Show();
            this.Close();
        }
        private void frmMainForm_Load(object sender, EventArgs e)
        {
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frmApplicationTypes = new frmManageApplicationTypes();
            frmApplicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestsTypes frmManageTestsTypes = new frmManageTestsTypes();
            frmManageTestsTypes.ShowDialog();
        }

        private void applicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void localDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLDLApplications frmLDLApplications = new frmManageLDLApplications(_User);
            frmLDLApplications.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewLDLApplication frmLDLApplication = new frmAddNewLDLApplication(_User, -1);
            frmLDLApplication.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDrivers Drivers = new frmDrivers();
            Drivers.ShowDialog();
        }

        private void internationalDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageInternationalLicenses ManageIntLicnese = new frmManageInternationalLicenses(_User);
            ManageIntLicnese.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frmAddILicense = new frmAddInternationalLicense(_User);
            frmAddILicense.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalLicense RenewLocalLicense = new frmRenewLocalLicense(_User);
            RenewLocalLicense.ShowDialog();
        }

        private void replacemntForLostOrDamegedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLostDamReplace LostOrDamagedReplacement = new frmLostDamReplace(_User);

            LostOrDamagedReplacement.Show();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageLDLApplications frmLDLApplications = new frmManageLDLApplications(_User);
            frmLDLApplications.ShowDialog();
        }

        private void detainALicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense DetainLicense = new frmDetainLicense(_User);
            DetainLicense.ShowDialog();
        }

        private void releaseADetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseLicense ReleaseLicense = new frmReleaseLicense(_User);
            ReleaseLicense.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses manageDetainedLicenses = new frmManageDetainedLicenses(_User);
            manageDetainedLicenses.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseLicense releaseLicense = new frmReleaseLicense(_User);
            releaseLicense.ShowDialog();
        }
    }
}
