using CM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.SQL
{
    public class AppointmentMsSqlContext : Interfaces.IAppointmentContext
    {
        private string con;

        public AppointmentMsSqlContext(string con)
        {
            this.con = con;
        }

        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            SqlConnection connection = new SqlConnection(con);
            string query = "select * from Afspraak";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            connection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.AppointmentID = Convert.ToInt32(reader["AfspraakID"]);
                    appointment.LinkID = Convert.ToInt32(reader["LinkID"]);
                    appointment.Duration = Convert.ToInt32(reader["Duur"]);
                    appointment.Date = Convert.ToDateTime(reader["Datum"]);
                    appointment.Coords = Convert.ToInt32(reader["Coords"]);
                    appointment.Description = Convert.ToString(reader["Description"]);
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }

        public List<Appointment> GetAppointmentsByUserId(Account account)
        {
            List<Appointment> appointments = new List<Appointment>();
            SqlConnection connection = new SqlConnection(con);
            using (connection)
            {
                SqlCommand command = new SqlCommand("select * from Afspraak where GebruikerID = @GebruikerID", connection);
                command.Parameters.AddWithValue("@GebruikerID", account.AccountID);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Appointment appointment = new Appointment();
                            appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                            appointment.LinkID = Convert.ToInt32(reader["LinkID"]);
                            appointment.Duration = Convert.ToInt32(reader["Duration"]);
                            appointment.Date = Convert.ToDateTime(reader["Date"]);
                            appointment.Coords = Convert.ToInt32(reader["Coords"]);
                            appointment.Description = Convert.ToString(reader["Description"]);
                            appointments.Add(appointment);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found.");
                    }
                }
            }
            return appointments;
        }

        public bool MakeAppointment(Appointment appointment)
        {
            try
            {
                SqlConnection connection = new SqlConnection(con);
                using (connection)
                {
                    SqlCommand command = new SqlCommand("insert into Apoointment (AppointmentID, LinkID, Duration, Date, Coords, Description) values (insert into Apoointment (@AppointmentID,@LinkID,@Duration,@Date,@Coords,@Description)", connection);
                    command.Parameters.AddWithValue("PatientID", appointment.LinkID);
                    command.Parameters.AddWithValue("Duration", appointment.Duration);
                    command.Parameters.AddWithValue("Date", appointment.Date);
                    command.Parameters.AddWithValue("Coords", appointment.Coords);
                    command.Parameters.AddWithValue("Description", appointment.Description);
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
        
    }
}
