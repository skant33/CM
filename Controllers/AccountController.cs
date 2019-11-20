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

        public AccountController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);                      
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

        public IActionResult Register()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Beheerder()
        {
            ViewData["Message"] = "Your agenda";

            return View("~/Views/Home/Beheerder.cshtml");
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
                if(accountrepo.CheckIfAdmin(HttpContext.Session.GetInt32("AccountID")) == true)
                {
                    HttpContext.Session.SetInt32("Admin", 1);
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
            if (accountrepo.Register(inkomend) == true)
            {
                //geregistreerd
                return View("~/Views/Home/Login.cshtml");
            }
            else
            {
                //mislukt
                return View("~/Views/Home/Login.cshtml");
            }
        }

        [HttpGet]
        public IActionResult LogOut(AccountDetailViewModel viewmodel)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("MyAccount","Account");
        }
    }
}