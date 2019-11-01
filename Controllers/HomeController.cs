using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CM.Models;
using CM.Context.Interfaces;
using CM.Repositories;
using CM.ViewModels;
using Microsoft.Extensions.Configuration;
using CM.Context.SQL;
using Microsoft.AspNetCore.Http;

namespace CM.Controllers
{
    public class HomeController : Controller
    {
        //appointment
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        AppointmentRepo appointmentrepo;
        IAppointmentContext iappointmentContext;

        public HomeController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentContext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentContext);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agenda()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Appointment()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Beheerder()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Dashboard()
        {
            appointmentViewModel.appointments = new List<Appointment>();
            Account opgehaald = new Account();
            opgehaald.AccountID = (int)HttpContext.Session.GetInt32("AccountID");
            foreach (Appointment appointment in appointmentrepo.GetAppointmentsByUserID(opgehaald))
            {
                appointmentViewModel.appointments.Add(appointment);
            }
            return View("~/Views/Home/Dashboard.cshtml", appointmentViewModel);
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("AccountID") == null)
            {
                return View();
            }
            else
            {
                return View("~/Views/Home/MyAccount.cshtml", accountDetailViewModel);
            }
        }

        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
