using CM.Models;
using CM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Converters
{
    public class NotificationConverter
    {
        public Notification ViewModelToNotification(NotificationDetailViewModel NDVM)
        {
            Notification notification = new Notification()
            {
                AccountID = NDVM.AccountID,
                TypeID = NDVM.TypeID,
                TimeTillSend = NDVM.TimeTillSend
            };

            return notification;
        }

        public NotificationDetailViewModel ViewModelFromNotification(Notification notification)
        {
            NotificationDetailViewModel NDVM = new NotificationDetailViewModel()
            {
                AccountID = notification.AccountID,
                TypeID = notification.TypeID,
                TimeTillSend = notification.TimeTillSend
            };

            return NDVM;
        }
    }
}
