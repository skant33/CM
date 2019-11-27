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
        IAppointmentContext iappointmentcontext;
        AppointmentRepo appointmentrepo;

        INotificationContext inotificationcontext;
        NotificationRepo notificationrepo;

        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        public APIInteraction(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);
            this.config = iconfiguration;

            inotificationcontext = new NotificationMsSqlContext(con);
            notificationrepo = new NotificationRepo(inotificationcontext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);
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
            NotificationController notificationController = new NotificationController(config);            
            foreach (Appointment appointment in AllSendableAppointments())
            {
                if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "SMS")
                {
                    Account patient = accountrepo.GetAccountByID(appointment.patient.AccountID);
                    await notificationController.SendSMS(patient, appointment);
                }
                else if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "eMail")
                {
                    Account patient = accountrepo.GetAccountByID(appointment.patient.AccountID);
                    await notificationController.SendEmail(patient, appointment);
                }
                else if (notificationrepo.GetNotificationTypeByAppointmentID(appointment) == "ByPhone")
                {
                    Account patient = accountrepo.GetAccountByID(appointment.patient.AccountID);
                    await notificationController.SendPhoneConversation(patient, appointment);
                }
            }                      
        }

        public List<Appointment> AllSendableAppointments()
        {
            List<Appointment> appointments = appointmentrepo.AllUpcomingAppointments();

            //marge
            DateTime margemin2 = DateTime.Now.AddMinutes(-2);
            DateTime margeplus2 = DateTime.Now.AddMinutes(2);

            foreach (Appointment appointment in appointments)
            {
                if (appointment.Date > margemin2 && appointment.Date < margeplus2)
                {
                    appointments.Add(appointment);
                }
            }
            return appointments;
        }
    }
}
