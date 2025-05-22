using DriverBusinessLayer;
using DVLDManagement.Properties;
using LicenseClassesBusinessLayer;
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
    public partial class crtIntLicenseInfo : UserControl
    {
        public crtIntLicenseInfo()
        {
            InitializeComponent();
        }

        public void _LoadData(clsILicense IntLicense, clsPerson Person)
        {


            lbFullName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
            lbIntLicenseID.Text = IntLicense.InternationalLicenseID.ToString();
            lbLocalLicenseID.Text = IntLicense.LocalLicenseID.ToString();
            lbNationalNo.Text = Person.NationalityCountryID.ToString();

            switch (Person.Gendor)
            {
                case 0:
                    lbGendor.Text = "Male";
                    break;
                case 1:
                    lbGendor.Text = "Female";
                    break;
                default:
                    break;
            }

            lbIssueDate.Text = IntLicense.IssueDate.ToString("yyyy-MM-dd");

            switch (IntLicense.isActive)
            {
                case false:
                    lbIsActive.Text = "NO";
                    break;
                case true:
                    lbIsActive.Text = "YES";
                    break;
                default:
                    break;
            }

            lbApplicationID.Text = IntLicense.ApplicationID.ToString();
            lbDateOfBirth.Text = Person.DateOfBirth.ToString("yyyy-MM-dd");
            lbDriverID.Text = IntLicense.DriverID.ToString();
            lbExpDate.Text = IntLicense.ExpirationDate.ToString("yyyy-MM-dd");

            switch (Person.Gendor)
            {
                case 0:
                    lbGendor.Text = "Male";
                    pictureBoxImg.Image = Resources.Male_512;
                    break;
                case 1:
                    lbGendor.Text = "Female";
                    pictureBoxImg.Image = Resources.Female_512;
                    break;
            }

            if (Person.ImagePath != "")
            {
                pictureBoxImg.Load(Person.ImagePath);
            }

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
