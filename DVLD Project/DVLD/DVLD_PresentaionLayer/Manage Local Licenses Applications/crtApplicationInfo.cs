using ApplicationBusinessLayer;
using ApplicationTypesBusinessLayer;
using DVLDManagement.People_Management;
using LDLApplicationBusinessLayer;
using LicenseClassesBusinessLayer;
using PersonBusinessLayer;
using System;
using System.Windows.Forms;
using UsersBusinessLayer;


namespace DVLDManagement.Manage_LDLApplications
{
    public partial class crtApplicationinfo : UserControl
    {
        public int _PersonID = 0;
        public crtApplicationinfo()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void _LoadLDLApplicationData(clsLocalLicenseApplication lDLApplication)
        {
            lbLDLApplicationID.Text = lDLApplication.LDLApplicationID.ToString();
            lbLicenseClass.Text = clsLicenseClass.Find(lDLApplication.LicenseClassID).ClassName;
            lbPassedTests.Text = lDLApplication.PassedTests.ToString() + "/3";

            clsApplication Application = clsApplication.Find(lDLApplication.LDLApplicationID);

            lbApplicationID.Text = Application.ApplicationID.ToString();
            switch(Application.ApplicationStatus)
            {
                case 1:
                    lbStatus.Text = "New";
                    break;
                case 2:
                    lbStatus.Text = "Cancelled";
                    break;
                case 3:
                    lbStatus.Text = "Completed";
                break;
            }

            lbType.Text = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationTypeTitle;
            lbFees.Text = clsApplicationType.Find(Application.ApplicationTypeID).ApplicationTypeFees.ToString();
            lbApplicant.Text = clsPerson.Find(Application.ApplicantPersonID).FirstName + " " + clsPerson.Find(Application.ApplicantPersonID).SecondName + " " + clsPerson.Find(Application.ApplicantPersonID).ThirdName + " " + clsPerson.Find(Application.ApplicantPersonID).LastName;
            lbDate.Text = Application.ApplicationDate.ToString("yyyy-MM-dd");
            lbStatusDate.Text = Application.LastStatusDate.ToString("yyyy-MM-dd");
            lbUser.Text = clsUser.Find(Application.CreatedByUserID).UserName;

            _PersonID = clsPerson.Find(Application.ApplicantPersonID).PersonId;

        }

        private void crtApplicationinfo_Load(object sender, EventArgs e)
        {
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDetails showDetails = new frmShowDetails(_PersonID);
            showDetails.ShowDialog();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
