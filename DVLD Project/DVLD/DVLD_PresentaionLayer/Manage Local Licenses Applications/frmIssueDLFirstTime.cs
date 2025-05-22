using ApplicationBusinessLayer;
using DriverBusinessLayer;
using LDLApplicationBusinessLayer;
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

namespace DVLDManagement.Manage_LDLApplications
{
    public partial class frmIssueDLFirstTime : Form
    {
        clsLocalLicenseApplication _LDLAppliction;
        clsUser _User;
        int _IssueReason;
        public frmIssueDLFirstTime(clsLocalLicenseApplication LDLAppliction, int IssueReason, clsUser User)
        {
            InitializeComponent();
            _LDLAppliction = LDLAppliction;
            _IssueReason = IssueReason;
            _User = User;
        }

        private void frmIssueDLFirstTime_Load(object sender, EventArgs e)
        {
            crtApplicationinfo1._LoadLDLApplicationData(_LDLAppliction);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsDriver Driver = new clsDriver();

            clsApplication Application = clsApplication.Find(_LDLAppliction.LDLApplicationID);
            Driver.PersonID = Application.ApplicantPersonID;
            Driver.CreatedByUserID = Application.CreatedByUserID;
            Driver.CreatedDate = DateTime.Now;


            if(!clsDriver.isDriverExist(Application.ApplicantPersonID))
            {
                if (Driver.Save())
                {

                }
                else
                {
                    MessageBox.Show("Driver Was NOT Saved");
                    this.Close();
                }
            }
            
            clsLLicense License = new clsLLicense();

            License.ApplicationID = Application.ApplicationID;
            License.DriverID = clsDriver.Find(Application.ApplicantPersonID).DriverID;
            License.LicenseClassID = _LDLAppliction.LicenseClassID;

            DateTime issueDate = DateTime.Today;

            License.IssueDate = issueDate;
            License.ExpirationDate = issueDate.AddYears(10);
            License.Notes = rtxNotes.Text;
            License.PaidFees = Application.PaidFees;
            License.isActive = true;
            License.IssueReason = _IssueReason;
            License.CreatedByUserID = _User.UserID;


            Application.ApplicationStatus = 3;
            if (License.Save())
            {
                MessageBox.Show("License Issued Successfully");
                Application.Save();
            }
        }
    }
}
