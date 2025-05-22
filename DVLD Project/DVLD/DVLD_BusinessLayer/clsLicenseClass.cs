using System;
using System.Data;
using LDLApplicationsDataAccessLayer;

namespace LicenseClassesBusinessLayer
{
    public class clsLicenseClass
    {

        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public int MinimumAgeAllowed { get; set; }
        public int DefaultValidityLength { get; set; }
        public decimal ClassFees { get; set; }

        public clsLicenseClass()
        {
            LicenseClassID = 0;
            ClassName = "";
            ClassDescription = "";
            MinimumAgeAllowed = 0;
            DefaultValidityLength = 0;
            ClassFees = 0;
        }

        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, int MinimumAllowedAge, int DfaultValidityLength, decimal ClassFees)
        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAgeAllowed = MinimumAllowedAge;
            this.DefaultValidityLength = DfaultValidityLength;
            this.ClassFees = ClassFees;
        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLocalLicenseApplicationDataAccess.GetAllLicenseClasses();
        }

        public static clsLicenseClass Find(string className)
        {
            string ClassDecsription = "";
            int LicenseClassID = 0, MinimumAgeAllowed = 0, DefaultValidityLength = 0;
            decimal ClassFees = 0;

            if (clsLocalLicenseApplicationDataAccess.GetClassInfoByName(className, ref LicenseClassID, ref ClassDecsription, ref MinimumAgeAllowed, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, className, ClassDecsription, MinimumAgeAllowed, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassDecsription = "", className = "";
            int MinimumAgeAllowed = 0, DefaultValidityLength = 0;
            decimal ClassFees = 0;

            if (clsLocalLicenseApplicationDataAccess.GetClassInfoByID(LicenseClassID, ref className, ref ClassDecsription, ref MinimumAgeAllowed, ref DefaultValidityLength, ref ClassFees))
            {
                return new clsLicenseClass(LicenseClassID, className, ClassDecsription, MinimumAgeAllowed, DefaultValidityLength, ClassFees);
            }
            else
                return null;
        }
    }
}
