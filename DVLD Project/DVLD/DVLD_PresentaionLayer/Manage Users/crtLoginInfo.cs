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
    public partial class crtLoginInfo : UserControl
    {
        public crtLoginInfo()
        {
            InitializeComponent();
        }

        public void _LoadData(clsUser User)
        {
            lbUserID.Text = User.UserID.ToString();
            lbUserName.Text = User.UserName;

            switch(User.isActive)
            {
                case true:
                    lbisActive.Text = "Yes";
                    break;
                case false:
                    lbisActive.Text = "No";
                break;
            }
        }
        private void crtLoginInfo_Load(object sender, EventArgs e)
        {
        }
    }
}
