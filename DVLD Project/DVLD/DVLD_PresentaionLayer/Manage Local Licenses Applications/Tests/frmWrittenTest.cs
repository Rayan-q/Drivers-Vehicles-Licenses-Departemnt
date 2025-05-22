using Appointments_And_Tests_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagement.Manage_LDLApplications.Tests
{
    public partial class frmWrittenTest : Form
    {
        int _Score = 0;
        public bool? _isPassed = null;
        clsTest _Test = new clsTest();
        clsWrittenTest _WrittenTest = new clsWrittenTest();
        clsAppointment _Appointment = new clsAppointment();
        public frmWrittenTest(clsTest testID, clsAppointment appointment)
        {
            InitializeComponent();
            _Test = testID;
            _Appointment = appointment;
        }
        public delegate void SendResultBack(object sender, bool Result);
        public event SendResultBack SendResult;
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(tbQs.SelectedIndex == 8 )
            {
                btnNext.Visible = false;
            }
            if (tbQs.SelectedIndex == 0)
            {
                button1.Visible = true;
            }
            tbQs.SelectedIndex += 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tbQs.SelectedIndex == 1)
            {
                button1.Visible = false;
            }
            if (tbQs.SelectedIndex == 9)
            {
                btnNext.Visible = true;
            }

            tbQs.SelectedIndex -= 1;
        }

        private void tbQs_Click(object sender, EventArgs e)
        {
            if (tbQs.SelectedIndex == 0)
            {
                button1.Visible = false;
            }
            else
            {
                button1.Visible = true;
            }

            if (tbQs.SelectedIndex == 9)
            {
                btnNext.Visible = false;
            }
            else
            {
                btnNext.Visible = true;
            }
        }

        

        private void _SetScore()
        {
            if (rdAnswer1.Checked)
                _Score++; rdAnswer1.ForeColor = Color.Green;
            if (rdAnswer2.Checked)
                _Score++; rdAnswer2.ForeColor = Color.Green;
            if (rdAnswer3.Checked)
                _Score++; rdAnswer3.ForeColor = Color.Green;
            if (rdAnswer4.Checked)
                _Score++; rdAnswer4.ForeColor = Color.Green;
            if (rdAnswer5.Checked)
                _Score++; rdAnswer5.ForeColor = Color.Green;
            if (rdAnswer6.Checked)
                _Score++; rdAnswer6.ForeColor = Color.Green;
            if (rdAnswer7.Checked)
                _Score++; rdAnswer7.ForeColor = Color.Green;
            if (rdAnswer8.Checked)
                _Score++; rdAnswer8.ForeColor = Color.Green;
            if (rdAnswer9.Checked)
                _Score++; rdAnswer9.ForeColor = Color.Green;
            if (rdAnswer10.Checked)
                _Score++; rdAnswer10.ForeColor = Color.Green;
        }

        private void _saveTest()
        {
            _Test.TestResult = (bool)_isPassed;

            if (_Test.Save())
            {
                MessageBox.Show("Test Taken Successfully");
            }
            else
            {
                MessageBox.Show("Failed To Take Test");
            }
        }

        private void _saveAppointment()
        {
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            crtTestTimer1.Stop();
            _SetScore();

            lbScore.Text = _Score.ToString();
            if(_Score >= 5)
            {
                _isPassed = true;
            }
            else
            {
                _isPassed = false;
            }

            _saveTest();

            _WrittenTest.TestID = _Test.TestID;
            _WrittenTest.Score = _Score;

            if(_WrittenTest.SubmitTest())
            {
                MessageBox.Show("Score is recorded");
            }
            else
            {
                MessageBox.Show("Score is NOT recorded");
            }
            _saveAppointment();

            btnSubmit.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(_isPassed != null)
            {
                SendResult?.Invoke(this, (bool)_isPassed);
            }
            this.Close();
        }

        private void crtTestTimer1_TimeUp(object sender, EventArgs e)
        {
            btnSubmit_Click(null, e);
        }

        private void frmWrittenTest_Load(object sender, EventArgs e)
        {
            lbTestTime.Text = crtTestTimer1.SetTestTimeMinutes.ToString();
            crtTestTimer1.Start();
        }
    }
}
