using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Context.Interfaces;
using CM.Context.SQL;
using CM.Controllers;
using CM.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CM.Models
{
    public class APIInteraction
    {
        private IConfiguration config;

        private BackgroundJobServer backgroundJobServer;
        readonly IAppointmentContext iappointmentcontext;
        readonly AppointmentRepo appointmentrepo;

        readonly INotificationContext inotificationcontext;
        readonly NotificationRepo notificationrepo;

        readonly IAccountContext iaccountcontext;
        readonly AccountRepo accountrepo;

        readonly NotificationController notificationController;

        public APIInteraction(IConfiguration iconfiguration)
        {
            config = iconfiguration;
            string con = config.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);

            inotificationcontext = new NotificationMsSqlContext(con);
            notificationrepo = new NotificationRepo(inotificationcontext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            notificationController = new NotificationController(config);
        }

        public async Task StartHangfire()
        {
            backgroundJobServer = new BackgroundJobServer();
            RecurringJob.AddOrUpdate(() => CheckForNotification(), Cron.MinuteInterval(5));
        }

        public async Task StopHangfire()
        {
            RecurringJob.RemoveIfExists("APIInteraction.CheckForNotification");
        }

        public async Task CheckForNotification()
        {
            
            foreach (Appointment appointment in AllSendableAppointments())
            {
                if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "SMS")
                {
                    await notificationController.SendSMS(appointment);
                }
                else if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "eMail")
                {
                    await notificationController.SendEmail(appointment);
                }
                else if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "ByPhone")
                {
                    await notificationController.SendPhoneConversation(appointment);
                }
            }
        }

        public List<Appointment> AllSendableAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            List<Appointment> upcoming = appointmentrepo.AllUpcomingAppointments();
            //marge
            DateTime margemin2 = DateTime.Now.AddMinutes(-2);
            DateTime margeplus2 = DateTime.Now.AddMinutes(2);

            foreach (Appointment appointment in upcoming)
            {
                if (appointment.SendTime > margemin2 && appointment.SendTime < margeplus2)
                {
                    appointment.doctor = accountrepo.GetAccountByID(appointment.doctor.AccountID);
                    appointment.patient = accountrepo.GetAccountByID(appointment.patient.AccountID);

                    appointments.Add(appointment);
                }
            }

            return appointments;
        }
    }
}
