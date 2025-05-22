using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text;

namespace DVLDManagement.Global_Classes
{
    internal static class clsGlobal
    {
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            try
            {
                string UserInfoHolderPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLDUserInfo";
                string UserNameHolder = "UserNameHolder";
                string PasswordHolder = "PasswordHolder";

                Registry.SetValue(UserInfoHolderPath, UserNameHolder, Username);
                Registry.SetValue(UserInfoHolderPath, PasswordHolder, Password);
                return true;

            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if(!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);

                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            //this will get the stored username and password and will return true if found and false if not found.
            try
            {
                string UserInfoHolderPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLDUserInfo";

                // We use concatination to reach the value name
                string UsernameHolder = "UserNameHolder";
                string PasswordHolder = "PasswordHolder";

                string _username = Registry.GetValue(UserInfoHolderPath, UsernameHolder, null) as string;
                string _password = Registry.GetValue(UserInfoHolderPath, PasswordHolder, null) as string;

                if(_username != "" && _password != "")
                {
                    Username = _username;
                    Password = _password;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);

                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
