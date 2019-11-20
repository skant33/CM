using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Controllers;
using Hangfire;

namespace CM.Models
{
    public class APIInteraction
    {
        public APIInteraction()
        {
            //var server = new BackgroundJobServer();
            //RecurringJob.AddOrUpdate(() => CheckForNotification(), Cron.Minutely);
        }

        public async void CheckForNotification()
        {
            NotificationController noti = new NotificationController(null);
            var result = await noti.SendSMS();
        }
    }
}
