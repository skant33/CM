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

        public AppointmentMsSqlContext()
        {

        }

        public List<Appointment> GetAllAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            SqlConnection connection = new SqlConnection(con);

            SqlCommand sqlCommand = new SqlCommand(@"SELECT * 
                                                    FROM Appointment", connection);

            connection.Open();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                    appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                    appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                    appointment.Duration = Convert.ToInt32(reader["Duration"]);
                    appointment.DateTime = Convert.ToDateTime(reader["DateTime"]);
                    appointment.Coords = Convert.ToInt32(reader["Coords"]);
                    appointment.Description = Convert.ToString(reader["Description"]);
                    appointments.Add(appointment);
                }
            }
            connection.Close();
            return appointments;
        }

        public List<Appointment> GetAppointmentsByUserId(Account account)
        {
            List<Appointment> appointments = new List<Appointment>();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT Appointment.*, AccountLink.DoctorID, AccountLink.PatientID
                                                    FROM Appointment 
                                                    INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID 
                                                    INNER JOIN Account ON AccountLink.PatientID = Account.AccountID 
                                                    WHERE Account.AccountID = @PatientID", connection);
                command.Parameters.AddWithValue("@PatientID", account.AccountID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Appointment appointment = new Appointment();
                            appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                            appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                            appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                            appointment.Duration = Convert.ToInt32(reader["Duration"]);
                            appointment.DateTime = Convert.ToDateTime(reader["DateTime"]);
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
            connection.Close();
            return appointments;
        }

        public List<Appointment> GetAppointmentsByDoctorId(Account account)
        {
            List<Appointment> appointments = new List<Appointment>();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT Appointment.*, AccountLink.DoctorID, AccountLink.PatientID
                                                    FROM Appointment 
                                                    INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID 
                                                    INNER JOIN Account ON AccountLink.DoctorID = Account.AccountID 
                                                    WHERE Account.AccountID = @PatientID", connection);
                command.Parameters.AddWithValue("@PatientID", account.AccountID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            Appointment appointment = new Appointment();
                            appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                            appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                            appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                            appointment.Duration = Convert.ToInt32(reader["Duration"]);
                            appointment.DateTime = Convert.ToDateTime(reader["DateTime"]);
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
            connection.Close();
            return appointments;
        }

        public Appointment GetAppointmentByID(int id)
        {
            Appointment appointment = new Appointment();

            SqlConnection connection = new SqlConnection(con);

            using (connection)
            {
                SqlCommand command = new SqlCommand(@"SELECT * 
                                                    FROM Appointment 
                                                    WHERE AppointmentID = @AppointmentID", connection);
                command.Parameters.AddWithValue("@AppointmentID", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                            appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                            appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                            appointment.Duration = Convert.ToInt32(reader["Duration"]);
                            appointment.DateTime = Convert.ToDateTime(reader["Date"]);
                            appointment.Coords = Convert.ToInt32(reader["Coords"]);
                            appointment.Description = Convert.ToString(reader["Description"]);
                        }
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine(x.Message);
                        throw;
                    }
                }
            }
            connection.Close();
            return appointment;
        }

        public List<Appointment> AppointmentsCurrentWeek(int id)
        {
            List<Appointment> appointments = new List<Appointment>();

            SqlConnection connection = new SqlConnection(con);

                SqlCommand command = new SqlCommand(@"Set DateFirst 1
	                                                  Select * 
	                                                  from Appointment
	                                                  INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID
	                                                  Where [DateTime] >= dateadd(day, 1-datepart(dw, getdate()), CONVERT(date,getdate())) 
	                                                  AND [DateTime] <  dateadd(day, 8-datepart(dw, getdate()), CONVERT(date,getdate()))
	                                                  And AccountLink.PatientID = @AccountID Or AccountLink.DoctorID = @AccountID
	                                                  order by [DateTime] asc", connection);

            connection.Open();
                command.Parameters.AddWithValue("@AccountID", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Appointment appointment = new Appointment();
                        appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                        appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                        appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                        appointment.Duration = Convert.ToInt32(reader["Duration"]);
                        appointment.DateTime = Convert.ToDateTime(reader["DateTime"]);
                        appointment.Coords = Convert.ToInt32(reader["Coords"]);
                        appointment.Description = Convert.ToString(reader["Description"]);
                        appointments.Add(appointment);
                    }
                }
            connection.Close();
            return appointments;
        }

        public List<Appointment> AllUpcomingAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            SqlConnection connection = new SqlConnection(con);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT Appointment.*, AccountLink.DoctorID, AccountLink.PatientID, FORMAT(DATEADD(HOUR, -[Notification].TimeTillSend, Appointment.[DateTime]),'dd-MM-yyyy HH:mm:ss') as 'SendTime'
                                                    FROM Appointment
                                                    INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID
                                                    INNER JOIN Account ON AccountLink.PatientID = Account.AccountID
                                                    INNER JOIN [Notification] on Account.AccountID = [Notification].AccountID
                                                    WHERE Appointment.[DateTime] > CURRENT_TIMESTAMP", connection);
            connection.Open();
            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Appointment appointment = new Appointment();
                    appointment.AppointmentID = Convert.ToInt32(reader["AppointmentID"]);
                    appointment.patient.AccountID = Convert.ToInt32(reader["PatientID"]);
                    appointment.doctor.AccountID = Convert.ToInt32(reader["DoctorID"]);
                    appointment.Duration = Convert.ToInt32(reader["Duration"]); 
                    appointment.DateTime = Convert.ToDateTime(reader["DateTime"]);
                    appointment.Coords = Convert.ToInt32(reader["Coords"]);
                    appointment.Description = Convert.ToString(reader["Description"]);
                    appointment.SendTime = Convert.ToDateTime(reader["SendTime"]);
                    appointments.Add(appointment);
                }
            }
            connection.Close();
            return appointments;
        }
        public bool MakeAppointment(Appointment appointment)
        {
            int linkid = 0;
            try
            {
                //lees linkid
                SqlConnection connection = new SqlConnection(con);

                SqlCommand sqlCommand = new SqlCommand("Select LinkID From AccountLink where DoctorID = @DoctorID and PatientID = @PatientID", connection);
                sqlCommand.Parameters.AddWithValue("@DoctorID", appointment.doctor.AccountID);
                sqlCommand.Parameters.AddWithValue("@PatientId", appointment.patient.AccountID);
                connection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        linkid = Convert.ToInt32(reader["LinkID"]);
                    }
                }

                using (connection)
                {
                    SqlCommand command = new SqlCommand("insert into Apoointment ( LinkID, Duration, DateTime, Coords, Description) values (insert into Apoointment (@AppointmentID,@LinkID,@Duration,@DateTime,@Coords,@Description)", connection);
                    command.Parameters.AddWithValue("LinkID", linkid);
                    command.Parameters.AddWithValue("Duration", appointment.Duration);
                    command.Parameters.AddWithValue("DateTime", appointment.DateTime);
                    command.Parameters.AddWithValue("Coords", appointment.Coords);
                    command.Parameters.AddWithValue("Description", appointment.Description);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
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
