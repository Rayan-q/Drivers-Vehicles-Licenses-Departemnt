using ApplicationTypesBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagement.Manage_Application_Types
{
    public partial class frmUpdateApplicationType : Form
    {
        clsApplicationType _Type;
        int _TypeID;
        public frmUpdateApplicationType(int TypeID)
        {
            InitializeComponent();
            _TypeID = TypeID;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _Type = clsApplicationType.Find(_TypeID);

            lbID.Text = _Type.ApplicationTypeID.ToString();
            txtTitle.Text = _Type.ApplicationTypeTitle;
            txtFees.Text = _Type.ApplicationTypeFees.ToString();

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

            _Type.ApplicationTypeTitle = txtTitle.Text;
            _Type.ApplicationTypeFees = Convert.ToDecimal(txtFees.Text);

            if(_Type.UpdateApplicationType())
            {
                MessageBox.Show("Application Type Updated Successfully");
            }
            else
            {
                MessageBox.Show("Application Type Was NOT Updated");
            }
        }
        private bool isFormValid()
        {
            bool isValid = true;

            if(string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title Cannot Be Empty");
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
