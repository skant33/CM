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

namespace CM.Controllers
{
    public class AccountController : Controller
    {
        //account
        AccountViewModelConverter accountViewModelConverter = new AccountViewModelConverter(); 
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

        [HttpPost]
        public IActionResult Login(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;            
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            Account opgehaald = accountrepo.Login(inkomend);          
            if (opgehaald.Email == inkomend.Email)
            {
                HttpContext.Session.SetInt32("AccountID", opgehaald.AccountID);
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                return View("~/Views/Home/Login.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountDetailViewModel viewmodel, string returnUrl = null)
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
            return RedirectToAction("Login","home");
        }
    }
}