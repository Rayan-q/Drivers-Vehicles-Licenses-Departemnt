using System;
using System.Data;
using DriversDataAccessLayer;

namespace DriverBusinessLayer
{
    public class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            DriverID = 0;
            PersonID = 0;
            CreatedByUserID = 0;
            CreatedDate = DateTime.Now;
        }

        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverDataAccess.GetAllDrivers();
        }

        public static clsDriver Find(int PersonID)
        {
            int driverID = 0, createdByUserID = 0;
            DateTime createdDate = DateTime.Now;

            if (clsDriverDataAccess.GetDriverInfo(PersonID, ref driverID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(driverID, PersonID, createdByUserID, createdDate);
            }
            else
                return null;
        }

        public static clsDriver FindByID(int DriverID)
        {
            int PersonID = 0, createdByUserID = 0;
            DateTime createdDate = DateTime.Now;

            if (clsDriverDataAccess.GetDriverInfoByID(DriverID, ref PersonID, ref createdByUserID, ref createdDate))
            {
                return new clsDriver(DriverID, PersonID, createdByUserID, createdDate);
            }
            else
                return null;
        }

        public static bool isDriverExist(int  PersonID)
        {
            return clsDriverDataAccess.isDriverExist(PersonID);
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverDataAccess.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        public bool Save()
        {
            return _AddNewDriver();
        }
    }
    public class clsLLicense
    {
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID  {get; set;}
        public int LicenseClassID {get; set;}
        public DateTime IssueDate {get; set;}
        public DateTime ExpirationDate {get; set;}
        public string Notes {get; set;}
        public decimal PaidFees {get; set;}
        public bool isActive {get; set;}
        public int IssueReason {get; set;}
        public int CreatedByUserID {get; set;}

        public clsLLicense()
        {
            this.LicenseID = 0;
            this.ApplicationID = 0;
            this.DriverID = 0;
            this.LicenseClassID = 0;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.isActive = true;
            this.IssueReason = 0;
            this.CreatedByUserID = 0;
        }

        private clsLLicense(int  licenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpDate, string Notes, decimal PaidFees, bool isActive, int IssueReason, int CreatedByUserID)
        {
            this.LicenseID= licenseID;
            this.ApplicationID= ApplicationID;
            this.DriverID= DriverID;
            this.IssueDate = IssueDate;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate;
            this.Notes = Notes;
            this.ExpirationDate = ExpDate;
            this.CreatedByUserID= CreatedByUserID;
            this.isActive= isActive;
            this.IssueReason = IssueReason;
            this.PaidFees= PaidFees;

        }

        public static DataTable GetLocalLicenseHistory(int PersonID)
        {
            return clsDriverDataAccess.GetLicenseHistory(PersonID);
        }

        public static clsLLicense GetLDLicenseInfo(int ApplicationID, int ClassID)
        {
            int LicenseID = 0, DriverID = 0, IssueReason = 0, CreatedByUserID = 0, LicenseClassID = 0;
            string Notes = "";
            DateTime IssueDate = DateTime.Now, ExpDate = DateTime.Now;

            Decimal PaidFees = 0;
            bool isActive = false;


            if (clsDriverDataAccess.GetLDLicenseInfo(ApplicationID, ClassID, ref LicenseID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpDate, ref Notes, ref PaidFees, ref isActive, ref IssueReason, ref CreatedByUserID))
                {
                return new clsLLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpDate, Notes, PaidFees, isActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsLLicense GetLDLicenseInfoByID(int LicenseID)
        {
            int DriverID = 0, ApplicationID = 0, IssueReason = 0, CreatedByUserID = 0, LicenseClassID = 0;
            string Notes = "";
            DateTime IssueDate = DateTime.Now, ExpDate = DateTime.Now;

            Decimal PaidFees = 0;
            bool isActive = false;


            if (clsDriverDataAccess.GetLDLicenseInfoByID(LicenseID, ref DriverID, ref ApplicationID, ref LicenseClassID, ref IssueDate, ref ExpDate, ref Notes, ref PaidFees, ref isActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpDate, Notes, PaidFees, isActive, IssueReason, CreatedByUserID);
            }
            else
                return null;
        }

        public bool DeactivateLicense()
        {
            this.isActive = false;
           return clsDriverDataAccess.DeactivateLDLicense(this.LicenseID);
        }

        public static bool isDetained(int LicenseID)
        {
            return clsDriverDataAccess.isLicenseDetained(LicenseID);
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsDriverDataAccess.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.isActive, this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }

        public bool Save()
        {
            return _AddNewLicense();
        }
    }

    public class clsDetain
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public decimal FineFees { get; set; }
        public int DetainedByUserID { get; set; }
        public bool isReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }

        public clsDetain()
        {
            DetainID = 0;
            LicenseID = 0;
            DetainDate = DateTime.Today;
            FineFees = 0;
            DetainedByUserID = 0;
            isReleased = false;
            ReleaseDate = DateTime.Today;
            ReleasedByUserID = 0;
            ReleaseApplicationID = 0;
        }

        private clsDetain(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, bool isReleased,
            DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;

            this.FineFees = FineFees;
            this.DetainedByUserID = CreatedByUserID;
            this.isReleased = isReleased;

            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
        }

        public static DataTable GetAllDetainedLicense()
        {
            return clsDriverDataAccess.GetAllDetainedLicenses();
        }

        public static clsDetain Find(int LicenseID)
        {
            int DetainID = 0, CreatedByUserID = 0, ReleasedByUserID = 0, ReleassApplicationID = 0;
            decimal FineFees = 0;
            DateTime DetainDate = DateTime.Today,ReleaseDate = DateTime.Today;            
            bool isReleased = false;

            if (clsDriverDataAccess.GetDetainedLicenseInfo(LicenseID, ref DetainID, ref DetainDate, ref FineFees,
                ref CreatedByUserID, ref isReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleassApplicationID))
            {
                return new clsDetain(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, isReleased, ReleaseDate, ReleasedByUserID, ReleassApplicationID);
            }
            else
                return null;
        }


        private bool _DetainLicense(int LicenseID)
        {
            this.LicenseID = LicenseID;

            this.DetainID = clsDriverDataAccess.DetainALicense(this.LicenseID, this.DetainDate, this.FineFees, this.DetainedByUserID, this.isReleased);

            return (this.DetainID != -1);
        }

        private bool _ReleaseLicense()
        {
            return clsDriverDataAccess.ReleaseLiceese(this.LicenseID, this.isReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        }

        public bool Detain(int LicenseID)
        {
            return _DetainLicense(LicenseID);
        }

        public bool Release()
        {
            return _ReleaseLicense();
        }
    }

    public class clsILicense
    {
        public int InternationalLicenseID {  get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool isActive { get; set; }
        public int CreatedByUserID { get; set; }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsDriverDataAccess.GetAllInternationalLicenses();
        }

        public static DataTable GetInternationalLicesenHistory(int PersonID)
        {
            return clsDriverDataAccess.GetInterNationalLicenseHistory(PersonID);
        }


        public clsILicense()
        {
            InternationalLicenseID = 0;
            ApplicationID = 0;
            DriverID = 0;
            LocalLicenseID = 0;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            isActive = false;
            CreatedByUserID = 0;
        }

        private clsILicense(int internationalLicenseID, int applicationID, int driverID, int localLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {
            InternationalLicenseID = internationalLicenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LocalLicenseID = localLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            this.isActive = isActive;
            CreatedByUserID = createdByUserID;
        }

        public static clsILicense Find(int IntLicenseID)
        {
            int DriverID = 0, ApplicationID = 0, LocalIicenseID = 0, CreatedByUserID = 0;
            DateTime IssueDate = DateTime.Now, ExpDate = DateTime.Now;
            bool isActive = false;

            if (clsDriverDataAccess.GetIDLicenseInfo(IntLicenseID, ref DriverID, ref ApplicationID, ref LocalIicenseID, ref IssueDate, ref ExpDate, ref isActive, ref CreatedByUserID))
            {
                return new clsILicense(IntLicenseID, ApplicationID, DriverID, LocalIicenseID, IssueDate, ExpDate, isActive, CreatedByUserID);
            }
            else
                return null;
        }

        

        public static bool isInterLicenseExist(int LocalLicensID)
        {
            return clsDriverDataAccess.isInterLicenseExist(LocalLicensID);
        }

        private bool _AddNewILicense()
        {
            this.InternationalLicenseID = clsDriverDataAccess.AddNewInernationalLicense(this.ApplicationID, this.DriverID, this.LocalLicenseID, this.IssueDate, this.ExpirationDate, this.isActive, this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }


        public bool Save()
        {
            return _AddNewILicense();
        }
    }
}
