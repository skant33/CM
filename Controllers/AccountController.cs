using System;
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

namespace CM.Controllers
{
    public class AccountController : Controller
    {
        //account
        AccountConverter accountViewModelConverter = new AccountConverter(); 
        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        //notification
        NotificationConverter notificationViewModelConverter = new NotificationConverter();
        INotificationContext inotificationcontext;
        NotificationRepo notificationrepo;

        public NotificationController noti;
        private IConfiguration config;

        public AccountController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);
            inotificationcontext = new NotificationMsSqlContext(con);
            notificationrepo = new NotificationRepo(inotificationcontext);
            noti = new NotificationController(iconfiguration);
            this.config = iconfiguration;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult MyAccount()
        {
            if (HttpContext.Session.GetInt32("AccountID") == null)
            {               
                return View("~/Views/Home/Login.cshtml");
            }
            else
            {
                Account account = new Account();
                AccountDetailViewModel accountDetailViewModel = new AccountDetailViewModel();
                account = accountrepo.GetAccountByID((int)HttpContext.Session.GetInt32("AccountID"));
                accountDetailViewModel = accountViewModelConverter.ViewModelFromAccount(account);
                return View("~/Views/Home/MyAccount.cshtml", accountDetailViewModel);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewData["Message"] = "Your agenda";

            return View("~/Views/Home/Register.cshtml");
        }

        [HttpGet]
        public IActionResult Beheerder()
        {
            List<Account> doctorlist = accountrepo.GetAllDoctors();
            ViewBag.AllDoctors = doctorlist;

            List<Account> patientlist = accountrepo.GetAllPatients();
            ViewBag.AllPatients = patientlist;

            return View("~/Views/Home/Beheerder.cshtml");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;            
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            Account opgehaald = accountrepo.Login(inkomend);
            if (opgehaald.Email == inkomend.Email)
            {
                HttpContext.Session.SetInt32("AccountID", opgehaald.AccountID);
                if (accountrepo.CheckRoleID(HttpContext.Session.GetInt32("AccountID")) == 3)
                {
                    HttpContext.Session.SetInt32("Admin", 1);
                }
                else if (accountrepo.CheckRoleID(HttpContext.Session.GetInt32("AccountID")) == 2)
                {
                    HttpContext.Session.SetInt32("Doctor", 1);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("~/Views/Home/Login.cshtml");
            }
        }

        [HttpPost]
        public IActionResult Register(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            if(accountrepo.CheckAccountExist(inkomend))
            {
                if (accountrepo.Register(inkomend) == true)
                {
                    //geregistreerd
                    return View("~/Views/Home/Login.cshtml");
                }
                else
                {
                    //mislukt (fout met database)
                    return View("~/Views/Home/Login.cshtml");
                }
            }
            else
            {
                //mislukt (bestaat al)
                return View("~/Views/Home/Login.cshtml");
            }
        }

        [HttpGet]
        public IActionResult LogOut(AccountDetailViewModel viewmodel)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("MyAccount","Account");
        }

        [HttpPost]
        public IActionResult LinkAccounts(int doctorid, int patientid)
        {
            accountrepo.LinkAccounts(patientid, doctorid);
            return RedirectToAction("Beheerder", "Account");
        }

        [HttpGet]
        public IActionResult EditNotification()
        {
            int accountid = (int)HttpContext.Session.GetInt32("AccountID");
            Notification notification = notificationrepo.GetNotificationForUser(accountid);
            NotificationDetailViewModel notificationDetailViewModel = new NotificationDetailViewModel();
            notificationDetailViewModel = notificationViewModelConverter.ViewModelFromNotification(notification);
            return View("~/Views/Home/EditNotification.cshtml", notificationDetailViewModel);
        }

        [HttpPost]
        public IActionResult EditNotification(int typeid, int timetillsend)
        {
            int accountid = (int)HttpContext.Session.GetInt32("AccountID");
            if(notificationrepo.UpdateNotificationForUser(accountid, typeid, timetillsend) == false)
            {
                return RedirectToAction("MyAccount", "Account");
            }
            else
            {
                return RedirectToAction("EditNotification", "Account");
            }
        }
    }
}