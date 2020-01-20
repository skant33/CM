using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Context.Interfaces;
using CM.Context.SQL;
using CM.Converters;
using CM.Helpers;
using CM.Models;
using CM.Repositories;
using CM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CM.Controllers
{
    public class AppointmentController : Controller
    {
        readonly AppointmentConverter appointmentconverter = new AppointmentConverter();
        readonly AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        readonly IAppointmentContext iappointmentcontext;
        readonly AppointmentRepo appointmentrepo;

        readonly IAccountContext iaccountcontext;
        readonly AccountRepo accountrepo;

        //helpers
        private AccountVerification accVeri;

        public AppointmentController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            //helpers
            accVeri = new AccountVerification();
        }


        public IActionResult Agenda()
        {
            if (accVeri.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == false)
            {
                return RedirectToAction("Login", "Account");
            }

            AppointmentViewModel viewmodel = new AppointmentViewModel();

            Account account = new Account();
            account.AccountID = (int)HttpContext.Session.GetInt32("AccountID");

            foreach (Appointment appointment in appointmentrepo.AppointmentsCurrentWeek(account.AccountID))
            {
                viewmodel.appointment.Add(appointmentconverter.ViewModelFromAppointment(appointment));
            }

            return View(viewmodel);
        }


        public IActionResult Index() //Create Appointment
        {
            if (HttpContext.Session.GetInt32("Doctor") == 1 || HttpContext.Session.GetInt32("Admin") == 1)
            {
                List<Account> LinkedPatients = new List<Account>();
                List<Appointment> appointments = new List<Appointment>();

                int id = (int)HttpContext.Session.GetInt32("AccountID");

                ViewBag.LinkedPatients = accountrepo.GetLinkedPatientsByDoctorID(id);

                return View();
            }

            return RedirectToAction("LogOut", "Account");
        }


        public IActionResult MakeAppointment(AppointmentDetailViewModel viewmodel)
        {
            Appointment inkomend = appointmentconverter.ViewModelToAppointment(viewmodel);
            inkomend.doctor.AccountID = (int)HttpContext.Session.GetInt32("AccountID");

            if (appointmentrepo.MakeAppointment(inkomend) == true)
            {
                //afspraak gepland
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //mislukt
                return RedirectToAction("Index", "Appointment");
            }
        }


        public IActionResult DeleteAppointment(int appointmentID)
        {
            if (appointmentrepo.DeleteAppointment(appointmentID) == true)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Agenda", "Appointment");
            }
        }
    }
}