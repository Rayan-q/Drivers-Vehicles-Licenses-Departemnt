using System;
using System.Data;
using PersonDataAccessLayer;

namespace PersonBusinessLayer
{
    enum enMode { Addnew = 0, Update = 1 }
    public class clsPerson
    {
        public int PersonId { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        enMode Mode;
        public clsPerson()
        {
            NationalNo = "";
            FirstName = "";
            SecondName = "";
            ThirdName = "";
            LastName = "";
            DateOfBirth = DateTime.Now;
            Gendor = 0;
            Address = "";
            Phone = "";
            Email = "";
            ImagePath = "";
            NationalityCountryID = -1;
            Mode = enMode.Addnew;
        }

        private clsPerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, string Email,
   string Phone, string Address, DateTime DateOfBirth, int Gendor, int NationalityCountryID, string ImagePath)
        {
            this.PersonId = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.ImagePath = ImagePath;
            this.NationalityCountryID = NationalityCountryID;
            Mode = enMode.Update;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonDataAccess.GetAllPeople();
        }

    

        public static clsPerson Find(int PersonID)
        {
            int Gendor = 0, NationalityCountryID = 0;
            DateTime DateOfBirth = DateTime.Now;
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPersonDataAccess.GetPersonInfoByID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref Email, ref Phone, ref Address, ref DateOfBirth, ref Gendor, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                    Email, Phone, Address, DateOfBirth, Gendor, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public static clsPerson Find(string NationaNo)
        {
            int Gendor = 0, NationalityCountryID = 0, PersonID = 0;
            DateTime DateOfBirth = DateTime.Now;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPersonDataAccess.GetPersonInfoByNationalNo(NationaNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref Email, ref Phone, ref Address, ref DateOfBirth, ref Gendor, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationaNo, FirstName, SecondName, ThirdName, LastName,
                    Email, Phone, Address, DateOfBirth, Gendor, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public static clsPerson FindByLicense(int LicenseID)
        {
            int Gendor = 0, NationalityCountryID = 0, PersonID = 0;
            DateTime DateOfBirth = DateTime.Now;
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";

            if (clsPersonDataAccess.GetPersonInfoByLicenseID(LicenseID, ref PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref Email, ref Phone, ref Address, ref DateOfBirth, ref Gendor, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                    Email, Phone, Address, DateOfBirth, Gendor, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }



        public static bool isPersonExist(int PersonID)
        {
            return clsPersonDataAccess.isPersonExist(PersonID);
        }
        private bool _AddNewPerson()
        {
            Nullable<int>PersonID = clsPersonDataAccess.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.Email, this.Phone, this.Address, this.DateOfBirth,
                this.Gendor, this.NationalityCountryID, this.ImagePath);
            this.PersonId = PersonID ?? 0;
            return (PersonID.HasValue);
        }

        private bool _UpdatePerson()
        {
            return clsPersonDataAccess.UpdatePerson(this.PersonId, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.Email, this.Phone, this.Address, this.DateOfBirth, this.Gendor, this.NationalityCountryID, this.ImagePath);
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonDataAccess.DeletePerson(PersonID);
        }

        public bool Save()
        {
           switch(Mode)
            {
                case enMode.Addnew:
                   if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }


    }
}
