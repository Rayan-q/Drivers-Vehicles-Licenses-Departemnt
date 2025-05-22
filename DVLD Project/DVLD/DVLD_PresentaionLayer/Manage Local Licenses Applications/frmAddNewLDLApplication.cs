using CountriesBusinessLayer;
using LDLApplicationBusinessLayer;
using PersonBusinessLayer;
using System;
using System.Data;
using System.Windows.Forms;
using UsersBusinessLayer;
using ApplicationBusinessLayer;
using ApplicationTypesBusinessLayer;
using LicenseClassesBusinessLayer;
using DVLDManagement.People_Management;

namespace DVLDManagement.Manage_Applications
{
    public partial class frmAddNewLDLApplication : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        clsPerson _Person;
        clsUser _User;
        clsApplication _Application = new clsApplication();
        clsLocalLicenseApplication _LDLApplication;
        int _LDLApplicationID;
        clsApplicationType _ApplicationType = clsApplicationType.Find(1);
        clsLicenseClass _LicenseClass;

        enMode _Mode;
        public frmAddNewLDLApplication(clsUser User, int LDLApplicationID)
        {
            InitializeComponent();
            _User = User;
            _LDLApplicationID = LDLApplicationID;

            if(_LDLApplicationID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
                lbCurrentAction.Text = "Edit L.D.L.Application";
            }
        }

        private void _FillClassesCBox()
        {
            DataTable LicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in LicenseClasses.Rows)
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }
        }
        private void frmAddNewLDLApplication_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            _FillClassesCBox();
            cbLicenseClasses.SelectedIndex = 2;
            lbApplicationFees.Text = Convert.ToInt32(_ApplicationType.ApplicationTypeFees).ToString();
            lbCreatedByUser.Text = _User.UserName;

            if (_Mode == enMode.AddNew)
            {
                lbApplicationDate.Text = DateTime.Now.ToString();
                _LDLApplication = new clsLocalLicenseApplication();
                return;
            }
            _LDLApplication = clsLocalLicenseApplication.Find(_LDLApplicationID);

            if(_LDLApplication == null)
            {
                MessageBox.Show("No L.D.L.Application Found This Form Will Be Closed");
                this.Close();
            }

            gbsearchBox.Enabled = false;

            _Person = clsPerson.Find(_LDLApplication.PersonID);
            crtShowDetails1._LoadData(_Person);
            lbLDLApplicationID.Text = _LDLApplication.LDLApplicationID.ToString();
            lbApplicationDate.Text = _LDLApplication.ApplicationDate.ToString();
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(clsLicenseClass.Find(_LDLApplication.LicenseClassID).ClassName);


        }
        private void _searchForPerson()
        {
            if (txtSearch.Text == "")
            {
                MessageBox.Show("Enter a valid input");
                return;
            }

            switch(cbFilterBy.SelectedIndex)
            {
                case 0:
                    int PersonId = Convert.ToInt32(txtSearch.Text);
                    _Person = clsPerson.Find(PersonId);
                    break;
                case 1:
                    string NatioanlNo = txtSearch.Text;
                    _Person = clsPerson.Find(NatioanlNo);
                    break;
                default:
                    break;
            }

            

            if (_Person != null)
            {
                crtShowDetails1._LoadData(_Person);
            }
            else
            {
                MessageBox.Show("Person was not found");
            }
        }

        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            _searchForPerson();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                _searchForPerson();
            }
        }

        private void _saveApplication()
        {
            _Application.ApplicantPersonID = _Person.PersonId;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = _ApplicationType.ApplicationTypeID;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = _ApplicationType.ApplicationTypeFees + _LicenseClass.ClassFees;
            _Application.CreatedByUserID = _User.UserID;

            _Application.Save();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _LicenseClass = clsLicenseClass.Find(cbLicenseClasses.Text);

            if (!isFormValid())
            {
                return;
            }

            if (_Mode == enMode.AddNew)
            {
                _saveApplication();
            }

            _LDLApplication.ApplicationID = _Application.ApplicationID;
            _LDLApplication.LicenseClassID = _LicenseClass.LicenseClassID;

            if(_LDLApplication.Save())
            {
                MessageBox.Show("LDL Application Saved Successfully");
            }
            else
            {
                MessageBox.Show("LDL Application NOT Saved");
            }

            lbCurrentAction.Text = "Edit L.D.L.Application";
            lbLDLApplicationID.Text = _LDLApplication.LDLApplicationID.ToString();
        }

        private bool isFormValid()
        {
            bool isValid = true;


            // Check Person Is Selected
            if (_Person == null && _Mode == enMode.AddNew)
            {
                MessageBox.Show("Select A Person before proceeding");
                isValid = false;

            }

            // Check Person does not have multiple applications of the same license class in New status or Completed Status
            if(clsLocalLicenseApplication.isPersonAndClassExist(_LicenseClass.LicenseClassID, _Person.PersonId))
            {
                MessageBox.Show("This Person Already Has an Active/Completed Application of this License Class. Choose a Different Class");
                isValid = false;
            }


                return isValid;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tbLDLApplicationInfo.SelectedIndex = 1;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            tbLDLApplicationInfo.SelectedIndex = 0;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addPerson = new frmAddEditPerson(-1);
            addPerson.ShowDialog();
        }
    }
}
