using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersDataAccess;

namespace UsersBusinessLayer
{
    enum enMode { AddNew = 1, Update = 2 }
    public class clsUser
    {
        
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool isActive { get; set; }
        //public clsPerson Person { get; set;}

        enMode _Mode;

        // Get All Users
        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        public clsUser()
        {
            UserID = 0;
            PersonID = 0;
            UserName = "";
            Password = "";
            isActive = false;
            _Mode = enMode.AddNew;
        }

        private clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            UserName = userName;
            Password = password;
            this.isActive = isActive;
            _Mode = enMode.Update;
        }

        public static clsUser Find(int UserID)
        {
            int PersonID = 0;
            string UserName = "", Password = "";
            bool isActive = false;

            if(clsUsersDataAccess.GetUserInfoByID(UserID, ref PersonID, ref UserName, ref Password, ref isActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, isActive);
            }
            else
                return null;
        }

        public static clsUser Find(string UserName)
        {
            int PersonID = 0, UserID = 0;
            string Password = "";
            bool isActive = false;

            if (clsUsersDataAccess.GetUserInfoByUserName(ref UserID, ref PersonID, UserName, ref Password, ref isActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, isActive);
            }
            else
                return null;
        }

        public static bool isUserExist(int UserID)
        {
            return clsUsersDataAccess.isUserExist(UserID);
        }

        public static bool isPersonExist(int PersonID)
        {
            return clsUsersDataAccess.isUserExistByPersonID(PersonID);
        }

        public static bool isPasswordOld(int UserID, string NewPassword)
        {
            return clsUsersDataAccess.IsPasswordOld(UserID, NewPassword);
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUsersDataAccess.AddNewUser(this.PersonID, this.UserName, this.Password, this.isActive);
            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUsersDataAccess.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.isActive);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    return _UpdateUser();
            }
            return false;
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);
        }

    }
}
