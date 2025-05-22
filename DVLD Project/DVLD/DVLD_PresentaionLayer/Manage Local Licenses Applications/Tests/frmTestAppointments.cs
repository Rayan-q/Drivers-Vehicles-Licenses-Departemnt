using ApplicationBusinessLayer;
using Appointments_And_Tests_Business_Layer;
using LDLApplicationBusinessLayer;
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
    public partial class frmTestAppointments : Form
    {
        clsUser _User;
        clsLocalLicenseApplication _LDLAppliction;
        int _TestType = 0;
        clsApplication _Application;
        public frmTestAppointments(clsLocalLicenseApplication LDLAppliction, int AppType, clsUser User)
        {
            InitializeComponent();
            _User = User;
            _LDLAppliction = LDLAppliction;
            _TestType = AppType;
        }

        private void _RefrishAppointmentsList()
        {
            dgvAppointments.DataSource = clsAppointment.GetAllAppointmentsWihtType(_LDLAppliction.LDLApplicationID, _TestType);
            lbNumOfRecrods.Text = dgvAppointments.Rows.Count.ToString();
        }

        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            switch(_TestType)
            {
                case 1:
                    lbCurrentAction.Text = "Vison Test Appointment";
                    break;
                case 2:
                    lbCurrentAction.Text = "Written Test Appointment";
                    break;
                case 3:
                    lbCurrentAction.Text = "Street Test Appointment";
                    break;
            }
            _RefrishAppointmentsList();
            crtApplicationinfo1._LoadLDLApplicationData(_LDLAppliction);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddLDLApplication_Click(object sender, EventArgs e)
        {

            if (!_isApplicationValid())
            {
                return;
            }

            _Application = clsApplication.Find(_LDLAppliction.LDLApplicationID);
            frmAddAppointment addAppointment = new frmAddAppointment(_LDLAppliction, _TestType, -1, _User);
            addAppointment.ShowDialog();
            _RefrishAppointmentsList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsAppointment.isActiveAppExist(_LDLAppliction.LDLApplicationID, _TestType))
            {
                MessageBox.Show("This Person Already Sat For This Test,");
                return;
            }
            _Application = clsApplication.Find(_LDLAppliction.LDLApplicationID);
            frmAddAppointment addAppointment = new frmAddAppointment(_LDLAppliction, _TestType, _LDLAppliction.LDLApplicationID, _User);
            addAppointment.ShowDialog();
            _RefrishAppointmentsList();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsAppointment.isActiveAppExist(_LDLAppliction.LDLApplicationID, _TestType))
            {
                MessageBox.Show("This Person Already Sat For This Test,");
                return;
            }

            _Application = clsApplication.Find(_LDLAppliction.LDLApplicationID);

            frmTakeTest TakeTest = new frmTakeTest(_LDLAppliction, _TestType);
            TakeTest.ShowDialog();
            _RefrishAppointmentsList();
        }

        private bool _isApplicationValid()
        {
            bool isValid = true;

            if (clsTest.isTestPassed(_LDLAppliction.LDLApplicationID, _TestType))
            {
                MessageBox.Show("This Person Has Already Passed This Test");
                isValid = false;
            }

            if (clsAppointment.isActiveAppExist(_LDLAppliction.LDLApplicationID, _TestType))
            {
                MessageBox.Show("This Person Already Has An Active Appointment");
                isValid = false;
            }
            return isValid;
        }
    }
}
