﻿using System;
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
            appointmentViewModel.appointments = new List<Appointment>();
            Account opgehaald = new Account();
            opgehaald.AccountID = (int)HttpContext.Session.GetInt32("AccountID");
            foreach (Appointment appointment in appointmentrepo.GetAppointmentsByUserID(opgehaald))
            {
                appointmentViewModel.appointments.Add(appointment);
            }
            return View("~/Views/Home/Dashboard.cshtml", appointmentViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
