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

namespace DVLDManagement.People_Management
{
    public partial class frmShowDetails : Form
    {
        int _PersonID;
        public frmShowDetails(int PersonID)
        {
            InitializeComponent();
            if(PersonID == 0)
            {
                this.Close();
            }
            this._PersonID = PersonID;
        }

        private void frmShowDetails_Load(object sender, EventArgs e)
        {
            clsPerson Person = clsPerson.Find(this._PersonID);
            crtShowDetails1._LoadData(Person);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbCurrentAction_Click(object sender, EventArgs e)
        {

        }
    }
}
