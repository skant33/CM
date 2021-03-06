﻿using CM.Models;
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

        public Account Login(Account account)
        {
            Account uitgaand = new Account();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand("SELECT AccountID, Email, Password FROM Account WHERE Email = @Email AND Password = @Password", connection);
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
                        return true;
                    }
                }
                connection.Close();

                return false;
            }
            catch
            {
                return true;
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
                    SqlCommand command = new SqlCommand("INSERT INTO Account (AccountRoleId, Name, BirthDate, Email, TelephoneNumber, Password) VALUES (@AccountRoleId, @Name, @BirthDate, @Email, @TelephoneNumber, @Password)", connection);
                    command.Parameters.AddWithValue("AccountRoleId", 1);
                    command.Parameters.AddWithValue("Name", account.Name);
                    DateTime dobb = account.DateOfBirth.Date;
                    command.Parameters.AddWithValue("BirthDate", dobb);
                    command.Parameters.AddWithValue("Email", account.Email);
                    command.Parameters.AddWithValue("TelephoneNumber", account.PhoneNumber);
                    command.Parameters.AddWithValue("Password", account.Password);

                    connection.Open();

                    command.ExecuteNonQuery();

                    //accountId dat aangemaakt is lezen
                    SqlCommand sqlCommand = new SqlCommand("SELECT AccountID FROM [Account] WHERE AccountID = IDENT_CURRENT('Account')", connection);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accountid = Convert.ToInt32(reader["AccountID"]);
                        }
                    }

                    //leeftijd berekenen voor melding id
                    int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                    int dob = int.Parse(account.DateOfBirth.ToString("yyyyMMdd"));
                    int age = (now - dob) / 10000;

                    if (age <= 18)
                    {
                        account.MeldingID = 1;
                    }

                    if (age > 18 && age <= 65)
                    {
                        account.MeldingID = 2;
                    }

                    if(age > 65)
                    {
                        account.MeldingID = 4;
                    }

                    SqlCommand comman2 = new SqlCommand("INSERT INTO Notification(TypeID, TimeTillSend, AccountID) VALUES(@TypeID, @TimeTillSend, @AccountID)", connection);
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
                SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE AccountID = @AccountID", connection);
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
                SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE Email = @Email", connection);
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
                SqlCommand command = new SqlCommand("SELECT * FROM account WHERE AccountRoleID = 2", connection);

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
                SqlCommand command = new SqlCommand("SELECT * FROM Account WHERE AccountRoleID = 1", connection);

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
                    SqlCommand command = new SqlCommand("INSERT INTO [AccountLink] (DoctorID, PatientID) VALUES (@DoctorID, @PatientID)", connection);
                    command.Parameters.AddWithValue("@DoctorID", doctorid);
                    command.Parameters.AddWithValue("@PatientID", patientid);

                    connection.Open();

                    command.ExecuteNonQuery();
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
                SqlCommand command = new SqlCommand("SELECT Account.* FROM Account WHERE AccountID IN (SELECT AccountLink.PatientID as 'Patient' FROM Account INNER JOIN AccountLink ON Account.AccountID = AccountLink.DoctorID WHERE AccountID = @AccountID )", connection);
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

        public List<Account> DoctorsFromPatient(int patientid)
        {
            List<Account> doctors = new List<Account>();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand("SELECT* FROM Account WHERE AccountID IN (SELECT AccountLink.DoctorID FROM Account INNER JOIN AccountLink ON Account.AccountID = PatientID WHERE Account.AccountID = @AccountID)", connection);
                command.Parameters.AddWithValue("@AccountID", patientid);

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
    }
}