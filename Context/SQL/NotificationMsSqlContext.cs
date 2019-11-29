using CM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.SQL
{
    public class NotificationMsSqlContext : Interfaces.INotificationContext
    {
        private string con;
        public NotificationMsSqlContext(string con)
        {
            this.con = con;
        }

        public string GetNotificationTypeByAppointmentID(Appointment appointment)
        {
            string notificationtype = "";
            SqlConnection connection = new SqlConnection(con);
            SqlCommand sqlCommand = new SqlCommand(@"SELECT [Notification].*, NotificationType.[Type]
                                                    FROM [Notification]
                                                    INNER JOIN Account ON [Notification].AccountID = Account.AccountID
                                                    INNER JOIN AccountLink ON Account.AccountID = AccountLink.PatientID
                                                    INNER JOIN NotificationType ON [Notification].TypeID = NotificationType.TypeID
                                                    INNER JOIN Appointment ON Appointment.LinkID = AccountLink.LinkID
                                                    WHERE AppointmentID = @AppointmentID", connection);
            sqlCommand.Parameters.AddWithValue("@AppointmentID", appointment.AppointmentID);
            connection.Open();

            using (SqlDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    notificationtype  = Convert.ToString(reader["Type"]);
                }
            }
            connection.Close();
            return notificationtype;
        }
    }
}
