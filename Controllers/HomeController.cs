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
using CM.Converters;
using CM.Helpers;

namespace CM.Controllers
{
    public class HomeController : Controller
    {
        //appointment
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        AppointmentRepo appointmentrepo;
        IAppointmentContext iappointmentContext;

        //account
        AccountViewModelConverter accountViewModelConverter = new AccountViewModelConverter();
        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        //helpers
        AccountVerification accountVerification;

        public HomeController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentContext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentContext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            accountVerification= new AccountVerification(con);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agenda()
        {
            if (accountVerification.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == true)
            {
                return View();
            }
            return View("~/Views/Home/Login.cshtml");
        }

        public IActionResult Appointment()
        {
            if (accountVerification.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == true)
            {
                if(accountVerification.CheckIfAdmin(HttpContext.Session.GetInt32("AccountID")) == true)
                {
                    return View();
                }
                else
                {
                    //return geen admin
                }
            }
            //return niet ingelogd
            return View("~/Views/Home/Login.cshtml");
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
                Account account = new Account();
                AccountDetailViewModel accountDetailViewModel = new AccountDetailViewModel();
                account = accountrepo.GetAccountByID((int)HttpContext.Session.GetInt32("AccountID"));
                accountDetailViewModel = accountViewModelConverter.AccountToViewModel(account);
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
