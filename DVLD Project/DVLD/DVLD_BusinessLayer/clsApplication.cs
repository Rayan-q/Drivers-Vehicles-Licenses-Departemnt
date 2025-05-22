using System;
using System.Data;
using ApplicationsDataAccessLayer;

namespace ApplicationBusinessLayer
{
    enum enMode { AddNew = 1, Update = 2}
    public class clsApplication
    {
        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ApplicationStatus {  get; set; }
        public DateTime LastStatusDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        enMode _Mode;

        public clsApplication ()
        {
            ApplicationID = 0;
            ApplicantPersonID = 0;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = 0;
            ApplicationStatus = 0;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = 0;
            _Mode = enMode.AddNew;
        }

        private clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID, int applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
            _Mode = enMode.Update;
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationDataAccess.GetAllApplications();
        }

        public static clsApplication Find(int LDLApplication)
        {
            int applicationID = 0, applicantPersonID = 0, applicationTypeID = 0, applicationStatus = 0, createdByUserID = 0;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;

            if (clsApplicationDataAccess.GetApplicatoinInfoByLDLApplicationID(LDLApplication, ref applicationID, ref applicantPersonID, ref applicationDate, ref applicationTypeID, ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                return new clsApplication(applicationID, applicantPersonID, applicationDate, applicationTypeID, applicationStatus, lastStatusDate, paidFees, createdByUserID);
            }
            else
                return null;
        }

        private bool _AddApplication()
        {
            this.ApplicationID = clsApplicationDataAccess.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate, this.ApplicationTypeID , this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            this.LastStatusDate = DateTime.Now;
            return clsApplicationDataAccess.UpdateApplication(this.ApplicationID, this.ApplicationStatus, this.LastStatusDate);
        }

        public static bool CancelApplication(int ApplicationID)
        {
            return clsApplicationDataAccess.CancelApplication(ApplicationID);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddApplication())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateApplication();
            }
            return false;
        }

    }
}
