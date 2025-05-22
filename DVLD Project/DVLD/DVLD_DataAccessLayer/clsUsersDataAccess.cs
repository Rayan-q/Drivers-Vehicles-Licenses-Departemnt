using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DataAccessLayerSettings;

namespace UsersDataAccess
{
    public class clsUsersDataAccess
    {
        // Get all Users in a table
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT * FROM Users";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }


        // Find User By ID
        public static bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["isActive"];
                }
                else
                {
                    // record was not found
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        // Find User By National Number
        public static bool GetUserInfoByUserName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref bool isActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserID = (int)reader["UserID"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["isActive"];
                }
                else
                {
                    // record was not found
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }


        // Checking if a User exists
        public static bool isUserExist(int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT Found = 1 FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
                reader.Close();
            }
            catch (SqlException ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }

        public static bool isUserExistByPersonID(int PersonID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
                reader.Close();
            }
            catch (SqlException ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }


        public static int AddNewUser(int PersonID, string Username, string Password, bool isActive)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"INSERT INTO Users
                           (
                            PersonID, UserName, Password, isActive
                           )
                            Values
                            (
                                @PersonID, @Username, @Password, @isActive
                             )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@isActive", isActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                }
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
            }
            finally
            {
                connection.Close();
            }

            return UserID;

        }

        // Check if Old Password Exist by UserID
        public static bool IsPasswordOld(int UserID, string NewPassword)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT * FROM OldPasswords
                            WHERE UserID = @UserID AND Password = @NewPassword";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@NewPassword", NewPassword);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;
                }
                else
                {
                    // record was not found
                    isFound = false;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;

        }

        // Update an existing User
        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool isActive)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"Update Users
                            set 
                                UserName = @UserName,
                                Password = @Password,
                                isActive = @isActive
                                Where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@isActive", isActive);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);

        }

        // Delete an Existing User
        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"DELETE FROM Users
                             WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";


                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                Console.WriteLine("Error" + ex.Message.ToString());
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }
    }
}
