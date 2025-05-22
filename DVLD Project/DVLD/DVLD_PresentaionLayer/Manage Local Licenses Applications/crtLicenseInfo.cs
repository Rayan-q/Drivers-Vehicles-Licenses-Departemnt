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

namespace DVLDManagement.Manage_LDLApplications
{
    public partial class crtLicenseInfo : UserControl
    {
        public crtLicenseInfo()
        {
            InitializeComponent();
        }


        public void _LoadData(clsLLicense License, clsPerson Person)
        {


            lbClass.Text = clsLicenseClass.Find(License.LicenseClassID).ClassName;
            lbFullName.Text = Person.FirstName + " " + Person.SecondName + " " + Person.ThirdName + " " + Person.LastName;
            lbLicID.Text = License.LicenseID.ToString();
            lbNationalNo.Text = Person.NationalityCountryID.ToString();

            switch(Person.Gendor)
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

            lbIssueDate.Text = License.IssueDate.ToString("yyyy-MM-dd");

            switch(License.IssueReason)
            {
                case 1:
                    lbIssueReason.Text = "First Time";
                    break;
                case 2:
                    lbIssueReason.Text = "Renew";
                    break;
                case 3:
                    lbIssueReason.Text = "Replacement For Damaged";
                    break;
                case 4:
                    lbIssueReason.Text = "Replacement For Lost";
                    break;
                default:
                    break;
            }
            lbNotes.Text = License.Notes;

            switch(License.isActive)
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

            lbDateOfBirth.Text = Person.DateOfBirth.ToString("yyyy-MM-dd");
            lbDriverID.Text = License.DriverID.ToString();
            lbExpDate.Text = License.ExpirationDate.ToString("yyyy-MM-dd");

            switch (clsLLicense.isDetained(License.LicenseID))
            {
                case true:
                    lbIsDetained.Text = "YES";
                    break;
                case false:
                    lbIsDetained.Text = "NO";
                    break;
                default:
                    break;
            }

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
        private void crtLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
