﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Context.Interfaces;
using CM.Context.SQL;
using CM.Converters;
using CM.Models;
using CM.Repositories;
using CM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Hangfire;
using Hangfire.Storage;
using CM.Helpers;

namespace CM.Controllers
{
    public class AccountController : Controller
    {
        //account
        readonly AccountConverter accountViewModelConverter = new AccountConverter();
        readonly IAccountContext iaccountcontext;
        readonly AccountRepo accountrepo;

        //notification
        readonly NotificationConverter notificationViewModelConverter = new NotificationConverter();
        readonly INotificationContext inotificationcontext;
        readonly NotificationRepo notificationrepo;

        private AccountVerification accVeri;

        public AccountController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;

            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            inotificationcontext = new NotificationMsSqlContext(con);
            notificationrepo = new NotificationRepo(inotificationcontext);

            accVeri = new AccountVerification();
        }


        [HttpGet]
        public IActionResult Index()  //MyAccount
        {
            if (accVeri.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == false)
            {
                return RedirectToAction("LogOut", "Account");
            }
            else
            {
                Account account = new Account();
                AccountDetailViewModel accountDetailViewModel = new AccountDetailViewModel();

                account = accountrepo.GetAccountByID((int)HttpContext.Session.GetInt32("AccountID"));
                accountDetailViewModel = accountViewModelConverter.ViewModelFromAccount(account);

                return View(accountDetailViewModel);
            }
        }


        [HttpGet]
        public IActionResult Beheerder()
        {
            if (HttpContext.Session.GetInt32("Admin") != 1)
            {
                return RedirectToAction("Index", "Account");
            }

            List<Account> doctorlist = accountrepo.GetAllDoctors();
            ViewBag.AllDoctors = doctorlist;

            List<Account> patientlist = accountrepo.GetAllPatients();
            ViewBag.AllPatients = patientlist;

            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;           
            
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            Account opgehaald = accountrepo.Login(inkomend);

            if (opgehaald.Email == inkomend.Email)
            {
                HttpContext.Session.SetInt32("AccountID", opgehaald.AccountID);

                if (accountrepo.CheckRoleID(HttpContext.Session.GetInt32("AccountID")) == 2)
                {
                    HttpContext.Session.SetInt32("Doctor", 1);
                }
                else if (accountrepo.CheckRoleID(HttpContext.Session.GetInt32("AccountID")) == 3)
                {
                    HttpContext.Session.SetInt32("Admin", 1);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);

            if(!accountrepo.CheckAccountExist(inkomend))
            {
                if (accountrepo.Register(inkomend))
                {
                    //geregistreerd
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    //mislukt (fout met database)
                    return RedirectToAction("Register", "Account");
                }
            }
            else
            {
                //mislukt (bestaat al)
                return RedirectToAction("Register", "Account");
            }
            
        }


        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login","Account");
        }


        [HttpPost]
        public IActionResult LinkAccounts(AppointmentDetailViewModel Advm)
        {
            int patientId = Advm.Patient.AccountID;
            int doctorId = Advm.Doctor.AccountID;

            accountrepo.LinkAccounts(patientId, doctorId);

            return RedirectToAction("Beheerder", "Account");
        }


        [HttpGet]
        public IActionResult EditNotification()
        {
            if (accVeri.CheckIfLoggedIn(HttpContext.Session.GetInt32("AccountID")) == false)
            {
                return RedirectToAction("LogOut", "Account");
            }

            Notification notification = notificationrepo.GetNotificationForUser((int)HttpContext.Session.GetInt32("AccountID"));
            NotificationDetailViewModel notificationDetailViewModel = new NotificationDetailViewModel();

            notificationDetailViewModel = notificationViewModelConverter.ViewModelFromNotification(notification);

            return View(notificationDetailViewModel);
        }


        [HttpPost]
        public IActionResult EditNotification(int typeid, int timetillsend)
        {
            if(notificationrepo.UpdateNotificationForUser((int)HttpContext.Session.GetInt32("AccountID"), typeid, timetillsend) == false)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                return RedirectToAction("EditNotification", "Account");
            }
        }
    }
}