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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace CM.Controllers
{
    public class AppointmentController : Controller
    {
        AppointmentViewModelConverter appointmentViewModelConverter = new AppointmentViewModelConverter();
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        IAppointmentContext iappointmentcontext;
        AppointmentRepo appointmentrepo;

        //helpers
        AccountVerification accountVerification;

        public AppointmentController(IConfiguration config)
        {
            string con = config.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);

            //helpers
            accountVerification = new AccountVerification(con);
        }

        public IActionResult Appointment()
        {
            if (accountVerification.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == true)
            {
                if (accountVerification.CheckIfAdmin(HttpContext.Session.GetInt32("AccountID")) == true)
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

        public IActionResult Agenda()
        {
            if (accountVerification.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == true)
            {
                return View();
            }
            return View("~/Views/Home/Login.cshtml");
        }

        public IActionResult Index()
        {
            return View();
        }
       
    }
}