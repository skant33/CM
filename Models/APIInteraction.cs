using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Controllers;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CM.Models
{
    public class APIInteraction
    {
        private IConfiguration config;
        private BackgroundJobServer backgroundJobServer;

        public APIInteraction(IConfiguration config)
        {         
            
            this.config = config;
        }

        public async Task StartHangfire()
        {
            backgroundJobServer = new BackgroundJobServer();
            RecurringJob.AddOrUpdate(() => CheckForNotification(), Cron.Minutely);
        }

        public async Task StopHangfire()
        {
            RecurringJob.RemoveIfExists("APIInteraction.CheckForNotification");
        }

        public async Task CheckForNotification()
        {
            NotificationController ni = new NotificationController(config);
            await ni.SendSMS();
        }
    }
}
