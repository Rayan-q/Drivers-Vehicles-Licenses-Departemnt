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
    public partial class frmAddEditPerson : Form
    {
        int _PersonID;
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }
        public delegate void PersonIDSendHandler(object sender, int PersonID);
        public event PersonIDSendHandler SendPersoinID;

        private void frmAddPerson_Load(object sender, EventArgs e)
        {
            crtPersonCard1.Execute(_PersonID);
        }

        private void crtPersonCard1_OnActionComplete()
        {
            
        }

        private void crtPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            int PersonID;
            if (crtPersonCard1.PersonID != 0)
            {
                PersonID = crtPersonCard1.PersonID;

                SendPersoinID?.Invoke(this, PersonID);
            }

            this.Close();
        }
    }
}
