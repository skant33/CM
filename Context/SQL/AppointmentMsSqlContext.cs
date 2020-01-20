using CM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Appointment", connection);

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
                SqlCommand command = new SqlCommand("SELECT Appointment.*, AccountLink.DoctorID, AccountLink.PatientID FROM Appointment INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID INNER JOIN Account ON AccountLink.PatientID = Account.AccountID OR AccountLink.DoctorID = Account.AccountID WHERE Account.AccountID = @PatientID", connection);
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
                SqlCommand command = new SqlCommand("SELECT * FROM Appointment WHERE AppointmentID = @AppointmentID", connection);
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
                            appointment.Description = Convert.ToString(reader["Description"]);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("No rows found");
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

            SqlCommand command = new SqlCommand("SELECT Appointment.*, AccountLink.* FROM Appointment INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID INNER JOIN Account ON AccountLink.PatientID = Account.AccountID OR AccountLink.DoctorID = Account.AccountID WHERE [DateTime] BETWEEN CURRENT_TIMESTAMP AND DATEADD(DAY, 7, CURRENT_TIMESTAMP) AND AccountID = @AccountID order by [DateTime] asc", connection);
            command.Parameters.AddWithValue("@AccountID", id);

            connection.Open();
            
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

            SqlCommand sqlCommand = new SqlCommand("SELECT DISTINCT Appointment.*, AccountLink.DoctorID, AccountLink.PatientID, FORMAT(DATEADD(HOUR, -[Notification].TimeTillSend, Appointment.[DateTime]),'yyyy-MM-dd HH:mm:ss') as 'SendTime' FROM Appointment INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID INNER JOIN Account ON AccountLink.PatientID = Account.AccountID INNER JOIN [Notification] on Account.AccountID = [Notification].AccountID WHERE Appointment.[DateTime] > CURRENT_TIMESTAMP", connection);

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
            bool canAdd = true;

            int linkid = 0;

            try
            {
                //lees linkid
                SqlConnection connection = new SqlConnection(con);

                SqlCommand sqlCommand = new SqlCommand("SELECT LinkID FROM AccountLink WHERE DoctorID = @DoctorID and PatientID = @PatientID", connection);
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

                SqlCommand checkAppCommand = new SqlCommand(@"SELECT Appointment.* FROM Appointment INNER JOIN AccountLink ON Appointment.LinkID = AccountLink.LinkID INNER JOIN Account ON AccountLink.DoctorID = Account.AccountID WHERE([DateTime] BETWEEN @DateNewAppointment AND DATEADD(MINUTE, @DurationNewAppointment, @DateNewAppointment) OR DATEADD(MINUTE, Duration, [DateTime]) BETWEEN @DateNewAppointment AND DATEADD(MINUTE, @DurationNewAppointment, @DateNewAppointment)) AND DoctorID = @DoctorID", connection);
                string appoin = string.Format("{0}-{1}-{2} {3}:{4}:{5}:{6}", appointment.DateTime.Year, appointment.DateTime.Month, appointment.DateTime.Day, appointment.DateTime.Hour, appointment.DateTime.Minute, appointment.DateTime.Second, appointment.DateTime.Millisecond);
                checkAppCommand.Parameters.AddWithValue("@DateNewAppointment", appoin);
                checkAppCommand.Parameters.AddWithValue("@DurationNewAppointment", appointment.Duration);
                checkAppCommand.Parameters.AddWithValue("@DoctorID", appointment.doctor.AccountID);

                using(SqlDataReader r = checkAppCommand.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(r);

                    if(dt.Rows.Count > 0)
                    {
                        canAdd = false;
                    }
                }

                if (canAdd)
                {
                    using (connection)
                    {
                        SqlCommand command = new SqlCommand("INSERT INTO Appointment (LinkID, Duration, DateTime, Description) values (@LinkID,@Duration,@DateTime,@Description)", connection);
                        command.Parameters.AddWithValue("LinkID", linkid);
                        command.Parameters.AddWithValue("Duration", appointment.Duration);
                        command.Parameters.AddWithValue("DateTime", appointment.DateTime);
                        command.Parameters.AddWithValue("Description", appointment.Description);

                        command.ExecuteNonQuery();

                        connection.Close();

                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
                catch
            {
                Console.WriteLine("No rows found");

                return false;
            }
        }

        public bool DeleteAppointment(int appointmentID)
        {
            SqlConnection conn = new SqlConnection(con);

            SqlCommand command = new SqlCommand("DELETE FROM Appointment WHERE AppointmentID = @AppointmentID", conn);
            command.Parameters.AddWithValue("AppointmentID", appointmentID);

            conn.Open();

            using (conn)
            {
                if (command.ExecuteNonQuery() > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
