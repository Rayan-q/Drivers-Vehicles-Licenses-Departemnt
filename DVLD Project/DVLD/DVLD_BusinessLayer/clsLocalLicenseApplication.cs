using LDLApplicationsDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LDLApplicationBusinessLayer
{
    enum enMode { AddNew = 1, Update = 2}
    public class clsLocalLicenseApplication
    {
        public int LDLApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public int PassedTests {  get; set; }
        public int PersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        enMode _Mode;

        public clsLocalLicenseApplication()
        {
            LDLApplicationID = 0;
            ApplicationID = 0;
            LicenseClassID = 0;
            ApplicationDate = DateTime.Now;
            PassedTests = 0;
            PersonID = 0;
            _Mode = enMode.AddNew;
        }

        private clsLocalLicenseApplication(int lDLApplicationID, int ApplicationID, int personID, int licenseClassID, DateTime applicationDate, int passedTests)
        {
            LDLApplicationID = lDLApplicationID;
            this.ApplicationID = ApplicationID;
            PersonID = personID;
            LicenseClassID = licenseClassID;
            ApplicationDate = applicationDate;
            PassedTests = passedTests;
            _Mode = enMode.Update;
        }

        public static DataTable GetAllLDLApplications()
        {
            return clsLocalLicenseApplicationDataAccess.GetAllLDLApplications();
        }

        public static clsLocalLicenseApplication Find(int LDLApplicationID)
        {
            int PersonID = 0, ClassID = 0, PassedTests = 0, ApplicationID = 0;
            DateTime ApplicationDate = DateTime.Now;
            if (clsLocalLicenseApplicationDataAccess.GetApplicationInfoByID(LDLApplicationID, ref ApplicationID, ref PersonID, ref ApplicationDate, ref ClassID, ref PassedTests))
            {
                return new clsLocalLicenseApplication(LDLApplicationID, ApplicationID, PersonID, ClassID, ApplicationDate, PassedTests);
            }
            else
                return null;
        }

        private bool _AddLDLApplication()
        {
            this.LDLApplicationID = clsLocalLicenseApplicationDataAccess.AddNewLDLApplication(this.ApplicationID, this.LicenseClassID);
            return (this.LDLApplicationID != -1);
        }

        private bool _UpdateLDLApplication()
        {
            return clsLocalLicenseApplicationDataAccess.UpdateLDLApplication(this.LDLApplicationID, this.LicenseClassID);
        }

        public static bool isPersonAndClassExist(int LicenseClassID, int ApplicantID)
        {
            return clsLocalLicenseApplicationDataAccess.isLicenseClassExist(LicenseClassID, ApplicantID);
        }

        public static bool DeleteApplication(int ApplicationID, int LDLApplicationID)
        {
            return clsLocalLicenseApplicationDataAccess.DeleteApplication(ApplicationID, LDLApplicationID);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddLDLApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLDLApplication();
            }
            return false;
        }
    }
}
