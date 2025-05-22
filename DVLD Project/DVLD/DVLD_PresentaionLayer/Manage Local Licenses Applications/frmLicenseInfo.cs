using DriverBusinessLayer;
using LDLApplicationBusinessLayer;
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
    public partial class frmLicenseInfo : Form
    {
        clsPerson _Person;
        clsLLicense _License;
        public frmLicenseInfo(clsLLicense LicenseInfo, clsPerson Person)
        {
            InitializeComponent();
            _License = LicenseInfo;
            _Person = Person;
        }

        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {
            crtLicenseInfo1._LoadData(_License, _Person);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
