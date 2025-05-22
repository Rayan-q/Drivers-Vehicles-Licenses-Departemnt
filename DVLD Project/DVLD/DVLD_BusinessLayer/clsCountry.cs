using System;
using System.Data;
using CountriesDataAccess;

namespace CountriesBusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            this.CountryName = "";
        }
        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountriesDataAccess.GetAllCountries();
        }

        public static clsCountry Find(int ID)
        {

            string CountryName = "", Code = "", PhoneCode = "";

            if (clsCountriesDataAccess.GetCountryByID(ID, ref CountryName))
            {
                return new clsCountry(ID, CountryName);
            }
            else
            {
                return null;
            }

        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = 0;
            string Code = "", PhoneCode = "";
            if (clsCountriesDataAccess.GetCountryByName(CountryName, ref CountryID))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }

        }

    }
}
