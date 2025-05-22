using DriverBusinessLayer;
using DVLDManagement.Manage_International_Licenses;
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

namespace DVLDManagement.Manage_LDLApplications
{
    public partial class frmLicenseHistory : Form
    {
        int _PersonID;
        public frmLicenseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void _RefreshLocalList()
        {
            dgvLocalLicenses.DataSource = clsLLicense.GetLocalLicenseHistory(_PersonID);
            dgvInternationalLicenses.DataSource = clsILicense.GetInternationalLicesenHistory(_PersonID);

            clsPerson Person = clsPerson.Find(_PersonID);
            crtShowDetails1._LoadData(Person);
            lbNumOfRecrods.Text = dgvLocalLicenses.Rows.Count.ToString();
            lbInterNationalRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            _RefreshLocalList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicenses.CurrentRow.Cells[0].Value;

            clsPerson Person = clsPerson.Find(_PersonID);
            clsLLicense License = clsLLicense.GetLDLicenseInfoByID(LicenseID);

            frmLicenseInfo ShowLicense = new frmLicenseInfo(License, Person);

            ShowLicense.Show();
        }

        private void showDetailsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;

            clsPerson Person = clsPerson.Find(_PersonID);
            clsILicense License = clsILicense.Find(LicenseID);

            frmShowInternationalLicenseInfo ShowLicense = new frmShowInternationalLicenseInfo(Person, License);

            ShowLicense.Show();
        }
    }
}
