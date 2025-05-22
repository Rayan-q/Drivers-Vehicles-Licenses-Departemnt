using System;
using System.Data;
using System.Runtime.CompilerServices;
using AppointmentsDataAccessLayer;

namespace Appointments_And_Tests_Business_Layer
{
    enum enMode { AddNew = 1, Update = 2}
    public class clsAppointment
    {
        public int AppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LDLApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool isLocked { get; set; }
        public int Trials {  get; set; }
        public int RetakeTestApplicationID { get; set; }

        enMode _Mode;

        public clsAppointment()
        { 
            this.AppointmentID = 0;
            this.TestTypeID = 0;
            this.LDLApplicationID = 0;
            this.CreatedByUserID = 0;
            this.PaidFees = 0;
            this.isLocked = false;
            this.AppointmentDate = DateTime.Now;
            this.Trials = 0;
            this.RetakeTestApplicationID = 0;
            _Mode = enMode.AddNew;
        }

        private clsAppointment(int appointmentID, int testTypeID, int lDLApplicationID, DateTime appointmentDate, decimal paidFees, int createdByUserID, bool isLocked, int trials, int RetakeTestApplication)
        {
            AppointmentID = appointmentID;
            TestTypeID = testTypeID;
            LDLApplicationID = lDLApplicationID;
            AppointmentDate = appointmentDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            this.isLocked = isLocked;
            Trials = trials;
            this.RetakeTestApplicationID = RetakeTestApplication;

            _Mode = enMode.Update;
        }

        public static DataTable GetAllAppointmentsWihtType(int LDLApplication, int AppTypeID)
        {
            return clsAppointmentDataAccess.GetAllAppointmentsWithTypeAndApplicant(LDLApplication, AppTypeID);
        }

        public static int GetTrials(int LDLApplicationID, int TestTypeID)
        {
            return clsAppointmentDataAccess.GetNumberOfTrials(LDLApplicationID, TestTypeID);
        }

        public static clsAppointment Find(int LDLApplicationID, int testTypeID)
        {
            int AppID = 0, TestTypeID = 0, CreatedByUserID = 0, Trials = 0, RetakeTestApplicationID = 0;
            DateTime AppointmentDate = DateTime.Now;
            decimal PaidFees = 0;
            bool isLocked = false;

            Trials = clsAppointmentDataAccess.GetNumberOfTrials(LDLApplicationID, testTypeID);

            if (clsAppointmentDataAccess.FindAppointmentByLDLAppID(LDLApplicationID, ref AppID, ref TestTypeID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref isLocked, ref RetakeTestApplicationID))
            {
                return new clsAppointment(AppID, TestTypeID, LDLApplicationID, AppointmentDate, PaidFees, CreatedByUserID, isLocked, Trials, RetakeTestApplicationID);
            }
            else
                return null;
        }

        public static bool isActiveAppExist(int LDLApplicationID, int AppointmentType)
        {
            return clsAppointmentDataAccess.isActiveTestTypeExistInAppointment(LDLApplicationID, AppointmentType);
        }

        private bool _AddNewAppointment()
        {
            this.AppointmentID = clsAppointmentDataAccess.AddNewAppointment(this.TestTypeID, this.LDLApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.isLocked, this.RetakeTestApplicationID);
            return (this.AppointmentID != -1);
        }

        private bool _UpdateAppointment()
        {
            return clsAppointmentDataAccess.UpdateAppointment(this.AppointmentID, this.AppointmentDate, this.isLocked);
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateAppointment();
             
            }
            return false;
        }
    }
    public class clsTest
    {
        public int TestID { get; set; }
        public int AppointmentID { get; set; }
        public bool TestResult { get; set; }
        public string Notes {  get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest()
        {
            this.TestID = 0;
            this.AppointmentID = 0;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = 0;
        }


        public static bool isTestPassed(int LDLApplication, int TestType)
        {

            bool isPassed = clsAppointmentDataAccess.isTestPassed(LDLApplication, TestType);
            return isPassed;
        }

        private bool _AddNewTest()
        {
            this.TestID = clsAppointmentDataAccess.AddNewTest(this.AppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return  (this.TestID != -1);
        }

        public bool Save()
        {
            return _AddNewTest();
        }
    }

    public class clsWrittenTest
    {
        public int WrittenTestID { get; set; }
        public int TestID { get; set; }
        public int Score { get; set; }


        public clsWrittenTest()
        {
            this.WrittenTestID = 0;
            this.TestID = 0;
            this.Score = 0;
        }

        private bool _AddNewWrittenTest()
        {
            int? isNull = clsAppointmentDataAccess.AddNewWrittenTest(this.TestID, this.Score);

            if(isNull != null)
            {
                this.WrittenTestID = (int)isNull;
            }

            return (isNull != null);
        }

        public bool SubmitTest()
        {
            return _AddNewWrittenTest();
        }
    }
}
