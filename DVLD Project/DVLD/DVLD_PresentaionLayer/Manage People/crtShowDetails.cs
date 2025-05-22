using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PersonBusinessLayer;
using CountriesBusinessLayer;
using static DVLDManagement.crtPersonCard;
using System.Resources;
using DVLDManagement.Properties;
using DriverBusinessLayer;

namespace DVLDManagement.People_Management
{
    public partial class crtShowDetails : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action OnActionComplete;
        // Create a protected method to raise the event with a parameter
        protected virtual void ActionOnComplete()
        {
            Action handler = OnActionComplete;
            if (handler != null)
            {
                handler(); // Raise the event with the parameter
            }
        }
        clsPerson _Person;
        public crtShowDetails()
        {
            InitializeComponent();
        }

        public void _LoadData(clsPerson Person)
        {
            _Person = Person;

            if (_Person == null)
            {
                MessageBox.Show("No Contact Found This Form Will Be Closed");
                return;
            }
            lbID.Text = _Person.PersonId.ToString();
            lbFullName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lbNationalNo.Text = _Person.NationalNo;
            lbDateOfBirth.Text = _Person.DateOfBirth.ToString("yyyy-MM-dd");
            lbPhone.Text = _Person.Phone;
            lbEmail.Text = _Person.Email;
            lbAddress.Text = _Person.Address;

            switch (_Person.Gendor)
            {
                case 0:
                    lbGendor.Text = "Male";
                    pictureBox1.Image = Resources.Male_512;
                    break;
                case 1:
                    lbGendor.Text = "Female";
                    pictureBox1.Image = Resources.Female_512;
                    break;
            }

            if (_Person.ImagePath != "")
            {
                pictureBox1.Load(_Person.ImagePath);
            }
            lbCountry.Text = clsCountry.Find(_Person.NationalityCountryID).CountryName;
        }

        private void llbEditInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson addPerson = new frmAddEditPerson(_Person.PersonId);
            addPerson.ShowDialog();

            //Update Person Info after Editing
            _Person = clsPerson.Find(_Person.PersonId);
            _LoadData(_Person);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ActionOnComplete();
        }

       
        private void crtShowDetails_Load(object sender, EventArgs e)
        {

            
        }

        private void lbDateOfBirth_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
