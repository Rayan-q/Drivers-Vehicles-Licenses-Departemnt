using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using DataAccessLayerSettings;

namespace LDLApplicationsDataAccessLayer
{
    public class clsLocalLicenseApplicationDataAccess
    {

        public static DataTable GetAllLDLApplications()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT 
                                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.ApplicationID',
                                    LicenseClasses.ClassName, People.NationalNo, People.FirstName + ' ' + 
                                    People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName AS 'Full Name', 
                                    Applications.ApplicationDate, 
                                    COUNT(CASE WHEN Tests.TestResult = 1 THEN 1 ELSE NULL END) AS 'Passed Tests',
                                    CASE
                                        WHEN Applications.ApplicationStatus = 1 THEN 'New'
                                        WHEN Applications.ApplicationStatus = 2 THEN 'Cancelled'
                                        WHEN Applications.ApplicationStatus = 3 THEN 'Completed'
                                    END AS 'Status'
                                FROM 
                                    LocalDrivingLicenseApplications 
                                    LEFT JOIN LicenseClasses 
                                        ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID 
                                    LEFT JOIN Applications 
                                        ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID 
                                    LEFT JOIN People 
                                        ON Applications.ApplicantPersonID = People.PersonID 
                                    LEFT JOIN TestAppointments 
                                        ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                                    LEFT JOIN Tests 
                                        ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                                GROUP BY 
                                    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID,
                                    LicenseClasses.ClassName, 
                                    People.NationalNo, 
                                    People.FirstName, 
                                    People.SecondName,
                                    People.ThirdName, 
                                    People.LastName, 
                                    Applications.ApplicationDate, 
                                    Applications.ApplicationStatus;
                                    ";

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

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT * FROM LicenseClasses";

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

        public static bool GetApplicationInfoByID(int LDLApplicationID, ref int ApplicationID, ref int PersonID, ref DateTime ApplicationDate, ref int LicenseClassID, ref int PassedTests)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT LocalDrivingLicenseApplications.ApplicationID, People.PersonID, Applications.ApplicationDate, LocalDrivingLicenseApplications.LicenseClassID, COUNT(CASE WHEN Tests.TestResult = 1 THEN 1 ELSE NULL END) as 'PassedTests'
                             FROM     Applications INNER JOIN LocalDrivingLicenseApplications 
                             ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID INNER JOIN People 
                             ON Applications.ApplicantPersonID = People.PersonID Left JOIN TestAppointments 
                             ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID Left JOIN Tests 
                             ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID INNER JOIN LicenseClasses 
                             ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
				             WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDLApplication
				             GROUP BY LocalDrivingLicenseApplications.ApplicationID, People.PersonID, Applications.ApplicationDate, LocalDrivingLicenseApplications.LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplication", LDLApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    ApplicationID = (int)reader["ApplicationID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    LicenseClassID = (int)reader["LicenseClassID"];
                    PassedTests = (int)reader["PassedTests"];

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

        public static bool GetClassInfoByName(string ClassName, ref int LicenseClassID, ref string ClassDescription,
            ref int MinimumAllowedAge, ref int DefaultValidityLength, ref decimal ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt32( reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    ClassFees = (decimal)reader["ClassFees"];
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

        public static bool GetClassInfoByID(int LicenseClassID , ref string ClassName, ref string ClassDescription,
    ref int MinimumAllowedAge, ref int DefaultValidityLength, ref decimal ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // record was found
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    DefaultValidityLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    ClassFees = (decimal)reader["ClassFees"];
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
        public static bool isLicenseClassExist(int LicenseClassID, int ApplicantID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"SELECT  FOUND = 1
                             FROM Applications INNER JOIN LocalDrivingLicenseApplications 
                             ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                             WHERE ApplicantPersonID = @ApplicantID 
                             AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                             AND (Applications.ApplicationStatus = 1 OR Applications.ApplicationStatus = 3)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@ApplicantID", ApplicantID);

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

        public static int AddNewLDLApplication(int ApplicationID, int LicenseClassID)
        {
            int LDLApplicationID = -1;
            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications
                           (
                            ApplicationID, LicenseClassID
                           )
                             VALUES
                            (
                            @ApplicationID, @LicenseClassID
                            )
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LDLApplicationID = insertedID;
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

            return LDLApplicationID;

        }

        public static bool UpdateLDLApplication(int LDLApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"UPDATE LocalDrivingLicenseApplications
                             SET LicenseClassID = @LicenseClassID
                             WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);



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


        public static bool DeleteApplication(int ApplicationID, int LDLApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(Settings.ConnectionString);

            string query = @"
                             DELETE Tests
                             FROM Tests
                             INNER JOIN TestAppointments 
                             ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                             INNER JOIN LocalDrivingLicenseApplications 
                             ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDLApplicationID;

                             DELETE TestAppointments
                             FROM TestAppointments
                             INNER JOIN LocalDrivingLicenseApplications ON TestAppointments.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LDLApplicationID;

                             DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LDLApplicationID
                             DELETE FROM Applications WHERE ApplicationID = @ApplicationID
                             ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LDLApplicationID", LDLApplicationID);



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
            return (rowsAffected > 1);
        }
    }
}
