using CM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            return uitgaand;
        }

        public bool Register(Account account)
        {
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

                    //Meldinggegevens moeten ook nog opgeslagen worden

                    connection.Open();

                    command.ExecuteNonQuery();

                    return true;
                }
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
                            uitgaand.PhoneNumber = Convert.ToInt32(reader["TelephoneNumber"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            return uitgaand;
        }

        public bool CheckIfAdmin(int? accountid)
        {
            int id;

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
                            if (id == 1)
                            {
                                return false;
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            return true;
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
                            uitgaand.PhoneNumber = Convert.ToInt32(reader["Telefoonnummer"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            return uitgaand;
        }
    }
}
