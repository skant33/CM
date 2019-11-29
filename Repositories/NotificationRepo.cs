using CM.Context.Interfaces;
using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Repositories
{
    public class NotificationRepo
    {
        private INotificationContext context;
        public NotificationRepo(INotificationContext _context)
        {
            this.context = _context;
        }

        public string GetNotificationTypeByAppointmentID(Appointment appointment)
        {
            return context.GetNotificationTypeByAppointmentID(appointment);
        }

        public bool UpdateNotificationForUser(int accountid, int typeid, int timetillsend)
        {
            return context.UpdateNotificationForUser(accountid, typeid, timetillsend);
        }

        public Notification GetNotificationForUser(int accountid)
        {
            return context.GetNotificationForUser(accountid);
        }
    }
}
