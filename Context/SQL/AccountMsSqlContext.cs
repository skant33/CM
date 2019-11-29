using CM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.SQL
{
    public class AccountMsSqlContext : Interfaces.IAccountContext
    {
        private string con;

        public AccountMsSqlContext(string con)
        {
            this.con = con;
        }

        public AccountMsSqlContext()
        {

        }

        public Account Login(Account account)
        {
            Account uitgaand = new Account();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand("select AccountID, Email, Password from Account where Email = @Email and Password = @Password", connection);
                command.Parameters.AddWithValue("@Email", account.Email);
                command.Parameters.AddWithValue("@Password", account.Password);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return uitgaand;
        }

        public bool CheckAccountExist(Account account)
        {
            try
            {
                SqlConnection connection = new SqlConnection(con);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE Email = @Email", connection);
                    command.Parameters.AddWithValue("@Email", account.Email);

                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
            catch
            {

                return false;
            }

        }

        public bool Register(Account account)
        {
            int accountid= 0;
            try
            {
                SqlConnection connection = new SqlConnection(con);

                using (connection)
                {
                    SqlCommand command = new SqlCommand("insert into Account (AccountRoleId, Name, BirthDate, Email, TelephoneNumber, Password) values (@AccountRoleId, @Name, @BirthDate, @Email, @TelephoneNumber, @Password)", connection);
                    command.Parameters.AddWithValue("AccountRoleId", 1);
                    command.Parameters.AddWithValue("Name", account.Name);
                    command.Parameters.AddWithValue("BirthDate", account.DateOfBirth);
                    command.Parameters.AddWithValue("Email", account.Email);
                    command.Parameters.AddWithValue("TelephoneNumber", account.PhoneNumber);
                    command.Parameters.AddWithValue("Password", account.Password);
                    connection.Open();
                    command.ExecuteNonQuery();

                    //accountid dat aangemaakt is lezen
                    SqlCommand sqlCommand = new SqlCommand("select AccountID from [Account] where AccountID = IDENT_CURRENT('Account')", connection);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accountid = Convert.ToInt32(reader["AccountID"]);
                        }
                    }

                    SqlCommand comman2 = new SqlCommand("insert into Notification(TypeID, TimeTillSend, AccountID) values(@TypeID, @TimeTillSend, @AccountID)", connection);
                    comman2.Parameters.AddWithValue("TypeId", account.MeldingID);
                    comman2.Parameters.AddWithValue("TimeTillSend", 5);
                    comman2.Parameters.AddWithValue("AccountID", accountid);
                    comman2.ExecuteNonQuery();
                }
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Account GetAccountByID(int AccountID)
        {
            Account uitgaand = new Account();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand("select * from Account where AccountID = @AccountID", connection);
                command.Parameters.AddWithValue("@AccountID", AccountID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                            uitgaand.Password = Convert.ToString(reader["Password"]);
                            uitgaand.RoleId.ID = Convert.ToInt32(reader["AccountRoleID"]);
                            uitgaand.Name = Convert.ToString(reader["Name"]);
                            uitgaand.DateOfBirth = Convert.ToDateTime(reader["BirthDate"]);
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.PhoneNumber = Convert.ToString(reader["TelephoneNumber"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return uitgaand;
        }

        public int CheckRoleID(int? accountid)
        {
            int id = 0;
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand("SELECT Account.AccountRoleID FROM Account WHERE AccountID = @AccountID", connection);
                command.Parameters.AddWithValue("@AccountID", accountid);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["AccountroleID"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return id;
        }

        public Account GetAccountByEmail(string Email)
        {
            Account uitgaand = new Account();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand("select * from Account where Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", Email);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                            uitgaand.Password = Convert.ToString(reader["Password"]);
                            uitgaand.RoleId.ID = Convert.ToInt32(reader["AccountRolID"]);
                            uitgaand.Name = Convert.ToString(reader["Naam"]);
                            uitgaand.DateOfBirth = Convert.ToDateTime(reader["Geboortedatum"]);
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.PhoneNumber = Convert.ToString(reader["Telefoonnummer"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return uitgaand;
        }
        public List<Account> GetAllDoctors()
        {
            List<Account> doctors = new List<Account>();
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand("select * from account where AccountRoleID = 2", connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Account uitgaand = new Account();
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                            uitgaand.Password = Convert.ToString(reader["Password"]);
                            uitgaand.RoleId.ID = Convert.ToInt32(reader["AccountRoleID"]);
                            uitgaand.Name = Convert.ToString(reader["Name"]);
                            uitgaand.DateOfBirth = Convert.ToDateTime(reader["BirthDate"]);
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.PhoneNumber = Convert.ToString(reader["TelephoneNumber"]);
                            doctors.Add(uitgaand);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return doctors;
        }

        public List<Account> GetAllPatients()
        {
            List<Account> patients = new List<Account>();
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand("select * from account where AccountRoleID = 1", connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Account uitgaand = new Account();
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                            uitgaand.Password = Convert.ToString(reader["Password"]);
                            uitgaand.RoleId.ID = Convert.ToInt32(reader["AccountRoleID"]);
                            uitgaand.Name = Convert.ToString(reader["Name"]);
                            uitgaand.DateOfBirth = Convert.ToDateTime(reader["BirthDate"]);
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.PhoneNumber = Convert.ToString(reader["TelephoneNumber"]);
                            patients.Add(uitgaand);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return patients;
        }

        public bool LinkAccounts(int patientid, int doctorid)
        {
            try
            {
                SqlConnection connection = new SqlConnection(con);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("insert into [AccountLink] (DoctorID, PatientID) values (@DoctorID, @PatientID)", connection);
                    command.Parameters.AddWithValue("@DoctorID", doctorid);
                    command.Parameters.AddWithValue("@PatientID", patientid);
                    connection.Open();
                }
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckLinkedAccounts(int patientid, int doctorid)
        {
            try
            {
                SqlConnection connection = new SqlConnection(con);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SELECT LinkId FROM AccountLink WHERE DoctorID = @DoctorID AND PatientID = @PatientID", connection);
                    command.Parameters.AddWithValue("@DoctorID", doctorid);
                    command.Parameters.AddWithValue("@PatientID", patientid);

                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        return false;
                    }
                }
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Account> GetLinkedPatientsByDoctorID(int doctorid)
        {
            List<Account> patients = new List<Account>();
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT Account.*
                                                    FROM Account
                                                    WHERE AccountID IN (
		                                                    SELECT AccountLink.PatientID as 'Patient'
		                                                    FROM Account
		                                                    INNER JOIN AccountLink ON Account.AccountID = AccountLink.DoctorID
		                                                    WHERE AccountID = @AccountID )", connection);
                command.Parameters.AddWithValue("@AccountID", doctorid);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Account uitgaand = new Account();
                            uitgaand.AccountID = Convert.ToInt32(reader["AccountID"]);
                            uitgaand.Password = Convert.ToString(reader["Password"]);
                            uitgaand.RoleId.ID = Convert.ToInt32(reader["AccountRoleID"]);
                            uitgaand.Name = Convert.ToString(reader["Name"]);
                            uitgaand.DateOfBirth = Convert.ToDateTime(reader["BirthDate"]);
                            uitgaand.Email = Convert.ToString(reader["Email"]);
                            uitgaand.PhoneNumber = Convert.ToString(reader["TelephoneNumber"]);
                            patients.Add(uitgaand);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            connection.Close();
            return patients;
        }
    }
}
