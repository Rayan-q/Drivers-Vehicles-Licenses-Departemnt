using DriverBusinessLayer;
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

namespace DVLDManagement.Manage_International_Licenses
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        clsPerson _Person;
        clsILicense _IntLicense;
        public frmShowInternationalLicenseInfo(clsPerson Person, clsILicense IntLicense)
        {
            InitializeComponent();
            _Person = Person;
            _IntLicense = IntLicense;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            crtIntLicenseInfo1._LoadData(_IntLicense, _Person);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
