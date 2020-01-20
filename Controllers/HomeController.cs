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
        AccountConverter accountViewModelConverter = new AccountConverter();
        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        //helpers
        AccountVerification accVeri;

        public HomeController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentContext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentContext);

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            accVeri = new AccountVerification(con);
        }

        public IActionResult Index()
        {
            if (accVeri.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == false)
            {
                return RedirectToAction("LogOut", "Account");
            }

            appointmentViewModel.appointments = new List<Appointment>();

            Account opgehaald = new Account();

            opgehaald.AccountID = (int)HttpContext.Session.GetInt32("AccountID");

            if((HttpContext.Session.GetInt32("Doctor") != 1 && HttpContext.Session.GetInt32("Admin") != 1))
            {
                //patient
                ViewBag.LinkedDoctors = accountrepo.GetDoctorsFromPatient(opgehaald.AccountID);
            }
            else
            {
                //doctor or admin
                ViewBag.LinkedPatients = accountrepo.GetLinkedPatientsByDoctorID(opgehaald.AccountID);
            }

            foreach (Appointment appointment in appointmentrepo.AppointmentsCurrentWeek(opgehaald.AccountID))
            {
                appointmentViewModel.appointments.Add(appointment);
            }
            return View(appointmentViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
