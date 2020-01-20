using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace CM.Models
{
    
    public class NotificationAutomation
    {
        public void FireAndForget()
        {
            var jobId = BackgroundJob.Enqueue(
            () => Console.WriteLine("Fire-and-forget!"));
        }

        public void DelayedJob()
        {
            var jobId = BackgroundJob.Schedule(
            () => Console.WriteLine("Delayed!"),
            TimeSpan.FromDays(7));
        }

        public void RecurringJobs()
        {
            RecurringJob.AddOrUpdate(
            () => Console.WriteLine("Recurring!"),
            Cron.Daily);
        }
    }
}
