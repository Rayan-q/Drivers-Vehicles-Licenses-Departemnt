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

namespace DVLDManagement.Manage_LDLApplications
{
    public partial class frmApplicationDetails : Form
    {
        clsLocalLicenseApplication _LDLApplication;
        public frmApplicationDetails(clsLocalLicenseApplication lDLApplication)
        {
            InitializeComponent();
            _LDLApplication = lDLApplication;
        }

        private void frmApplicationDetails_Load(object sender, EventArgs e)
        {
            crtApplicationinfo1._LoadLDLApplicationData(_LDLApplication);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
