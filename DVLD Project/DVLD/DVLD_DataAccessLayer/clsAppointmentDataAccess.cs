using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using DataAccessLayerSettings;

namespace AppointmentsDataAccessLayer
{
    public class clsAppointmentDataAccess
    {
        public static DataTable GetAllAppointmentsWithTypeAndApplicant(int LDLApplicationID, int AppTypeID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked FROM TestAppointments
                             WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID AND TestTypeID = @AppTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

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

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static bool FindAppointmentByLDLAppID(int LDLApplicationID, ref int AppID, ref int AppTypeID, ref DateTime AppDate, ref decimal PaidFees, ref int CreatedByUserID, ref bool isLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID AND isLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    AppID = (int)reader["TestAppointmentID"];
                    AppTypeID = (int)reader["TestTypeID"];
                    AppDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = (decimal)reader["PaidFees"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    isLocked = (bool)reader["isLocked"];
                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                    {
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                    }
                    else
                    {
                        RetakeTestApplicationID = 0;
                    }
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

                if (!EventLog.Exists(sourceName))
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

        public static bool isActiveTestTypeExistInAppointment(int LDLapplicationID, int TestTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT Found = 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID AND TestTypeID = @TestTypeID AND isLocked = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLapplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }

        public static int AddNewAppointment(int TestTypeID, int LDLApplicationID, DateTime AppDate, decimal PaidFees, int CreatedByUserID, bool isLocked, int RetakeTestApplicationID)
        {
            int AppID = -1;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"INSERT INTO TestAppointments
                           (
                            TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, isLocked
                           )
                             VALUES
                            (
                            @TestTypeID, @LDLApplicationID, @AppDate, @PaidFees, @CreatedByUserID, @isLocked
                            )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@AppDate", AppDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@isLocked", isLocked);
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    AppID = insertedID;
                }
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return AppID;
        }

        public static bool UpdateAppointment(int AppID, DateTime AppDate, bool isLocked)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"Update TestAppointments
                            set 
                                AppointmentDate = @AppDate,
                                isLocked = @isLocked
                                Where TestAppointmentID = @AppID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppID", AppID);
            command.Parameters.AddWithValue("@AppDate", AppDate);
            command.Parameters.AddWithValue("@isLocked", isLocked);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                //Console.WriteLine("Error: " + ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);

        }

        public static int GetNumberOfTrials(int LDLApplicationID, int TestTypeID)
        {
            int Trials = 0;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT COUNT(*) AS Trials FROM TestAppointments WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID AND isLocked = 1 AND TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    Trials = (int)reader["Trials"];
                }
                else
                {
                    // record was not found
                    Trials = 0;
                }
                reader.Close();

            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
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
            return Trials;
        }


        // Tests
        public static bool isTestPassed(int LDLApplicationID, int TestTypeID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"
                            SELECT Passed = 1
                            FROM Tests inner join TestAppointments
                            ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                            inner join LocalDrivingLicenseApplications
                            ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDLApplicationID
                            And TestAppointments.TestTypeID = @TestTypeID AND Tests.TestResult = 1
                            ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
                isFound = false;
            }
            finally
            {
                connection.Close();

            }
            return isFound;
        }

        public static int AddNewTest(int AppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"INSERT INTO Tests
                           (
                            TestAppointmentID, TestResult, Notes, CreatedByUserID
                           )
                             VALUES
                            (
                            @AppointmentID, @TestResult, @Notes, @CreatedByUserID
                            )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            command.Parameters.AddWithValue("@Notes", Notes);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return TestID;
        }

        public static int? AddNewWrittenTest(int TestID, int Score)
        {
            int? writtenTestID = null;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"INSERT INTO WrittenTests
                           (
                            TestID, Score
                           )
                             VALUES
                            (
                            @TestID, @Score
                            )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);
            command.Parameters.AddWithValue("@Score", Score);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    writtenTestID = insertedID;
                }
            }
            catch (Exception ex)
            {
                string sourceName = "DVLD";

                if (!EventLog.Exists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                connection.Close();
            }

            return writtenTestID;
        }

    }
}
