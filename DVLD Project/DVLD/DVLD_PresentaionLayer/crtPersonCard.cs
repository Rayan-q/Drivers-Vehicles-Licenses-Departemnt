using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PersonBusinessLayer;
using CountriesBusinessLayer;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using System.IO;
using DVLDManagement.Properties;


namespace DVLDManagement
{
    public partial class crtPersonCard : UserControl
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
        public enum enMode { AddNew = 0, Update = 1}
        clsPerson _Person;
        private int _PersonID;

        public int PersonID = 0;

        enMode _Mode;
        public crtPersonCard()
        {
            InitializeComponent();
        }

        private void _SpecifyMode(int PersonID)
        {
            _PersonID = PersonID;

            if(PersonID == -1)
            {
                lbCurrentAction.Text = "Add A New Person";
                _Mode = enMode.AddNew;
            }
            else
                _Mode = enMode.Update;
        }

        private void _FillCountriesInComboBox()
        {
            DataTable CountriesTable = clsCountry.GetAllCountries();

            foreach (DataRow row in CountriesTable.Rows)
            {
                cboxCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _LoadData(int PersonID)
        {
            _FillCountriesInComboBox();
            cboxCountry.SelectedIndex = 0;

            _SpecifyMode(PersonID);

            if(_Mode == enMode.AddNew)
            {
                _Person = new clsPerson();
                lbCurrentAction.Text = "Add A New Person";
                rdFemale.Checked = true;
                return;
            }

            _Person = clsPerson.Find(_PersonID);

            if( _Person == null )
            {
                MessageBox.Show("No Contact Found This Form Will Be Closed");
                ActionOnComplete();
                return;
            }
            lbID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dateBirthDate.Value = _Person.DateOfBirth;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            rtxtAddress.Text = _Person.Address;

            switch(_Person.Gendor)
            {
                case 0:
                    rdMale.Checked = true;
                    pictureBox1.Image = Resources.Male_512;
                    break;
                case 1:
                    rdFemale.Checked = true;
                    pictureBox1.Image = Resources.Female_512;
                    break;
            }
            
            if(_Person.ImagePath != "")
            {
                pictureBox1.Load(_Person.ImagePath);
            }

            llRemoveImage.Visible = (_Person.ImagePath != "");

            cboxCountry.SelectedIndex = cboxCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateForm())
            {
                MessageBox.Show("Check there is not an error");
                return;
            }
            int CountryID = clsCountry.Find(cboxCountry.Text).CountryID;
            
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dateBirthDate.Value;
            _Person.Phone = txtPhone.Text;
            _Person.Email = txtEmail.Text;
            _Person.Address = rtxtAddress.Text;
            _Person.NationalityCountryID = CountryID;

            if (rdFemale.Checked == true)
            {
                _Person.Gendor = 1;
            }
            else
                _Person.Gendor = 0;

            if(pictureBox1.ImageLocation == null)
            {
                _Person.ImagePath = "";
            }

            if (_Person.Save())
            {
                MessageBox.Show("Person saved successfully");
            }
            else
            {
                MessageBox.Show("Error: Person not saved ");
            }

            _Mode = enMode.Update;
            lbCurrentAction.Text = "Edit Person Info";
            lbID.Text = _Person.PersonId.ToString();
            PersonID = _Person.PersonId;

        }

        // Destination Directory
        private string _ProfilePictureDirectory = "C:\\Programming\\19 DVLD Project\\DVLDProfilePictures";


        private void llblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedImagePath = openFileDialog1.FileName;               
                pictureBox1.Load(selectedImagePath);
                // ...
                string fileName = Path.GetFileName(selectedImagePath);

                // Ensure destination directory exists
                if (!Directory.Exists(_ProfilePictureDirectory))
                {
                    // Create one if it doesn't exist
                    Directory.CreateDirectory(_ProfilePictureDirectory);
                }

                // Determine the destination path for the new image
                string destinationPath = Path.Combine(_ProfilePictureDirectory, fileName);

                //if (_Person.ImagePath != null && File.Exists(_Person.ImagePath))
                //{
                //    File.Delete(_Person.ImagePath);
                //}

                File.Copy(selectedImagePath, destinationPath, true);

                _Person.ImagePath = destinationPath;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.ImageLocation = null;
            switch (_Person.Gendor)
            {
                case 0:
                    rdMale.Checked = true;
                    pictureBox1.Image = Resources.Male_512;
                    break;
                case 1:
                    rdFemale.Checked = true;
                    pictureBox1.Image = Resources.Female_512;
                    break;
            }
            llRemoveImage.Visible = false;
        }

        public void Execute(int PersonID)
        {
            _LoadData(PersonID);
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            // Check National Number is unique
            if (clsPerson.Find(txtNationalNo.Text) != null && _Mode == enMode.AddNew)
            {
                errorProvider1.SetError(txtNationalNo, "National Number Is Taken");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }

            // Check Phone number is required
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone Is Required");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Email is required.");
                isValid = false;
            }
            // Check email format
            else if (!IsValidEmail(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Invalid email format.");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtEmail, ""); // Clear error if valid
            }

            return isValid;
        }

        private void txtBoxes_Validating(object sender, CancelEventArgs e)
        {
            // Check National Number is unique
            if (clsPerson.Find(txtNationalNo.Text) != null && _Mode == enMode.AddNew)
            {
                errorProvider1.SetError(txtNationalNo, "National Number Is Taken");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }

            // Check Phone number is required
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone Is Required");
            }
            else
            {
                errorProvider1.SetError(txtPhone, "");
            }

            // Check email format
            if (!IsValidEmail(txtEmail.Text))
            {
                errorProvider1.SetError(txtEmail, "Invalid email format.");
            }
            else
            {
                errorProvider1.SetError(txtEmail, ""); // Clear error if valid
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }


        private void rdMale_CheckedChanged(object sender, EventArgs e)
        {
            if (_Person.ImagePath == "")
            {
                pictureBox1.Image = Resources.Male_512;
            }
        }

        private void rdFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (_Person.ImagePath == "")
            {
                pictureBox1.Image = Resources.Female_512;
            }
        }
    }
}
