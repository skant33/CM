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
            string query = "select * from Appointment";
            SqlCommand sqlCommand = new SqlCommand(query, connection);
            connection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                    appointment.PatientID = Convert.ToInt32(reader["ClientID"]);
                    appointment.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                    appointment.Duration = Convert.ToInt32(reader["Duration"]);
                    appointment.Date = Convert.ToDateTime(reader["Date"]);
                    appointment.Coords = Convert.ToInt32(reader["Coords"]);
                    appointment.Done = Convert.ToBoolean(reader["Done"]);
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
                SqlCommand command = new SqlCommand("select * from Appointment where ClientID = @ClientID", connection);
                command.Parameters.AddWithValue("@ClientID", account.AccountID);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Appointment appointment = new Appointment();
                            appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                            appointment.PatientID = Convert.ToInt32(reader["ClientID"]);
                            appointment.DoctorID = Convert.ToInt32(reader["DoctorID"]);
                            appointment.Duration = Convert.ToInt32(reader["Duration"]);
                            appointment.Date = Convert.ToDateTime(reader["Date"]);
                            appointment.Coords = Convert.ToInt32(reader["Coords"]);
                            appointment.Done = Convert.ToBoolean(reader["Done"]);
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
    }
}
