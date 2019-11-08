using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace CM.Models
{
    public class APIInteraction
    {
        public APIInteraction()
        {
            RecurringJob.AddOrUpdate(() => CheckForNotification(), Cron.Minutely);
        }

        public void CheckForNotification()
        {
            Console.WriteLine("Recurring");
        }
    }
}
