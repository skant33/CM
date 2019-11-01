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
                    SqlCommand command = new SqlCommand("insert into Account (MeldingID, AccountRolId, Naam, Geboortedatum, Email, Telefoonnummer, Password) values (@MeldingID,@AccountRolId,@Naam,@Geboortedatum,@Email, @Telefoonnummer,@Password)", connection);
                    command.Parameters.AddWithValue("MeldingID", account.MeldingID);
                    command.Parameters.AddWithValue("AccountRolId", 1);
                    command.Parameters.AddWithValue("Naam", account.Name);
                    command.Parameters.AddWithValue("Geboortedatum", account.DateOfBirth);
                    command.Parameters.AddWithValue("Email", account.Email);
                    command.Parameters.AddWithValue("Telefoonnummer", account.PhoneNumber);
                    command.Parameters.AddWithValue("Password", account.Password);
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
                            uitgaand.AccountRole = Convert.ToInt32(reader["AccountRolID"]);
                            uitgaand.MeldingID = Convert.ToInt32(reader["MeldingID"]);
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

        public bool CheckIfAdmin(int? accountid)
        {
            int id;
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand("select AccountRol.AccountRolID from AccountRol inner join Account on AccountRol.AccountRolID = Account.AccountRolID where AccountID = @AccountID", connection);
                command.Parameters.AddWithValue("@AccountID", accountid);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["AccountrolID"]);
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
    }
}
