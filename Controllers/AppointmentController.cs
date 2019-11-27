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
        AppointmentConverter appointmentconverter = new AppointmentConverter();
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        IAppointmentContext iappointmentcontext;
        AppointmentRepo appointmentrepo;

        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        //helpers
        AccountVerification accountVerification;

        public AppointmentController(IConfiguration config)
        {
            string con = config.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            //helpers
            accountVerification = new AccountVerification(con);
        }

        public IActionResult Agenda()
        {
            if (accountVerification.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == true)
            {
                AppointmentViewModel viewmodel = new AppointmentViewModel();
                Account account = new Account();
                account.AccountID = (int)HttpContext.Session.GetInt32("AccountID");
                foreach (Appointment appointment in appointmentrepo.AppointmentsCurrentWeek(account.AccountID))
                {
                    viewmodel.appointment.Add(appointmentconverter.ViewModelFromAppointment(appointment));
                }
                return View("~/Views/Afspraak/AfspraakPage.cshtml", viewmodel);
            }
            return View("~/Views/Home/Login.cshtml");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Doctor") == 1 || HttpContext.Session.GetInt32("Admin") == 1)
            {
                List<Account> LinkedPatients = new List<Account>();
                int id = (int)HttpContext.Session.GetInt32("AccountID");
                LinkedPatients = accountrepo.GetLinkedPatientsByDoctorID(id);
                ViewBag.LinkedPatients = LinkedPatients;
                return View("~/Views/Home/Appointment.cshtml");
            }
            return View("~/Views/Home/Login.cshtml");
        }

        [HttpGet]
        public IActionResult ViewAfspraak(int id)
        {
            AppointmentDetailViewModel ADVM = appointmentconverter.ViewModelFromAppointment(appointmentrepo.GetAppointmentByID(id));
            Appointment appointment = appointmentrepo.GetAppointmentByID(id);
            //ADVM.DoctorEmail = accountrepo.GetAccountByID(appointment.DoctorID).Email;
            return View("~/Views/Afspraak/AfspraakDetail.cshtml", ADVM);
        }

        [HttpGet]
        public IActionResult BackToList()
        {
            Account CurrentUser = JsonConvert.DeserializeObject<Account>(HttpContext.Session.GetString("CurrentUser"));
            List<Appointment> afspraken = appointmentrepo.AppointmentsCurrentWeek(CurrentUser.AccountID);
            AppointmentViewModel AVM = new AppointmentViewModel();
            foreach (var item in afspraken)
            {
                AppointmentDetailViewModel ADVM = appointmentconverter.ViewModelFromAppointment(item);
                AVM.appointment.Add(ADVM);
            }
            return View("~/Views/Afspraak/AfspraakPage.cshtml", AVM);
        }

        public IActionResult MakeAppointment(AppointmentDetailViewModel viewmodel)
        {
            Appointment inkomend = appointmentconverter.ViewModelToAppointment(viewmodel);
            if (appointmentrepo.MakeAppointment(inkomend) == true)
            {
                //afspraak gepland
                return View("~/Views/Home/Index.cshtml");
            }
            else
            {
                //mislukt
                return View("~/Views/Home/Appointment.cshtml");
            }
        }

    }
}