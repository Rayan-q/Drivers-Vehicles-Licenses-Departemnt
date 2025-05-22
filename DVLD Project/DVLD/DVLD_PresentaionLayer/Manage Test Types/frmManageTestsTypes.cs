using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTypeBusiness;

namespace DVLDManagement.Manage_Test_Types
{
    public partial class frmManageTestsTypes : Form
    {
        public frmManageTestsTypes()
        {
            InitializeComponent();
        }

        private void _RefreshTestTypesList()
        {
            dataGridView1.DataSource = clsTestType.GetAllTestTypes();
            lbNumOfRecrods.Text = dataGridView1.Rows.Count.ToString();
        }

        private void frmManageTestsTypes_Load(object sender, EventArgs e)
        {
            _RefreshTestTypesList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType manageTestsTypes = new frmUpdateTestType((int)dataGridView1.CurrentRow.Cells[0].Value);
            manageTestsTypes.ShowDialog();
            _RefreshTestTypesList();
        }
    }
}
