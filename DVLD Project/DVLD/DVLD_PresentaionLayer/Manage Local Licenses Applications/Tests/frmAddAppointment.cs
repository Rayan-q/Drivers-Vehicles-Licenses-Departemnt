using ApplicationBusinessLayer;
using ApplicationTypesBusinessLayer;
using Appointments_And_Tests_Business_Layer;
using LDLApplicationBusinessLayer;
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
using TestTypeBusiness;
using UsersBusinessLayer;

namespace DVLDManagement.Manage_LDLApplications
{
    enum enMode { AddNew = 1, Update =  2 }
    public partial class frmAddAppointment : Form
    {
        clsLocalLicenseApplication _LDLApplication;
        clsApplication _Application;
        clsAppointment _Appointment;
        clsApplicationType _RetakeTestApplicationType = clsApplicationType.Find(8);
        clsApplication _RetakeTestApplication = new clsApplication();

        clsUser _User;

        int _TestTypeID, _AppID;
        enMode _Mode;
        bool isRetak;
        public frmAddAppointment(clsLocalLicenseApplication ldlApplication, int TestTypeID, int AppID, clsUser Uesr)
        {
            InitializeComponent();
            _LDLApplication = ldlApplication;
            _TestTypeID = TestTypeID;
            _AppID = AppID;
            _User = Uesr;

            if(_AppID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
        }

        private void frmAddAppointment_Load(object sender, EventArgs e)
        {
            
            _Application = clsApplication.Find(_LDLApplication.LDLApplicationID);
            lbDLAPPID.Text = _LDLApplication.LDLApplicationID.ToString();
            lbClass.Text = clsLicenseClass.Find(_LDLApplication.LicenseClassID).ClassName;

            clsPerson person = clsPerson.Find(_Application.ApplicantPersonID);
            lbName.Text = person.FirstName + " " + person.SecondName + " " + person.ThirdName + " " + person.LastName;

            lbTrial.Text = clsAppointment.GetTrials(_LDLApplication.LDLApplicationID, _TestTypeID).ToString();

            lbFees.Text = Convert.ToInt32(clsTestType.Find(_TestTypeID).TestTypeFees).ToString();

            if (clsAppointment.GetTrials(_LDLApplication.LDLApplicationID, _TestTypeID) >= 1)
            {
                

                isRetak = true;
                gbRtTest.Enabled = true;
                int Fees = int.Parse(lbFees.Text);
                int TotalFees = Fees + Convert.ToInt32(_RetakeTestApplicationType.ApplicationTypeFees);
                lbRetakFees.Text = 5.ToString();
                lbTotalFees.Text = TotalFees.ToString();
            }

            if (_Mode == enMode.AddNew)
            {
                _Appointment = new clsAppointment();
                return;
            }

            _Appointment = clsAppointment.Find(_LDLApplication.LDLApplicationID, _TestTypeID);
            
            if(_Appointment == null)
            {
                MessageBox.Show("No Test Appointment Found. This Form Will Be Closed");
                this.Close();
            }

            dtAppDate.Value = _Appointment.AppointmentDate;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _retakeTestApplication()
        {
            _RetakeTestApplication.ApplicantPersonID = _User.PersonID;
            _RetakeTestApplication.ApplicationDate = DateTime.Now;
            _RetakeTestApplication.ApplicationTypeID = 8;
            _RetakeTestApplication.ApplicationStatus = 1;
            _RetakeTestApplication.LastStatusDate = DateTime.Now;
            _RetakeTestApplication.PaidFees = _RetakeTestApplicationType.ApplicationTypeFees;
            _RetakeTestApplication.CreatedByUserID = _User.UserID;
            _RetakeTestApplication.Save();

            lbRetakeAppID.Text = _RetakeTestApplication.ApplicationID.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(isRetak == true)
            {
                _retakeTestApplication();
                _Appointment.RetakeTestApplicationID = _RetakeTestApplication.ApplicationID;
            }


            _Appointment.TestTypeID = _TestTypeID;
            _Appointment.LDLApplicationID = Convert.ToInt32(lbDLAPPID.Text);
            _Appointment.AppointmentDate = dtAppDate.Value;

            _Appointment.CreatedByUserID = _User.UserID;
            _Appointment.isLocked = false;

            if (isRetak)
            {
                _Appointment.PaidFees = Convert.ToDecimal(lbFees.Text) + _RetakeTestApplicationType.ApplicationTypeFees;
            }
            else
            {
                _Appointment.PaidFees = Convert.ToDecimal(lbFees.Text);
            }


            if (_Appointment.Save())
            {
                MessageBox.Show("Appointment Saved Successfully");
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Appointment NOT Saved");
            }
        }
    }
}
