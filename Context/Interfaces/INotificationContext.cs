using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.Interfaces
{
    public interface INotificationContext
    {
        string GetNotificationTypeByAppointmentID(Appointment appointment);
        bool UpdateNotificationForUser(int accountid, int typeid, int timetillsend);
        Notification GetNotificationForUser(int accountid);
    }
}
