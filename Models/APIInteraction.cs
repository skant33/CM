using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Controllers;
using Hangfire;
using Microsoft.Extensions.Configuration;

namespace CM.Models
{
    public class APIInteraction
    {
        private IConfiguration config;

        public APIInteraction(IConfiguration config)
        {
            var server = new BackgroundJobServer();
            RecurringJob.AddOrUpdate(() => CheckForNotification(), Cron.Minutely);
            this.config = config;
        }

        public async void CheckForNotification()
        {
            NotificationController ni = new NotificationController(config);
            await ni.SendSMS();
        }
    }
}
