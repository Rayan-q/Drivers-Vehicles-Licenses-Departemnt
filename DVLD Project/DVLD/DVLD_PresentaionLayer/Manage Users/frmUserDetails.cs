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

namespace DVLDManagement.Users_Management
{
    public partial class frmUserDetails : Form
    {
        clsUser _User;
        clsPerson _Person;
        public frmUserDetails(int PersonID, clsUser User)
        {
            InitializeComponent();
            _User = User;
            _Person = clsPerson.Find(PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmUserDetails_Load(object sender, EventArgs e)
        {
            crtShowDetails1._LoadData(_Person);
            crtLoginInfo1._LoadData(_User);
        }
    }
}
