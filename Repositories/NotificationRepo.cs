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
        public NotificationRepo(INotificationContext context)
        {
            this.context = context;
        }

        public string GetNotificationTypeByAppointmentID(Appointment appointment)
        {
            return context.GetNotificationTypeByAppointmentID(appointment);
        }
    }
}
