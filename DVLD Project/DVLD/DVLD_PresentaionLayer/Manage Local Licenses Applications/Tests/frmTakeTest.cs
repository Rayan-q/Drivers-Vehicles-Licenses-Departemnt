using ApplicationBusinessLayer;
using Appointments_And_Tests_Business_Layer;
using DVLDManagement.Manage_LDLApplications.Tests;
using LDLApplicationBusinessLayer;
using LicenseClassesBusinessLayer;
using PersonBusinessLayer;
using System;
using System.Windows.Forms;
using TestTypeBusiness;

namespace DVLDManagement.Manage_LDLApplications
{

    public partial class frmTakeTest : Form
    {
        clsLocalLicenseApplication _LDLApplication;
        clsApplication _Application;
        clsAppointment _Appointment;
        clsTest _Test = new clsTest();
        int _TestTypeID;
        public frmTakeTest(clsLocalLicenseApplication ldlApplication, int TestTypeID)
        {
            InitializeComponent();
            _LDLApplication = ldlApplication;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            if(_TestTypeID == 2)
            {
                rdFail.Visible = false;
                rdPass.Visible = false;
                btnStartTest.Visible = true;
                btnSave.Visible = false;
            }

            _Application = clsApplication.Find(_LDLApplication.LDLApplicationID);

            label1.Text = _LDLApplication.LDLApplicationID.ToString();
            lbClass.Text = clsLicenseClass.Find(_LDLApplication.LicenseClassID).ClassName;

            clsPerson person = clsPerson.Find(_Application.ApplicantPersonID);
            lbName.Text = person.FirstName + " " + person.SecondName + " " + person.ThirdName + " " + person.LastName;

            lbFees.Text = Convert.ToInt32(clsTestType.Find(_TestTypeID).TestTypeFees).ToString();

            _Appointment = clsAppointment.Find(_LDLApplication.LDLApplicationID, _TestTypeID);
            lbDate.Text = _Appointment.AppointmentDate.ToString("yyyy-MM-dd");

            lbTrial.Text = _Appointment.Trials.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Test.AppointmentID = _Appointment.AppointmentID;
            _Test.Notes = rtxNotes.Text;
            _Test.CreatedByUserID = _Appointment.CreatedByUserID;

            if (MessageBox.Show("Are You Sure You Want To Save? You Cannot Change Pass/Fail Results After Saving", "Confirm Cancel", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (_Test.Save())
                {
                    MessageBox.Show("Test Taken Successfully.");

                }

                else
                { 
                    MessageBox.Show("Test is NOT Taken.");
                    return;
                }
                lbTestID.Text = _Test.TestID.ToString();
                _Appointment.isLocked = true;
                if (_Appointment.Save())
                {
                    MessageBox.Show("The Test Was Set And This Appointment Will Be Locked");
                }
                else
                {
                    MessageBox.Show("The Test Was NOT Set And This Appointment Is Yet To Be Locked");
                }
            }
            
            

        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            _Test.AppointmentID = _Appointment.AppointmentID;
            _Test.Notes = rtxNotes.Text;
            _Test.CreatedByUserID = _Appointment.CreatedByUserID;

            frmWrittenTest WrittenTest = new frmWrittenTest(_Test, _Appointment);
            WrittenTest.SendResult += ReceiveResult;
            WrittenTest.ShowDialog();
        }

        private void ReceiveResult(object sender, bool Result)
        {
            if(Result)
            {
                lbResult.Text = "Passed";
            }
            else
            {
                lbResult.Text = "Failed";
            }
            btnStartTest.Enabled = false;
            _Test.TestResult = Result;
        }

        private void rdPass_CheckedChanged(object sender, EventArgs e)
        {
            lbResult.Text = "Passed";
            _Test.TestResult = true;
        }

        private void rdFail_CheckedChanged(object sender, EventArgs e)
        {
            lbResult.Text = "Failed";
            _Test.TestResult = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
