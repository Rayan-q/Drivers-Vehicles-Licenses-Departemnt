using ApplicationBusinessLayer;
using DriverBusinessLayer;
using DVLDManagement.Manage_LDLApplications;
using DVLDManagement.People_Management;
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

namespace DVLDManagement.Manage_International_Licenses
{
    public partial class frmManageInternationalLicenses : Form
    {
        clsUser _User;
        clsPerson _Person;
        clsILicense _ILicense;
        DataTable _dtInternationalLicenseApplications;
        public frmManageInternationalLicenses(clsUser User)
        {
            InitializeComponent();
            _User = User;
        }

        private void _RefreshList()
        {
            _dtInternationalLicenseApplications = clsILicense.GetAllInternationalLicenses();
            dgvIntLicenses.DataSource = _dtInternationalLicenseApplications;

            lbNumOfRecrods.Text = dgvIntLicenses.Rows.Count.ToString();
        }

        private void frmManageInternationalLicenses_Load(object sender, EventArgs e)
        {
            _RefreshList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddLDLApplication_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicense frmAddILicense = new frmAddInternationalLicense(_User);
            frmAddILicense.ShowDialog();
            _RefreshList();
        }

        private void personLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvIntLicenses.CurrentRow.Cells[2].Value;

            clsDriver _Driver = clsDriver.FindByID(DriverID);
            frmLicenseHistory ShowLicenseHistory = new frmLicenseHistory(_Driver.PersonID);
            ShowLicenseHistory.ShowDialog();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvIntLicenses.CurrentRow.Cells[2].Value;
            clsDriver _Driver = clsDriver.FindByID(DriverID);

            frmShowDetails PersonDetails = new frmShowDetails(_Driver.PersonID);
            PersonDetails.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get License Info
            int LicenseID = (int)dgvIntLicenses.CurrentRow.Cells[0].Value;
            // Get Driver info
            int DriverID = (int)dgvIntLicenses.CurrentRow.Cells[2].Value;
            clsDriver _Driver = clsDriver.FindByID(DriverID);

            // Get Person Info by through driver
            _Person = clsPerson.Find(_Driver.PersonID);

            // Get International License info by Driver ID
            _ILicense = clsILicense.Find(LicenseID);

            frmShowInternationalLicenseInfo IntLicenseInfo = new frmShowInternationalLicenseInfo(_Person, _ILicense);
            IntLicenseInfo.Show();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }

            else

            {

                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsReleased.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                    //_dtDetainedLicenses.DefaultView.RowFilter = "";
                    //lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsReleased.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lbNumOfRecrods.Text = _dtInternationalLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    {
                        FilterColumn = "ApplicationID";
                        break;
                    };

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;

                case "Is Active":
                    FilterColumn = "IsActive";
                    break;


                default:
                    FilterColumn = "None";
                    break;
            }


            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
                lbNumOfRecrods.Text = dgvIntLicenses.Rows.Count.ToString();
                return;
            }



            _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            lbNumOfRecrods.Text = _dtInternationalLicenseApplications.Rows.Count.ToString();
        }
    }
}
