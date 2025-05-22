using ApplicationTypesBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTypeBusiness;

namespace DVLDManagement.Manage_Test_Types
{
       public partial class frmUpdateTestType : Form
    {
        clsTestType _TestType;
        int _TestID;
        public frmUpdateTestType(int TestID)
        {
            InitializeComponent();
            _TestID = TestID;
        }

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestID);

            lbID.Text = _TestType.TestTypeID.ToString();
            txtTitle.Text = _TestType.TestTypeTitle;
            rtxTestDescription.Text = _TestType.TestTypeDescription;
            txtFees.Text = _TestType.TestTypeFees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!isFormValid())
            {
                return;
            }

            _TestType.TestTypeTitle = txtTitle.Text;
            _TestType.TestTypeDescription = rtxTestDescription.Text;
            _TestType.TestTypeFees = Convert.ToDecimal(txtFees.Text);

            if(_TestType.UpdateApplicationType())
            {
                MessageBox.Show("Test Type Updated Successfully");
            }
            else
            {
                MessageBox.Show("Test Type Was NOT Updated");
            }
        }

        private bool isFormValid()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title Cannot Be Empty");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(rtxTestDescription.Text))
            {
                MessageBox.Show("Description Cannot Be Empty");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(txtFees.Text))
            {
                MessageBox.Show("Fees Cannot Be Empty");
                isValid = false;
            }
            return isValid;
        }
    }
}
